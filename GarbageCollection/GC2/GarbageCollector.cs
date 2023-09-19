using System.Runtime.InteropServices;

namespace Rainbow.GarbageCollection.GC2;

public unsafe class GarbageCollector
{
    public StackBlock _stack { get; set; }
    public List<IBlock<byte>> _allocated { get; set; } = new();

    #pragma warning disable CS8618
    public GarbageCollector(int stackSize = 4096 * 1024)
    {
        StackSetup(stackSize);
    }
    #pragma warning restore CS8618

    private void StackSetup(int size)
    {
        byte *ptr = (byte *)Marshal.AllocHGlobal(size);
        _stack = new StackBlock(new Pointer<byte>(ptr, size));
    }

    public IBlock<byte>? Allocate(int size, BlockType tp = BlockType.BLOCK)
    {
        switch(tp)
        {
            case BlockType.BLOCK:
            {
                break;
            }

            case BlockType.POINTER:
            {
                break;
            }

            case BlockType.STRUCT:
            {
                break;
            }
        }

        return null;
    }
}