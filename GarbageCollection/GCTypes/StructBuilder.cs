using Rainbow.GarbageCollection;

namespace Rainbow.GarbageCollection.GCTypes;

public class StructBuilder
{
    public GarbageCollector gc { get; set; }
    public StructInfo info { get; set; }

    public StructBuilder(ref GarbageCollector gc, StructInfo info)
    {
        this.gc = gc;
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