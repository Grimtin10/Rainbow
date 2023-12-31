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
        Block<byte> bt = StackAlloc<byte>(ptr.isref, ptr.length);

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
        Block<byte> bt = StackAlloc<byte>(ptr.isref, ptr.length);

        for(int i = 0; i < bt.length; i++)
        {
            bt.SetPos(i, ptr[i]);
        }

        bt.isref = ptr.isref;

        currentBlockSize = ptr.length;

        //Console.WriteLine(bt.isref + " " + ptr.isref);
        //Console.WriteLine(bt.ToString() + " " + ptr.ToString());

        return bt;
    }

    public Block<T> StackAlloc<T>(bool blockIsRef, int size = 1) where T: unmanaged
    {
        byte *start = (byte *)((long)mem._ref + pos);

        if(pos + (sizeof(T) * size) >= mem.length)
        {
            throw new StackOverflowException("Allocated memory is larger than the stack!");
        }

        Block<byte> obj = new Block<byte>(start, sizeof(T) * size);
        pos = pos + (size * sizeof(T));
        obj.isref = blockIsRef;

        //Console.WriteLine("Allocated on stack: " + obj.ToString());
        ptrs.Add(new(blockIsRef, obj));

        return obj.MarshalBlock<T>();
    }

    public Block<byte> Pop()
    {
        for(int i = 0; i < currentBlockSize; i++)
        {
            ptrs[ptrs.Count - 1].Value.SetPos(i, 0);
        }

        pos = pos - (ptrs[ptrs.Count - 1].Value.length * sizeof(byte));

        Block<byte> ret = ptrs[^1].Value;
        ptrs.RemoveAt(ptrs.Count - 1);

        Console.WriteLine("Popping value off of stack.");

        return ret;
    }

    public Block<byte> CopyTo(ref Block<byte> tocopy)
    {
        Block<Block<byte>> alloc = StackAlloc<Block<byte>>(true, sizeof(Block<byte>));
        alloc.SetPos(0, tocopy);

        return alloc[0];
    }
}