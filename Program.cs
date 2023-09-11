using Rainbow.Compilation.Assembler;
using Rainbow.Execution;

namespace Rainbow;

class Program {
    public unsafe static void Main(params string[] args) {
        if(args.Contains("nightly")) {
            Nightly();
        } else {
            if(args.Length < 1) {
                throw new ArgumentException("Not enough arguments provided! Num args: " + args.Length);
            }

            byte[] bytes = Assembler.Assemble(args[0]);
            File.WriteAllBytes("C:\\Users\\Grim\\source\\repos\\Rainbow\\Rainbow\\Examples\\Sub2\\sub2.rbb", bytes);

            Executor executor = new Executor(bytes);
            executor.Execute();
        }
    }

    public unsafe static void Nightly()
    {
        //string code = @"
        //    using System;
        //    using System.IO;
        //    using System.Collections.Generic;
            
        //    namespace Rainbow.GarbageCollection
        //    {
        //        public static class GCExtensions
        //        {
        //            public static void Clear(this GarbageCollector gc)
        //            {
        //                gc.refs = new();
        //            }
        //        }
        //    }
        //";

        //RuntimeInjector.InjectPartial(code);

        //typeof(GarbageCollector).InvokeMember(
        //    "Clear",
        //    BindingFlags.Default | BindingFlags.InvokeMethod,
        //    null,
        //    Globals.GarbageCollector,
        //    null
        //);
    }
}