using System.Text;

namespace Rainbow.Marshalling;

public static class Marshalling
{
    public static float ToFloat32(byte[] arr)
    {
        return BitConverter.ToSingle(arr, 0);
    }

    public static double ToFloat64(byte[] arr)
    {
        return BitConverter.ToDouble(arr, 0);
    }

    public static char ToChar(byte[] arr)
    {
        return BitConverter.ToChar(arr, 0);
    }

    public static half ToFloat16(byte[] arr)
    {
        return BitConverter.ToHalf(arr, 0);
    }

    public static byte ToUint8(byte[] arr)
    {
        return arr[0];
    }

    public static ulong ToUint64(byte[] arr)
    {
        return BitConverter.ToUInt64(arr, 0);
    }

    public static uint ToUint32(byte[] arr)
    {
        return BitConverter.ToUInt32(arr, 0);
    }

    public static ushort ToUint16(byte[] arr)
    {
        return BitConverter.ToUInt16(arr, 0);
    }

    public static sbyte ToInt8(byte[] arr)
    {
        return (sbyte)arr[0];
    }

    public static long ToInt16(byte[] arr)
    {
        return BitConverter.ToInt16(arr, 0);
    }

    public static int ToInt32(byte[] arr)
    {
        return BitConverter.ToInt32(arr, 0);
    }

    public static long ToInt64(byte[] arr)
    {
        return BitConverter.ToInt64(arr, 0);
    }

    public static string ToString(byte[] arr) {
        return Encoding.UTF8.GetString(arr);
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