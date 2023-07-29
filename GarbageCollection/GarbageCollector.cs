using Rainbow.GarbageCollection.GCTypes;
using System.Runtime.InteropServices;

namespace Rainbow.GarbageCollection;

public unsafe class GarbageCollector
{
    public StackBlock stack { get; set; } = new(); //TODO : FreeStack(int index); FreeAllStacks();
    public List<Block<byte>> refs { get; set; } = new();
    public int totalCollected = 0;
    public int collectionThreshold { get; set; }
    public bool canCollect { get; set; }

    public GarbageCollector(int size = 4096 * 1024, int threshold = 1024)
    {
        AllocateStack(size);
        this.totalCollected = 0;
        this.collectionThreshold = threshold;
    }

    public void AllocateStack(int size = 1024)
    {
        Block<byte> stackmem = Alloc(size * sizeof(byte));
        //Console.WriteLine((int)stackmem._ref);

        StackBlock st = new StackBlock(stackmem);
        stack = st;
    }

    public void FreeRootStack()
    {
        Console.WriteLine("Freeing root stack.");
        foreach(Block<byte> r in refs)
        {
            if(r._ref == stack.mem._ref)
            {
                ForceFree<byte>(r._ref);
            }
        }
    }

    public Block<byte> Alloc(int size, bool alloccollect = true)
    {
        if(collectionThreshold > 1024) {
            collectionThreshold -= collectionThreshold >> 2;
        }

        if(collectionThreshold < 1024) {
            collectionThreshold = 1024;
        }

        if(size > collectionThreshold)
        {
            collectionThreshold = size + (size >> 1);
        }

        IntPtr iptr = Marshal.AllocHGlobal(size);
        byte *ptr = (byte *)iptr.ToPointer();

        Block<byte> mem = new Block<byte>(ptr, size);
        totalCollected = totalCollected + size;

        refs.Add(mem);
        
        if(totalCollected >= collectionThreshold && alloccollect)
        {
            this.Collect();
        }

        canCollect = false;

        return mem;
    }

    public Block<T> AllocUntrackedGeneric<T>(int size) where T: unmanaged
    {
        IntPtr iptr = Marshal.AllocHGlobal(size);
        T *ptr = (T *)iptr.ToPointer();

        Block<T> mem = new Block<T>(ptr, size);
        return mem;
    }

    public void ForceFree<T>(T *ptr) where T: unmanaged
    {
        try
        {
            Marshal.FreeHGlobal(new(ptr));
        } catch(Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    public void Collect()
    {
        if(canCollect)
        {
            List<Block<byte>> reachable = FindReachable();
            List<int> freedAddrs = new();
            List<Block<byte>> newrefs = new();

            //Console.WriteLine("lengths: " + reachable.Count + " " + refs.Count);

            for(int i = 0; i < refs.Count; i++)
            {
                //Console.WriteLine("ref: " + refs[i].ToString());
                if(Contains(reachable, refs[i]))
                {
                    newrefs.Add(refs[i]);
                    Console.WriteLine("Not Freeing : " + (int)refs[i]._ref);
                } else if(!freedAddrs.Contains((int)refs[i]._ref)) {
                    Console.WriteLine("Freeing : " + (int)refs[i]._ref);
                    freedAddrs.Add((int)refs[i]._ref);
                    ForceFree<byte>(refs[i]!);
                }
            }

            refs = newrefs;
        }

        if(collectionThreshold > 1024) {
            collectionThreshold -= collectionThreshold >> 1;
        }

        if(collectionThreshold < 1024) {
            collectionThreshold = 1024;
        }
    }

    public List<Block<byte>> FindReachable()
    {
        List<Block<byte>> ret = new();

        ret.Add(stack.mem);
        for (int i = 0; i < stack.ptrs.Count; i++)
        {
            if(stack.ptrs[i].Value.isref) {
                //Console.WriteLine("was ref, searching deeper");
                ret.AddRange(SearchCollectable(stack.ptrs[i].Value));
            } else {
                //Console.WriteLine("wasnt ref");
                ret.Add(stack.ptrs[i].Value);
            }
        }

        return ret;
    }

    
    public List<Block<byte>> SearchCollectable(Block<byte> collectable)
    {
        List<Block<byte>> ret = new();

        //ret.Add(collectable);

        //Console.WriteLine(collectable.ToString());

        if(collectable.isref)
        {
            //Console.WriteLine("was ref, searching deeper");

            Block<Block<byte>> ptrtptr = collectable.MarshalBlock<Block<byte>>();
            ret.AddRange(SearchCollectable(ptrtptr[0]));
        } else {
            //Console.WriteLine("wasnt ref");
            ret.Add(collectable);
        }

        //Console.WriteLine(ret.Count);

        return ret;
    }

    private bool Contains(List<Block<byte>> blocks, Block<byte> block) {
        foreach(Block<byte> b in blocks) {
            //Console.WriteLine("b: " + b.ToString());
            if(block.Equals(b)) return true;
        }
        return false;
    }

    public void PushStack(Block<byte> ptr, bool isref = false)
    { 
        if(isref)
        {
            refs = refs.Where(x => !x.Equals(ptr)).ToList();
            stack.PushRef(ref ptr);

            //Console.WriteLine("Freeing :: " + (int)ptr._ref + " Realloced to: " + (int)stack.ptrs[stack.ptrs.Count - 1].Value._ref);
            ForceFree<byte>(ptr);
        } else
        {
            stack.Push(ref ptr);
        }
    }
}