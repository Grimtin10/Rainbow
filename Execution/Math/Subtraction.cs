using Rainbow.Exceptions;

namespace Rainbow.Execution.Math {
    // i just replaced all addition with subtraction so pray to god it didnt break it
    internal class Subtraction {
        // TODO FIXME PLEASE HELP GOD: theres gotta be a better way
        public static void SubUInt8(byte val1, byte type2, byte[] val2, ref Instance _var) {
            switch((Type) type2) {
                case Type.uint8: {
                    byte v = conv.ToUInt8(val2);
                    Move.SetUInt8((byte) (val1 - v), ref _var);
                    break;
                }
                case Type.uint16: {
                    ushort v = conv.ToUInt16(val2);
                    Move.SetUInt8((byte) (val1 - v), ref _var);
                    break;
                }
                case Type.uint32: {
                    uint v = conv.ToUInt32(val2);
                    Move.SetUInt8((byte) (val1 - v), ref _var);
                    break;
                }
                case Type.uint64: {
                    ulong v = conv.ToUInt64(val2);
                    Move.SetUInt8((byte) (val1 - v), ref _var);
                    break;
                }
                case Type.int8: {
                    sbyte v = conv.ToInt8(val2);
                    Move.SetUInt8((byte) (val1 - v), ref _var);
                    break;
                }
                case Type.int16: {
                    short v = conv.ToInt16(val2);
                    Move.SetUInt8((byte) (val1 - v), ref _var);
                    break;
                }
                case Type.int32: {
                    int v = conv.ToInt32(val2);
                    Move.SetUInt8((byte) (val1 - v), ref _var);
                    break;
                }
                case Type.int64: {
                    long v = conv.ToInt64(val2);
                    Move.SetUInt8((byte) (val1 - v), ref _var);
                    break;
                }
                case Type.float16: {
                    half v = conv.ToFloat16(val2);
                    Move.SetUInt8((byte) (val1 - v), ref _var);
                    break;
                }
                case Type.float32: {
                    float v = conv.ToFloat32(val2);
                    Move.SetUInt8((byte) (val1 - v), ref _var);
                    break;
                }
                case Type.float64: {
                    double v = conv.ToFloat64(val2);
                    Move.SetUInt8((byte) (val1 - v), ref _var);
                    break;
                }
                case Type._string: {
                    throw new InvalidTypeException("Cannot convert from string to uint8");
                }
                case Type._char: {
                    char v = conv.ToChar(val2);
                    Move.SetUInt8((byte) (val1 - v), ref _var);
                    break;
                }
            }
        }

        public static void SubUInt16(ushort val1, byte type2, byte[] val2, ref Instance _var) {
            switch((Type) type2) {
                case Type.uint8: {
                    byte v = conv.ToUInt8(val2);
                    Move.SetUInt16((ushort) (val1 - v), ref _var);
                    break;
                }
                case Type.uint16: {
                    ushort v = conv.ToUInt16(val2);
                    Move.SetUInt16((ushort) (val1 - v), ref _var);
                    break;
                }
                case Type.uint32: {
                    uint v = conv.ToUInt32(val2);
                    Move.SetUInt16((ushort) (val1 - v), ref _var);
                    break;
                }
                case Type.uint64: {
                    ulong v = conv.ToUInt64(val2);
                    Move.SetUInt16((ushort) (val1 - v), ref _var);
                    break;
                }
                case Type.int8: {
                    sbyte v = conv.ToInt8(val2);
                    Move.SetUInt16((ushort) (val1 - v), ref _var);
                    break;
                }
                case Type.int16: {
                    short v = conv.ToInt16(val2);
                    Move.SetUInt16((ushort) (val1 - v), ref _var);
                    break;
                }
                case Type.int32: {
                    int v = conv.ToInt32(val2);
                    Move.SetUInt16((ushort) (val1 - v), ref _var);
                    break;
                }
                case Type.int64: {
                    long v = conv.ToInt64(val2);
                    Move.SetUInt16((ushort) (val1 - v), ref _var);
                    break;
                }
                case Type.float16: {
                    half v = conv.ToFloat16(val2);
                    Move.SetUInt16((ushort) (val1 - (float) v), ref _var);
                    break;
                }
                case Type.float32: {
                    float v = conv.ToFloat32(val2);
                    Move.SetUInt16((ushort) (val1 - v), ref _var);
                    break;
                }
                case Type.float64: {
                    double v = conv.ToFloat64(val2);
                    Move.SetUInt16((ushort) (val1 - v), ref _var);
                    break;
                }
                case Type._string: {
                    throw new InvalidTypeException("Cannot convert from string to uint8");
                }
                case Type._char: {
                    char v = conv.ToChar(val2);
                    Move.SetUInt16((ushort) (val1 - v), ref _var);
                    break;
                }
            }
        }

