using Backend.Infrastructure.Memory;

namespace Backend.Infrastructure.Emulator;

public interface IEmulatorSession
{
    IMemoryAccessor? Accessor { get; }
}
