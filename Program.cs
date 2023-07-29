using Rainbow.Execution;
using Rainbow.GarbageCollection;
using Rainbow.GarbageCollection.GCTypes;
using Rainbow.SmartAllocation;

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

        for(int i = 0; i < 10; i++)
        {
            Block<byte> b = gc.Alloc(2048);
        }

        Block<byte> ptr = gc.Alloc(40);
        gc.stack.CopyTo(ref ptr);
        //gc.stack.CopyTo(ref b);

        gc.canCollect = true;

        watch.Start();
        gc.Collect();
        watch.Stop();

        Console.WriteLine("Elapsed Time: " + watch.Elapsed.TotalMilliseconds);

        gc.stack.Pop();
        gc.Collect();
        
        //Block<byte> x = gc.Alloc(1023);
    }
}