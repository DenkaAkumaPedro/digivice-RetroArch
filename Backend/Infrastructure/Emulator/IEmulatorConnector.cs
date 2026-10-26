namespace Backend.Infrastructure.Emulator;

public interface IEmulatorConnector
{
    ConnectionAttemptResult EnsureConnection();
    void ClearSession();
}

