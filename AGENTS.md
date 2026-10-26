# AGENTS.md

Windows-only desktop companion app for Digimon World 2003. Three parts in one repo:

- `Backend/` — ASP.NET Core (net10.0) + SignalR: reads emulator RAM, diffs state, pushes events.
- `Frontend/` — Vue 3 + Pinia + Vite 7 + Tailwind 4; all app code under `Frontend/src`.
- `Tauri/` — Rust shell: spawns `Backend.exe` as a sidecar, owns the window.
- `Tests/` — xUnit for Backend only. Solution: `Digivice.slnx` (Backend + Tests).
- `Tools/` — MemoryScanner console app (not in the solution) + one-off data scripts.
- `AI/`, `DEFERRED/`, `.cursor/` — docs/backlogs, no runtime code.

Entrypoints: `Backend/Program.cs`, `Frontend/src/main.ts`, `Tauri/src/lib.rs`.

Data flow: Addresses → Loader → Reader → Resource → Assembler → State → diff → SignalR DTO → frontend converter → syncer → Pinia store → component.

## Commands

Requires .NET 10 SDK and Node 20.19+/22.12+.

```text
dotnet build Digivice.slnx
dotnet test  Digivice.slnx
dotnet test --filter "FullyQualifiedName~MemoryReaderTests"   # single class
```

Frontend (run from `Frontend/`):

```text
npm run lint          # oxlint --fix, then eslint --fix
npm run type-check    # vue-tsc --build
npm run format        # prettier --write src/
npm run build         # type-check + vite build
```

Dev:

```text
dotnet run --project Backend     # http://localhost:5000, hub /gamehub
npm run dev                      # cwd Frontend/, browser only (proxies /gamehub)
npm run tauri:dev                # full app; wraps Tauri/run-tauri.ps1
```

Order matters: `lint` (it rewrites files) → `type-check` → `test`. There is no CI and no pre-commit hook — verify yourself.

- `npm run lint` currently **exits non-zero**: oxlint reports ~10 unfixable pre-existing errors (unused imports/vars, BOM before `import` in `src/viewmodels/desert/` and `src/repositories/`) and `run-s` then skips eslint. `npm run build` does not run lint, so a green build does not mean lint-clean. Do not assume your change broke it — diff against `main` if unsure.
- Verified working: `dotnet build Digivice.slnx`, `dotnet test Digivice.slnx` (387 tests), `npm run type-check`.
- `dotnet run --project Backend` uses `Backend/Properties/launchSettings.json` (Development, `127.0.0.1:5000`). If the port is taken the host dies with `AddressInUseException` — port 5000 was already occupied by another local app while this file was written. Tauri picks a free port itself (`get_free_port`), but the Vite proxy is hardcoded to `localhost:5000`.

## Hard rules (easy to get wrong)

- **No frontend tests.** Vitest, `*.spec.ts`, `__tests__` are banned on purpose.
- Never execute an implementation plan without explicit user approval (`.cursor/rules/digivice-core.mdc`, `alwaysApply`).
- Canonical rule sources: `AI/CODE_RULES.md` (style + layering) and `AI/BUSINESS_RULES.md` (domain invariants); `.cursor/rules/*.mdc` mirrors them. Read the section for your task before writing code.
- Frontend: `@/` imports, double quotes, trailing `;`, one type per `*.viewmodel.ts`, no new code in `presenters/helper/` or `src/helpers/` (use `src/services/`).
- Backend: primary constructors, no `_` prefix on fields, private members above public, stateless Readers/Converters; new RAM fields extend the existing `{Entity}Addresses` / `{Entity}Reader` / `{Entity}Resource` / `{Entity}Loader` pipeline — consumers call Loaders, never Readers.
- Tests mirror production files 1:1 (`Foo.cs` → `FooTests.cs`), except integration and cross-cutting contract suites.
- Comments and commit messages in English; the rule docs are PT-BR.

## Facts you cannot guess

- The emulator connector is chosen by `Emulator:Type` in `Backend/appsettings.json` (`Duckstation` default, `RetroArch` currently). The README still says Duckstation-only.
- Backend resolves `Memory/Definitions` relative to its own base directory (`Program.cs` sets the CWD to `AppContext.BaseDirectory`).
- Sidecar contract: `Tauri/binaries/backend-x86_64-pc-windows-msvc.exe` **plus** `appsettings.json` and `Memory/` in the same folder. The filename must carry the target triple (`externalBin: ["binaries/backend"]`). `Tauri/scripts/copy-sidecar-runtime.ps1` runs as `beforeBundleCommand`.
- Hub URL: relative `/gamehub` behind the Vite proxy in dev; in the packaged app the frontend asks Tauri for a free port (`get_backend_port` in `Tauri/src/lib.rs`).
- Static game data lives in `Frontend/src/database/` (JSON) + `src/assets/`; `imageName` must match the asset filename exactly, case included.
- `Tauri/binaries/Memory/Definitions/` and root `Memory/Definitions/` are **copies** of `Backend/Memory/Definitions/` (the canonical one). Several tracked copies under `Tauri/binaries/` are stale (differ from Backend); only `build_app.ps1`'s publish step refreshes them. Editing addresses? Change `Backend/Memory/Definitions/` and re-run the publish step, or the packaged app ships the old values. `Tauri/.gitignore` ignores `/binaries/` except files force-added earlier.

## Repo state / gotchas

- `build_app.ps1` hardcodes `$RootPath = "c:\Projetos\digivice"` — fix the path before running it here.
- `Tauri/binaries/` currently holds `Backend.exe`, not the triple-named file, so a raw `npm run tauri:build` fails until the publish/rename step has run.
- The Tauri app dir is `Tauri/`. Untracked `Frontend/src-tauri/` contains only generated output (`binaries/`, `gen/`, `target/`; no `tauri.conf.json`/`Cargo.toml`) — ignore it.
- Untracked local artifacts at the root: `backend.exe`, `appsettings.json`, `Memory/`, `digivice-release/`. Do not commit them. (`node_modules/.cache/prettier/...` is already tracked by accident — don't add more.)
- Current branch `v4.0-retroarch`: `Backend/Infrastructure/{Emulator,RetroArch}` is new WIP and `Infrastructure/Duckstation/*` is deleted in the working tree.
