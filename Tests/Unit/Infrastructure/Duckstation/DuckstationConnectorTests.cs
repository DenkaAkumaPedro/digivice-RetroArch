using Backend.Infrastructure.Duckstation;
using Backend.Infrastructure.Emulator;
using Backend.Infrastructure.Memory;
using Backend.Infrastructure.Processes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Tests.Unit.Infrastructure.Duckstation;

public class DuckstationConnectorTests
{
    private static DuckstationConnector CreateConnector(
        EmulatorSession emulatorSession,
        Mock<IProcessService> processServiceMock,
        Mock<IMemoryProvider> memoryProviderMock,
        Mock<IConfiguration> configurationMock)
    {
        return new DuckstationConnector(
            emulatorSession,
            processServiceMock.Object,
            memoryProviderMock.Object,
            configurationMock.Object,
            NullLogger<DuckstationConnector>.Instance);
    }

    private static Mock<IConfiguration> CreateConfigurationMock(string emulatorProcessName)
    {
        var configSectionMock = new Mock<IConfigurationSection>();
        configSectionMock.Setup(section => section.Value).Returns(emulatorProcessName);
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(configuration => configuration.GetSection("Emulator:Duckstation:ProcessName")).Returns(configSectionMock.Object);
        var emptySection = new Mock<IConfigurationSection>();
        emptySection.Setup(section => section.Value).Returns((string?)null);
        configurationMock.Setup(configuration => configuration.GetSection("EmulatorProcessName")).Returns(emptySection.Object);
        return configurationMock;
    }

    [Fact]
    public void EnsureConnection_ShouldReturnSuccess_WhenProcessAndAccessorExist()
    {
        var emulatorSession = new EmulatorSession();
        var processServiceMock = new Mock<IProcessService>();
        processServiceMock.Setup(processService => processService.GetProcessIdByName("duckstation")).Returns(1234);

        var memoryAccessorMock = new Mock<IMemoryAccessor>();
        var memoryProviderMock = new Mock<IMemoryProvider>();
        memoryProviderMock.Setup(memoryProvider => memoryProvider.OpenExisting("duckstation_1234")).Returns(memoryAccessorMock.Object);

        var connector = CreateConnector(emulatorSession, processServiceMock, memoryProviderMock, CreateConfigurationMock("duckstation"));
        var result = connector.EnsureConnection();

        Assert.True(result.IsSuccess);
        Assert.Null(result.ErrorCode);
        Assert.Same(memoryAccessorMock.Object, emulatorSession.Accessor);
    }

    [Fact]
    public void EnsureConnection_ShouldReturnProcessNotFound_WhenProcessNotFound()
    {
        var emulatorSession = new EmulatorSession();
        var processServiceMock = new Mock<IProcessService>();
        processServiceMock.Setup(processService => processService.GetProcessIdByName("duckstation")).Returns((int?)null);
        var memoryProviderMock = new Mock<IMemoryProvider>();
        var connector = CreateConnector(emulatorSession, processServiceMock, memoryProviderMock, CreateConfigurationMock("duckstation"));
        var result = connector.EnsureConnection();
        Assert.False(result.IsSuccess);
        Assert.Equal(EmulatorConnectionErrorCodes.ProcessNotFound, result.ErrorCode);
        Assert.Null(emulatorSession.Accessor);
    }

    [Fact]
    public void EnsureConnection_ShouldReturnSuccess_WhenConnectedProcessStillExists()
    {
        var emulatorSession = new EmulatorSession();
        var processServiceMock = new Mock<IProcessService>();
        processServiceMock.Setup(processService => processService.GetProcessIdByName("duckstation")).Returns(1234);

        var memoryAccessorMock = new Mock<IMemoryAccessor>();
        var memoryProviderMock = new Mock<IMemoryProvider>();
        memoryProviderMock.Setup(memoryProvider => memoryProvider.OpenExisting("duckstation_1234")).Returns(memoryAccessorMock.Object);

        var connector = CreateConnector(emulatorSession, processServiceMock, memoryProviderMock, CreateConfigurationMock("duckstation"));

        Assert.True(connector.EnsureConnection().IsSuccess);
        Assert.True(connector.EnsureConnection().IsSuccess);
    }

