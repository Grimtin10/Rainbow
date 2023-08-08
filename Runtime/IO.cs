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

//this class can also be used to operate on any Block<byte>... I may update it later
public class FileOperator
{
    public Block<byte> ptr { get; set; }
    public int pos { get; set; } = 0;

    public FileOperator(Block<byte> pointer)
    {
        ptr = pointer;
    }

    public char ReadNext()
    {
        char ret = (char)ptr[pos + 1];
        pos = pos + 1;

        return ret;
    }

    public string Read()
    {
        StringBuilder builder = new StringBuilder();

        foreach(byte b in ptr)
        {
            builder.Append((char)b);
        }

        return builder.ToString();
    }

    public unsafe MemoryStream ToStream()
    {
        MemoryStream stream = new MemoryStream();
        Span<byte> p = new(ptr._ref, ptr.length);

        stream.Write(p);

        return stream;
    }

    public unsafe void Dispose()
    {
        Unsafe.Free<byte>(ptr);
    }
}

#endregion