using Rainbow.Exceptions;
using System;
using System.ComponentModel;
using System.Text;

namespace Rainbow.Compilation.Assembler {
    public class Assembler {
        public static byte[] Assemble(string file) {
            string[] asm = File.ReadAllLines(file);

            List<byte> output = new();

            List<Token> tokens = Lexer.Lex(asm);

            bool inFuncDef = false;
            string funcName = ""; // for error purposes
            int funcOffset = 0;
            uint byteCount = 0;
            for(int i = 0; i < tokens.Count; i++) {
                Console.WriteLine(tokens[i]);
                if(tokens[i].type == TokenType.TYPE) {
                    if(tokens[i + 2].type == TokenType.LPAREN) {
                        output.Add(0xFF);
                        inFuncDef = true;
                    }
                }

                if(tokens[i].type == TokenType.RPAREN && inFuncDef) {
                    for(int j = i; j < tokens.Count; j++) {
                        if(tokens[j].type == TokenType.RBRACKET) {
                            inFuncDef = false;
                        }
                    }

                    if(inFuncDef) {
                        throw new UnfinishedFuncException($"Function {funcName} never closes!");
                    } else {
                        funcOffset = (int) byteCount + 1;
                        byteCount = 0;
                        for(int j = 0; j < 4; j++) {
                            output.Add(0x00); // temp values
                        }
                    }
                }

                switch(tokens[i].type) {
                    case TokenType.TYPE: {
                        output.Add(Lexer.types[tokens[i].value]);
                        byteCount++;
                        break;
                    }
                    case TokenType.STR: {
                        if(inFuncDef) {
                            funcName = tokens[i].value;
                        }

                        byte len = (byte)tokens[i].value.Length;
                        byte[] str = Encoding.UTF8.GetBytes(tokens[i].value);

                        output.Add(len);
                        foreach(byte b in str) {
                            output.Add(b);
                        }
                        byteCount += (uint) str.Length + 1;
                        break;
                    }
                    case TokenType.SYSCALL: {
                        output.Add(Lexer.syscalls[tokens[i].value]);
                        byteCount++;
                        break;
                    }
                    case TokenType.INSTR: {
                        byte[] instr = ParseInstr(tokens, ref i);
                        foreach(byte b in instr) {
                            output.Add(b);
                        }
                        byteCount += (uint) instr.Length;
                        break;
                    }
                    case TokenType.LBRACKET: {
                        output.Add(0xFE);
                        byteCount++;
                        break;
                    }
                    case TokenType.RBRACKET: {
                        output.Add(0xFD);
                        byteCount++;

                        byte[] bytes = BitConverter.GetBytes(byteCount - 1);
                        if(BitConverter.IsLittleEndian) {
                            Array.Reverse(bytes);
                        }

                        for(int j = 0; j < bytes.Length; j++) {
                            output[j + funcOffset] = bytes[j];
                        }
                        break;
                    }
                }
            }

            return output.ToArray();
        }

