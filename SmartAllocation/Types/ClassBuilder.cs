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

    public Class WriteClass()
    {
        Block<Block<byte>> ptrs = eng.gc.stack.StackAlloc<Block<byte>>(true, inf.variableInfo.Count);
        Class ret = new();
        
        int pos = 0;
        foreach(KeyValuePair<string, VariableInfo> i in inf.variableInfo)
        {
            Block<byte> varptr = eng.AllocSimple(i.Value.size);
            ptrs.SetPos(pos, varptr);

            pos = pos + 1;

            ret.accessor.Add(i.Key, new(i.Value.type, varptr));
        }

        eng.gc.canCollect = true;

        ret.ptr = ptrs.GetBlockBytes();

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

    public unsafe int SizeOf()
    {
        return variableInfo.Count * sizeof(Block<byte>);
    }
}
