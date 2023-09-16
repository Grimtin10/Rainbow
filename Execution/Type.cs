namespace Rainbow.Execution {
    public class Types {
        public static byte[] lengths = new byte[] {
            1, // uint8
            2, // uint16
            4, // uint32
            8, // uint64
            1, // int8
            2, // int16
            4, // int32
            8, // int64
            2, // float16
            4, // float32
            8, // float64
            1, // char
            8, // pointer
            1, // struct
            0, // void
            0, // undefined
        };  
    }

    public enum Type {
        uint8 = 0,
        uint16,
        uint32,
        uint64,
        int8,
        int16,
        int32,
        int64,
        float16,
        float32,
        float64,
        _char,
        pointer,
        _struct,
        _void,
        undefined 
    }
}