using Rainbow.Exceptions;
using Rainbow.Execution.Math;
using Rainbow.GarbageCollection;
using Rainbow.GarbageCollection.GCTypes;
using Rainbow.Handlers;
using Rainbow.Marshalling;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using static System.Formats.Asn1.AsnWriter;

// TODO: split this into multiple files
namespace Rainbow.Execution {
    public class Scope {
        public List<KeyValuePair<Instruction, byte[]>> instructions = new();
        public Dictionary<string, Instance> variables = new();

        public Dictionary<string, Function> functions = new();

        public List<Scope> scopes = new();
        public Scope? parentScope;

        public int stackStart;

        public bool isGlobal = false;

        // TODO: implement the scope length fix
        public Scope(byte[] bytes, Scope? parentScope) {
            this.parentScope = parentScope;

            bool inFunction = false;
            bool inArgs = false;

            string funcName = "";
            Type funcType = Type.undefined;
            List<Type[]> arguments = new();

            int scopeDepth = 0;
            List<byte> scopeBytes = new();

            for(int i = 0; i < bytes.Length; i++) {
                if(bytes[i] == 0xFF && scopeDepth == 0) {
                    inFunction = true;
                    inArgs = true;
                    funcName = "";
                    funcType = Type.undefined;
                    arguments = new();
                    continue;
                }

                if(bytes[i] == 0xFE) {
                    if(inArgs) {
                        throw new UnfinishedFuncException($"Arguments for function {funcName} never finish!");
                    }
                    scopeDepth++;
                    continue;
                }

                if(bytes[i] == 0xFD) {
                    scopeDepth--;
                    if(scopeDepth == 0) {
                        if(inFunction) {
                            functions.Add(funcName, new Function(funcType, funcName, arguments, new Scope(scopeBytes.ToArray(), this)));
                        } else {
                            scopes.Add(new Scope(scopeBytes.ToArray(), this));
                        }
                        scopeBytes.Clear();
                        inFunction = false;
                    }
                    continue;
                }

                if(bytes[i] == 0xFA) {
                    inArgs = false;
                }

                //Console.WriteLine(string.Format("{0:X2} ", bytes[i]));

                if(scopeDepth > 0) {
                    scopeBytes.Add(bytes[i]);
                } else {
                    if(inFunction) {
                        if(funcType == Type.undefined) {
                            funcType = (Type) bytes[i];
                        } else if(funcName == "") {
                            byte length = bytes[i];
                            char[] str = new char[length];
                            i++;

                            for(int j = 0; j < length && i < bytes.Length; j++, i++) {
                                str[j] = (char) bytes[i];
                            }

                            funcName = new string(str);
                            i--;
                        } else if(inArgs) {
                            int index = i;
                            while(bytes[index] == 0x0C) {
                                index++;
                            }

                            int count = index - i + 1;
                            Type[] type = new Type[count];
                            for(int j = 0; j < count; j++, i++) {
                                type[j] = (Type) bytes[i];
                            }

                            byte length = bytes[i];
                            char[] str = new char[length];
                            i++;

                            for(int j = 0; j < length && i < bytes.Length; j++, i++) {
                                str[j] = (char) bytes[i];
                            }

                            i--;
                        }
                    } else {
                        instructions.Add(ParseInstruction(bytes, ref i));
                    }
                }
            }

            foreach(Scope scope in scopes) {
                foreach(KeyValuePair<string, Function> func in functions) {
                    scope.functions.Add(func.Key, func.Value);
                }
                foreach(KeyValuePair<string, Instance> var in variables) {
                    scope.variables.Add(var.Key, var.Value);
                }
            }

            foreach(Function scope in functions.Values) {
                foreach(KeyValuePair<string, Function> func in functions) {
                    scope.scope.functions.Add(func.Key, func.Value);
                }
            }
        }

