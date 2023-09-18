﻿using Rainbow.Compilation.Assembler;
using Rainbow.Execution;
using Newtonsoft.Json;

namespace Rainbow;

class Program
{
    public unsafe static void Main(params string[] args)
    {
        //future arg handler
        for (int i = 0; i < args.Length; i++) {
            if (args[i] == "build") {
                Build(args[i + 1]);
                return;
            }

            if (args[i] == "nightly") {
                Nightly(args);
                return;
            }
        }

        if (args.Length < 1) {
            throw new ArgumentException("Not enough arguments provided! Num args: " + args.Length);
        }

        byte[] bytes = Assembler.Assemble(args[0]);
        File.WriteAllBytes($"C:/Users/{System.Environment.UserName}/source/repos/Rainbow/Rainbow/Examples/Sub2/sub2.rbb", bytes);

        Executor executor = new Executor(bytes);
        executor.Execute();
    }

    public unsafe static void Nightly(string[] args)
    {
        RuntimeConfig? conf;
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "--conf")
            {
                conf = JsonConvert.DeserializeObject<RuntimeConfig>(File.ReadAllText(args[i] + 1));
                //RunConfigSetup();
            }

            if (args[i] == "--write-conf")
            {
                List<LinkerArgs> linkerArgs = new();
                linkerArgs.Add(new LinkerArgs("MyCoolLib.dll"));
                linkerArgs.Add(new LinkerArgs("MyAwesomeLib.so"));
                linkerArgs.Add(new LinkerArgs("MyFantasticLib.rbb", true));

                File.WriteAllText("./exampleconfig.json", JsonConvert.SerializeObject(new RuntimeConfig(true, new AssemblerArgs(linkerArgs))));
            }
        }
    }

    public static void Build(string path)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Rains compiler is not yet finished! Aimed for completion around Q1 2024!");
        Console.ForegroundColor = ConsoleColor.White;
    }
}