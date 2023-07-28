using Rainbow.Execution;
using Rainbow.GarbageCollection;
using Rainbow.GarbageCollection.GCTypes;

namespace Rainbow;

class Program {
    public unsafe static void Main(params string[] args) {
        if(args.Contains("nightly")) {
            Nightly();
        } else {
            if(args.Length < 1) {
                throw new ArgumentException("Not enough arguments provided! Num args: " + args.Length);
            }

            Executor executor = new Executor(args[0]);
            executor.Execute();
        }
    }

    public unsafe static void Nightly()
    {
        System.Diagnostics.Stopwatch watch = new();
        GarbageCollector gc = new();

        Block<byte> test1 = gc.Alloc(5);
        gc.stack[0].PushRef(ref test1);

        Block<byte> test2 = gc.Alloc(5);
        gc.stack[0].PushRef(ref test2);

        Block<byte> ptr1 = gc.Alloc(sizeof(Block<Block<byte>>));
        ptr1.isref = true;
        Block<byte> ptr2 = gc.Alloc(sizeof(int));

        ptr1.SetPos(0, ptr2[0]);

        //Block<Block<byte>> pptr1 = ptr1.MarshalBlock<Block<Block<byte>>>()[0];
        //pptr1.SetPos(0, gc.Alloc(sizeof(int)));

        Console.WriteLine(ptr1.isref);
        gc.stack[0].PushRef(ref ptr1);

        gc.Collect(); //no collections

        gc.stack[0].Pop();

        Console.WriteLine("=========================================");

        gc.Collect(); //1 collection

        /*
        watch.Start();
        gc.PerformSynchronousCollection();
        watch.Stop();*/

        //Console.WriteLine("Collection Elapsed: " + watch.Elapsed.TotalMilliseconds);
        //Console.WriteLine(gc.refs.Count);
        //Console.WriteLine(gc.HasRef(r._ref));
    }
}