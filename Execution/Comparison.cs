using Rainbow.Exceptions;
using System;

namespace Rainbow.Execution {
    internal class Comparison {
        // actually dying
        // TODO: actually finish lmao
        public static bool Compare(byte type1, byte[] val1, byte type2, byte[] val2, byte cmp) {
            switch((Type) type1) {
                case Type.uint8: {
                    byte v = conv.ToUInt8(val1);
                    return CompareUInt8(v, type2, val2, cmp);
                }
                case Type.int32: {
                    int v = conv.ToInt32(val1);
                    return CompareInt32(v, type2, val2, cmp);
                }
                default:
                    throw new NotImplementedException();
            }
        }

        public static bool CompareUInt8(byte val1, byte type2, byte[] val2, byte cmp) {
            switch((Type) type2) {
                case Type.uint8:
                    byte v = conv.ToUInt8(val2);
                    return Compare(val1, v, cmp);
                default:
                    throw new InvalidArgumentException($"Cannot compare values of type uint8 and {(Type) type2}, most likely not implemented lol");
            }
        }

        public static bool CompareInt32(int val1, byte type2, byte[] val2, byte cmp) {
            switch((Type) type2) {
                case Type.int32:
                    int v = conv.ToInt32(val2);
                    return Compare(val1, v, cmp);
                default:
                    throw new InvalidArgumentException($"Cannot compare values of type int32 and {(Type) type2}, most likely not implemented lol");
            }
        }

        #region generic comparisons
        public static bool Compare(byte v1, byte v2, byte cmp) {
            switch(cmp) {
                case 0:
                    return v1 < v2;
                case 1:
                    return v1 > v2;
                case 2:
                    return v1 <= v2;
                case 3:
                    return v1 >= v2;
                case 4:
                    return v1 == v2;
                case 5:
                    return v1 != v2;
                default:
                    throw new InvalidArgumentException($"Invalid comparison type {cmp}");
            }
        }

        public static bool Compare(int v1, int v2, byte cmp) {
            switch(cmp) {
                case 0:
                    return v1 < v2;
                case 1:
                    return v1 > v2;
                case 2:
                    return v1 <= v2;
                case 3:
                    return v1 >= v2;
                case 4:
                    return v1 == v2;
                case 5:
                    return v1 != v2;
                default:
                    throw new InvalidArgumentException($"Invalid comparison type {cmp}");
            }
        }
        #endregion
    }
}
