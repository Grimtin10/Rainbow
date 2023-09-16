namespace Rainbow.Execution.Math {
    internal class Arithmetic {
        public static void Add(byte type1, byte[] val1, byte type2, byte[] val2, ref Instance var) {
            switch((Type) type1) {
                case Type.uint8:
                    add.AddUInt8(conv.ToUInt8(val1), type2, val2, ref var);
                    break;
                case Type.uint16:
                    add.AddUInt16(conv.ToUInt16(val1), type2, val2, ref var);
                    break;
                case Type.uint32:
                    add.AddUInt32(conv.ToUInt32(val1), type2, val2, ref var);
                    break;
                case Type.uint64:
                    add.AddUInt64(conv.ToUInt64(val1), type2, val2, ref var);
                    break;
                case Type.int8:
                    add.AddInt8(conv.ToInt8(val1), type2, val2, ref var);
                    break;
                case Type.int16:
                    add.AddInt16(conv.ToInt16(val1), type2, val2, ref var);
                    break;
                case Type.int32:
                    add.AddInt32(conv.ToInt32(val1), type2, val2, ref var);
                    break;
                case Type.int64:
                    add.AddInt64(conv.ToInt64(val1), type2, val2, ref var);
                    break;
                case Type.float16:
                    add.AddFloat16(conv.ToFloat16(val1), type2, val2, ref var);
                    break;
                case Type.float32:
                    add.AddFloat32(conv.ToFloat32(val1), type2, val2, ref var);
                    break;
                case Type.float64:
                    add.AddFloat64(conv.ToFloat64(val1), type2, val2, ref var);
                    break;
                case Type._string:
                    add.AddString(conv.ToString(val1), type2, val2, ref var);
                    break;
                case Type._char:
                    add.AddUInt8(conv.ToUInt8(val1), type2, val2, ref var);
                    break;
            }
        }

        public static void Sub(byte type1, byte[] val1, byte type2, byte[] val2, ref Instance var) {
            switch((Type) type1) {
                case Type.uint8:
                    sub.SubUInt8(conv.ToUInt8(val1), type2, val2, ref var);
                    break;
                case Type.uint16:
                    sub.SubUInt16(conv.ToUInt16(val1), type2, val2, ref var);
                    break;
                case Type.uint32:
                    sub.SubUInt32(conv.ToUInt32(val1), type2, val2, ref var);
                    break;
                case Type.uint64:
                    sub.SubUInt64(conv.ToUInt64(val1), type2, val2, ref var);
                    break;
                case Type.int8:
                    sub.SubInt8(conv.ToInt8(val1), type2, val2, ref var);
                    break;
                case Type.int16:
                    sub.SubInt16(conv.ToInt16(val1), type2, val2, ref var);
                    break;
                case Type.int32:
                    sub.SubInt32(conv.ToInt32(val1), type2, val2, ref var);
                    break;
                case Type.int64:
                    sub.SubInt64(conv.ToInt64(val1), type2, val2, ref var);
                    break;
                case Type.float16:
                    sub.SubFloat16(conv.ToFloat16(val1), type2, val2, ref var);
                    break;
                case Type.float32:
                    sub.SubFloat32(conv.ToFloat32(val1), type2, val2, ref var);
                    break;
                case Type.float64:
                    sub.SubFloat64(conv.ToFloat64(val1), type2, val2, ref var);
                    break;
                case Type._string:
                    sub.SubString(conv.ToString(val1), type2, val2, ref var);
                    break;
                case Type._char:
                    sub.SubUInt8(conv.ToUInt8(val1), type2, val2, ref var);
                    break;
            }
        }
    }
}
