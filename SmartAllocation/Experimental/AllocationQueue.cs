using Rainbow.GarbageCollection.GCTypes;

namespace Rainbow.SmartAllocation.Experimental;

public class AllocationQueue 
{ 
    public List<Block<byte>> allocs { get; set; } = new();

    public void Add(Block<byte> inf)
    {
        allocs.Add(inf);
    }
}