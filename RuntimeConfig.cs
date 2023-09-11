namespace Rainbow;

public record RuntimeConfig(bool useGc, AssemblerArgs asmArgs);

public record AssemblerArgs(List<LinkerArgs> linkerArgs, bool aotCast = false);
public record LinkerArgs(string path, bool isStatic = false); //static only supported by rbb links or AOT casting