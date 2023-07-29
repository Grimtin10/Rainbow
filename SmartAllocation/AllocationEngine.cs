using Rainbow.SmartAllocation.Experimental;
using Rainbow.GarbageCollection;
using Rainbow.GarbageCollection.GCTypes;

using System.Runtime.InteropServices;

namespace Rainbow.SmartAllocation;

public class AllocationEngine
{
    public AllocationQueue queue { get; set; } = new();
    public bool runThreadedCollection { get; set; }
    public GarbageCollector gc { get; set; }
    public Thread? gcthread { get; set; }

    public AllocationEngine(ref GarbageCollector gc, bool threadedcollection = false)
    {
        this.gc = gc;
        runThreadedCollection = threadedcollection;

        if(threadedcollection)
        {
            RunGarbageCollection();
        }
    }

    public unsafe Block<byte> AllocSimple(int size)
    {
        if(runThreadedCollection)
        {
            IntPtr iptr = Marshal.AllocHGlobal(size);
            byte *ptr = (byte *)iptr.ToPointer();

            Block<byte> ret = new Block<byte>(ptr, size);
            gc.totalCollected = gc.totalCollected + size;
            
            lock (queue) lock(queue.allocs) 
            {
                queue.Add(ret);
            }

            return ret;
        }

        return gc.Alloc(size);
    }

    public Block<byte> AllocateAndFinalize(int size)
    {
        bool cancol = gc.canCollect;

        Block<byte> alloced = gc.Alloc(size);
        Block<byte> ret = gc.stack.CopyTo(ref alloced);
        
        if(cancol)
        {
            gc.canCollect = true;
        }

        return ret;
    }

    public void RunGarbageCollection()
    {
        Thread thread = new(new ThreadStart(GarbageCollectionThread));
        thread.Start();

        gcthread = thread;
    }

    public void GarbageCollectionThread()
    {
        while(runThreadedCollection)
        {
            if(gc.totalCollected >= gc.collectionThreshold)
            {
                gc.Collect();
            }

            if(queue.allocs.Count > 0)
            {
                foreach(Block<byte> inf in queue.allocs)
                {
                    gc.refs.Add(inf);
                }

                lock (queue.allocs) 
                {
                    queue.allocs = new();
                }
            }

            Thread.Sleep(1000);
        }
    }

    public void Dispose()
    {
        runThreadedCollection = false;
    }

    public void RuntimeSuspendGC()
    {
        gc.canCollect = false;
    }

    public void RuntimeEnableGC()
    {
        gc.canCollect = true;
    }
}