        public static void SubUInt32(uint val1, byte type2, byte[] val2, ref Instance _var) {
            switch((Type) type2) {
                case Type.uint8: {
                    byte v = conv.ToUInt8(val2);
                    Move.SetUInt32(val1 - v, ref _var);
                    break;
                }
                case Type.uint16: {
                    ushort v = conv.ToUInt16(val2);
                    Move.SetUInt32(val1 - v, ref _var);
                    break;
                }
                case Type.uint32: {
                    uint v = conv.ToUInt32(val2);
                    Move.SetUInt32(val1 - v, ref _var);
                    break;
                }
                case Type.uint64: {
                    ulong v = conv.ToUInt64(val2);
                    Move.SetUInt32((uint) (val1 - v), ref _var);
                    break;
                }
                case Type.int8: {
                    sbyte v = conv.ToInt8(val2);
                    Move.SetUInt32((uint) (val1 - v), ref _var);
                    break;
                }
                case Type.int16: {
                    short v = conv.ToInt16(val2);
                    Move.SetUInt32((uint) (val1 - v), ref _var);
                    break;
                }
                case Type.int32: {
                    int v = conv.ToInt32(val2);
                    Move.SetUInt32((uint) (val1 - v), ref _var);
                    break;
                }
                case Type.int64: {
                    long v = conv.ToInt64(val2);
                    Move.SetUInt32((uint) (val1 - v), ref _var);
                    break;
                }
                case Type.float16: {
                    half v = conv.ToFloat16(val2);
                    Move.SetUInt32((uint) (val1 - (float) v), ref _var);
                    break;
                }
                case Type.float32: {
                    float v = conv.ToFloat32(val2);
                    Move.SetUInt32((uint) (val1 - v), ref _var);
                    break;
                }
                case Type.float64: {
                    double v = conv.ToFloat64(val2);
                    Move.SetUInt32((uint) (val1 - v), ref _var);
                    break;
                }
                case Type._string: {
                    throw new InvalidTypeException("Cannot convert from string to uint8");
                }
                case Type._char: {
                    char v = conv.ToChar(val2);
                    Move.SetUInt32(val1 - v, ref _var);
                    break;
                }
            }
        }

        public static void SubUInt64(ulong val1, byte type2, byte[] val2, ref Instance _var) {
            switch((Type) type2) {
                case Type.uint8: {
                    byte v = conv.ToUInt8(val2);
                    Move.SetUInt64(val1 - v, ref _var);
                    break;
                }
                case Type.uint16: {
                    ushort v = conv.ToUInt16(val2);
                    Move.SetUInt64(val1 - v, ref _var);
                    break;
                }
                case Type.uint32: {
                    uint v = conv.ToUInt32(val2);
                    Move.SetUInt64(val1 - v, ref _var);
                    break;
                }
                case Type.uint64: {
                    ulong v = conv.ToUInt64(val2);
                    Move.SetUInt64(val1 - v, ref _var);
                    break;
                }
                case Type.int8: {
                    sbyte v = conv.ToInt8(val2);
                    Move.SetUInt64(val1 - (byte) v - 128, ref _var);
                    break;
                }
                case Type.int16: {
                    short v = conv.ToInt16(val2);
                    Move.SetUInt64(val1 - (ushort) v - 32767, ref _var);
                    break;
                }
                case Type.int32: {
                    int v = conv.ToInt32(val2);
                    Move.SetUInt64(val1 - (uint) v - 2147483647, ref _var);
                    break;
                }
                case Type.int64: {
                    long v = conv.ToInt64(val2);
                    Move.SetUInt64(val1 - (ulong) v - 9223372036854775807L, ref _var);
                    break;
                }
                case Type.float16: {
                    half v = conv.ToFloat16(val2);
                    Move.SetUInt64((ulong) (val1 - (float) v), ref _var);
                    break;
                }
                case Type.float32: {
                    float v = conv.ToFloat32(val2);
                    Move.SetUInt64((ulong) (val1 - v), ref _var);
                    break;
                }
                case Type.float64: {
                    double v = conv.ToFloat64(val2);
                    Move.SetUInt64((ulong) (val1 - v), ref _var);
                    break;
                }
                case Type._string: {
                    throw new InvalidTypeException("Cannot convert from string to uint8");
                }
                case Type._char: {
                    char v = conv.ToChar(val2);
                    Move.SetUInt64(val1 - v, ref _var);
                    break;
                }
            }
        }

