namespace Tests.Memory.Readers;

using System;
using Backend.Infrastructure.Emulator;
using Backend.Infrastructure.Memory;
using Backend.Memory;
using Backend.Memory.Readers;
using Moq;
using Xunit;

public class MemoryReaderTests
{
    private static (EmulatorSession Session, Mock<IMemoryAccessor> AccessorMock, MemoryReader Reader) CreateConnectedReader()
    {
        var emulatorSession = new EmulatorSession();
        var accessorMock = new Mock<IMemoryAccessor>();
        emulatorSession.SetAccessor(accessorMock.Object);

        return (emulatorSession, accessorMock, new MemoryReader(emulatorSession));
    }

    [Fact]
    public void ReadInt32_ShouldThrowMemoryReadException_WhenDisconnected()
    {
        var emulatorSession = new EmulatorSession();
        var reader = new MemoryReader(emulatorSession);

        var exception = Assert.Throws<MemoryReadException>(() => reader.ReadInt32(0x800100));

        Assert.Equal(0x800100, exception.Address);
    }

    [Fact]
    public void ReadInt32_ShouldThrowMemoryReadException_WhenAccessorThrows()
    {
        var (_, accessorMock, reader) = CreateConnectedReader();
        accessorMock.Setup(accessor => accessor.ReadInt32(It.IsAny<long>())).Throws(new Exception("Physical mapped I/O error"));

        var exception = Assert.Throws<MemoryReadException>(() => reader.ReadInt32(0x800100));

        Assert.Equal(0x800100, exception.Address);
        Assert.NotNull(exception.InnerException);
    }

    [Fact]
    public void ReadInt32_ShouldReturnInt_WhenConnectedAndAccessorSucceeds()
    {
        var (_, accessorMock, reader) = CreateConnectedReader();
        accessorMock.Setup(accessor => accessor.ReadInt32(0x800100)).Returns(9999);

        var result = reader.ReadInt32(0x800100);

        Assert.Equal(9999, result);
    }

    [Fact]
    public void ReadInt16_ShouldReturnShort_WhenConnectedAndAccessorSucceeds()
    {
        var (_, accessorMock, reader) = CreateConnectedReader();
        accessorMock.Setup(accessor => accessor.ReadInt16(0x800100)).Returns((short)45);

        var result = reader.ReadInt16(0x800100);

        Assert.Equal((short)45, result);
    }

    [Fact]
    public void ReadInt16_ShouldThrowMemoryReadException_WhenAccessorThrows()
    {
        var (_, accessorMock, reader) = CreateConnectedReader();
        accessorMock.Setup(accessor => accessor.ReadInt16(It.IsAny<long>())).Throws(new Exception("I/O error"));

        Assert.Throws<MemoryReadException>(() => reader.ReadInt16(0x800100));
    }

    [Fact]
    public void ReadInt16_ShouldThrowMemoryReadException_WhenDisconnected()
    {
        var emulatorSession = new EmulatorSession();
        var reader = new MemoryReader(emulatorSession);

        Assert.Throws<MemoryReadException>(() => reader.ReadInt16(0x800100));
    }

    [Fact]
    public void ReadBytes_ShouldReturnBytes_WhenConnectedAndAccessorSucceeds()
    {
        var (_, accessorMock, reader) = CreateConnectedReader();
        var expectedBytes = new byte[] { 1, 2, 3 };
        accessorMock.Setup(accessor => accessor.ReadArray(0x800100, It.IsAny<byte[]>(), 0, 3))
            .Callback<long, byte[], int, int>((address, buffer, index, count) => { Array.Copy(expectedBytes, buffer, 3); });

        var result = reader.ReadBytes(0x800100, 3);

        Assert.Equal(expectedBytes, result);
    }

    [Fact]
    public void ReadBytes_ShouldThrowMemoryReadException_WhenAccessorThrows()
    {
        var (_, accessorMock, reader) = CreateConnectedReader();
        accessorMock.Setup(accessor => accessor.ReadArray(It.IsAny<long>(), It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
            .Throws(new Exception("I/O error"));

        Assert.Throws<MemoryReadException>(() => reader.ReadBytes(0x800100, 3));
    }

    [Fact]
    public void ReadBytes_ShouldThrowMemoryReadException_WhenDisconnected()
    {
        var emulatorSession = new EmulatorSession();
        var reader = new MemoryReader(emulatorSession);

        Assert.Throws<MemoryReadException>(() => reader.ReadBytes(0x800100, 3));
    }
}
