using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.Compilation.Assembler {
    public class Assembler {
        public static void Assemble(string file) {
            string[] asm = File.ReadAllLines(file);

            List<byte> output = new List<byte>();

            List<Token> tokens = Lexer.Lex(asm);
        }
    }
}
