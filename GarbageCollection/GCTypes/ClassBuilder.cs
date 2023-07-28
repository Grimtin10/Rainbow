using Rainbow.GarbageCollection;

namespace Rainbow.GarbageCollection.GCTypes;

public unsafe class ClassBuilder
{
    public GarbageCollector gc { get; set; }

    public ClassBuilder(ref GarbageCollector gc)
    {
        this.gc = gc;
    }
}