        public static void SubInt8(sbyte val1, byte type2, byte[] val2, ref Instance _var) {
            switch((Type) type2) {
                case Type.uint8: {
                    byte v = conv.ToUInt8(val2);
                    Move.SetInt8((sbyte) (val1 - v), ref _var);
                    break;
                }
                case Type.uint16: {
                    ushort v = conv.ToUInt16(val2);
                    Move.SetInt8((sbyte) (val1 - v), ref _var);
                    break;
                }
                case Type.uint32: {
                    uint v = conv.ToUInt32(val2);
                    Move.SetInt8((sbyte) (val1 - v), ref _var);
                    break;
                }
                case Type.uint64: {
                    ulong v = conv.ToUInt64(val2);
                    Move.SetInt8((sbyte) (val1 - (long) v), ref _var);
                    break;
                }
                case Type.int8: {
                    sbyte v = conv.ToInt8(val2);
                    Move.SetInt8((sbyte) (val1 - v), ref _var);
                    break;
                }
                case Type.int16: {
                    short v = conv.ToInt16(val2);
                    Move.SetInt8((sbyte) (val1 - v), ref _var);
                    break;
                }
                case Type.int32: {
                    int v = conv.ToInt32(val2);
                    Move.SetInt8((sbyte) (val1 - v), ref _var);
                    break;
                }
                case Type.int64: {
                    long v = conv.ToInt64(val2);
                    Move.SetInt8((sbyte) (val1 - v), ref _var);
                    break;
                }
                case Type.float16: {
                    half v = conv.ToFloat16(val2);
                    Move.SetInt8((sbyte) (val1 - v), ref _var);
                    break;
                }
                case Type.float32: {
                    float v = conv.ToFloat32(val2);
                    Move.SetInt8((sbyte) (val1 - v), ref _var);
                    break;
                }
                case Type.float64: {
                    double v = conv.ToFloat64(val2);
                    Move.SetInt8((sbyte) (val1 - v), ref _var);
                    break;
                }
                case Type._string: {
                    throw new InvalidTypeException("Cannot convert from string to uint8");
                }
                case Type._char: {
                    char v = conv.ToChar(val2);
                    Move.SetInt8((sbyte) (val1 - v), ref _var);
                    break;
                }
            }
        }

