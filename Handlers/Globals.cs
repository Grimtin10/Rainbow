global using half = System.Half;
global using conv = Rainbow.Marshalling.Marshalling;

using Rainbow.GarbageCollection;

namespace Rainbow.Handlers {
    public class Globals {
        public static GarbageCollector GarbageCollector = new();
    }
}
