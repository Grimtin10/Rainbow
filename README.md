# Rainbow

Rainbow is a bytecode runtime written in C# meant to give
the user complete freedom over literally everything. The
goal is to achieve complete freedom while still promoting
safe written code, while also providing the tools to
do so. 

### [Documentation](/Documentation/TABLEOFCONTENTS.md)

# What is the ideology of the runtime?

The runtime's main goal is to be friendly to both
safe and unsafe ideologies. Meaning Rainbow supports
many different features and functions that normal general-purpose 
directed bytecodes do not normally highlight.

So how do we do it?

### Garbage Collection
Fully controlled garbage collector: Rainbow includes a garbage
collector that is highly dynamic and friendly to manual control.
Meaning that the garbage collector *can* run on its own but can
also completely be controlled and managed. See [Extended Garbage Collection](#extended-garbage-collection)

### Object Oriented and Procedural Programming
Rainbow supports both procedural and object-oriented
schemas. Since it all comes down to bytes in the end why does it matter
right? Rainbow provides ways to manipulate pointers and
references while also keeping management over them. This allows
you to manage objects and values from either a procedural standpoint
or an OOP standpoint

### Runtime Allowed Internal Functions
While there are still some internal runtime functions
that are not allowed to be accessed from the bytecode,
Rainbow provides a large array of functions that provide
access to the corresponding runtime functions.

### Compilation/Transpilation Options
While this feature is not yet available. Rainbow plans to
support and provide ways to compile to native machine
code and transpile to binary formats like LLVM, CIL, and
possibly more (JVM? WASM!?).

# Extended Garbage Collection

Rainbow's garbage collector is highly dynamic and manageable compared
to just about any other languages' garbage collector. It allows many
different allocation methods and reference management methods. It even
allows and promotes usage of untracked pointers (although the creators do
not promote this).

While the garbage collector allows for unsafe code it also provides fixes
to garbage collected/safe languages memory-wise. Rainbows GC allows for usage
of safe pointers which act like arrays but let you directly touch memory. You
can even construct a managed pointer from an address! Meaning you can easily
convert from unsafe code to safe code!

# Future TODOS

Here are our plans for Rainbow:

- [ ] Add support for compilation to assembly and LLVM
- [ ] Add support for reflection
- [ ] Create a language called Rain that compiles to Rainbow
- [ ] Rewrite runtime in Rain
- [ ] Improve the runtime and add features
- [ ] Experiment with garbage collection and make ours better
- [ ] Make program self-collection (I don't know if this is possible but I want to try it - YendisFish)

# Contributor Pages
[YendisFish](https://github.com/YendisFish)  
[Grimtin10](https://github.com/Grimtin10)

### Developer Quote

Rainbow: does what it shouldn't!