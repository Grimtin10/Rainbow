using System.Runtime.InteropServices;

namespace Rainbow.Runtime.FFI;

public class Importer
{
    public unsafe static Func<T> Dlopen<T>(string path)
    {
        void *del = null;
        Func<T> ret = Marshal.GetDelegateForFunctionPointer<Func<T>>(new(del));
        return ret;
    }

    public unsafe static Func<T, T1>? Dlopen<T, T1>(string path)
    {
        return null;
    }

    public unsafe static Func<T, T1, T2>? Dlopen<T, T1, T2>(string path)
    {
        return null;
    }

    public unsafe static Func<T, T1, T2, T3>? Dlopen<T, T1, T2, T3>(string path)
    {
        return null;
    }

    public unsafe static Func<T, T1, T2, T3, T4>? Dlopen<T, T1, T2, T3, T4>(string path)
    {
        return null;
    }

    public unsafe static Func<T, T1, T2, T3, T4, T5>? Dlopen<T, T1, T2, T3, T4, T5>(string path)
    {
        return null;
    }
}