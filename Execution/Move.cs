using Rainbow.Exceptions;

namespace Rainbow.Execution {
    // this is literally implicit type conversion
    // TODO: all this shit is untested pls test
    internal class Move {
        public static void SetUInt8(byte val, ref Instance var) {
            switch(var.type) {
                case Type.uint8:
                case Type.uint16:
                case Type.uint32:
                case Type.uint64:
                case Type.int8:
                case Type.int16:
                case Type.int32:
                case Type.int64: {
                    var.SetPos(0, val);
                    break;
                }
                case Type.float16: {
                    var.FillBytes(conv.GetBytes((half) val));
                    break;
                }
                case Type.float32: {
                    var.FillBytes(conv.GetBytes((float) val));
                    break;
                }
                case Type.float64: {
                    var.FillBytes(conv.GetBytes((double) val));
                    break;
                }
                case Type._string: {
                    var.FillBytes(conv.GetBytes(val.ToString()));
                    break;
                }
                case Type._char: {
                    var.SetPos(0, val);
                    break;
                }
            }
        }

        public static void SetUInt16(ushort val, ref Instance var) {
            switch(var.type) {
                case Type.uint8:
                case Type.int8:
                    var.SetPos(0, (byte) val);
                    break;
                case Type.uint16:
                case Type.uint32:
                case Type.uint64: {
                    var.FillBytes(conv.GetBytes(val));
                    break;
                }
                case Type.int16:
                case Type.int32:
                case Type.int64: {
                    var.FillBytes(conv.GetBytes((short) val));
                    break;
                }
                case Type.float16: {
                    var.FillBytes(conv.GetBytes((half) val));
                    break;
                }
                case Type.float32: {
                    var.FillBytes(conv.GetBytes((float) val));
                    break;
                }
                case Type.float64: {
                    var.FillBytes(conv.GetBytes((double) val));
                    break;
                }
                case Type._string: {
                    var.FillBytes(conv.GetBytes(val.ToString()));
                    break;
                }
                case Type._char: {
                    var.SetPos(0, (byte) val);
                    break;
                }
            }
        }

        public static void SetUInt32(uint val, ref Instance var) {
            switch(var.type) {
                case Type.uint8:
                case Type.int8:
                    var.SetPos(0, (byte) val);
                    break;
                case Type.uint16:
                    var.FillBytes(conv.GetBytes((ushort) val));
                    break;
                case Type.int16:
                    var.FillBytes(conv.GetBytes((short) val));
                    break;
                case Type.uint32:
                case Type.uint64: {
                    var.FillBytes(conv.GetBytes(val));
                    break;
                }
                case Type.int32:
                case Type.int64: {
                    var.FillBytes(conv.GetBytes((int) val));
                    break;
                }
                case Type.float16: {
                    var.FillBytes(conv.GetBytes((half) val));
                    break;
                }
                case Type.float32: {
                    var.FillBytes(conv.GetBytes((float) val));
                    break;
                }
                case Type.float64: {
                    var.FillBytes(conv.GetBytes((double) val));
                    break;
                }
                case Type._string: {
                    var.FillBytes(conv.GetBytes(val.ToString()));
                    break;
                }
                case Type._char: {
                    var.SetPos(0, (byte) val);
                    break;
                }
            }
        }

        public static void SetUInt64(ulong val, ref Instance var) {
            switch(var.type) {
                case Type.uint8:
                case Type.int8:
                    var.SetPos(0, (byte) val);
                    break;
                case Type.uint16:
                    var.FillBytes(conv.GetBytes((ushort) val));
                    break;
                case Type.int16:
                    var.FillBytes(conv.GetBytes((short) val));
                    break;
                case Type.uint32:
                    var.FillBytes(conv.GetBytes((uint) val));
                    break;
                case Type.int32:
                    var.FillBytes(conv.GetBytes((int) val));
                    break;
                case Type.uint64: {
                    var.FillBytes(conv.GetBytes(val));
                    break;
                }
                case Type.int64: {
                    var.FillBytes(conv.GetBytes((long) val));
                    break;
                }
                case Type.float16: {
                    var.FillBytes(conv.GetBytes((half) val));
                    break;
                }
                case Type.float32: {
                    var.FillBytes(conv.GetBytes((float) val));
                    break;
                }
                case Type.float64: {
                    var.FillBytes(conv.GetBytes((double) val));
                    break;
                }
                case Type._string: {
                    var.FillBytes(conv.GetBytes(val.ToString()));
                    break;
                }
                case Type._char: {
                    var.SetPos(0, (byte) val);
                    break;
                }
            }
        }

