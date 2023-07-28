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
            stopwatch.Stop();

            Console.WriteLine("Parsing program took " + stopwatch.ElapsedMilliseconds + "ms");
        }

        public void Execute() {
            globalScope.Execute();

            Function? mainFunc;
            if(!globalScope.functions.TryGetValue("main", out mainFunc)) {
                throw new Exception("No main function found! Create a main function to run the program.");
            }

            mainFunc.Execute();
        }
    }
}
