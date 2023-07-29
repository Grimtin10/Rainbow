#pragma warning disable CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
global using half = System.Half;
global using conv = Rainbow.Marshalling.Marshalling;
global using add = Rainbow.Execution.Math.Addition;
global using sub = Rainbow.Execution.Math.Subtraction;
#pragma warning restore CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.

using Rainbow.GarbageCollection;

namespace Rainbow.Handlers {
    public class Globals {
        public static GarbageCollector GarbageCollector = new();
    }
}
