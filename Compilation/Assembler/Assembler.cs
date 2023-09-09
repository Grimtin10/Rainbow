using Rainbow.Exceptions;
using System.Text;

namespace Rainbow.Compilation.Assembler {
    public class Assembler {
        public static void Assemble(string file) {
            string[] asm = File.ReadAllLines(file);

            List<byte> output = new();

            List<Token> tokens = Lexer.Lex(asm);

            for(int i = 0; i < tokens.Count; i++) {
                Console.WriteLine(tokens[i]);
                if(tokens[i].type == TokenType.TYPE) {
                    if(tokens[i + 2].type == TokenType.LPAREN) {
                        output.Add(0xFF);
                    }
                }

                switch(tokens[i].type) {
                    case TokenType.TYPE: {
                        output.Add(Lexer.types[tokens[i].value]);
                        break;
                    }
                    case TokenType.STR: {
                        byte len = (byte)tokens[i].value.Length;
                        byte[] str = Encoding.UTF8.GetBytes(tokens[i].value);

                        output.Add(len);
                        foreach(byte b in str) {
                            output.Add(b);
                        }
                        break;
                    }
                    case TokenType.SYSCALL: {
                        output.Add(Lexer.syscalls[tokens[i].value]);
                        break;
                    }
                    case TokenType.INSTR: {
                        byte[] instr = ParseInstr(tokens, ref i);
                        foreach(byte b in instr) {
                            output.Add(b);
                        }
                        break;
                    }
                }
            }

            for(int i=0;i<output.Count;i++) {
                Console.Write(string.Format("{0:X2} ", output[i]));
            }
        }

        private static byte[] ParseInstr(List<Token> tokens, ref int i) {
            string instr = tokens[i].value;
            switch(instr) {
                case "VARDEF":
                    if() { 
                        
                    }
                    string type = tokens[i++ + 1].value;
                    break;
                default:
                    throw new UnknownInstrException("Unknown instruction " + instr);
            }
        }
    }
}