        public static void SubInt16(short val1, byte type2, byte[] val2, ref Instance _var) {
            switch((Type) type2) {
                case Type.uint8: {
                    byte v = conv.ToUInt8(val2);
                    Move.SetInt16((short) (val1 - v), ref _var);
                    break;
                }
                case Type.uint16: {
                    ushort v = conv.ToUInt16(val2);
                    Move.SetInt16((short) (val1 - v), ref _var);
                    break;
                }
                case Type.uint32: {
                    uint v = conv.ToUInt32(val2);
                    Move.SetInt16((short) (val1 - v), ref _var);
                    break;
                }
                case Type.uint64: {
                    ulong v = conv.ToUInt64(val2);
                    Move.SetInt16((short) (val1 - (long) v), ref _var);
                    break;
                }
                case Type.int8: {
                    sbyte v = conv.ToInt8(val2);
                    Move.SetInt16((short) (val1 - v), ref _var);
                    break;
                }
                case Type.int16: {
                    short v = conv.ToInt16(val2);
                    Move.SetInt16((short) (val1 - v), ref _var);
                    break;
                }
                case Type.int32: {
                    int v = conv.ToInt32(val2);
                    Move.SetInt16((short) (val1 - v), ref _var);
                    break;
                }
                case Type.int64: {
                    long v = conv.ToInt64(val2);
                    Move.SetInt16((short) (val1 - v), ref _var);
                    break;
                }
                case Type.float16: {
                    half v = conv.ToFloat16(val2);
                    Move.SetInt16((short) (val1 - (float) v), ref _var);
                    break;
                }
                case Type.float32: {
                    float v = conv.ToFloat32(val2);
                    Move.SetInt16((short) (val1 - v), ref _var);
                    break;
                }
                case Type.float64: {
                    double v = conv.ToFloat64(val2);
                    Move.SetInt16((short) (val1 - v), ref _var);
                    break;
                }
                case Type._string: {
                    throw new InvalidTypeException("Cannot convert from string to uint8");
                }
                case Type._char: {
                    char v = conv.ToChar(val2);
                    Move.SetInt16((short) (val1 - v), ref _var);
                    break;
                }
            }
        }

        public static void SubInt32(int val1, byte type2, byte[] val2, ref Instance _var) {
            switch((Type) type2) {
                case Type.uint8: {
                    byte v = conv.ToUInt8(val2);
                    Move.SetInt32(val1 - v, ref _var);
                    break;
                }
                case Type.uint16: {
                    ushort v = conv.ToUInt16(val2);
                    Move.SetInt32(val1 - v, ref _var);
                    break;
                }
                case Type.uint32: {
                    uint v = conv.ToUInt32(val2);
                    Move.SetInt32((int) (val1 - v), ref _var);
                    break;
                }
                case Type.uint64: {
                    ulong v = conv.ToUInt64(val2);
                    Move.SetInt32((int) (val1 - (long) v), ref _var);
                    break;
                }
                case Type.int8: {
                    sbyte v = conv.ToInt8(val2);
                    Move.SetInt32(val1 - v, ref _var);
                    break;
                }
                case Type.int16: {
                    short v = conv.ToInt16(val2);
                    Move.SetInt32(val1 - v, ref _var);
                    break;
                }
                case Type.int32: {
                    int v = conv.ToInt32(val2);
                    Move.SetInt32(val1 - v, ref _var);
                    break;
                }
                case Type.int64: {
                    long v = conv.ToInt64(val2);
                    Move.SetInt32((int) (val1 - v), ref _var);
                    break;
                }
                case Type.float16: {
                    half v = conv.ToFloat16(val2);
                    Move.SetInt32((int) (val1 - (float) v), ref _var);
                    break;
                }
                case Type.float32: {
                    float v = conv.ToFloat32(val2);
                    Move.SetInt32((int) (val1 - v), ref _var);
                    break;
                }
                case Type.float64: {
                    double v = conv.ToFloat64(val2);
                    Move.SetInt32((int) (val1 - v), ref _var);
                    break;
                }
                case Type._string: {
                    throw new InvalidTypeException("Cannot convert from string to uint8");
                }
                case Type._char: {
                    char v = conv.ToChar(val2);
                    Move.SetInt32(val1 - v, ref _var);
                    break;
                }
            }
        }

