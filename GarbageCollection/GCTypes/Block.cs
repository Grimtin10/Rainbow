using System.Runtime.InteropServices;
using System.Text;

namespace Rainbow.GarbageCollection.GCTypes;

public unsafe struct Block<T> : IDisposable where T : unmanaged 
{
    public T* _ref { get; set; }
    public int length { get; set; }
    public bool isref { get; set; } = false;
    public bool isCollection { get; set; } = false;

    public T this[int index] 
    {
        get {
            if(index >= length) {
                throw new AccessViolationException("Unable to read memory!");
            } else {
                return _ref[index];
            }
        }
    }

    public Block(T* ptr, int len) 
    {
        this._ref = ptr;
        this.length = len;
    }

    public IEnumerator<T> GetEnumerator()
    {
        for(int i = 0; i < length; i++)
        {
            yield return this[i];
        }
    }

    public void SetPos(int index, T value) {
        if(index >= length) {
            Console.WriteLine(index);
            throw new AccessViolationException("Unable to write memory!");
        } else {
            _ref[index] = value;
        }
    }

    public void Dispose() {
        Marshal.FreeHGlobal(new(_ref));
    }

    public static implicit operator T*(Block<T> blk) => blk._ref;

    public bool Equals(Block<T> val) {
        //Console.WriteLine("this: " + (int) _ref + " other: " + (int) val._ref);
        if(this._ref == val._ref && this.length == val.length) {
            return true;
        }

        return false;
    }

    public override string ToString() 
    {
        return (int) _ref + " " + length;
    }
}

public unsafe static class BlockExtensions {
    public static string ReadString(this Block<char> blk)
    {
        StringBuilder bld = new();
        
        for(int i = 0; i < blk.length; i++) 
        {
            bld.Append(blk[i]);
        }

        return bld.ToString();
    }

    public static void FillBytes(this Block<byte> blk, byte[] bytes) 
    {
        for(int i = 0; i < bytes.Length; i++) 
        {
            blk.SetPos(i, bytes[i]);
        }
    }

    public static Block<T> MarshalBlock<T>(this Block<byte> blk, int forcedlen = 0) where T: unmanaged
    {
        if(forcedlen > 0)
        {
            return new Block<T>((T*)blk._ref, forcedlen);
        } else
        {
            return new Block<T>((T*)blk._ref, blk.length / sizeof(T));
        }
    }

    public static Block<byte> GetBlockBytes<T>(this Block<T> blk) where T: unmanaged
    {
        return new Block<byte>((byte *)blk._ref, blk.length * sizeof(T));
    }

    public static byte[] GetBytes(this Block<byte> blk) 
    {
        byte[] bytes = new byte[blk.length];
        
        for(int i = 0; i < blk.length; i++) 
        {
            bytes[i] = blk[i];
        }
        
        return bytes;
    }

    public static string ReadString(this Block<byte> blk) 
    {
        StringBuilder bld = new();

        Block<Block<byte>> deref = blk.MarshalBlock<Block<byte>>();
        
        for(int i = 0; i < blk.length; i++) {
            bld.Append((char) deref[0][i]);
        }

        return bld.ToString();
    }
}