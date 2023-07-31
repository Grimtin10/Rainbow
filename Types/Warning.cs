namespace Rainbow.Types;

public class Warning
{
    public WarningID id { get; set; }
    public string message { get; set; }

    public Warning(string m, WarningID id)
    {
        message = m;
        this.id = id;
    }
}

public enum WarningID
{
    ForcedCollection = 1,
    UnsafeMarshal = 2
}