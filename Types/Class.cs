using Rainbow.GarbageCollection.GCTypes;

namespace Rainbow.Types;

public class Class
{
    public Block<byte> ptr { get; set; }
    public Dictionary<string, KeyValuePair<Type, Block<byte>>> accessor { get; set; } = new();

    #region Runtime Allowed Funcs 
    #endregion
}