        public void Execute() {
            stackStart = Globals.GarbageCollector.stack.ptrs.Count;

            for(int i = 0; i < instructions.Count; i++) {
                KeyValuePair<Instruction, byte[]> instruction = instructions[i];

                // if i'm being honest this feels jank but i'm just working with what i have
                // (i wrote it)
                int ret = ExecInstruction(instruction);
                if(ret == -1) {
                    // we returned, stop execution
                    Cleanup();
                    return;
                } else if(ret >= 0) {
                    i = ret - 1;
                }
            }

            foreach(Scope scope in scopes) {
                scope.Execute();
            }

            if(isGlobal) {
                Function? mainFunc;
                if(!functions.TryGetValue("main", out mainFunc)) {
                    throw new Exception("No main function found! Create a main function to run the program.");
                }

                mainFunc.Execute();
            }

            Cleanup();
        }

        private void Cleanup() {
            Console.WriteLine("Cleaning up scope");

            // TODO: force enable GC
            while(Globals.GarbageCollector.stack.ptrs.Count > stackStart) {
                Globals.GarbageCollector.stack.Pop();
            }
            if(isGlobal) {
                Globals.GarbageCollector.FreeRootStack();
            }
            Globals.GarbageCollector.Collect();
        }

        private int ExecInstruction(KeyValuePair<Instruction, byte[]> instruction) {
            Instruction instr = instruction.Key;
            byte[] args = instruction.Value;

            int index = 0;

            switch((byte) instr) {
                case 0x01: {
                    Instance? arg1 = GetRef(args, ref index);
                    if(arg1 == null) {
                        throw new NullRefException();
                    }

                    Instance? arg2 = GetRef(args, ref index);
                    if(arg2 == null) {
                        throw new NullRefException();
                    }

                    Instance? var = GetRef(args, ref index);
                    if(var == null) {
                        throw new NullRefException();
                    }

                    Arithmetic.Add((byte) arg1.type[0], arg1.data.GetBytes(), (byte) arg2.type[0], arg2.data.GetBytes(), ref var);
                    break;
                }
                case 0x02: {
                    byte type1 = args[index++];
                    byte[] bytes1 = GetBytes(args, type1, ref index);

                    Instance? arg2 = GetRef(args, ref index);
                    if(arg2 == null) {
                        throw new NullRefException();
                    }

                    Instance? var = GetRef(args, ref index);
                    if(var == null) {
                        throw new NullRefException();
                    }

                    Arithmetic.Add(type1, bytes1, (byte) arg2.type[0], arg2.data.GetBytes(), ref var);
                    break;
                }
                case 0x03: {
                    Instance? arg1 = GetRef(args, ref index);
                    if(arg1 == null) {
                        throw new NullRefException();
                    }

                    byte type2 = args[index++];
                    byte[] bytes2 = GetBytes(args, type2, ref index);

                    Instance? var = GetRef(args, ref index);
                    if(var == null) {
                        throw new NullRefException();
                    }

                    Arithmetic.Add((byte) arg1.type[0], arg1.data.GetBytes(), type2, bytes2, ref var);
                    break;
                }
                case 0x04: {
                    byte type1 = args[index++];
                    byte[] bytes1 = GetBytes(args, type1, ref index);
                    byte type2 = args[index++];
                    byte[] bytes2 = GetBytes(args, type2, ref index);

                    Instance? var = GetRef(args, ref index);
                    if(var == null) {
                        throw new NullRefException();
                    }

                    Arithmetic.Add(type1, bytes1, type2, bytes2, ref var);
                    break;
                }
                case 0x05: {
                    Instance? arg1 = GetRef(args, ref index);
                    if(arg1 == null) {
                        throw new NullRefException();
                    }

                    Instance? arg2 = GetRef(args, ref index);
                    if(arg2 == null) {
                        throw new NullRefException();
                    }

                    Instance? var = GetRef(args, ref index);
                    if(var == null) {
                        throw new NullRefException();
                    }

                    Arithmetic.Sub((byte) arg1.type[0], arg1.data.GetBytes(), (byte) arg2.type[0], arg2.data.GetBytes(), ref var);
                    break;
                }
                case 0x06: {
                    byte type1 = args[index++];
                    byte[] bytes1 = GetBytes(args, type1, ref index);

                    Instance? arg2 = GetRef(args, ref index);
                    if(arg2 == null) {
                        throw new NullRefException();
                    }

                    Instance? var = GetRef(args, ref index);
                    if(var == null) {
                        throw new NullRefException();
                    }

                    Arithmetic.Sub(type1, bytes1, (byte) arg2.type[0], arg2.data.GetBytes(), ref var);
                    break;
                }
                case 0x07: {
                    Instance? arg1 = GetRef(args, ref index);
                    if(arg1 == null) {
                        throw new NullRefException();
                    }

                    byte type2 = args[index++];
                    byte[] bytes2 = GetBytes(args, type2, ref index);

                    Instance? var = GetRef(args, ref index);
                    if(var == null) {
                        throw new NullRefException();
                    }

                    Arithmetic.Sub((byte) arg1.type[0], arg1.data.GetBytes(), type2, bytes2, ref var);
                    break;
                }
                case 0x08: {
                    byte type1 = args[index++];
                    byte[] bytes1 = GetBytes(args, type1, ref index);
                    byte type2 = args[index++];
                    byte[] bytes2 = GetBytes(args, type2, ref index);

                    Instance? var = GetRef(args, ref index);
                    if(var == null) {
                        throw new NullRefException();
                    }

                    Arithmetic.Sub(type1, bytes1, type2, bytes2, ref var);
                    break;
                }
                case 0x1C: {
                    byte type = args[index++];
                    byte[] bytes = GetBytes(args, type, ref index);

                    if(type != 0x06) {
                        throw new InvalidArgumentException("JMP expects int32 as args");
                    }

                    return Converter.ToInt32(bytes);
                }
                case 0x2A: {
                    byte cmp = args[index++];

                    Instance? var1 = GetRef(args, ref index);
                    if(var1 == null) {
                        throw new NullRefException();
                    }

                    byte type1 = (byte)var1.type[0];
                    byte[] bytes1 = var1.data.GetBytes();

                    byte type2 = args[index++];
                    byte[] bytes2 = GetBytes(args, type2, ref index);

                    byte type3 = args[index++];
                    byte[] bytes3 = GetBytes(args, type3, ref index);

                    if(type3 != 0x06) {
                        throw new InvalidArgumentException("JMPC expects int32 as arg 4");
                    }

                    if(Comparison.Compare(type1, bytes1, type2, bytes2, cmp)) {
                        return Converter.ToInt32(bytes3);
                    }

                    break;
                }
                case 0x2D: { // TODO: argument handling
                    string name = GetSTR(args, ref index);

                    string[] split = name.Split(".");

                    byte argCount = args[index++];
                    List<Instance> argList = new List<Instance>();
                    for(int j = 0; j < argCount; j++) {
                        switch(args[index++]) {
                            case 0x0F:
                                Instance? var = GetRef(args, ref index);
                                if(var == null) {
                                    throw new NullRefException();
                                }

                                argList.Add(var);
                                break;
                        }
                    }

                    if(split[0] == "IO") {
                        HandleIO(split[1], argList);
                    } else { 
                        if(functions.ContainsKey(name)) {
                            functions[name].Execute();
                        } else {
                            throw new UnknownFunctionException($"Unknown function {name}");
                        }
                    }
                    break;
                }
                case 0x2F: {
                    // TODO: actually get return value
                    return -1;
                }
                case 0x39: { // VALUE
                    Type[] type = GetType(args, ref index);

                    string name = GetSTR(args, ref index);
                    byte[] bytes = GetBytes(args, (byte) type[0], ref index);
                    Block<byte> data = Globals.GarbageCollector.Alloc(bytes.Length);
                    data = Globals.GarbageCollector.stack.CopyTo(ref data);
                    for(int i = 0; i < bytes.Length; i++) {
                        data.SetPos(i, bytes[i]);
                    }
                    variables.Add(name, new Instance(name, type, data));
                    //Console.WriteLine(type.ToString("X2") + " " + name + " " + Encoding.UTF8.GetString(bytes));
                    break;
                }
                case 0x3C: { // VARDEF
                    Type[] type = GetType(args, ref index);

                    string name = GetSTR(args, ref index);
                    Block<byte> data = Globals.GarbageCollector.Alloc(GetTypeLength((byte) type[0], args, index));
                    data = Globals.GarbageCollector.stack.CopyTo(ref data);
                    variables.Add(name, new Instance(name, type, data));
                    //Console.WriteLine(type.ToString("X2") + " " + name);
                    break;
                }
                default:
                    throw new UnhandledArgumentException($"Unhandled instruction {instr} (0x{(byte) instr:X2})");
            }

            return -999; // it just needs to be negative to tell the runtime to not jump anywhere
        }

