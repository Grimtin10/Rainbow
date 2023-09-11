# Runtime Configs

Here is a full example of a runtime config
JSON file:

[Source](../exampleconfig.json)
```json
{
    "useGc":true,
    "asmArgs": {
        "linkerArgs": [
            {
                "path":"MyCoolLib.dll",
                "isStatic":false
            },
            {
                "path":"MyAwesomeLib.so",
                "isStatic":false
            },
            {
                "path":"MyFantasticLib.rbb",
                "isStatic":true
            }
        ],
        "aotCast":false
    }
}
```
This chapter goes through these arguments and
explains what they do.

# Use GC (useGc)

# Assembler Args (asmArgs)

# Linker Args (linkerArgs)

### Static Linking (isStatic)