        private static byte[] ParseInstr(List<Token> tokens, ref int i) {
            string instr = tokens[i].value;
            byte[] res;

            switch(instr) {
                case "SUB": {
                    if(tokens[i + 1].type != TokenType.STR && tokens[i + 1].type != TokenType.VALUE) {
                        throw new InvalidArgumentException($"Invalid argument type {tokens[i + 1].type} at token {i}");
                    }
                    if(tokens[i + 2].type != TokenType.STR && tokens[i + 2].type != TokenType.VALUE) {
                        throw new InvalidArgumentException($"Invalid argument type {tokens[i + 2].type} at token {i}");
                    }
                    if(tokens[i + 3].type != TokenType.STR) {
                        throw new InvalidArgumentException($"Invalid argument type {tokens[i + 3].type} at token {i}");
                    }

                    bool arg1 = GetArgType(tokens[i + 1]);
                    bool arg2 = GetArgType(tokens[i + 2]);

                    byte[] bytes1;
                    byte[] bytes2;

                    if(!arg1 && !arg2) {
                        bytes1 = StrToBytes(tokens[i + 1].value);
                        bytes2 = StrToBytes(tokens[i + 2].value);

                        res = new byte[bytes1.Length + bytes2.Length + 1];

                        res[0] = 0x05;
                    } else if(arg1 && !arg2) {
                        bytes1 = ValToBytes(tokens[i + 1]);
                        bytes2 = StrToBytes(tokens[i + 2].value);

                        res = new byte[bytes1.Length + bytes2.Length + 1];

                        res[0] = 0x06;

                    } else if(!arg1 && arg2) {
                        bytes1 = StrToBytes(tokens[i + 1].value);
                        bytes2 = ValToBytes(tokens[i + 2]);

                        res = new byte[bytes1.Length + bytes2.Length + 1];

                        res[0] = 0x07;
                    } else {
                        bytes1 = ValToBytes(tokens[i + 1]);
                        bytes2 = ValToBytes(tokens[i + 2]);

                        res = new byte[bytes1.Length + bytes2.Length + 1];

                        res[0] = 0x08;
                    }

                    int off = 1;
                    for(int j = 0; j < bytes1.Length; j++, off++) {
                        res[off] = bytes1[j];
                    }
                    for(int j = 0; j < bytes2.Length; j++, off++) {
                        res[off] = bytes2[j];
                    }

                    i += 2;

                    break;
                }
                case "RET": {
                    if(tokens[i + 1].type != TokenType.STR && tokens[i + 1].type != TokenType.VALUE) {
                        res = new byte[] { 0x2F, 0x00 };
                    } else {
                        bool arg1 = GetArgType(tokens[i + 1]);
                        byte[] bytes1;

                        if(!arg1) {
                            bytes1 = StrToBytes(tokens[i + 1].value);

                            res = new byte[bytes1.Length + 1];

                            res[0] = 0x2E;
                        } else {
                            bytes1 = ValToBytes(tokens[i + 1]);

                            res = new byte[bytes1.Length + 1];

                            res[0] = 0x2F;
                        }

                        int off = 1;
                        for(int j = 0; j < bytes1.Length; j++, off++) {
                            res[off] = bytes1[j];
                        }

                        i++;
                    }
                    break;
                }
                case "VARDEF": {
                    if(tokens[i + 1].type != TokenType.TYPE) {
                        throw new InvalidArgumentException($"Invalid argument type {tokens[i + 1].type} at token {i}");
                    }
                    if(tokens[i + 2].type != TokenType.STR) {
                        throw new InvalidArgumentException($"Invalid argument type {tokens[i + 2].type} at token {i}");
                    }

                    res = new byte[3 + tokens[i + 2].value.Length];

                    byte type = Lexer.types[tokens[i + 1].value];
                    byte[] str = StrToBytes(tokens[i + 2].value);

                    res[0] = 0x3C;
                    res[1] = type;

                    for(int j = 0; j < str.Length; j++) {
                        res[j + 2] = str[j];
                    }

                    i += 2;

                    break;
                }
                case "SYSCALL": {
                    bool arg1 = GetArgType(tokens[i + 1]);
                    byte[] bytes1;

                    if(!arg1) {
                        bytes1 = StrToBytes(tokens[i + 1].value);

                        res = new byte[bytes1.Length + 1];

                        res[0] = 0x3A;
                    } else {
                        bytes1 = ValToBytes(tokens[i + 1]);

                        res = new byte[bytes1.Length + 1];

                        res[0] = 0x3B;
                    }

                    int off = 1;
                    for(int j = 0; j < bytes1.Length; j++, off++) {
                        res[off] = bytes1[j];
                    }

                    i++;
                    break;
                }
                default:
                    throw new UnknownInstrException($"Unknown instruction {instr}");
            }

            return res;
        }

        // FALSE - REF
        // TRUE  - IMM
        private static bool GetArgType(Token arg) {
            return arg.type == TokenType.VALUE || arg.type == TokenType.SYSCALL;
        }

        private static byte[] StrToBytes(string str) {
            byte len = (byte)str.Length;
            byte[] bytes = Encoding.UTF8.GetBytes(str);

            byte[] res = new byte[1 + len];

            res[0] = len;
            for(int j = 0; j < bytes.Length; j++) {
                res[j + 1] = bytes[j];
            }

            return res;
        }

        private static byte[] ValToBytes(Token val) {
            if(val.type == TokenType.SYSCALL) {
                return new byte[] { Lexer.syscalls[val.value] };
            }

            if(val.type != TokenType.VALUE) {
                throw new InvalidTypeException($"Cannot use ValToBytes on token with type {val.type}");
            }

            // TODO: make types fit into as little bytes as possible
            byte[] res;
            if(int.TryParse(val.value, out int valInt)) {
                res = new byte[5];
                res[0] = 0x06;

                byte[] bytes = BitConverter.GetBytes(valInt);
                if(BitConverter.IsLittleEndian) {
                    Array.Reverse(bytes);
                }

                Array.Copy(bytes, 0, res, 1, bytes.Length);
            } else if(float.TryParse(val.value, out float valFloat)) { // TODO: fit float types into small bytes
                res = new byte[5];
                res[0] = 0x09;

                byte[] bytes = BitConverter.GetBytes(valFloat);
                if(BitConverter.IsLittleEndian) {
                    Array.Reverse(bytes);
                }
                Array.Copy(bytes, 0, res, 1, bytes.Length);
            } else {
                res = new byte[val.value.Length + 1];

                Array.Copy(Encoding.UTF8.GetBytes(val.value), 0, res, 0, val.value.Length);
                res[^1] = 0x00;
            }

            return res;
        }
    }
}
