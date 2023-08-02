using Rainbow.Execution;
using Rainbow.GarbageCollection;
using Rainbow.GarbageCollection.GCTypes;
using Rainbow.SmartAllocation;
using Rainbow.Types;
using Rainbow.SmartAllocation.Types;
using Rainbow.Marshalling;

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
        GarbageCollector gc = new();
        AllocationEngine eng = new(ref gc);

        ClassInfo personInfo = new();
        personInfo.Add("age", new VariableInfo(typeof(int), false, sizeof(int)));
        personInfo.Add("first", new VariableInfo(typeof(char), false, sizeof(char)));

        ClassBuilder bldr = new ClassBuilder(ref eng, personInfo);
        
        Class c = bldr.WriteClass();

        gc.PopStack();

        Block<byte> x = eng.AllocateAndFinalize(2);
        Block<byte> y = eng.AllocateAndFinalize(2);

        gc.PopStack();
        gc.PopStack();

        Block<byte> z = eng.AllocateAndFinalize(2);
        Block<byte> a = eng.AllocateAndFinalize(2);
        Block<byte> b = eng.AllocateAndFinalize(2);

        gc.PopStack();
        gc.PopStack();

        Console.WriteLine("trying to make GC collect");
        Block<byte> d = eng.AllocateAndFinalize(2);
    }
}