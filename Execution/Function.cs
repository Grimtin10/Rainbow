namespace Rainbow.Execution {
    public class Function {
        public Type type { get; set; }
        public string name { get; set; }
        public List<Instance> arguments { get; set; }

        public int execTimer { get; set; } // TODO(grimtin10): implement :)

        public Scope scope { get; set; }

        public Function(Type type, string name, List<Instance> arguments, Scope scope) { 
            this.type = type;
            this.name = name;
            this.arguments = arguments;
            this.scope = scope;
        }

        public void Execute() {
            scope.Execute();
        }
    }
}
