using Backend.Infrastructure.Emulator;
using Backend.Infrastructure.Memory;
using Backend.Infrastructure.Processes;

namespace Backend.Infrastructure.Duckstation;

public sealed class DuckstationConnector(
    EmulatorSession emulatorSession,
    IProcessService processService,
    IMemoryProvider memoryProvider,
    IConfiguration configuration,
    ILogger<DuckstationConnector> logger) : IEmulatorConnector
{
    private string? EmulatorProcessName
    {
        get
        {
            var value = configuration.GetSection("Emulator:Duckstation:ProcessName").Value;
            if (!string.IsNullOrEmpty(value)) return value;
            value = configuration["Emulator:Duckstation:ProcessName"];
            if (!string.IsNullOrEmpty(value)) return value;
            value = configuration.GetSection("EmulatorProcessName").Value;
            if (!string.IsNullOrEmpty(value)) return value;
            value = configuration["EmulatorProcessName"];
            return string.IsNullOrEmpty(value) ? null : value;
        }
    }

    private int? ConnectedProcessId { get; set; }

    private bool HasActiveConnection =>
        ConnectedProcessId is not null && emulatorSession.Accessor is not null;
    private bool ProcessIdChanged => EmulatorProcessName != null && processService.GetProcessIdByName(EmulatorProcessName) != ConnectedProcessId;

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
                logger.LogError("EmulatorProcessName not found in appsettings.json");
                return ConnectionAttemptResult.Failure(EmulatorConnectionErrorCodes.ConfigMissing);
            }

            int? processId = processService.GetProcessIdByName(EmulatorProcessName);

            if (processId == null)
            {
                logger.LogDebug("Duckstation process not found for {ProcessName}", EmulatorProcessName);
                return ConnectionAttemptResult.Failure(EmulatorConnectionErrorCodes.ProcessNotFound);
            }

            string duckstationMapName = $"duckstation_{processId}";
            IMemoryAccessor? memoryAccessor = memoryProvider.OpenExisting(duckstationMapName);

            if (memoryAccessor == null)
            {
                logger.LogDebug("Duckstation memory mapping not found: {MapName}", duckstationMapName);
                return ConnectionAttemptResult.Failure(EmulatorConnectionErrorCodes.MappingNotFound);
            }

            emulatorSession.SetAccessor(memoryAccessor);
            ConnectedProcessId = processId;
            logger.LogInformation("Connected to DuckStation! Mapping found: {MapName}", duckstationMapName);
            return ConnectionAttemptResult.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to connect to DuckStation");
            return ConnectionAttemptResult.Failure(EmulatorConnectionErrorCodes.ConnectionFailed, ex.Message);
        }
    }

    public void ClearSession()
    {
        emulatorSession.ClearAccessor();
        ConnectedProcessId = null;
    }
}
