#pragma warning disable CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
global using half = System.Half;
global using conv = Rainbow.Marshalling.Converter;
global using add = Rainbow.Execution.Math.Addition;
global using sub = Rainbow.Execution.Math.Subtraction;
#pragma warning restore CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.

using Rainbow.GarbageCollection;
using Rainbow.GarbageCollection.GCTypes;
using Rainbow.SmartAllocation;

namespace Rainbow.Handlers {
    public class Globals {
        public static GarbageCollector GarbageCollector = new();
        public static Block<byte> filePtr { get; set; }
        public static AllocationEngine ae = new(ref GarbageCollector);
    }
}