    [Fact]
    public void EnsureConnection_ShouldReturnProcessNotFound_WhenEmulatorProcessIsGone()
    {
        var emulatorSession = new EmulatorSession();
        var processServiceMock = new Mock<IProcessService>();
        processServiceMock.SetupSequence(processService => processService.GetProcessIdByName("duckstation"))
            .Returns(1234)
            .Returns((int?)null);

        var memoryAccessorMock = new Mock<IMemoryAccessor>();
        var memoryProviderMock = new Mock<IMemoryProvider>();
        memoryProviderMock.Setup(memoryProvider => memoryProvider.OpenExisting("duckstation_1234")).Returns(memoryAccessorMock.Object);

        var connector = CreateConnector(emulatorSession, processServiceMock, memoryProviderMock, CreateConfigurationMock("duckstation"));

        Assert.True(connector.EnsureConnection().IsSuccess);

        var result = connector.EnsureConnection();

        Assert.False(result.IsSuccess);
        Assert.Equal(EmulatorConnectionErrorCodes.ProcessNotFound, result.ErrorCode);
        Assert.Null(emulatorSession.Accessor);
    }

    [Fact]
    public void EnsureConnection_ShouldReturnMappingNotFound_WhenEmulatorProcessIdChangesAndMappingIsMissing()
    {
        var emulatorSession = new EmulatorSession();
        var processServiceMock = new Mock<IProcessService>();
        processServiceMock.SetupSequence(processService => processService.GetProcessIdByName("duckstation"))
            .Returns(1234)
            .Returns(5678)
            .Returns(5678);

        var memoryAccessorMock = new Mock<IMemoryAccessor>();
        var memoryProviderMock = new Mock<IMemoryProvider>();
        memoryProviderMock.Setup(memoryProvider => memoryProvider.OpenExisting("duckstation_1234")).Returns(memoryAccessorMock.Object);

        var connector = CreateConnector(emulatorSession, processServiceMock, memoryProviderMock, CreateConfigurationMock("duckstation"));

        Assert.True(connector.EnsureConnection().IsSuccess);

        var result = connector.EnsureConnection();

        Assert.False(result.IsSuccess);
        Assert.Equal(EmulatorConnectionErrorCodes.MappingNotFound, result.ErrorCode);
        Assert.Null(emulatorSession.Accessor);
    }

    [Fact]
    public void EnsureConnection_ShouldReturnSuccess_WhenEmulatorProcessIdChangesAndMappingExists()
    {
        var emulatorSession = new EmulatorSession();
        var processServiceMock = new Mock<IProcessService>();
        processServiceMock.SetupSequence(processService => processService.GetProcessIdByName("duckstation"))
            .Returns(1234)
            .Returns(5678)
            .Returns(5678);

        var oldMemoryAccessorMock = new Mock<IMemoryAccessor>();
        var newMemoryAccessorMock = new Mock<IMemoryAccessor>();
        var memoryProviderMock = new Mock<IMemoryProvider>();
        memoryProviderMock.Setup(memoryProvider => memoryProvider.OpenExisting("duckstation_1234")).Returns(oldMemoryAccessorMock.Object);
        memoryProviderMock.Setup(memoryProvider => memoryProvider.OpenExisting("duckstation_5678")).Returns(newMemoryAccessorMock.Object);

        var connector = CreateConnector(emulatorSession, processServiceMock, memoryProviderMock, CreateConfigurationMock("duckstation"));

        Assert.True(connector.EnsureConnection().IsSuccess);

        var result = connector.EnsureConnection();

        Assert.True(result.IsSuccess);
        Assert.Same(newMemoryAccessorMock.Object, emulatorSession.Accessor);
        oldMemoryAccessorMock.Verify(accessor => accessor.Dispose(), Times.Once);
    }

