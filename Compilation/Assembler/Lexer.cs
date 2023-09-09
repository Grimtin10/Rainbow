namespace Rainbow.Compilation.Assembler {
    internal class Lexer {
        public static List<Token> Lex(string[] input) {
            List<Token> tokens = new List<Token>();

            for(int i = 0; i < input.Length; i++) {
                string line = input[i];

                string curString = "";
                for(int j = 0; j < line.Length; j++) {
                    switch(line[j]) {
                        case ' ':
                        case ',':
                            if(curString.Trim().Length > 0) {
                                tokens.Add(TokenizeString(curString.Trim()));
                                curString = "";
                            }
                            break;
                        case '*':
                            if(curString.Trim().Length > 0) {
                                tokens.Add(TokenizeString(curString.Trim()));
                                curString = "*";

                                tokens.Add(TokenizeString(curString.Trim()));
                                curString = "";
                            }
                            break;
                        case '(':
                        case ')':
                            if(curString.Trim().Length > 0) {
                                tokens.Add(TokenizeString(curString.Trim()));

                                curString = "" + line[j];

                                tokens.Add(TokenizeString(curString.Trim()));

                                curString = "";
                            }
                            break;
                        default:
                            curString += line[j];
                            break;
                    }
                }

                if(curString.Trim().Length > 0) {
                    tokens.Add(TokenizeString(curString.Trim()));
                }
            }

            return tokens;
        }

        private static Token TokenizeString(string input) {
            if(types.ContainsKey(input)) {
                return new Token(TokenType.TYPE, input);
            } else if(instructions.Contains(input)) {
                return new Token(TokenType.INSTR, input);
            } else if(int.TryParse(input, out _) || long.TryParse(input, out _) || float.TryParse(input, out _) || double.TryParse(input, out _)) {
                return new Token(TokenType.VALUE, input); // horrid if statement
            } else if(input.Equals("{")) {
                return new Token(TokenType.LBRACKET, input);
            } else if(input.Equals("}")) {
                return new Token(TokenType.RBRACKET, input);
            } else if(input.Equals("(")) {
                return new Token(TokenType.LPAREN, input);
            } else if(input.Equals(")")) {
                return new Token(TokenType.RPAREN, input);
            } else { // if you dont know what it is, it might be a string
                return new Token(TokenType.STR, input);
            }
        }

        public static Dictionary<String, byte> types = new () {
            { "uint8",   0x00 },
            { "uint16",  0x01 },
            { "uint32",  0x02 },
            { "uint64",  0x03 },
            { "int8",    0x04 },
            { "int16",   0x05 },
            { "int32",   0x06 },
            { "int64",   0x07 },
            { "float16", 0x08 },
            { "float32", 0x09 },
            { "float64", 0x0A },
            { "string",  0x0B },
            { "object",  0x0C },
            { "char",    0x0D },
            { "*",       0x0E },
            { "void",    0x0F },
        };

        public static List<String> instructions = new () {
            "NOP",
            "ADD",
            "SUB",
            "RSH",
            "LSH",
            "AND",
            "OR",
            "XOR",
            "NOT",
            "JMP",
            "JMPC",
            "CALL",
            "RET",
            "PUSH",
            "POP",
            "PEEK",
            "MOV",
            "ALLOC",
            "VALUE",
            "VARDEF",
            "SYSCALL",
        };
    }
}
