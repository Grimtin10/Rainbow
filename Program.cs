using Rainbow.Execution;
using Rainbow.GarbageCollection;
using Rainbow.GarbageCollection.GCTypes;
using Rainbow.SmartAllocation;
using Rainbow.Types;
using Rainbow.SmartAllocation.Types;

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
        AllocationEngine eng = new(ref gc);

        ClassInfo inf = new();
        inf.Add("prop1", new VariableInfo(typeof(int), false, sizeof(int)));
        inf.Add("prop2", new VariableInfo(typeof(byte), false, sizeof(byte) * 7));

        ClassBuilder bldr = new(ref eng, inf);
        Class s = bldr.WriteClass();

        s.accessor["prop1"].Value.MarshalBlock<int>().SetPos(0, 5);
        s.accessor["prop2"].Value.MarshalBlock<int>().SetPos(0, 0xFF);

        Console.WriteLine(s.accessor["prop1"].Value.MarshalBlock<int>()[0]);
        Console.WriteLine(s.accessor["prop2"].Value.MarshalBlock<int>()[0]);
        //s.accessor["prop1"].Value.MarshalBlock<int>().SetPos(1, 5);
    }
}