        public static void SetInt8(sbyte val, ref Instance var) {
            switch(var.type) {
                case Type.uint8:
                case Type.uint16:
                case Type.uint32:
                case Type.uint64:
                case Type.int8:
                case Type.int16:
                case Type.int32:
                case Type.int64: {
                    var.SetPos(0, (byte) val);
                    break;
                }
                case Type.float16: {
                    var.FillBytes(conv.GetBytes((half) val));
                    break;
                }
                case Type.float32: {
                    var.FillBytes(conv.GetBytes((float) val));
                    break;
                }
                case Type.float64: {
                    var.FillBytes(conv.GetBytes((double) val));
                    break;
                }
                case Type._string: {
                    var.FillBytes(conv.GetBytes(val.ToString()));
                    break;
                }
                case Type._char: {
                    var.SetPos(0, (byte) val);
                    break;
                }
            }
        }

        public static void SetInt16(short val, ref Instance var) {
            switch(var.type) {
                case Type.uint8:
                case Type.int8:
                    var.SetPos(0, (byte) val);
                    break;
                case Type.uint16:
                case Type.uint32:
                case Type.uint64: {
                    var.FillBytes(conv.GetBytes((ushort) val));
                    break;
                }
                case Type.int16:
                case Type.int32:
                case Type.int64: {
                    var.FillBytes(conv.GetBytes(val));
                    break;
                }
                case Type.float16: {
                    var.FillBytes(conv.GetBytes((half) val));
                    break;
                }
                case Type.float32: {
                    var.FillBytes(conv.GetBytes((float) val));
                    break;
                }
                case Type.float64: {
                    var.FillBytes(conv.GetBytes((double) val));
                    break;
                }
                case Type._string: {
                    var.FillBytes(conv.GetBytes(val.ToString()));
                    break;
                }
                case Type._char: {
                    var.SetPos(0, (byte) val);
                    break;
                }
            }
        }

        public static void SetInt32(int val, ref Instance var) {
            switch(var.type) {
                case Type.uint8:
                case Type.int8:
                    var.SetPos(0, (byte) val);
                    break;
                case Type.uint16:
                    var.FillBytes(conv.GetBytes((ushort) val));
                    break;
                case Type.int16:
                    var.FillBytes(conv.GetBytes((short) val));
                    break;
                case Type.uint32:
                case Type.uint64: {
                    var.FillBytes(conv.GetBytes((uint) val));
                    break;
                }
                case Type.int32:
                case Type.int64: {
                    var.FillBytes(conv.GetBytes(val));
                    break;
                }
                case Type.float16: {
                    var.FillBytes(conv.GetBytes((half) val));
                    break;
                }
                case Type.float32: {
                    var.FillBytes(conv.GetBytes((float) val));
                    break;
                }
                case Type.float64: {
                    var.FillBytes(conv.GetBytes((double) val));
                    break;
                }
                case Type._string: {
                    var.FillBytes(conv.GetBytes(val.ToString()));
                    break;
                }
                case Type._char: {
                    var.SetPos(0, (byte) val);
                    break;
                }
            }
        }

        public static void SetInt64(long val, ref Instance var) {
            switch(var.type) {
                case Type.uint8:
                case Type.int8:
                    var.SetPos(0, (byte) val);
                    break;
                case Type.uint16:
                    var.FillBytes(conv.GetBytes((ushort) val));
                    break;
                case Type.int16:
                    var.FillBytes(conv.GetBytes((short) val));
                    break;
                case Type.uint32:
                    var.FillBytes(conv.GetBytes((uint) val));
                    break;
                case Type.int32:
                    var.FillBytes(conv.GetBytes((int) val));
                    break;
                case Type.uint64: {
                    var.FillBytes(conv.GetBytes((ulong) val));
                    break;
                }
                case Type.int64: {
                    var.FillBytes(conv.GetBytes(val));
                    break;
                }
                case Type.float16: {
                    var.FillBytes(conv.GetBytes((half) val));
                    break;
                }
                case Type.float32: {
                    var.FillBytes(conv.GetBytes((float) val));
                    break;
                }
                case Type.float64: {
                    var.FillBytes(conv.GetBytes((double) val));
                    break;
                }
                case Type._string: {
                    var.FillBytes(conv.GetBytes(val.ToString()));
                    break;
                }
                case Type._char: {
                    var.SetPos(0, (byte) val);
                    break;
                }
            }
        }

        public static void SetFloat16(half val, ref Instance var) {
            switch(var.type) {
                case Type.uint8:
                case Type.int8:
                    var.SetPos(0, (byte) val);
                    break;
                case Type.uint16:
                    var.FillBytes(conv.GetBytes((ushort) val));
                    break;
                case Type.int16:
                    var.FillBytes(conv.GetBytes((short) val));
                    break;
                case Type.uint32:
                    var.FillBytes(conv.GetBytes((uint) val));
                    break;
                case Type.int32:
                    var.FillBytes(conv.GetBytes((int) val));
                    break;
                case Type.uint64: {
                    var.FillBytes(conv.GetBytes((ulong) val));
                    break;
                }
                case Type.int64: {
                    var.FillBytes(conv.GetBytes((long) val));
                    break;
                }
                case Type.float16: {
                    var.FillBytes(conv.GetBytes(val));
                    break;
                }
                case Type.float32: {
                    var.FillBytes(conv.GetBytes((float) val));
                    break;
                }
                case Type.float64: {
                    var.FillBytes(conv.GetBytes((double) val));
                    break;
                }
                case Type._string: {
                    var.FillBytes(conv.GetBytes(val.ToString()));
                    break;
                }
                case Type._char: {
                    var.SetPos(0, (byte) val);
                    break;
                }
            }
        }

