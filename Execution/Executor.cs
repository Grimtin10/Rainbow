using System.Diagnostics;

namespace Rainbow.Execution {
    public class Executor {
        public byte[] program { get; set; }

        public Scope globalScope { get; set; }

        public Executor(string path) {
            program = File.ReadAllBytes(path);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            globalScope = new Scope(program, null);
            globalScope.isGlobal = true;
            stopwatch.Stop();

            Console.WriteLine("Parsing program took " + stopwatch.ElapsedMilliseconds + "ms");
        }

        public void Execute() {
            globalScope.Execute();
        }
    }
}
