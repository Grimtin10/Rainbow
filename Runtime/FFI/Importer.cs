namespace Rainbow.Runtime.FFI;

public class Importer
{
    public FileInfo inf { get; set; }

    public Importer(string path)
    {
        inf = new FileInfo(path);
    }

    public unsafe void* Load<T>()
    {
        return null;
    }
}