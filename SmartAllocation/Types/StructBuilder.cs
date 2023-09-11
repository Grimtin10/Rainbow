using Rainbow.GarbageCollection.GCTypes;
using Rainbow.Types;

namespace Rainbow.SmartAllocation.Types;

public class StructBuilder
{
    public StructInfo info { get; set; } = new();
    public AllocationEngine engine { get; set; }

    public StructBuilder(ref AllocationEngine eng, StructInfo info)
    {
        this.engine = eng;
        this.info = info;
    }

    public int CalculateSize()
    {
        int ret = 0;
        foreach(KeyValuePair<string, VariableInfo> inf in info.variableInfo)
        {
            ret = ret + inf.Value.size;
        }

        return ret;
    }

    public unsafe Struct WriteStruct(Block<byte> mymem)
    {
        Struct s = new();
        s.ptr = mymem;

        int pos = 0;
        foreach(KeyValuePair<string, VariableInfo> inf in info.variableInfo)
        {
            byte *start = (byte *)((long)mymem._ref + pos);
            Block<byte> blk = new Block<byte>(start, inf.Value.size);

            s.accessor.Add(inf.Key, new KeyValuePair<Type, Block<byte>>(inf.Value.type, blk));
            pos = pos + inf.Value.size;
        }

        return s;
    }
}

public class StructInfo
{
    public Dictionary<string, VariableInfo> variableInfo { get; set; } = new();

    public StructInfo()
    {
        this.variableInfo = new();
    }

    public void Add(string propertyName, VariableInfo inf)
    {
        variableInfo.Add(propertyName, inf);
    }

    public unsafe int SizeOf()
    {
        int size = 0;

        foreach(KeyValuePair<string, VariableInfo> inf in variableInfo)
        {
            size = size + inf.Value.size;
        }

        return size;
    }
}

public class VariableInfo
{
    public int size { get; set; }
    public Type type { get; set; }
    public bool alloc { get; set; }

    public VariableInfo(Type t, bool alloc = false, int size = 0)
    {
        type = t;
        this.alloc = alloc;
        this.size = size;
    }
}