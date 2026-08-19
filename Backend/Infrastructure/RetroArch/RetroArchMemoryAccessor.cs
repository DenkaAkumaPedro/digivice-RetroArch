using System.Globalization;
using Backend.Infrastructure.Memory;

namespace Backend.Infrastructure.RetroArch;

public sealed class RetroArchMemoryAccessor(
    IRetroArchNetworkClient networkClient) : IMemoryAccessor
{
    private readonly IRetroArchNetworkClient _networkClient = networkClient;

    public int ReadInt32(long address)
    {
        byte[] bytes = ReadBytes(address, 4);
        return BitConverter.ToInt32(bytes, 0);
    }

    public short ReadInt16(long address)
    {
        byte[] bytes = ReadBytes(address, 2);
        return BitConverter.ToInt16(bytes, 0);
    }

    public void ReadArray(long address, byte[] buffer, int index, int count)
    {
        byte[] data = ReadBytes(address, count);
        Array.Copy(data, 0, buffer, index, count);
    }

    private byte[] ReadBytes(long address, int length)
    {
        string command = $"READ_CORE_MEMORY {address:X} {length}";

        string? response = _networkClient.SendCommand(command);

        if (string.IsNullOrEmpty(response))
        {
            throw new IOException($"No response from RetroArch for address 0x{address:X}");
        }

        return ParseReadResponse(response, address);
    }

    private byte[] ParseReadResponse(string response, long requestedAddress)
    {
        string[] parts = response.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length < 3)
        {
            throw new IOException($"Invalid RetroArch response: {response}");
        }

        string secondPart = parts[1];

        if (secondPart == "-1")
        {
            string errorMessage = parts.Length > 2 ? string.Join(" ", parts.Skip(2)) : "unknown error";
            throw new IOException($"RetroArch read failed at 0x{requestedAddress:X}: {errorMessage}");
        }

        string hexData = string.Join("", parts.Skip(2));

        if (hexData.Length % 2 != 0)
        {
            throw new IOException($"Invalid hex data length from RetroArch: {hexData}");
        }

        byte[] result = new byte[hexData.Length / 2];

        for (int i = 0; i < result.Length; i++)
        {
            result[i] = byte.Parse(hexData.Substring(i * 2, 2), NumberStyles.HexNumber);
        }

        return result;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}

