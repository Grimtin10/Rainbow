using Rainbow.GarbageCollection;

namespace Rainbow.SmartAllocation.Types;

public class StructBuilder
{
    public StructInfo info { get; set; } = StructInfo.Empty;

    public StructBuilder(StructInfo info)
    {
        this.info = info;
    }
}

public class StructInfo
{
    public int totalSize { get; set; }
    public List<VariableInfo> variableInfo { get; set; } = new();

    public StructInfo(List<VariableInfo> info, int size = 0)
    {
        this.totalSize = size;
        this.variableInfo = info;
    }

    public static StructInfo Empty => new StructInfo(new(), 0);
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