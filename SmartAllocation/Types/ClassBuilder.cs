using Rainbow.Types;
using Rainbow.GarbageCollection.GCTypes;

namespace Rainbow.SmartAllocation.Types;

public unsafe class ClassBuilder
{
    public ClassInfo inf { get; set; }
    public AllocationEngine eng { get; set; }

    public ClassBuilder(ref AllocationEngine eng, ClassInfo inf)
    {
        this.eng = eng;
        this.inf = inf;
    }

    public int CalculateSize()
    {
        return inf.variableInfo.Count;
    }

    public Class WriteClass()
    {
        Block<Block<byte>> mem = eng.gc.stack.StackAlloc<Block<byte>>(true, CalculateSize());
        Class ret = new Class();

        int pos = 0;
        foreach(KeyValuePair<string, VariableInfo> var in inf.variableInfo)
        {
            Block<byte> ptr = new Block<byte>((byte *)((int)mem._ref) + (pos * sizeof(Block<byte>)), sizeof(Block<byte>));
            ret.accessor.Add(var.Key, new(var.Value.type, ptr));

            Block<byte> b = eng.AllocateAndFinalize(var.Value.size);
            b.isref = true;

            mem.SetPos(pos, b);

            //ret.accessor.Add(var.Key, new(var.Value.type, ptr));

            pos = pos + 1;
        }

        ret.ptr = mem.GetBlockBytes();

        return ret;
    }
}

public class ClassInfo
{
    public Dictionary<string, VariableInfo> variableInfo { get; set; } = new();

    public ClassInfo()
    {
        this.variableInfo = new();
    }

    public void Add(string propertyName, VariableInfo inf)
    {
        variableInfo.Add(propertyName, inf);
    }
}
