using Rainbow.Execution;
using Type = Rainbow.Execution.Type;

namespace Rainbow.Compilation.Assembler {
    internal class FuncDef {
        public string name;
        public List<Type> args;

        public FuncDef(string name) { 
            this.name = name;
            this.args = new();
        }

        public void AddArg(Type t) {
            args.Add(t);
        }

        public override string ToString() {
            string argString = "";
            foreach (Type t in args) { 
                argString += t.ToString();
            }

            return name + " : " + argString;
        }
    }
}
