using System.Dynamic;
using Rainbow.GarbageCollection.GCTypes;

namespace Rainbow.GarbageCollection.GC2;

public unsafe interface IBlock<T> where T: unmanaged
{
    public T* _ref { get; set; }
    public int Length { get; set; }
    public byte _type { get; set; }
}

public unsafe struct Block<T> : IBlock<T> where T: unmanaged
{
    public T* _ref { get; set; }
    public int Length { get; set; }
    public byte _type { get; set; }

    public Block(T *ptr, int len)
    {
        _ref = ptr;
        Length = len;
    }
}

public unsafe struct Pointer<T> : IBlock<T> where T: unmanaged
{
    public T* _ref { get; set; }
    public int Length { get; set; }
    public byte _type { get; set; }

    public T this[int index] 
    {
        get {
            if(index >= Length) 
            {
                throw new AccessViolationException("Unable to read memory!");
            } else {
                return _ref[index];
            }
        }

        set => SetIndex(index, value);
    }

    public Pointer(T *ptr, int len)
    {
        _ref = ptr;
        Length = len;
    }

    private void SetIndex(int index, T value)
    {
        if(index >= Length) 
        {
            Console.WriteLine(index);
            throw new AccessViolationException("Unable to write memory!");
        } else {
            _ref[index] = value;
        }
    }
}

public unsafe struct Struct : IBlock<byte>
{
    public byte* _ref { get; set; }
    public int Length { get; set; }
    public byte _type { get; set; }

    public List<KeyValuePair<string, Pointer<byte>>> _accessor { get; set; } = new();

    public Struct(byte *ptr, int len)
    {
        _ref = ptr;
        Length = len;
        _type = 0b01100000;
    }
}

public enum BlockType
{
    BLOCK,
    POINTER,
    STRUCT
}