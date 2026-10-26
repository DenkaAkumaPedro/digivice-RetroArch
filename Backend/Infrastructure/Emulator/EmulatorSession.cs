using Backend.Infrastructure.Memory;

namespace Backend.Infrastructure.Emulator;

public sealed class EmulatorSession : IEmulatorSession
{
    public IMemoryAccessor? Accessor { get; private set; }

    internal void SetAccessor(IMemoryAccessor accessor)
    {
        Accessor = accessor;
    }

    internal void ClearAccessor()
    {
        Accessor?.Dispose();
        Accessor = null;
    }
}

