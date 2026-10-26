namespace Backend.Infrastructure.RetroArch;

public interface IRetroArchNetworkClient : IDisposable
{
    bool IsConnected { get; }
    void Connect(string host, int port);
    void Disconnect();
    string? SendCommand(string command);
}
