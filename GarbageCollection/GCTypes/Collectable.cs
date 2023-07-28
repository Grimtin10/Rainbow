using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Rainbow.GarbageCollection.GCTypes;

public interface ICollectable
{
    public ICollectable? _ref { get; set; }
    public Block<byte> ptr { get; set; }
    public bool ContainsRefs { get; set; }
    public bool marked { get; set; }
}


public unsafe struct Collectable
{
    public Block<byte> ptr { get; set; }
    public bool ContainsRefs { get; set; } = true;

    public Collectable(Block<byte> mem)
    {
        ptr = mem;
    }
}

public unsafe struct StackBlock
{
    public Block<byte> mem { get; set; }
    public List<KeyValuePair<bool, Block<byte>>> ptrs { get; set; } = new();
    public int pos = 0;
    public int currentBlockSize = 0;

    public StackBlock(Block<byte> ptr)
    {
        mem = ptr;
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

    public Block<byte> PushRef(ref Collectable ptr)
    { 
        Block<byte> bt = StackAlloc<byte>(ptr.ptr.isref, sizeof(Collectable), true);
        Block<Collectable> collptr = bt.MarshalBlock<Collectable>();

        collptr.SetPos(0, ptr);

        currentBlockSize = bt.length;

        return bt;
    }

    public Block<T> StackAlloc<T>(bool blockIsRef, int size = 1, bool isref = false) where T: unmanaged
    {
        byte *start = &mem._ref[pos];
        
        if((mem.length - pos) + (sizeof(T) * size) < mem.length)
        {
            throw new StackOverflowException("Allocated memory is larger than the stack!");
        }

        Block<byte> obj = new Block<byte>(start, sizeof(T) * size);
        pos = pos + obj.length;
        obj.isref = blockIsRef;

        Console.WriteLine("stackalloc: " + obj.ToString());
        ptrs.Add(new(isref, obj));

        return obj.MarshalBlock<T>();
    }

    public void Pop()
    {
        for(int i = 0; i < currentBlockSize; i++)
        {
            ptrs[ptrs.Count - 1].Value.SetPos(i, 0);
        }

        ptrs.RemoveAt(ptrs.Count - 1);
    }
}