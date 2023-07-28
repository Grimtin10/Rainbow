using Rainbow.GarbageCollection.GCTypes;

namespace Rainbow.Execution {
    // TODO: reference counting
    // TODO: scope (public, private, protected)
    public unsafe class Instance {
        public string name { get; set; }
        public Type type { get; set; }
        public Block<byte> data { get; set; }
        public int referenceCount { get; set; }

        public Instance(string name, Type type, Block<byte> data) {
            Console.WriteLine("Creating instance with addr " + (int)data._ref);
            this.name = name;
            this.type = type;
            this.data = data;
        }
    }
}
