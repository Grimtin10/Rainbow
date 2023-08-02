using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis;
using System.Reflection;

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

    public void ExecuteVoid(string code)
    {
        SyntaxTree ast = CSharpSyntaxTree.ParseText(code);
        
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
            "./",
            syntaxTrees: new[] { ast },
            references: refs,
            options: new CSharpCompilationOptions(
                OutputKind.DynamicallyLinkedLibrary    
            )
        );

        //TODO : Add actual execution of the C#!!!
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