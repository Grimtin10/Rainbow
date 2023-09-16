using System.Text;

namespace Rainbow.Marshalling;

public static class Converter {
    public static byte ToUInt8(byte[] arr) {
        return arr[0];
    }

    public static ushort ToUInt16(byte[] arr) {
        byte[] copy = new byte[arr.Length];
        Array.Copy(arr, copy, arr.Length);
        if(BitConverter.IsLittleEndian) {
            Array.Reverse(copy);
        }
        return BitConverter.ToUInt16(copy);
    }

    public static uint ToUInt32(byte[] arr) {
        byte[] copy = new byte[arr.Length];
        Array.Copy(arr, copy, arr.Length);
        if(BitConverter.IsLittleEndian) {
            Array.Reverse(copy);
        }
        return BitConverter.ToUInt32(copy);
    }

    public static ulong ToUInt64(byte[] arr) {
        byte[] copy = new byte[arr.Length];
        Array.Copy(arr, copy, arr.Length);
        if(BitConverter.IsLittleEndian) {
            Array.Reverse(copy);
        }
        return BitConverter.ToUInt64(copy);
    }

    public static sbyte ToInt8(byte[] arr) {
        return (sbyte) arr[0];
    }

    public static short ToInt16(byte[] arr) {
        byte[] copy = new byte[arr.Length];
        Array.Copy(arr, copy, arr.Length);
        if(BitConverter.IsLittleEndian) {
            Array.Reverse(copy);
        }
        return BitConverter.ToInt16(copy);
    }

    public static int ToInt32(byte[] arr) {
        byte[] copy = new byte[arr.Length];
        Array.Copy(arr, copy, arr.Length);
        if(BitConverter.IsLittleEndian) {
            Array.Reverse(copy);
        }
        return BitConverter.ToInt32(copy);
    }

    public static long ToInt64(byte[] arr) {
        byte[] copy = new byte[arr.Length];
        Array.Copy(arr, copy, arr.Length);
        if(BitConverter.IsLittleEndian) {
            Array.Reverse(copy);
        }
        return BitConverter.ToInt64(copy);
    }

    public static half ToFloat16(byte[] arr) {
        return BitConverter.ToHalf(arr);
    }

    public static float ToFloat32(byte[] arr) {
        return BitConverter.ToSingle(arr);
    }

    public static double ToFloat64(byte[] arr) {
        return BitConverter.ToDouble(arr);
    }

    public static string ToString(byte[] arr) {
        return Encoding.UTF8.GetString(arr);
    }

    public static char ToChar(byte[] arr) {
        return (char) arr[0]; // i'd use bitconverter but chars are UTF-16 so
    }

    // these are all just wrapper functions
    // they make my code shorter so
    #region getbytes
    public static byte[] GetBytes(short b) {
        byte[] bytes = BitConverter.GetBytes(b);
        if(BitConverter.IsLittleEndian) {
            Array.Reverse(bytes);
        }
        return bytes;
    }

    public static byte[] GetBytes(int b) {
        byte[] bytes = BitConverter.GetBytes(b);
        if(BitConverter.IsLittleEndian) {
            Array.Reverse(bytes);
        }
        return bytes;
    }

    public static byte[] GetBytes(long b) {
        byte[] bytes = BitConverter.GetBytes(b);
        if(BitConverter.IsLittleEndian) {
            Array.Reverse(bytes);
        }
        return bytes;
    }

    public static byte[] GetBytes(ushort b) {
        byte[] bytes = BitConverter.GetBytes(b);
        if(BitConverter.IsLittleEndian) {
            Array.Reverse(bytes);
        }
        return bytes;
    }

    public static byte[] GetBytes(uint b) {
        byte[] bytes = BitConverter.GetBytes(b);
        if(BitConverter.IsLittleEndian) {
            Array.Reverse(bytes);
        }
        return bytes;
    }

    public static byte[] GetBytes(ulong b) {
        byte[] bytes = BitConverter.GetBytes(b);
        if(BitConverter.IsLittleEndian) {
            Array.Reverse(bytes);
        }
        return bytes;
    }

    public static byte[] GetBytes(half b) {
        return BitConverter.GetBytes(b);
    }

    public static byte[] GetBytes(float b) {
        return BitConverter.GetBytes(b);
    }

    public static byte[] GetBytes(double b) {
        return BitConverter.GetBytes(b);
    }

    public static byte[] GetBytes(string b) {
        return Encoding.UTF8.GetBytes(b);
    }
    #endregion
}