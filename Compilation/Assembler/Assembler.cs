﻿using Rainbow.Exceptions;
using Rainbow.Execution;
using System.Diagnostics;
using System.Text;
using Type = Rainbow.Execution.Type;

namespace Rainbow.Compilation.Assembler {
    public class Assembler {
        public static byte[] Assemble(string file) {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            string[] asm = File.ReadAllLines(file);

            List<byte> output = new();

            List<Token> tokens = Lexer.Lex(asm);

            Dictionary<string, FuncDef> funcs = new();
            Dictionary<string, int> labels = new();

            bool inFuncDef = false;
            string funcName = "";
            int funcOffset = 0;
            uint byteCount = 0;
            int instrCount = 0;

            for(int i = 0; i < tokens.Count; i++) {
                if(inFuncDef) {
                    if(tokens[i].type == TokenType.STR) {
                        if(funcName.Length == 0) {
                            funcName = tokens[i].value;
                            funcs.Add(funcName, new FuncDef(funcName));
                        }
                    }

                    if(tokens[i].type == TokenType.TYPE) {
                        if(tokens[i].type == TokenType.TYPE && tokens[i].value == "*") {
                            continue; // TODO: pointer types
                        }
                        // lol
                        funcs.Values.ToList()[^1].AddArg((Type) Lexer.types[tokens[i].value]);
                    }
                }

                if(tokens[i].type == TokenType.TYPE) {
                    if(tokens[i + 2].type == TokenType.LPAREN) {
                        inFuncDef = true;
                        funcName = "";
                        instrCount = 0;
                    }
                }

                if(tokens[i].type == TokenType.LBRACKET) {
                    inFuncDef = false;
                }

                if(tokens[i].type == TokenType.INSTR) {
                    instrCount++;
                }

                if(tokens[i].type == TokenType.LABEL) {
                    tokens[i].type = TokenType.VALUE;
                }
            }

            inFuncDef = false;

            for(int i = 0; i < tokens.Count; i++) {
                if(tokens[i].type == TokenType.TYPE) {
                    if(tokens[i + 2].type == TokenType.LPAREN) {
                        output.Add(0xFF);
                        inFuncDef = true;
                        funcName = "";
                        funcOffset = output.Count;
                    }
                }

                if(tokens[i].type == TokenType.RPAREN && inFuncDef) {
                    output.Add(0xFA);
                    byteCount++;
                    funcOffset = output.Count;
                    byteCount = 0;
                    for(int j = 0; j < 4; j++) {
                        output.Add(0x00); // temp values
                    }
                }

                switch(tokens[i].type) {
                    case TokenType.TYPE: {
                        output.Add(Lexer.types[tokens[i].value]);
                        byteCount++;
                        break;
                    }
                    case TokenType.STR: {
                        if(inFuncDef && funcName.Length == 0) {
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
                    case TokenType.INSTR: {
                        byte[] instr = ParseInstr(tokens, funcs, ref i);
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

            stopwatch.Stop();

            Console.WriteLine("Assembling program took " + stopwatch.ElapsedMilliseconds + "ms");

            return output.ToArray();
        }

        private static byte[] ParseInstr(List<Token> tokens, Dictionary<string, FuncDef> funcs, ref int i) {
            string instr = tokens[i].value;
            byte[] res;

            switch(instr) {
                case "ADD": {
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
                    byte[] bytes3;

                    bytes3 = StrToBytes(tokens[i + 3].value);

                    if(!arg1 && !arg2) {
                        bytes1 = StrToBytes(tokens[i + 1].value);
                        bytes2 = StrToBytes(tokens[i + 2].value);

                        res = new byte[bytes1.Length + bytes2.Length + bytes3.Length + 1];

                        res[0] = 0x01;
                    } else if(arg1 && !arg2) {
                        bytes1 = ValToBytes(tokens[i + 1]);
                        bytes2 = StrToBytes(tokens[i + 2].value);

                        res = new byte[bytes1.Length + bytes2.Length + bytes3.Length + 1];

                        res[0] = 0x02;

                    } else if(!arg1 && arg2) {
                        bytes1 = StrToBytes(tokens[i + 1].value);
                        bytes2 = ValToBytes(tokens[i + 2]);

                        res = new byte[bytes1.Length + bytes2.Length + bytes3.Length + 1];

                        res[0] = 0x03;
                    } else {
                        bytes1 = ValToBytes(tokens[i + 1]);
                        bytes2 = ValToBytes(tokens[i + 2]);

                        res = new byte[bytes1.Length + bytes2.Length + bytes3.Length + 1];

                        res[0] = 0x04;
                    }

                    int off = 1;
                    for(int j = 0; j < bytes1.Length; j++, off++) {
                        res[off] = bytes1[j];
                    }
                    for(int j = 0; j < bytes2.Length; j++, off++) {
                        res[off] = bytes2[j];
                    }
                    for(int j = 0; j < bytes3.Length; j++, off++) {
                        res[off] = bytes3[j];
                    }

                    i += 3;

                    break;
                }
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
                    byte[] bytes3;

                    bytes3 = StrToBytes(tokens[i + 3].value);

                    if(!arg1 && !arg2) {
                        bytes1 = StrToBytes(tokens[i + 1].value);
                        bytes2 = StrToBytes(tokens[i + 2].value);

                        res = new byte[bytes1.Length + bytes2.Length + bytes3.Length + 1];

                        res[0] = 0x05;
                    } else if(arg1 && !arg2) {
                        bytes1 = ValToBytes(tokens[i + 1]);
                        bytes2 = StrToBytes(tokens[i + 2].value);

                        res = new byte[bytes1.Length + bytes2.Length + bytes3.Length + 1];

                        res[0] = 0x06;

                    } else if(!arg1 && arg2) {
                        bytes1 = StrToBytes(tokens[i + 1].value);
                        bytes2 = ValToBytes(tokens[i + 2]);

                        res = new byte[bytes1.Length + bytes2.Length + bytes3.Length + 1];

                        res[0] = 0x07;
                    } else {
                        bytes1 = ValToBytes(tokens[i + 1]);
                        bytes2 = ValToBytes(tokens[i + 2]);

                        res = new byte[bytes1.Length + bytes2.Length + bytes3.Length + 1];

                        res[0] = 0x08;
                    }

                    int off = 1;
                    for(int j = 0; j < bytes1.Length; j++, off++) {
                        res[off] = bytes1[j];
                    }
                    for(int j = 0; j < bytes2.Length; j++, off++) {
                        res[off] = bytes2[j];
                    }
                    for(int j = 0; j < bytes3.Length; j++, off++) {
                        res[off] = bytes3[j];
                    }

                    i += 3;

                    break;
                }
                case "JMP": {
                    if(tokens[i + 1].type != TokenType.STR && tokens[i + 1].type != TokenType.VALUE) {
                        throw new InvalidArgumentException($"Invalid argument type {tokens[i + 1].type} at token {i}");
                    }

                    bool arg1 = GetArgType(tokens[i + 1]);

                    byte[] bytes1;

                    if(!arg1) {
                        bytes1 = StrToBytes(tokens[i + 1].value);

                        res = new byte[bytes1.Length + 1];
                        res[0] = 0x1B;
                    } else {
                        bytes1 = ValToBytes(tokens[i + 1]);

                        res = new byte[6];
                        res[0] = 0x1C;
                    }

                    int off = 1;
                    for(int j = 0; j < bytes1.Length; j++, off++) {
                        res[off] = bytes1[j];
                    }

                    i++;

                    break;
                }
                case "JMPC": { // TODO: array loo;up
                    if(tokens[i + 1].type != TokenType.STR) {
                        throw new InvalidArgumentException($"Invalid argument type {tokens[i + 1].type} at token {i}");
                    }
                    if(tokens[i + 2].type != TokenType.STR && tokens[i + 2].type != TokenType.VALUE) {
                        throw new InvalidArgumentException($"Invalid argument type {tokens[i + 2].type} at token {i}");
                    }
                    if(tokens[i + 3].type != TokenType.STR && tokens[i + 3].type != TokenType.VALUE) {
                        throw new InvalidArgumentException($"Invalid argument type {tokens[i + 3].type} at token {i}");
                    }
                    if(tokens[i + 4].type != TokenType.STR && tokens[i + 4].type != TokenType.VALUE) {
                        throw new InvalidArgumentException($"Invalid argument type {tokens[i + 4].type} at token {i}");
                    }

                    string v = tokens[i + 1].value;
                    if(v == "<" || v == ">" || v == "<=" || v == ">=" || v == "==" || v == "!=") {
                        tokens[i + 1].type = TokenType.CMP;
                    }

                    bool arg1 = GetArgType(tokens[i + 1]);
                    bool arg2 = GetArgType(tokens[i + 2]);
                    bool arg3 = GetArgType(tokens[i + 3]);
                    bool arg4 = GetArgType(tokens[i + 4]);

                    byte[] bytes1;
                    byte[] bytes2;
                    byte[] bytes3;
                    byte[] bytes4;

                    // F O R M A T T I N G
                    if(       !arg1 && !arg2 && !arg3 && !arg4) { // REF, REF, REF, REF
                        bytes1 = StrToBytes(tokens[i + 1].value);
                        bytes2 = StrToBytes(tokens[i + 2].value);
                        bytes3 = StrToBytes(tokens[i + 3].value);
                        bytes4 = StrToBytes(tokens[i + 4].value);

                        res = new byte[bytes1.Length + bytes2.Length + bytes3.Length + bytes4.Length + 1];

                        res[0] = 0x1D;
                    } else if( arg1 && !arg2 && !arg3 && !arg4) { // IMM, REF, REF, REF
                        bytes1 = ValToBytes(tokens[i + 1]);
                        bytes2 = StrToBytes(tokens[i + 2].value);
                        bytes3 = StrToBytes(tokens[i + 3].value);
                        bytes4 = StrToBytes(tokens[i + 4].value);

                        res = new byte[bytes1.Length + bytes2.Length + bytes3.Length + bytes4.Length + 1];

                        res[0] = 0x1E;
                    } else if(!arg1 &&  arg2 && !arg3 && !arg4) { // REF, IMM, REF, REF
                        bytes1 = StrToBytes(tokens[i + 1].value);
                        bytes2 = ValToBytes(tokens[i + 2]);
                        bytes3 = StrToBytes(tokens[i + 3].value);
                        bytes4 = StrToBytes(tokens[i + 4].value);

                        res = new byte[bytes1.Length + bytes2.Length + bytes3.Length + bytes4.Length + 1];

                        res[0] = 0x1F;
                    } else if( arg1 &&  arg2 && !arg3 && !arg4) { // IMM, IMM, REF, REF
                        bytes1 = ValToBytes(tokens[i + 1]);
                        bytes2 = ValToBytes(tokens[i + 2]);
                        bytes3 = StrToBytes(tokens[i + 3].value);
                        bytes4 = StrToBytes(tokens[i + 4].value);

                        res = new byte[bytes1.Length + bytes2.Length + bytes3.Length + bytes4.Length + 1];

                        res[0] = 0x20;
                    } else if(!arg1 && !arg2 &&  arg3 && !arg4) { // REF, REF, IMM, REF
                        bytes1 = StrToBytes(tokens[i + 1].value);
                        bytes2 = StrToBytes(tokens[i + 2].value);
                        bytes3 = ValToBytes(tokens[i + 3]);
                        bytes4 = StrToBytes(tokens[i + 4].value);

                        res = new byte[bytes1.Length + bytes2.Length + bytes3.Length + bytes4.Length + 1];

                        res[0] = 0x21;
                    } else if( arg1 && !arg2 &&  arg3 && !arg4) { // IMM, REF, IMM, REF
                        bytes1 = ValToBytes(tokens[i + 1]);
                        bytes2 = StrToBytes(tokens[i + 2].value);
                        bytes3 = ValToBytes(tokens[i + 3]);
                        bytes4 = StrToBytes(tokens[i + 4].value);

                        res = new byte[bytes1.Length + bytes2.Length + bytes3.Length + bytes4.Length + 1];

                        res[0] = 0x22;
                    } else if(!arg1 &&  arg2 &&  arg3 && !arg4) { // REF, IMM, IMM, REF
                        bytes1 = StrToBytes(tokens[i + 1].value);
                        bytes2 = ValToBytes(tokens[i + 2]);
                        bytes3 = ValToBytes(tokens[i + 3]);
                        bytes4 = StrToBytes(tokens[i + 4].value);

                        res = new byte[bytes1.Length + bytes2.Length + bytes3.Length + bytes4.Length + 1];

                        res[0] = 0x23;
                    } else if( arg1 &&  arg2 &&  arg3 && !arg4) { // IMM, IMM, IMM, REF
                        bytes1 = ValToBytes(tokens[i + 1]);
                        bytes2 = ValToBytes(tokens[i + 2]);
                        bytes3 = ValToBytes(tokens[i + 3]);
                        bytes4 = StrToBytes(tokens[i + 4].value);

                        res = new byte[bytes1.Length + bytes2.Length + bytes3.Length + bytes4.Length + 1];

                        res[0] = 0x24;
                    } else if(!arg1 && !arg2 && !arg3 &&  arg4) { // REF, REF, REF, IMM
                        bytes1 = StrToBytes(tokens[i + 1].value);
                        bytes2 = StrToBytes(tokens[i + 2].value);
                        bytes3 = StrToBytes(tokens[i + 3].value);
                        bytes4 = ValToBytes(tokens[i + 4]);

                        res = new byte[bytes1.Length + bytes2.Length + bytes3.Length + bytes4.Length + 1];

                        res[0] = 0x25;
                    } else if( arg1 && !arg2 && !arg3 &&  arg4) { // IMM, REF, REF, IMM
                        bytes1 = ValToBytes(tokens[i + 1]);
                        bytes2 = StrToBytes(tokens[i + 2].value);
                        bytes3 = StrToBytes(tokens[i + 3].value);
                        bytes4 = ValToBytes(tokens[i + 4]);

                        res = new byte[bytes1.Length + bytes2.Length + bytes3.Length + bytes4.Length + 1];

                        res[0] = 0x26;
                    } else if(!arg1 &&  arg2 && !arg3 &&  arg4) { // REF, IMM, REF, IMM
                        bytes1 = StrToBytes(tokens[i + 1].value);
                        bytes2 = ValToBytes(tokens[i + 2]);
                        bytes3 = StrToBytes(tokens[i + 3].value);
                        bytes4 = ValToBytes(tokens[i + 4]);

                        res = new byte[bytes1.Length + bytes2.Length + bytes3.Length + bytes4.Length + 1];

                        res[0] = 0x27;
                    } else if( arg1 &&  arg2 && !arg3 &&  arg4) { // IMM, IMM, REF, IMM
                        bytes1 = ValToBytes(tokens[i + 1]);
                        bytes2 = ValToBytes(tokens[i + 2]);
                        bytes3 = StrToBytes(tokens[i + 3].value);
                        bytes4 = ValToBytes(tokens[i + 4]);

                        res = new byte[bytes1.Length + bytes2.Length + bytes3.Length + bytes4.Length + 1];

                        res[0] = 0x28;
                    } else if(!arg1 && !arg2 &&  arg3 &&  arg4) { // REF, REF, IMM, IMM
                        bytes1 = StrToBytes(tokens[i + 1].value);
                        bytes2 = StrToBytes(tokens[i + 2].value);
                        bytes3 = ValToBytes(tokens[i + 3]);
                        bytes4 = ValToBytes(tokens[i + 4]);

                        res = new byte[bytes1.Length + bytes2.Length + bytes3.Length + bytes4.Length + 1];

                        res[0] = 0x29;
                    } else if( arg1 && !arg2 &&  arg3 &&  arg4) { // IMM, REF, IMM, IMM
                        bytes1 = ValToBytes(tokens[i + 1]);
                        bytes2 = StrToBytes(tokens[i + 2].value);
                        bytes3 = ValToBytes(tokens[i + 3]);
                        bytes4 = ValToBytes(tokens[i + 4]);

                        res = new byte[bytes1.Length + bytes2.Length + bytes3.Length + bytes4.Length + 1];

                        res[0] = 0x2A;
                    } else if(!arg1 &&  arg2 &&  arg3 &&  arg4) { // REF, IMM, IMM, IMM
                        bytes1 = StrToBytes(tokens[i + 1].value);
                        bytes2 = ValToBytes(tokens[i + 2]);
                        bytes3 = ValToBytes(tokens[i + 3]);
                        bytes4 = ValToBytes(tokens[i + 4]);

                        res = new byte[bytes1.Length + bytes2.Length + bytes3.Length + bytes4.Length + 1];

                        res[0] = 0x2B;
                    } else {                                      // IMM, IMM, IMM, IMM
                        bytes1 = ValToBytes(tokens[i + 1]);
                        bytes2 = ValToBytes(tokens[i + 2]);
                        bytes3 = ValToBytes(tokens[i + 3]);
                        bytes4 = ValToBytes(tokens[i + 4]);

                        res = new byte[bytes1.Length + bytes2.Length + bytes3.Length + bytes4.Length + 1];

                        res[0] = 0x2C;
                    }

                    int off = 1;
                    for(int j = 0; j < bytes1.Length; j++, off++) {
                        res[off] = bytes1[j];
                    }
                    for(int j = 0; j < bytes2.Length; j++, off++) {
                        res[off] = bytes2[j];
                    }
                    for(int j = 0; j < bytes3.Length; j++, off++) {
                        res[off] = bytes3[j];
                    }
                    for(int j = 0; j < bytes4.Length; j++, off++) {
                        res[off] = bytes4[j];
                    }

                    i += 4;

                    break;
                }
                case "CALL": {
                    if(tokens[i + 1].type != TokenType.STR) {
                        throw new InvalidArgumentException($"Invalid argument type {tokens[i + 1].type} at token {i}");
                    }

                    int index = i + 2;

                    List<byte> bytes = new List<byte>();
                    while(tokens[index].type == TokenType.STR || tokens[index].type == TokenType.VALUE) {
                        if(tokens[index].type == TokenType.STR) {
                            byte[] str = StrToBytes(tokens[index].value);
                            bytes.Add(0x0F);
                            foreach(byte b in str) {
                                bytes.Add(b);
                            }
                        } else {
                            byte[] val = ValToBytes(tokens[index]);
                            foreach(byte b in val) {
                                bytes.Add(b);
                            }
                        }

                        index++;
                    }

                    byte[] bytes1;

                    bytes1 = StrToBytes(tokens[i + 1].value);

                    res = new byte[bytes1.Length + 2 + bytes.Count];
                    res[0] = 0x2D;

                    int off = 1;
                    for(int j = 0; j < bytes1.Length; j++, off++) {
                        res[off] = bytes1[j];
                    }
                    res[off] = (byte) (index - (i + 2));
                    off++;
                    for(int j = 0; j < bytes.Count; j++, off++) {
                        res[off] = bytes[j];
                    }

                    i += 1 + index - (i + 2);
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
                default:
                    throw new UnknownInstrException($"Unknown instruction {instr}");
            }

            return res;
        }

        // FALSE - REF
        // TRUE  - IMM
        private static bool GetArgType(Token arg) {
            return arg.type == TokenType.VALUE || arg.type == TokenType.CMP || arg.type == TokenType.LABEL;
        }

        private static byte[] StrToBytes(string str) {
            byte len = (byte)str.Length;

            byte[] res = new byte[1 + len];

            res[0] = len;
            for(int j = 0; j < str.Length; j++) {
                res[j + 1] = (byte) str[j];
            }

            return res;
        }

        private static byte[] ValToBytes(Token val) {
            if(val.type == TokenType.CMP) {
                switch(val.value) {
                    case "<":
                        return new byte[] { 0x00 };
                    case ">":
                        return new byte[] { 0x01 };
                    case "<=":
                        return new byte[] { 0x02 };
                    case ">=":
                        return new byte[] { 0x03 };
                    case "==":
                        return new byte[] { 0x04 };
                    case "!=":
                        return new byte[] { 0x05 };
                }
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