        public static void SetFloat32(float val, ref Instance var) {
            switch(var.type) {
                case Type.uint8:
                case Type.int8:
                    var.SetPos(0, (byte) val);
                    break;
                case Type.uint16:
                    var.FillBytes(conv.GetBytes((ushort) val));
                    break;
                case Type.int16:
                    var.FillBytes(conv.GetBytes((short) val));
                    break;
                case Type.uint32:
                    var.FillBytes(conv.GetBytes((uint) val));
                    break;
                case Type.int32:
                    var.FillBytes(conv.GetBytes((int) val));
                    break;
                case Type.uint64: {
                    var.FillBytes(conv.GetBytes((ulong) val));
                    break;
                }
                case Type.int64: {
                    var.FillBytes(conv.GetBytes((long) val));
                    break;
                }
                case Type.float16: {
                    var.FillBytes(conv.GetBytes((half) val));
                    break;
                }
                case Type.float32: {
                    var.FillBytes(conv.GetBytes(val));
                    break;
                }
                case Type.float64: {
                    var.FillBytes(conv.GetBytes((double) val));
                    break;
                }
                case Type._string: {
                    var.FillBytes(conv.GetBytes(val.ToString()));
                    break;
                }
                case Type._char: {
                    var.SetPos(0, (byte) val);
                    break;
                }
            }
        }

        public static void SetFloat64(double val, ref Instance var) {
            switch(var.type) {
                case Type.uint8:
                case Type.int8:
                    var.SetPos(0, (byte) val);
                    break;
                case Type.uint16:
                    var.FillBytes(conv.GetBytes((ushort) val));
                    break;
                case Type.int16:
                    var.FillBytes(conv.GetBytes((short) val));
                    break;
                case Type.uint32:
                    var.FillBytes(conv.GetBytes((uint) val));
                    break;
                case Type.int32:
                    var.FillBytes(conv.GetBytes((int) val));
                    break;
                case Type.uint64: {
                    var.FillBytes(conv.GetBytes((ulong) val));
                    break;
                }
                case Type.int64: {
                    var.FillBytes(conv.GetBytes((long) val));
                    break;
                }
                case Type.float16: {
                    var.FillBytes(conv.GetBytes((half) val));
                    break;
                }
                case Type.float32: {
                    var.FillBytes(conv.GetBytes((float) val));
                    break;
                }
                case Type.float64: {
                    var.FillBytes(conv.GetBytes(val));
                    break;
                }
                case Type._string: {
                    var.FillBytes(conv.GetBytes(val.ToString()));
                    break;
                }
                case Type._char: {
                    var.SetPos(0, (byte) val);
                    break;
                }
            }
        }

        public static void SetString(string val, ref Instance var) {
            switch(var.type) {
                case Type.uint8:
                case Type.int8:
                case Type.uint16:
                case Type.int16:
                case Type.uint32:
                case Type.int32:
                case Type.uint64:
                case Type.int64:
                case Type.float16:
                case Type.float32:
                case Type.float64:
                    throw new InvalidTypeException("Cannot convert from string to " + var.type);
                case Type._string: {
                    var.FillBytes(conv.GetBytes(val.ToString()));
                    break;
                }
                case Type._char: {
                    var.SetPos(0, (byte) val[0]);
                    break;
                }
            }
        }

        public static void SetChar(char val, ref Instance var) {
            switch(var.type) {
                case Type.uint8:
                case Type.int8:
                    var.SetPos(0, (byte) val);
                    break;
                case Type.uint16:
                    var.FillBytes(conv.GetBytes(val));
                    break;
                case Type.int16:
                    var.FillBytes(conv.GetBytes((short) val));
                    break;
                case Type.uint32:
                    var.FillBytes(conv.GetBytes((uint) val));
                    break;
                case Type.int32:
                    var.FillBytes(conv.GetBytes((int) val));
                    break;
                case Type.uint64: {
                    var.FillBytes(conv.GetBytes((ulong) val));
                    break;
                }
                case Type.int64: {
                    var.FillBytes(conv.GetBytes((long) val));
                    break;
                }
                case Type.float16: {
                    var.FillBytes(conv.GetBytes((half) val));
                    break;
                }
                case Type.float32: {
                    var.FillBytes(conv.GetBytes((float) val));
                    break;
                }
                case Type.float64: {
                    var.FillBytes(conv.GetBytes(val));
                    break;
                }
                case Type._string: {
                    var.FillBytes(conv.GetBytes(val.ToString()));
                    break;
                }
                case Type._char: {
                    var.SetPos(0, (byte) val);
                    break;
                }
            }
        }

    }
}
