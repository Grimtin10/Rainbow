using Rainbow.GarbageCollection.GCTypes;

namespace Rainbow.Types;

public struct Struct
{
    public Block<byte> ptr { get; set; }
    public Dictionary<string, KeyValuePair<Type, Block<byte>>> accessor { get; set; } = new();

    public Struct() { }

    #region Runtime Allowed Funcs 
    #endregion
}