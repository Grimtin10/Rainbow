﻿namespace Rainbow.Execution {
    public class Instructions {
        private static readonly Dictionary<Instruction, int> instructionLengths = new(){
            {Instruction.NOP, 1},
            {Instruction.ADD_RR, 3},
            {Instruction.ADD_RI, 3},
            {Instruction.ADD_IR, 3},
            {Instruction.ADD_II, 3},
            {Instruction.SUB_RR, 3},
            {Instruction.SUB_RI, 3},
            {Instruction.SUB_IR, 3},
            {Instruction.SUB_II, 3},
            {Instruction.RSH_R, 2},
            {Instruction.RSH_I, 2},
            {Instruction.LSH_R, 2},
            {Instruction.LSH_I, 2},
            {Instruction.AND_RR, 3},
            {Instruction.AND_RI, 3},
            {Instruction.AND_IR, 3},
            {Instruction.AND_II, 3},
            {Instruction.OR_RR, 3},
            {Instruction.OR_RI, 3},
            {Instruction.OR_IR, 3},
            {Instruction.OR_II, 3},
            {Instruction.XOR_RR, 3},
            {Instruction.XOR_RI, 3},
            {Instruction.XOR_IR, 3},
            {Instruction.XOR_II, 3},
            {Instruction.NOT_R, 2},
            {Instruction.NOT_I, 2},
            {Instruction.JMP_R, 2},
            {Instruction.JMP_I, 2},
            {Instruction.JMPC_RRRR, 5},
            {Instruction.JMPC_RRRI, 5},
            {Instruction.JMPC_RRIR, 5},
            {Instruction.JMPC_RRII, 5},
            {Instruction.JMPC_RIRR, 5},
            {Instruction.JMPC_RIRI, 5},
            {Instruction.JMPC_RIIR, 5},
            {Instruction.JMPC_RIII, 5},
            {Instruction.JMPC_IRRR, 5},
            {Instruction.JMPC_IRRI, 5},
            {Instruction.JMPC_IRIR, 5},
            {Instruction.JMPC_IRII, 5},
            {Instruction.JMPC_IIRR, 5},
            {Instruction.JMPC_IIRI, 5},
            {Instruction.JMPC_IIIR, 5},
            {Instruction.JMPC_IIII, 5},
            {Instruction.CALL, 2},
            {Instruction.RET_R, 2},
            {Instruction.RET_I, 2},
            {Instruction.PUSH_R, 2},
            {Instruction.PUSH_I, 2},
            {Instruction.POP, 1},
            {Instruction.PEEK_R, 2},
            {Instruction.PEEK_I, 2},
            {Instruction.MOV_R, 2},
            {Instruction.MOV_I, 2},
            {Instruction.ALLOC_R, 2},
            {Instruction.ALLOC_I, 2},
            {Instruction.VALUE, 1},
            {Instruction.SYSCALL_R, 2},
            {Instruction.SYSCALL_I, 2},
        };

        public static Dictionary<Instruction, int> InstructionLengths = instructionLengths;
    }

    // hell
    public enum Instruction {
        NOP = 0,
        ADD_RR,
        ADD_RI,
        ADD_IR,
        ADD_II,
        SUB_RR,
        SUB_RI,
        SUB_IR,
        SUB_II,
        RSH_R,
        RSH_I,
        LSH_R,
        LSH_I,
        AND_RR,
        AND_RI,
        AND_IR,
        AND_II,
        OR_RR,
        OR_RI,
        OR_IR,
        OR_II,
        XOR_RR,
        XOR_RI,
        XOR_IR,
        XOR_II,
        NOT_R,
        NOT_I,
        JMP_R,
        JMP_I,
        JMPC_RRRR,
        JMPC_RRRI,
        JMPC_RRIR,
        JMPC_RRII,
        JMPC_RIRR,
        JMPC_RIRI,
        JMPC_RIIR,
        JMPC_RIII,
        JMPC_IRRR,
        JMPC_IRRI,
        JMPC_IRIR,
        JMPC_IRII,
        JMPC_IIRR,
        JMPC_IIRI,
        JMPC_IIIR,
        JMPC_IIII,
        CALL,
        RET_R,
        RET_I,
        PUSH_R,
        PUSH_I,
        POP,
        PEEK_R,
        PEEK_I,
        MOV_R,
        MOV_I,
        ALLOC_R,
        ALLOC_I,
        VALUE,
        SYSCALL_R,
        SYSCALL_I,
        VARDEF,
    }
}
