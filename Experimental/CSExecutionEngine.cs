using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis;
using System.Reflection;
using System.Text;

namespace Rainbow.Experimental;

public class CSExecutionEngine
{
    public string nspace { get; set; }
    public string[] imports { get; set; }

    public CSExecutionEngine()
    {
        nspace = "";
        imports = new string[0];
    }

    public void ExecuteVoid(string code, CSExecutionInfo info)
    {
        SyntaxTree ast = CSharpSyntaxTree.ParseText(code, new CSharpParseOptions(LanguageVersion.CSharp9));
        
        List<MetadataReference> refs = new()
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location)
        };

        Assembly[] asm = AppDomain.CurrentDomain.GetAssemblies();

        foreach(Assembly a in asm)
        {
            refs.Add(MetadataReference.CreateFromFile(a.Location));
        }

        CSharpCompilation compilation = CSharpCompilation.Create(
            "RuntimeAssembly." + Guid.NewGuid().ToString(),
            syntaxTrees: new[] { ast },
            references: refs,
            options: new CSharpCompilationOptions(
                OutputKind.DynamicallyLinkedLibrary    
            )
        );

        using(MemoryStream stream = new MemoryStream())
        {
            EmitResult res = compilation.Emit(stream);

            if(res.Success)
            {
                stream.Seek(0, SeekOrigin.Begin);
                Assembly assembly = AppDomain.CurrentDomain.Load(stream.ToArray());
                
                Type tp = assembly.GetType(info.methodPath) ?? typeof(void);
                object obj = Activator.CreateInstance(tp) ?? 0;

                tp.InvokeMember(info.methodName, BindingFlags.Default | BindingFlags.InvokeMethod, null, obj, info.args);
            } else {
                Console.WriteLine("Failed!");

                StringBuilder bldr = new();
                foreach(Diagnostic d in res.Diagnostics)
                {
                    bldr.AppendLine(d.GetMessage());
                }

                throw new Exception(bldr.ToString());
            }
        }
    }
}

public static class CSExecutionInjector
{
    public static void InjectImport(string import, ref CSExecutionEngine eng)
    {
        eng.imports = new string[eng.imports.Length + 1];
        eng.imports[eng.imports.Length - 1] = import;
    }

    public static void InjectNamespace(string nspace, ref CSExecutionEngine eng)
    {
        eng.nspace = nspace;
    }
}

public record CSExecutionInfo(string methodPath, string methodName, params object[]? args);