        public Instance? GetRef(string name) {
            Instance? ret;
            if(!variables.TryGetValue(name, out ret)) {
                if(parentScope != null) {
                    ret = parentScope.GetRef(name);
                }
            }

            return ret;
        }

        private Type[] GetType(byte[] args, ref int index) {
            int count = 1;
            int i2 = index;
            while(args[i2++] == 0x0C) {
                count++;
            }

            Type[] type = new Type[count];
            for(int i = 0; i < count; i++, index++) {
                type[index] = (Type) args[index];
            }

            return type;
        }

        private void HandleIO(string func, List<Instance> args) {
            switch(func) {
                case "println":
                    println(args[0]);
                    break;
                default:
                    throw new UnknownFunctionException($"Unknown function {func}");
            }
        }

        private void println(Instance var) {
            byte[] bytes = var.data.GetBytes();
            switch(var.type[0]) {
                case Type.uint8:
                    Console.WriteLine(conv.ToUInt8(bytes));
                    break;
                case Type.uint16:
                    Console.WriteLine(conv.ToUInt16(bytes));
                    break;
                case Type.uint32:
                    Console.WriteLine(conv.ToUInt32(bytes));
                    break;
                case Type.uint64:
                    Console.WriteLine(conv.ToUInt64(bytes));
                    break;
                case Type.int8:
                    Console.WriteLine(conv.ToInt8(bytes));
                    break;
                case Type.int16:
                    Console.WriteLine(conv.ToInt16(bytes));
                    break;
                case Type.int32:
                    Console.WriteLine(conv.ToInt32(bytes));
                    break;
                case Type.int64:
                    Console.WriteLine(conv.ToInt64(bytes));
                    break;
                case Type.float16:
                    Console.WriteLine(conv.ToFloat16(bytes));
                    break;
                case Type.float32:
                    Console.WriteLine(conv.ToFloat32(bytes));
                    break;
                case Type.float64:
                    Console.WriteLine(conv.ToFloat64(bytes));
                    break;
                case Type.pointer:
                    if(var.type[1] == Type._char) {
                        Console.WriteLine(var.data.ReadString());
                    }
                    break;
                case Type._char:
                    Console.WriteLine(conv.ToChar(bytes));
                    break;
                default:
                    throw new UnsupportedException("Attempted to println var with unknown type " + var.type);
            }
        }

