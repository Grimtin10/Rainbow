using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.Compilation.Assembler {
    internal enum TokenType {
        TYPE,
        STR,
        INSTR,
        VALUE,
        LBRACKET,
        RBRACKET,
    }
}
