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

        AllocationEngine eng = new(ref gc, true);
        
        Block<byte> x = eng.AllocSimple(1023);
        gc.stack.CopyTo(ref x);

        Block<byte> y = eng.AllocSimple(2);

        Thread.Sleep(2000);
        eng.Dispose();
        //Block<byte> x = gc.Alloc(1023);
    }
}