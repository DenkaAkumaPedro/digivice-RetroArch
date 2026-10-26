using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Backend.Infrastructure.RetroArch;

public sealed class RetroArchNetworkClient(ILogger<RetroArchNetworkClient> logger) : IRetroArchNetworkClient
{
    private UdpClient? _udpClient;
    private IPEndPoint? _endPoint;
    private readonly TimeSpan _timeout = TimeSpan.FromSeconds(2);

    public bool IsConnected => _udpClient is not null && _endPoint is not null;

    public void Connect(string host, int port)
    {
        Disconnect();

        _udpClient = new UdpClient();
        _udpClient.Client.ReceiveTimeout = (int)_timeout.TotalMilliseconds;
        _udpClient.Client.SendTimeout = (int)_timeout.TotalMilliseconds;
        _endPoint = new IPEndPoint(IPAddress.Parse(host), port);

        logger.LogInformation("RetroArch UDP client configured for {Host}:{Port}", host, port);
    }

    public void Disconnect()
    {
        _udpClient?.Dispose();
        _udpClient = null;
        _endPoint = null;
    }

    public string? SendCommand(string command)
    {
        if (_udpClient is null || _endPoint is null)
        {
            logger.LogWarning("RetroArch UDP client not connected");
            return null;
        }

        try
        {
            byte[] commandBytes = Encoding.ASCII.GetBytes(command);
            _udpClient.Send(commandBytes, commandBytes.Length, _endPoint);

            IPEndPoint remoteEndPoint = new(IPAddress.Any, 0);
            byte[] responseBytes = _udpClient.Receive(ref remoteEndPoint);
            return Encoding.ASCII.GetString(responseBytes).Trim();
        }
        catch (SocketException ex)
        {
            logger.LogWarning("RetroArch UDP communication error: {Error}", ex.Message);
            return null;
        }
        catch (ObjectDisposedException)
        {
            logger.LogWarning("RetroArch UDP client was disposed");
            return null;
        }
    }

    public void Dispose()
    {
        Disconnect();
    }
}
