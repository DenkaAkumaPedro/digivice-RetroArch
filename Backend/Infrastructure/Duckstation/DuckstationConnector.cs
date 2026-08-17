using Backend.Infrastructure.Emulator;
using Backend.Infrastructure.Memory;
using Backend.Infrastructure.Processes;
using Serilog;

namespace Backend.Infrastructure.Duckstation;

public sealed class DuckstationConnector(
    EmulatorSession emulatorSession,
    IProcessService processService,
    IMemoryProvider memoryProvider,
    IConfiguration configuration) : IEmulatorConnector
{
    private readonly string? EmulatorProcessName = configuration.GetValue<string>("Emulator:Duckstation:ProcessName");
    private int? ConnectedProcessId { get; set; }

    private bool HasActiveConnection =>
        ConnectedProcessId is not null && emulatorSession.Accessor is not null;
    private bool ProcessIdChanged => processService.GetProcessIdByName(EmulatorProcessName!) != ConnectedProcessId;

    public ConnectionAttemptResult EnsureConnection()
    {
        if (HasActiveConnection && !ProcessIdChanged)
        {
            return ConnectionAttemptResult.Success();
        }

        ClearSession();

        try
        {
            if (string.IsNullOrEmpty(EmulatorProcessName))
            {
                Log.Error("EmulatorProcessName not found in appsettings.json");
                return ConnectionAttemptResult.Failure(EmulatorConnectionErrorCodes.ConfigMissing);
            }

            int? processId = processService.GetProcessIdByName(EmulatorProcessName);

            if (processId == null)
            {
                Log.Debug("Duckstation process not found for {ProcessName}", EmulatorProcessName);
                return ConnectionAttemptResult.Failure(EmulatorConnectionErrorCodes.ProcessNotFound);
            }

            string duckstationMapName = $"duckstation_{processId}";
            IMemoryAccessor? memoryAccessor = memoryProvider.OpenExisting(duckstationMapName);

            if (memoryAccessor == null)
            {
                Log.Debug("Duckstation memory mapping not found: {MapName}", duckstationMapName);
                return ConnectionAttemptResult.Failure(EmulatorConnectionErrorCodes.MappingNotFound);
            }

            emulatorSession.SetAccessor(memoryAccessor);
            ConnectedProcessId = processId;
            Log.Information("Connected to DuckStation! Mapping found: {MapName}", duckstationMapName);
            return ConnectionAttemptResult.Success();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to connect to DuckStation");
            return ConnectionAttemptResult.Failure(EmulatorConnectionErrorCodes.ConnectionFailed, ex.Message);
        }
    }

    public void ClearSession()
    {
        emulatorSession.ClearAccessor();
        ConnectedProcessId = null;
    }
}
