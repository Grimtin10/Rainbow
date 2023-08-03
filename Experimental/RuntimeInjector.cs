using System.Reflection;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis;
using System.Text;

namespace Rainbow.Experimental;

public static class RuntimeInjector
{
    public static void InjectPartial(string code)
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