        private Instance? GetRef(byte[] bytes, ref int index) {
            string name = GetSTR(bytes, ref index);

            Instance? ret = GetRef(name);

            return ret;
        }

        private string GetSTR(byte[] bytes, ref int index) {
            byte len = bytes[index++];
            string str = "";

            for(int i = 0; i < len; i++, index++) {
                str += (char) bytes[index];
            }
            return str;
        }

        private byte[] GetBytes(byte[] bytes, byte type, ref int index) {
            byte[] result;

            switch(type) {
                case 0x0B:
                    int i = index;
                    int len = 0;
                    while(bytes[i] != 0) {
                        i++;
                        len++;
                    }
                    result = new byte[len];
                    break;
                case 0x0C:
                    result = new byte[0];
                    break;
                default:
                    result = new byte[Types.lengths[type]];
                    break;
            }

            for(int i = 0; i < result.Length; i++, index++) {
                result[i] = bytes[index];
            }

            return result;
        }

        // dont look at this
        private KeyValuePair<Instruction, byte[]> ParseInstruction(byte[] bytes, ref int i) {
            byte instr = bytes[i];
            byte[] args;

            byte len = 0;
            int offset = 1;

            switch(instr) {
                case 0x00:
                    break;
                case 0x01:
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x02:
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x03:
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x04:
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x05:
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x06:
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x07:
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x08:
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x09:
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x0A:
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x0B:
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x0C:
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x0D:
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x0E:
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x0F:
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x10:
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x11:
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x12:
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x13:
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x14:
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x15:
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x16:
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x17:
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x18:
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x19:
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x1A:
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x1B:
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x1C:
                    GetTypeLength(bytes, i, ref len, ref offset);
                    break;
                case 0x1D:
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x1E:
                    offset++;
                    len++;
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x1F:
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x20:
                    offset++;
                    len++;
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x21:
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x22:
                    offset++;
                    len++;
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x23:
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x24:
                    offset++;
                    len++;
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x25:
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    break;
                case 0x26:
                    offset++;
                    len++;
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    break;
                case 0x27:
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    break;
                case 0x28:
                    offset++;
                    len++;
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    break;
                case 0x29:
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    break;
                case 0x2A:
                    offset++;
                    len++;
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    break;
                case 0x2B:
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    break;
                case 0x2C:
                    offset++;
                    len++;
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    break;
                case 0x2D:
                    GetRefLength(bytes, i, ref len, ref offset);

                    len++;

                    byte argCount = bytes[i + offset];
                    offset++;
                    for(int j = 0; j < argCount; j++) {
                        GetArgLength(bytes, i, ref len, ref offset);
                    }
                    break;
                case 0x2E:
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x2F:
                    GetTypeLength(bytes, i, ref len, ref offset);
                    break;
                case 0x30:
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x31:
                    GetTypeLength(bytes, i, ref len, ref offset);
                    break;
                case 0x32:
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x33:
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x34:
                    GetTypeLength(bytes, i, ref len, ref offset);
                    break;
                case 0x35:
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x36:
                    GetTypeLength(bytes, i, ref len, ref offset);
                    break;
                case 0x37:
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x38:
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x39: {
                    byte type = bytes[i + offset];

                    offset++;
                    len++;

                    GetRefLength(bytes, i, ref len, ref offset);
                    len++;

                    switch(type) {
                        case 0x0B:
                            offset++;
                            len++;
                            while(bytes[i + offset] != 0) {
                                len++;
                                offset++;
                            }
                            break;
                        case 0x0C:
                            break; // TODO(grimtin10): object specification
                        default:
                            len += Types.lengths[type];
                            break;
                    }
                    break;
                }
                case 0x3A:
                    len += bytes[i];
                    len++;
                    break;
                case 0x3B: {
                    byte type = bytes[i + offset];

                    offset++;
                    len++;

                    switch(type) {
                        case 0x00:
                            GetRefLength(bytes, i, ref len, ref offset);
                            break;
                        case 0x01:
                            GetRefLength(bytes, i, ref len, ref offset);
                            GetRefLength(bytes, i, ref len, ref offset);
                            break;
                    }
                    break;
                }
                case 0x3C: {
                    byte type = bytes[i + offset];

                    offset++;
                    len++;

                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                }
            }

            //Console.WriteLine(i + " " + instr + " " + len);

            args = new byte[len];

            for(int j = 0; j < args.Length; j++, i++) {
                args[j] = bytes[i + 1];
            }

            return new KeyValuePair<Instruction, byte[]>((Instruction) instr, args);
        }