        public static void SubInt64(long val1, byte type2, byte[] val2, ref Instance _var) {
            switch((Type) type2) {
                case Type.uint8: {
                    byte v = conv.ToUInt8(val2);
                    Move.SetInt64(val1 - v, ref _var);
                    break;
                }
                case Type.uint16: {
                    ushort v = conv.ToUInt16(val2);
                    Move.SetInt64(val1 - v, ref _var);
                    break;
                }
                case Type.uint32: {
                    uint v = conv.ToUInt32(val2);
                    Move.SetInt64(val1 - v, ref _var);
                    break;
                }
                case Type.uint64: {
                    ulong v = conv.ToUInt64(val2);
                    Move.SetInt64(val1 - (long) v, ref _var);
                    break;
                }
                case Type.int8: {
                    sbyte v = conv.ToInt8(val2);
                    Move.SetInt64(val1 - v, ref _var);
                    break;
                }
                case Type.int16: {
                    short v = conv.ToInt16(val2);
                    Move.SetInt64(val1 - v, ref _var);
                    break;
                }
                case Type.int32: {
                    int v = conv.ToInt32(val2);
                    Move.SetInt64(val1 - v, ref _var);
                    break;
                }
                case Type.int64: {
                    long v = conv.ToInt64(val2);
                    Move.SetInt64(val1 - v, ref _var);
                    break;
                }
                case Type.float16: {
                    half v = conv.ToFloat16(val2);
                    Move.SetInt64((long) (val1 - (float) v), ref _var);
                    break;
                }
                case Type.float32: {
                    float v = conv.ToFloat32(val2);
                    Move.SetInt64((long) (val1 - v), ref _var);
                    break;
                }
                case Type.float64: {
                    double v = conv.ToFloat64(val2);
                    Move.SetInt64((long) (val1 - v), ref _var);
                    break;
                }
                case Type._string: {
                    throw new InvalidTypeException("Cannot convert from string to uint8");
                }
                case Type._char: {
                    char v = conv.ToChar(val2);
                    Move.SetInt64(val1 - v, ref _var);
                    break;
                }
            }
        }

        public static void SubFloat16(half val1, byte type2, byte[] val2, ref Instance _var) {
            switch((Type) type2) {
                case Type.uint8: {
                    byte v = conv.ToUInt8(val2);
                    Move.SetFloat16(val1 - v, ref _var);
                    break;
                }
                case Type.uint16: {
                    ushort v = conv.ToUInt16(val2);
                    Move.SetFloat16((half) ((float) val1 - v), ref _var);
                    break;
                }
                case Type.uint32: {
                    uint v = conv.ToUInt32(val2);
                    Move.SetFloat16((half) ((float) val1 - v), ref _var);
                    break;
                }
                case Type.uint64: {
                    ulong v = conv.ToUInt64(val2);
                    Move.SetFloat16((half) ((float) val1 - v), ref _var);
                    break;
                }
                case Type.int8: {
                    sbyte v = conv.ToInt8(val2);
                    Move.SetFloat16(val1 - v, ref _var);
                    break;
                }
                case Type.int16: {
                    short v = conv.ToInt16(val2);
                    Move.SetFloat16((half) ((float) val1 - v), ref _var);
                    break;
                }
                case Type.int32: {
                    int v = conv.ToInt32(val2);
                    Move.SetFloat16((half) ((float) val1 - v), ref _var);
                    break;
                }
                case Type.int64: {
                    long v = conv.ToInt64(val2);
                    Move.SetFloat16((half) ((float) val1 - v), ref _var);
                    break;
                }
                case Type.float16: {
                    half v = conv.ToFloat16(val2);
                    Move.SetFloat16(val1 - v, ref _var);
                    break;
                }
                case Type.float32: {
                    float v = conv.ToFloat32(val2);
                    Move.SetFloat16((half) ((float) val1 - v), ref _var);
                    break;
                }
                case Type.float64: {
                    double v = conv.ToFloat64(val2);
                    Move.SetFloat16((half) ((float) val1 - v), ref _var);
                    break;
                }
                case Type._string: {
                    throw new InvalidTypeException("Cannot convert from string to uint8");
                }
                case Type._char: {
                    char v = conv.ToChar(val2);
                    Move.SetFloat16((half) ((float) val1 - v), ref _var);
                    break;
                }
            }
        }

