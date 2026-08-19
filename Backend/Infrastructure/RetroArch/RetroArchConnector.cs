using Backend.Infrastructure.Emulator;
using Serilog;

namespace Backend.Infrastructure.RetroArch;

public sealed class RetroArchConnector(
    EmulatorSession emulatorSession,
    IRetroArchNetworkClient networkClient,
    IConfiguration configuration) : IEmulatorConnector
{
    private bool _isConnected;
    private string? _host;
    private int _port;

    public ConnectionAttemptResult EnsureConnection()
    {
        if (_isConnected)
        {
            return ConnectionAttemptResult.Success();
        }

        ClearSession();

        try
        {
            if (!LoadConfiguration())
            {
                return ConnectionAttemptResult.Failure(EmulatorConnectionErrorCodes.ConfigMissing);
            }

            networkClient.Connect(_host!, _port);

            string? statusResponse = networkClient.SendCommand("GET_STATUS");

            if (string.IsNullOrEmpty(statusResponse) || statusResponse.Contains("error", StringComparison.OrdinalIgnoreCase))
            {
                Log.Debug("RetroArch not responding at {Host}:{Port}", _host, _port);
                return ConnectionAttemptResult.Failure(EmulatorConnectionErrorCodes.ConnectionFailed, "RetroArch not responding");
            }

            Log.Information("Connected to RetroArch at {Host}:{Port}. Status: {Status}", _host, _port, statusResponse);

            var memoryAccessor = new RetroArchMemoryAccessor(networkClient);
            emulatorSession.SetAccessor(memoryAccessor);
            _isConnected = true;

            return ConnectionAttemptResult.Success();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to connect to RetroArch");
            return ConnectionAttemptResult.Failure(EmulatorConnectionErrorCodes.ConnectionFailed, ex.Message);
        }
    }

    public void ClearSession()
    {
        emulatorSession.ClearAccessor();
        _isConnected = false;
    }

    private bool LoadConfiguration()
    {
        _host = configuration.GetValue<string>("Emulator:RetroArch:Host") ?? "127.0.0.1";
        _port = configuration.GetValue<int?>("Emulator:RetroArch:Port") ?? 55355;

        return true;
    }
}
