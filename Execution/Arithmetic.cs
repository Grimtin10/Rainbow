namespace Rainbow.Execution {
    internal class Arithmetic {
        #region addition
        public static void Add(byte type1, byte[] val1, byte type2, byte[] val2, ref Instance var) {
            switch((Type)type1) {
                case Type.uint8:
                    AddUInt8(val1[0], type2, val2, ref var);
                    break;
            }
        }

        public static void AddUInt8(byte val1, byte type2, byte[] val2, ref Instance var) {
            switch((Type)type2) {
                case Type.uint8:
                    SetUInt8((byte) (val1 + val2[0]), ref var);
                    break;
            }
        }
        #endregion

        public static void SetUInt8(byte val, ref Instance var) {
            switch(var.type) {
                case Type.uint8:
                    var.data.SetPos(0, val);
                    break;
            }
        }
    }
}