        public static void SubFloat32(float val1, byte type2, byte[] val2, ref Instance _var) {
            switch((Type) type2) {
                case Type.uint8: {
                    byte v = conv.ToUInt8(val2);
                    Move.SetFloat32((float) (val1 - v), ref _var);
                    break;
                }
                case Type.uint16: {
                    ushort v = conv.ToUInt16(val2);
                    Move.SetFloat32((float) (val1 - v), ref _var);
                    break;
                }
                case Type.uint32: {
                    uint v = conv.ToUInt32(val2);
                    Move.SetFloat32((float) (val1 - v), ref _var);
                    break;
                }
                case Type.uint64: {
                    ulong v = conv.ToUInt64(val2);
                    Move.SetFloat32((float) (val1 - v), ref _var);
                    break;
                }
                case Type.int8: {
                    sbyte v = conv.ToInt8(val2);
                    Move.SetFloat32((float) (val1 - v), ref _var);
                    break;
                }
                case Type.int16: {
                    short v = conv.ToInt16(val2);
                    Move.SetFloat32((float) (val1 - v), ref _var);
                    break;
                }
                case Type.int32: {
                    int v = conv.ToInt32(val2);
                    Move.SetFloat32((float) (val1 - v), ref _var);
                    break;
                }
                case Type.int64: {
                    long v = conv.ToInt64(val2);
                    Move.SetFloat32((float) (val1 - v), ref _var);
                    break;
                }
                case Type.float16: {
                    half v = conv.ToFloat16(val2);
                    Move.SetFloat32((float) (val1 - (float) v), ref _var);
                    break;
                }
                case Type.float32: {
                    float v = conv.ToFloat32(val2);
                    Move.SetFloat32((float) (val1 - v), ref _var);
                    break;
                }
                case Type.float64: {
                    double v = conv.ToFloat64(val2);
                    Move.SetFloat32((float) (val1 - v), ref _var);
                    break;
                }
                case Type._string: {
                    throw new InvalidTypeException("Cannot convert from string to uint8");
                }
                case Type._char: {
                    char v = conv.ToChar(val2);
                    Move.SetFloat32((float) (val1 - v), ref _var);
                    break;
                }
            }
        }

        public static void SubFloat64(double val1, byte type2, byte[] val2, ref Instance _var) {
            switch((Type) type2) {
                case Type.uint8: {
                    byte v = conv.ToUInt8(val2);
                    Move.SetFloat64(val1 - v, ref _var);
                    break;
                }
                case Type.uint16: {
                    ushort v = conv.ToUInt16(val2);
                    Move.SetFloat64(val1 - v, ref _var);
                    break;
                }
                case Type.uint32: {
                    uint v = conv.ToUInt32(val2);
                    Move.SetFloat64(val1 - v, ref _var);
                    break;
                }
                case Type.uint64: {
                    ulong v = conv.ToUInt64(val2);
                    Move.SetFloat64((double) (val1 - v), ref _var);
                    break;
                }
                case Type.int8: {
                    sbyte v = conv.ToInt8(val2);
                    Move.SetFloat64((double) (val1 - v), ref _var);
                    break;
                }
                case Type.int16: {
                    short v = conv.ToInt16(val2);
                    Move.SetFloat64((double) (val1 - v), ref _var);
                    break;
                }
                case Type.int32: {
                    int v = conv.ToInt32(val2);
                    Move.SetFloat64((double) (val1 - v), ref _var);
                    break;
                }
                case Type.int64: {
                    long v = conv.ToInt64(val2);
                    Move.SetFloat64((double) (val1 - v), ref _var);
                    break;
                }
                case Type.float16: {
                    half v = conv.ToFloat16(val2);
                    Move.SetFloat64((double) (val1 - (float) v), ref _var);
                    break;
                }
                case Type.float32: {
                    float v = conv.ToFloat32(val2);
                    Move.SetFloat64((double) (val1 - v), ref _var);
                    break;
                }
                case Type.float64: {
                    double v = conv.ToFloat64(val2);
                    Move.SetFloat64((double) (val1 - v), ref _var);
                    break;
                }
                case Type._string: {
                    throw new InvalidTypeException("Cannot convert from string to uint8");
                }
                case Type._char: {
                    char v = conv.ToChar(val2);
                    Move.SetFloat64(val1 - v, ref _var);
                    break;
                }
            }
        }

        // this isnt even a thing
        // you cant do this
        public static void SubString(string val1, byte type2, byte[] val2, ref Instance _var) {
            throw new InvalidTypeException("Cannot subtract strings");
        }
    }
}
