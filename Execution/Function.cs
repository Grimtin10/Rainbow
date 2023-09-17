namespace Rainbow.Execution {
    public class Function {
        public Type type { get; set; }
        public string name { get; set; }
        public List<Type[]> arguments { get; set; }

        // what is this
        public int execTimer { get; set; } // TODO(grimtin10): implement :)

        public Scope scope { get; set; }

        public Function(Type type, string name, List<Type[]> arguments, Scope scope) { 
            this.type = type;
            this.name = name;
            this.arguments = arguments;
            this.scope = scope;
        }
        
        // TODO: argument support
        public void Execute() {
            Console.WriteLine("executing " + name);
            scope.Execute();
        }
    }
}