    [Fact]
    public void ClearSession_ShouldClearSessionAccessor_AndDisposeAccessor()
    {
        var emulatorSession = new EmulatorSession();
        var processServiceMock = new Mock<IProcessService>();
        processServiceMock.Setup(processService => processService.GetProcessIdByName("duckstation")).Returns(1234);
        var memoryAccessorMock = new Mock<IMemoryAccessor>();
        var memoryProviderMock = new Mock<IMemoryProvider>();
        memoryProviderMock.Setup(memoryProvider => memoryProvider.OpenExisting("duckstation_1234")).Returns(memoryAccessorMock.Object);
        var connector = CreateConnector(emulatorSession, processServiceMock, memoryProviderMock, CreateConfigurationMock("duckstation"));
        connector.EnsureConnection();
        connector.ClearSession();
        Assert.Null(emulatorSession.Accessor);
        memoryAccessorMock.Verify(accessor => accessor.Dispose(), Times.Once);
    }

    [Fact]
    public void ClearSession_ShouldBeIdempotent_WhenAlreadyDisconnected()
    {
        var emulatorSession = new EmulatorSession();
        var processServiceMock = new Mock<IProcessService>();
        var memoryProviderMock = new Mock<IMemoryProvider>();
        var connector = CreateConnector(emulatorSession, processServiceMock, memoryProviderMock, CreateConfigurationMock("duckstation"));
        connector.ClearSession();
        connector.ClearSession();
        Assert.Null(emulatorSession.Accessor);
    }

    [Fact]
    public void EnsureConnection_ShouldReturnConfigMissing_WhenConfigValueIsEmpty()
    {
        var emulatorSession = new EmulatorSession();
        var processServiceMock = new Mock<IProcessService>();
        var memoryProviderMock = new Mock<IMemoryProvider>();
        var connector = CreateConnector(emulatorSession, processServiceMock, memoryProviderMock, CreateConfigurationMock(string.Empty));
        var result = connector.EnsureConnection();
        Assert.False(result.IsSuccess);
        Assert.Equal(EmulatorConnectionErrorCodes.ConfigMissing, result.ErrorCode);
        Assert.Null(emulatorSession.Accessor);
    }

    [Fact]
    public void EnsureConnection_ShouldReturnMappingNotFound_WhenAccessorIsNull()
    {
        var emulatorSession = new EmulatorSession();
        var processServiceMock = new Mock<IProcessService>();
        processServiceMock.Setup(processService => processService.GetProcessIdByName("duckstation")).Returns(1234);
        var memoryProviderMock = new Mock<IMemoryProvider>();
        memoryProviderMock.Setup(memoryProvider => memoryProvider.OpenExisting("duckstation_1234")).Returns((IMemoryAccessor?)null);
        var connector = CreateConnector(emulatorSession, processServiceMock, memoryProviderMock, CreateConfigurationMock("duckstation"));
        var result = connector.EnsureConnection();
        Assert.False(result.IsSuccess);
        Assert.Equal(EmulatorConnectionErrorCodes.MappingNotFound, result.ErrorCode);
        Assert.Null(emulatorSession.Accessor);
    }

    [Fact]
    public void EnsureConnection_ShouldReturnConnectionFailed_WhenOpenExistingThrowsException()
    {
        var emulatorSession = new EmulatorSession();
        var processServiceMock = new Mock<IProcessService>();
        processServiceMock.Setup(processService => processService.GetProcessIdByName("duckstation")).Returns(1234);
        var memoryProviderMock = new Mock<IMemoryProvider>();
        memoryProviderMock.Setup(memoryProvider => memoryProvider.OpenExisting("duckstation_1234")).Throws(new Exception("Open mapping error"));
        var connector = CreateConnector(emulatorSession, processServiceMock, memoryProviderMock, CreateConfigurationMock("duckstation"));
        var result = connector.EnsureConnection();
        Assert.False(result.IsSuccess);
        Assert.Equal(EmulatorConnectionErrorCodes.ConnectionFailed, result.ErrorCode);
        Assert.Equal("Open mapping error", result.ErrorDetail);
        Assert.Null(emulatorSession.Accessor);
    }
}