        public void GetArgLength(byte[] bytes, int i, ref byte len, ref int offset) {
            byte type = bytes[i + offset];

            len++;
            offset++;

            if(type == 0x0F) {
                GetRefLength(bytes, i, ref len, ref offset);
            } else {
                GetTypeLength(bytes, i, ref len, ref offset);
            }
        }

        public void GetTypeLength(byte[] bytes, int i, ref byte len, ref int offset) {
            i += offset;

            byte type = bytes[i++];

            len++;
            offset++;

            switch(type) {
                case 0x0D:
                    break; // TODO(grimtin10): object specification
                default:
                    len += Types.lengths[type];
                    offset += Types.lengths[type];
                    //Console.WriteLine("type: " + type + " len: " + Types.lengths[type]);
                    break;
            }
        }

        public int GetTypeLength(byte type, byte[] bytes, int i) {
            int len = 0;

            switch(type) {
                case 0x0B:
                    i++;
                    while(bytes[i] != 0) {
                        len++;
                        i++;
                    }
                    break;
                case 0x0C:
                    break; // TODO(grimtin10): object specification
                default:
                    len += Types.lengths[type];
                    break;
            }

            return len;
        }

        public byte GetRefLength(byte[] bytes, int i, ref byte len, ref int offset) {
            i += offset;
            //Console.Write("i: " + i + " " + bytes[i] + " ");
            byte length = bytes[i];
            len += length;
            len++;
            offset += length + 1;
            //Console.WriteLine("r: " + length);

            return length;
        }
    }
}
