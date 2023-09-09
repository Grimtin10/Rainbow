using Rainbow.GarbageCollection.GCTypes;

namespace Rainbow.Types;

public struct Class
{
    public Block<byte> ptr { get; set; }
    public Dictionary<string, KeyValuePair<Type, Block<byte>>> accessor { get; set; } = new();

    public Class() { }

    #region Runtime Allowed Funcs 
    #endregion
}