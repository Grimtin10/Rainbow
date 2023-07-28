using Rainbow.Exceptions;
using Rainbow.GarbageCollection.GCTypes;
using Rainbow.Handlers;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
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

        public Scope(byte[] bytes, Scope? parentScope) {
            this.parentScope = parentScope;

            bool inFunction = false;

            string funcName = "";
            Type funcType = Type.undefined;
            List<Instance> arguments = new();

            int scopeDepth = 0;
            List<byte> scopeBytes = new();

            for(int i = 0; i < bytes.Length; i++) {
                if(bytes[i] == 0xFF) {
                    inFunction = true;
                    funcName = "";
                    funcType = Type.undefined;
                    arguments = new();
                    continue;
                }

                if(bytes[i] == 0xFE) {
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
                        inFunction = false;
                    }
                    continue;
                }

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
                        }
                    } else {
                        instructions.Add(ParseInstruction(bytes, ref i));
                    }
                }
            }
        }

        public void Execute() {
            stackStart = Globals.GarbageCollector.stack.ptrs.Count;

            foreach(KeyValuePair<Instruction, byte[]> instruction in instructions) {
                Console.Write(instruction.Key.ToString() + " ");
                foreach(byte b in instruction.Value) {
                    Console.Write(b.ToString("X2") + " ");
                }
                Console.WriteLine();
                ExecInstruction(instruction);
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
            while(Globals.GarbageCollector.stack.ptrs.Count > stackStart) {
                Globals.GarbageCollector.stack.Pop();
            }
            if(isGlobal) {
                Globals.GarbageCollector.FreeRootStack();
            }
            Globals.GarbageCollector.Collect();
        }

        private void ExecInstruction(KeyValuePair<Instruction, byte[]> instruction) {
            Instruction instr = instruction.Key;
            byte[] args = instruction.Value;

            int index = 0;

            switch((byte)instr) {
                case 0x2F: {

                    break;
                }
                case 0x39: { // VALUE
                    byte type = args[index++];
                    string name = GetSTR(args, ref index);
                    byte[] bytes = GetBytes(args, type, ref index);
                    Block<byte> data = Globals.GarbageCollector.Alloc(bytes.Length);
                    Globals.GarbageCollector.PushStack(data, true);
                    for(int i=0;i<bytes.Length;i++) {
                        data.SetPos(i, bytes[i]);
                    }
                    variables.Add(name, new Instance(name, (Type) type, data));
                    Console.WriteLine(type.ToString("X2") + " " + name + " " + Encoding.UTF8.GetString(bytes));
                    break;
                }
                case 0x3B: { // SYSCALL_I
                    byte type = args[index++];
                    HandleSyscall(type, args, ref index);
                    break;
                }
                default:
                    throw new UnhandledArgumentException($"Unhandled instruction {instr} (0x{(byte) instr:X2})");
            }
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

        private void HandleSyscall(byte type, byte[] args, ref int index) {
            switch(type) {
                case 0x00:
                    Instance? var = GetRef(args, ref index);
                    if(var == null) throw new NullRefException();

                    Console.WriteLine(var.data.ReadString());

                    break;
                default:
                    throw new UnhandledArgumentException($"Unhandled syscall {type:X2}");
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
                    break;
                case 0x02:
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    break;
                case 0x03:
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x04:
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    break;
                case 0x05:
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x06:
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    break;
                case 0x07:
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x08:
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    break;
                case 0x09:
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x0A:
                    GetTypeLength(bytes, i, ref len, ref offset);
                    break;
                case 0x0B:
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x0C:
                    GetTypeLength(bytes, i, ref len, ref offset);
                    break;
                case 0x0D:
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x0E:
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    break;
                case 0x0F:
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x10:
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    break;
                case 0x11:
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x12:
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    break;
                case 0x13:
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x14:
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    break;
                case 0x15:
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x16:
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    break;
                case 0x17:
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x18:
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    break;
                case 0x19:
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x1A:
                    GetTypeLength(bytes, i, ref len, ref offset);
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
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    break;
                case 0x1F:
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x20:
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    break;
                case 0x21:
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x22:
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    break;
                case 0x23:
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x24:
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    break;
                case 0x25:
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x26:
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    break;
                case 0x27:
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x28:
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    break;
                case 0x29:
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x2A:
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    break;
                case 0x2B:
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetRefLength(bytes, i, ref len, ref offset);
                    break;
                case 0x2C:
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    GetTypeLength(bytes, i, ref len, ref offset);
                    break;
                case 0x2D:
                    GetRefLength(bytes, i, ref len, ref offset);
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
            }

            args = new byte[len];

            for(int j = 0; j < args.Length; j++, i++) {
                args[j] = bytes[i + 1];
            }

            return new KeyValuePair<Instruction, byte[]>((Instruction) instr, args);
        }

        public void GetTypeLength(byte[] bytes, int i, ref byte len, ref int offset) {
            i += offset;
            byte type = bytes[i++];

            len++;
            offset++;

            switch(type) {
                case 0x0B:
                    i++;
                    offset++;
                    while(bytes[i] != 0) {
                        len++;
                        i++;
                        offset++;
                    }
                    break;
                case 0x0C:
                    break; // TODO(grimtin10): object specification
                default:
                    len += Types.lengths[type];
                    offset += Types.lengths[type];
                    break;
            }
        }

        public byte GetRefLength(byte[] bytes, int i, ref byte len, ref int offset) {
            i += offset;
            byte length = bytes[i];
            len += length;
            len++;
            offset += length + 1;

            return length;
        }
    }
}
