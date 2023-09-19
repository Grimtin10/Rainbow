using System.Runtime.InteropServices;

namespace Rainbow.GarbageCollection.GC2;

public class StackBlock
{
    public Pointer<byte> ptr { get; set; }
    public int pos { get; set; } = 0;

    public List<KeyValuePair<Pointer<byte>, byte>> segments { get; set; } = new();

    public StackBlock(Pointer<byte> ptr)
    {
        this.ptr = ptr;
        pos = 0;
    }
}