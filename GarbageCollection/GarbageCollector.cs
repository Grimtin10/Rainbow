using Rainbow.GarbageCollection.GCTypes;
using System;
using System.Runtime.InteropServices;

namespace Rainbow.GarbageCollection;

public unsafe class GarbageCollector
{
    public StackBlock stack { get; set; } = new(); //TODO : FreeStack(int index); FreeAllStacks();
    public List<Block<byte>> refs { get; set; } = new();

    public GarbageCollector(int size = 4096 * 1024)
    {
        AllocateStack(size);
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
        foreach(Block<byte> r in refs)
        {
            if(r._ref == stack.mem._ref)
            {
                ForceFree<byte>(r._ref);
            }
        }
    }

    public Block<byte> Alloc(int size)
    {
        IntPtr iptr = Marshal.AllocHGlobal(size);
        byte *ptr = (byte *)iptr.ToPointer();

        Block<byte> mem = new Block<byte>(ptr, size);
        
        refs.Add(mem);
        
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
        List<Block<byte>> reachable = FindReachable();
        List<int> freedAddrs = new();

        //Console.WriteLine(reachable.Count + " " + refs.Count);

        for(int i = 0; i < refs.Count; i++)
        {
            //Console.WriteLine(refs[i].ToString());
            if(Contains(reachable, refs[i]))
            {
                Console.WriteLine("Not Freeing : " + (int)refs[i]._ref);
            } else if(!freedAddrs.Contains((int)refs[i]._ref) && !reachable.Contains(refs[i])) {
                Console.WriteLine("Freeing : " + (int)refs[i]._ref);
                freedAddrs.Add((int)refs[i]._ref);
                ForceFree<byte>(refs[i]);
            }
        }

        refs = refs.Where(x => reachable.Contains(x)).ToList();
    }

    public List<Block<byte>> FindReachable()
    {
        List<Block<byte>> ret = new();

        ret.Add(stack.mem);
        for (int i = 0; i < stack.ptrs.Count; i++)
        {
            if (!stack.ptrs[i].Key)
            {
                ret.Add(stack.ptrs[i].Value);
            }
            else if (stack.ptrs[i].Value.isref)
            {
                ret.AddRange(SearchCollectable(stack.ptrs[i].Value));
            }
        }

        return ret;
    }

    
    public List<Block<byte>> SearchCollectable(Block<byte> collectable)
    {
        List<Block<byte>> ret = new();

        //ret.Add(collectable);

        Console.WriteLine(collectable.ToString());

        if(collectable.isref)
        {
            Console.WriteLine("was ref, searching deeper");

            Block<Block<byte>> ptrtptr = collectable.MarshalBlock<Block<byte>>();
            ret.AddRange(SearchCollectable(ptrtptr[0]));
        } else {
            Console.WriteLine("wasnt ref");
            ret.Add(collectable);
        }

        Console.WriteLine(ret.Count);

        return ret;
    }

    private bool Contains(List<Block<byte>> blocks, Block<byte> block) {
        foreach(Block<byte> b in blocks) { 
            if(b.Equals(block)) return true;
        }
        return false;
    }

    public void PushStack(Block<byte> ptr, bool isref = false)
    { 
        if(isref)
        {
            refs = refs.Where(x => !x.Equals(ptr)).ToList();
            stack.PushRef(ref ptr);

            Console.WriteLine("Freeing :: " + (int)ptr._ref + " Realloced to: " + (int)stack.ptrs[stack.ptrs.Count - 1].Value._ref);
            ForceFree<byte>(ptr);
        } else
        {
            stack.Push(ref ptr);
        }
    }
}