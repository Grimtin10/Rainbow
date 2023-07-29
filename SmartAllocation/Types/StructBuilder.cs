using Rainbow.GarbageCollection;
using Rainbow.GarbageCollection.GCTypes;

namespace Rainbow.SmartAllocation.Types;

public class StructBuilder
{
    public StructInfo info { get; set; } = StructInfo.Empty;
    public AllocationEngine engine { get; set; }

    public StructBuilder(ref AllocationEngine eng, StructInfo info)
    {
        this.engine = eng;
        this.info = info;
    }

    /*
    public Block<byte> Allocate()
    {
        int totalsize = 0;
        for(int i = 0; i < info.variableInfo.Count; i++)
        {
            totalsize = totalsize + info.variableInfo[i].size;
        }

        Block<byte> reference = engine.AllocSimple(totalsize);
        int blockpos = 0;
    }*/

    public void WriteType() { }
}

public class StructInfo
{
    public List<VariableInfo> variableInfo { get; set; } = new();

    public StructInfo(List<VariableInfo> info)
    {
        this.variableInfo = info;
    }

    public static StructInfo Empty => new StructInfo(new());
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