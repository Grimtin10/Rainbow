using Rainbow.GarbageCollection.GCTypes;
using Rainbow.Marshalling;
using System.Text;

namespace Rainbow.Runtime;

#region Runtime Allowed Funcs

public static class IO
{
    public unsafe static Block<byte> LoadFile(string path)
    {
        Span<byte> buffer = File.ReadAllBytes(path);
        Block<byte> buf = Unsafe.UnsafeAlloc(buffer.Length);

        buffer.CopyTo(new(buf._ref, buf.length));

        return buf;
    }
}

#endregion