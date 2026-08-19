# Guia de Configuração - RetroArch

Este projeto suporta **Duckstation** e **RetroArch** como emuladores para leitura de memória do Digimon World 2003.

---

## Modificações Realizadas

### Arquivos Novos

| Arquivo | Descrição |
|---------|-----------|
| `Backend/Infrastructure/Emulator/IEmulatorConnector.cs` | Interface genérica para conectores de emulador |
| `Backend/Infrastructure/Emulator/IEmulatorSession.cs` | Interface genérica para sessões de emulador |
| `Backend/Infrastructure/Emulator/EmulatorSession.cs` | Sessão compartilhada que armazena o accessor de memória |
| `Backend/Infrastructure/Emulator/EmulatorType.cs` | Enum com os tipos: `Duckstation`, `RetroArch` |
| `Backend/Infrastructure/Emulator/ConnectionAttemptResult.cs` | Resultado de tentativa de conexão |
| `Backend/Infrastructure/Emulator/EmulatorConnectionErrorCodes.cs` | Códigos de erro de conexão |
| `Backend/Infrastructure/RetroArch/RetroArchConnector.cs` | Implementação do conector para RetroArch via UDP |
| `Backend/Infrastructure/RetroArch/RetroArchNetworkClient.cs` | Cliente UDP para comunicação com RetroArch |
| `Backend/Infrastructure/RetroArch/RetroArchMemoryAccessor.cs` | Leitura de memória via comandos UDP do RetroArch |

### Arquivos Modificados

| Arquivo | Mudança |
|---------|---------|
| `Backend/Infrastructure/Duckstation/DuckstationConnector.cs` | Implementa `IEmulatorConnector` em vez de `IDuckstationConnector` |
| `Backend/Infrastructure/DependencyInjection.cs` | Registro por tipo de emulador via config (`Emulator:Type`) |
| `Backend/Application/GameLoopService.cs` | Usa `IEmulatorConnector` + log de debug do estado lido |
| `Backend/Memory/Readers/MemoryReader.cs` | Usa `IEmulatorSession` em vez de `IDuckstationSession` |
| `Backend/appsettings.json` | Seção `Emulator` com config para Duckstation e RetroArch |

### Arquivos Removidos

| Arquivo | Motivo |
|---------|--------|
| `Backend/Infrastructure/Duckstation/IDuckstationConnector.cs` | Substituído por `IEmulatorConnector` |
| `Backend/Infrastructure/Duckstation/IDuckstationSession.cs` | Substituído por `IEmulatorSession` |
| `Backend/Infrastructure/Duckstation/DuckstationSession.cs` | Substituído por `EmulatorSession` |
| `Backend/Infrastructure/Duckstation/ConnectionAttemptResult.cs` | Movido para `Infrastructure/Emulator/` |
| `Backend/Infrastructure/Duckstation/EmulatorConnectionErrorCodes.cs` | Movido para `Infrastructure/Emulator/` |

---

## Configuração do RetroArch

### 1. Ativar Comandos de Rede

No RetroArch:

```
Configurações > Rede > Comandos de Rede → Ativado
```

Ou edite o arquivo `retroarch.cfg` (na pasta do RetroArch ou em `%APPDATA%`):

```
network_cmd_enable = "true"
network_cmd_port = "55355"
```

### 2. Núcleo Necessário

O RetroArch precisa de um núcleo PS1 compatível. Baixe via **Atualizador de Core**:

- **Beetle PSX** - Recomendado. Mais preciso.
- **PCSX-ReARMed** - Compatível. Mais leve, porém menos preciso.

> **Nota:** O SwanStation **não é compatível** com este app pois não implementa o mapa de memória necessário.

### 3. Carregar o Jogo

1. Carregue o core escolhido
2. Abra **Digimon World 2003** (.bin/.cue ou .iso)
3. Chegue ao ponto em que seu Digimon está no mapa

---

## Configuração do Backend

Edite `Backend/appsettings.json`:

```json
{
    "Emulator": {
        "Type": "RetroArch",
        "Duckstation": {
            "ProcessName": "duckstation-qt-x64-ReleaseLTCG"
        },
        "RetroArch": {
            "Host": "127.0.0.1",
            "Port": 55355,
            "Core": "BeetlePSX"
        }
    }
}
```

Para usar **Duckstation**, mude `"Type": "RetroArch"` para `"Type": "Duckstation"`.

---

## Como Funciona

### Duckstation (original)
- Lê memória diretamente via **Named Memory-Mapped Files** do Windows (`duckstation_{pid}`)
- Mais rápido e confiável
- Requer a opção "Export Memory Shared" habilitada no Duckstation

### RetroArch (novo)
- Comunica via **UDP Network Commands** (porta 55355)
- Usa comandos `GET_STATUS` e `READ_CORE_MEMORY`
- Requer "Comandos de Rede" habilitado no RetroArch

---

## Compilar e Rodar

### Modo Desenvolvimento

```powershell
cd Backend
dotnet run
```

### Publicar como Executável

```powershell
dotnet publish Backend\Backend.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o out_temp
```

### Rodar com o App Original (Tauri)

1. Extraia `digivice-3.0.0.zip`
2. Substitua `backend.exe` pelo executável publicado
3. Substitua `appsettings.json` pela versão com RetroArch
4. Execute `digivice.exe`

---

## Solução de Problemas

| Erro | Causa | Solução |
|------|-------|---------|
| `RetroArch UDP communication error` | Comandos de Rede desabilitados | Ative em Configurações > Rede |
| `RetroArch not responding` | RetroArch não está rodando | Abra o RetroArch antes de rodar o backend |
| `ConfigMissing` | Configuração ausente no appsettings.json | Verifique a seção `Emulator` |
| `ConnectionFailed` | Host ou porta incorretos | Verifique `Host` e `Port` no appsettings.json |
| `address already in use` | Porta 5000 ocupada | Feche outro processo na porta ou mude no appsettings.json |
