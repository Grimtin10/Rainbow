using Rainbow.GarbageCollection.GCTypes;
using System.Runtime.InteropServices;

namespace Rainbow.Marshalling;

#region These are all runtime allowed funcs!

public unsafe static class Unsafe
{
    public static Block<byte> UnsafeAlloc(int size = 1)
    {
        IntPtr iptr = Marshal.AllocHGlobal(size);
        byte *ptr = (byte *)iptr.ToPointer();

        return new Block<byte>(ptr, size);
    }

    public static Block<T> UnsafeGenericAlloc<T>(int size) where T: unmanaged
    {
        return new Block<T>((T *)Marshal.AllocHGlobal(sizeof(T) * size), size);
    }

    public static void Free<T>(T *ptr) where T: unmanaged
    {
        Marshal.FreeHGlobal(new(ptr));
    }

    public static T[] ToSafe<T>(Block<byte> ptr) where T: unmanaged
    {
        T[] ret = new T[ptr.length];

        Block<T> marshalled = ptr.MarshalBlock<T>();

        for(int i = 0; i < ptr.length; i++)
        {
            ret[i] = marshalled[i];
        }

        return ret;
    }
}

#endregion