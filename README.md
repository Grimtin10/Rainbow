# Rainbow

Rainbow is a bytecode runtime written in C# meant to give
the user complete freedom over literally everything. The
goal is to acheive complete freedome while still promoting
safe written code, and while also providing the tools to
do so.  

# What is the ideology of the runtime?

As stated before Rainbows main focus is on providing complete
freedom to the user while still promoting well written code. So
how do we do it?

### Garbage Collection
Fully controlled garbage collector: Rainbow includes a garbage
collector that is highly dynamic and friendly to manual control.
Meaning that the garbage collector *can* run on its own but can
also completely be controlled and managed. See [Extended Garbage Collection](#extended-garbage-collection)

### Object Oriented and Functional Programming
Rainbow supports both functional and object oriented
schemas. Since it all comes down to bytes in the end why does it matter
right? Rainbow provides ways to manipulate pointers and
references while also keeping management over them. We call
them safe pointers!

### Runtime Allowed Internal Functions
While there are still some internal runtime functions
that are not allowed to be accessed from the bytecode,
Rainbow provides a large array of functions that provide
access to the corresponding runtime functions.

### Compilation/Transpilation Options
While this feature is not yet available. Rainbow plans to
support and provide ways to compile/transpile to native machine
code, LLVM, CIL, and possibly more alternative binary formats.

# Extended Garbage Collection

Rainbows garbage collector is highly dynamic and manageable compared
to just about any other languages garbage collector. It allows many
different allocation methods and reference management methods. It even
allows and promotes use of untracked pointers (although the creators do
not promote this).

While the garbage collector allowes for unsafe code it also provides fixes
to garbage collected/safe languages memory wise. Rainbows GC allowes for use
of safe pointers which act like arrays but let you directly touch memory. You
can even construct a managed pointer from an address! Meaning you can easily
convert from unsafe code to safe code!

# Future TODOS

Here are our future plans for Rainbow:

- [ ] Add support for compilation to assembly and LLVM
- [ ] Add support for reflection
- [ ] Create a language called Rain that compiles to Rainbow
- [ ] Rewrite runtime in Rain
- [ ] Improve the runtime and add features
- [ ] Experiment with garbage collection and make ours better
- [ ] Make program self collection (I don't know if this is possible but I want to try it - YendisFish)

# Contributor Pages
[YendisFish](https://github.com/YendisFish)  
[Grimtin10](https://github.com/Grimtin10)
