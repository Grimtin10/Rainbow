using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Rainbow.GarbageCollection.GCTypes;

public unsafe struct StackBlock
{
    public Block<byte> mem { get; set; }
    public List<KeyValuePair<bool, Block<byte>>> ptrs { get; set; } = new();
    public static int pos { get; set; }
    public int currentBlockSize = 0;
    public int getpos => pos;

    public StackBlock(Block<byte> ptr)
    {
        mem = ptr;
        pos = 0;
    }

    public Block<byte> Push(ref Block<byte> ptr)
    { 
        Block<byte> bt = StackAlloc<byte>(ptr.isref, ptr.length, false);

        for(int i = 0; i < bt.length; i++)
        {
            bt.SetPos(i, ptr[i]);
        }

        bt.isref = ptr.isref;

        currentBlockSize = ptr.length;

        return bt;
    }

    public Block<byte> PushRef(ref Block<byte> ptr)
    { 
        Block<byte> bt = StackAlloc<byte>(ptr.isref, ptr.length, true);

        for(int i = 0; i < bt.length; i++)
        {
            bt.SetPos(i, ptr[i]);
        }

        bt.isref = ptr.isref;

        currentBlockSize = ptr.length;

        Console.WriteLine(bt.isref + " " + ptr.isref);
        Console.WriteLine(bt.ToString() + " " + ptr.ToString());

        return bt;
    }

    public Block<T> StackAlloc<T>(bool blockIsRef, int size = 1, bool isref = false) where T: unmanaged
    {
        byte *start = (byte *)((long)mem._ref + pos);

        if(pos + (sizeof(T) * size) >= mem.length)
        {
            throw new StackOverflowException("Allocated memory is larger than the stack!");
        }

        Console.WriteLine(sizeof(T) * size);
        Block<byte> obj = new Block<byte>(start, sizeof(T) * size);
        pos = pos + (size * sizeof(T)); Console.WriteLine("New pos is: " + pos);
        obj.isref = blockIsRef;

        Console.WriteLine("Allocated on stack: " + obj.ToString());
        ptrs.Add(new(isref, obj));

        return obj.MarshalBlock<T>();
    }

    public void Pop()
    {
        for(int i = 0; i < currentBlockSize; i++)
        {
            ptrs[ptrs.Count - 1].Value.SetPos(i, 0);
        }

        pos = pos - (ptrs[ptrs.Count - 1].Value.length * sizeof(byte));

        ptrs.RemoveAt(ptrs.Count - 1);
    }
}