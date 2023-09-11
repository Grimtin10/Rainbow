using System.Text;

namespace Rainbow.Marshalling;

public static class Converter
{
    public static byte ToUInt8(byte[] arr) {
        return arr[0];
    }

    public static ushort ToUInt16(byte[] arr) {
        if(BitConverter.IsLittleEndian) {
            Array.Reverse(arr);
        }
        return BitConverter.ToUInt16(arr, 0);
    }

    public static uint ToUInt32(byte[] arr) {
        if(BitConverter.IsLittleEndian) {
            Array.Reverse(arr);
        }
        return BitConverter.ToUInt32(arr, 0);
    }

    public static ulong ToUInt64(byte[] arr) {
        if(BitConverter.IsLittleEndian) {
            Array.Reverse(arr);
        }
        return BitConverter.ToUInt64(arr, 0);
    }

    public static sbyte ToInt8(byte[] arr) {
        return (sbyte) arr[0];
    }

    public static short ToInt16(byte[] arr) {
        if(BitConverter.IsLittleEndian) {
            Array.Reverse(arr);
        }
        return BitConverter.ToInt16(arr, 0);
    }

    public static int ToInt32(byte[] arr) {
        if(BitConverter.IsLittleEndian) {
            Array.Reverse(arr);
        }
        return BitConverter.ToInt32(arr, 0);
    }

    public static long ToInt64(byte[] arr) {
        if(BitConverter.IsLittleEndian) {
            Array.Reverse(arr);
        }
        return BitConverter.ToInt64(arr, 0);
    }

    public static half ToFloat16(byte[] arr) {
        return BitConverter.ToHalf(arr, 0);
    }

    public static float ToFloat32(byte[] arr)
    {
        return BitConverter.ToSingle(arr, 0);
    }

    public static double ToFloat64(byte[] arr)
    {
        return BitConverter.ToDouble(arr, 0);
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
        return BitConverter.GetBytes(b);
    }

    public static byte[] GetBytes(int b) {
        return BitConverter.GetBytes(b);
    }

    public static byte[] GetBytes(long b) {
        return BitConverter.GetBytes(b);
    }

    public static byte[] GetBytes(ushort b) {
        return BitConverter.GetBytes(b);
    }

    public static byte[] GetBytes(uint b) {
        return BitConverter.GetBytes(b);
    }

    public static byte[] GetBytes(ulong b) {
        return BitConverter.GetBytes(b);
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