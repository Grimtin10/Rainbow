# Rain 1.0 [Experimental]
Rain language mockups for version 1

# Table Of Contents
- [Top Level Statements](#top-level-statements)
  - [Structs](#structs)
  - [Classes](#classes)
  - [Usage of Data Types](#usage-of-data-types)
  - [Constructors](#constructors)
    - [Default Keyword](#default-keyword)
    - [Intrinsic this.fields Property](#intrisic-thisfields-property)
  - [Struct and Class Getters/Setters](#structclass-getterssetters)
    - [Immutable Types Using Getters/Setters](#immutable-types-using-getterssetters)
    - [Hiding Immutability](#hiding-immutability-string-example)
  - [Templates](#templates)
  - [Methods](#methods)
    - [Static Class Methods](#static-class-methods)
    - [Private Methods](#private-methods)
- [IL](#il-intrinsic-see-intrisic-types-and-methods)
- [Pointers and Reference Types](#pointers-and-reference-types)
  - [Ref](#ref)
    - [Fixed Refs](#fixed-refs)
    - [Unsafe Refs](#unsafe-refs)
  - [C Style Pointers](#c-styled-pointers)
- [Libraries](#libraries)
  - [AOT Imports](#aot-imports)
  - [C# and Java Interop](#c-and-java-interop)
  - [WebAssembly](#webassembly)
  - [Module Definitions](#module-definitions)
- [Intrinsic Types and Methods](#intrinsic-types-and-methods)
  - [Sizeof](#sizeof)
  - [Runtime Variable](#runtime-variable)
  - [Intrinsic Overrides](#intrinsic-overrides)
- [Overrides](#overrides)


# Top Level Statements
Top level statements incude [structs](#structs), [classes](#classes), [methods](#methods),
[templates](#templates), and [IL](#il).

## Data Types
Data types in Rain are definable structures like
[classes](#classes) and [structs](#structs)

### Structs
Structs in Rain behave similarly to those
in C#. Except they specifically behave like
those marked with the ``unsafe`` tag. All
struct types are pass by value and treated
very similarly to primitives.

```cs
struct Person
{
    string name { get; set; }
    int age { get; set; }
}
```

### Classes
Classes in Rain behave similarly to how
you would expect any class to behave. They
are simply blocks of memory on the heap!

```cs
class Color
{
    int r { get; set; }
    int g { get; set; }
    int b { get; set; }
}
```

### Usage of Data Types
Declaring instances of a variable/object is quite easy
in Rain. Just make usage of the ``new`` keyword:

```cs
Person p = new Person("Jeremy", 45);
```

### Constructors
Constructors provide a way to define initialization code
for classes and structs.

```cs
class Car
{
    string make { get; set; }
    string model { get; set; }

    Car(string make, string model)
    {
        this.make = make;
        this.model = model;
    }
}
```

#### Default keyword
Constructors in Rain bring new syntactical functionality
to the table by making use of the ``default`` keyword! Usage
of the ``default`` keyword fills out a basic constructor for
structs and classes.

```cs
class BigClass
{
    int a { get; set; }
    int b { get; set; }
    int c { get; set; }
    int d { get; set; }
    int e { get; set; }
    int f { get; set; }
    int g { get; set; }

    BigClass(default);

    /* ^ This is translated to the following constructor
        BigClass(int a, int b, int c, int d, int e, int f, int g)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
            this.e = e;
            this.f = f;
            this.g = g;
        }
    */
}
```
But not only does the ``default`` keyword do this. It can also
be used with bodies. For example:

```cs
class BigClass
{
    int a { get; set; }
    int b { get; set; }
    int c { get; set; }
    int d { get; set; }
    int e { get; set; }
    int f { get; set; }
    int g { get; set; }

    BigClass(default) //resolved to BigClass(int a, int b, int c, int d, int e, int f, int g)
    {
        int newint = default.a + default.b; // a and b arent treated as if they exist syntactically. Under the hood Rains compiler resolves this
        this.c = newint;
    }
}
```
One more thing about the default parameter: it does not have to be the
only parameter given to a constructor.

```cs
class BigClass
{
    int a { get; set; }
    int b { get; set; }
    int c { get; set; }
    int d { get; set; }
    int e { get; set; }
    int f { get; set; }
    int g { get; set; }

    BigClass(default, int toAdd)
    {
        this.a = default.a + toAdd;
    }
}
```

#### Intrisic this.fields Property
Each datatype has an auto created ``this.fields`` parameter
that you can assign the ``default`` keyword to. This way you
can quickly set the fields of a class within a constructor
body.

```cs
class BigClass
{
    int a { get; set; }
    int b { get; set; }
    int c { get; set; }
    int d { get; set; }
    int e { get; set; }
    int f { get; set; }
    int g { get; set; }

    BigClass(default)
    {
        this.fields = default;
        this.g = default.a + default.b;
    }
}
```

### Struct/Class Getters/Setters
Rain brings a new syntactical sugar to the table in the
form of getters and setters for the entirety of classes.
This allowes you to write a logic that is run whenever a
variable is accessed.

```cs
struct Person { get; set; }
{
    string name { get; set; }
    int age { get; set; }

    Person get
    {
        return this;
    }

    set(Person p) {
        this = p
    }

    Person(default);
}
```

#### Immutable Types Using Getters/Setters
Data types alternatively have the ``init`` 
setter. Init is a setter that can only be
called once.

```cs
class Person { get; init; }
{
    // all variables within an "init only" class must also be init only
    string name { get; init; }
    int age { get; init; }

    Person get
    {
        return this;
    }

    init(Person p) {
        this = new Person(p.fields);
    }

    Person(default);
}
```

And now we can take a look at what using the
variable is like!

```cs
Person person = new Person("John", 35);

person.name = "Hello!"; // this is illegal
person = new Person("Robert", 27); // also illegal

Person p = person; // totally legal
```

#### Hiding Immutability (string example)
You can make fake mutability through setters. A good
example of this is the string struct:

```cs
struct string { get; set; }
{
    private ref char[] ptr { get; set; }
    readonly int length => ptr.length;

    T this[int index]
    {
        get {
            return ptr[index];
        }

        set(char value) {
            ptr[index] = value;
        }
    }

    string get {
        return this;
    }

    char[] get {
        return ptr;
    }

    set(char[] value) {
        ptr = new ref char[value.length];
        
        for(int i = 0; i < ptr.length; i++)
        {
            ptr[i] = value[i];
        }
    }
}
```

## Templates
Templates provide a layout for a class without
having the ability to be an instance. They simply
extend a class. Equivelant would be interfaces in
C#/Java.

```cpp
template PersonTemplate
{
    string name { get; set; }
    int age { get; set; }
}

class Person : PersonTemplate
{
    int height { get; set; }

    Person(default)
    {
        this.fields = default;
    }
}
```

## Methods
Methods in Rain are just what youd expect
from any language. C styled and basic!

```cs
int main(params string[] args)
{
    for(int i = 0; i < args.length; i++)
    {
        if(args[i] == "Foo")
        {
            return 1;
        }
    }
}
```

### Static Class Methods
Unlike other languages that support OOP; Rain supports static
methods only within classes. This is mainly because
not everything within Rain is a class. But upon usage
of static methods, you will realize they behave in a
different way than other class methods. Static methods
are available from classes that have not been instanciated.
They are also not available from instanciated objects.

```cs
class Foo
{
    static int Bar()
    {
        return 5;
    }
}
```

And here is what the usage looks like:

```cs
int x = Foo.Bar(); // x is set to 5

Foo f = new Foo();
int y = f.Bar(); // not allowed
```

### Private Methods
Private methods are methods that cant be accessed outside
of a the class/struct or module definition they belong to. 
Otherwise they behave exactly as they do in other languages:

```cs
struct Foo
{
    int num { get; set; }

    Foo(default);

    int AddToNum(int toAdd)
    {
        return num + toAdd;
    }

    private int GetNum()
    {
        return num;
    }
}
```

Or in a module:

```cs
define myModule;

private int GetInt()
{
    return 5;
}
```

## IL (Intrinsic, see [Intrisic Types and Methods](#intrinsic-types-and-methods))
The IL keyword behaves very similarly to the
asm keyword in C. You can direcly put IL into
this keyword. It can exist inside of functions
and as a top level statement:

```cs
il {
    SYSCALL CONOUT "Hello World!"
}
```

The IL keyword is not only limited to IL! If your
program AOT compiles you can use assembly!

<span style="color: #eda539">**WARNING: THIS ASSEMBLY CODE MUST BE PLATFORM SPECIFIC! IT WILL MAKE YOUR PROGRAM LESS CROSS PLATFORM!**</span>

```cs
il(1) {
    mov eax, 5
    push eax
}
```

If your program needs to JIT compile *and* AOT compile
you can use * in the IL arguments:

```cs
il(*) {
    SYSCALL CONOUT "Hello World!"
}
```

This IL is casted to whatever platform your Rain is running
on at compile time!

# Pointers and Reference Types
Rain supports pointers and reference types! However it behaves
in an interesting way while dealing with them. It *does*
interface with a garbage collector, but has a number of options
on how you can define and manage reference types.

## Ref
The most common pointer/reference type you will see in Rain is
the ``ref`` keyword. This defines a reference type and has quite
a bit of logic to it.

```cs
ref int x = ref 5;
x = 6;

// ref keywords are automatically dereferenced in Rain
io.conout("{int}", x); // 6
```

Since refs are automatically dereferenced you always have
to pass one with the ``ref`` keyword:

```cs
ref int x = ref 5;
ref int y = ref x;
```

The ``ref`` keyword can also be used to allocate a block of a type
on the heap by allocating an array:

```cs
ref byte[] buff = ref new byte[100];
```

But you can allocate an array of refs on the stack by doing this:

```cs
ref[] byte bytePtrs = new ref byte[5];
```

And of course this allows us to create pointer chains:

```cs
ref[][][] byte refChain = new ref byte[1][1][1];
x[0][0][0] = ref 0x05;
```

### Fixed Refs
Automatically dereferenced ``ref``s can get annoying! So Rain
provides a fix in the form of ``fixed ref`` types. A ``fixed ref``
is not automatically dereferenced and can just be passed around:

```cs
fixed ref int x = ref 5;
int y = *5;
```

<span style="color: #43fa6e">**FUN FACT: Rains compiler doesnt know the difference between defining a class and defining a fixed ref!**</span>

### Unsafe Refs
Unsafe ``ref``s are not tracked by the garbage collector and can
be implicitely converted to and from [c pointers](#c-styled-pointers):

```cs
int *x = runtime.alloc(sizoef(int));
unsafe ref int ptr = x;

*x = 6;

io.conout("{int}", ptr); // 6

runtime.free(ptr); // you can also do runtime.free(x);
```

## C Styled Pointers
Rain supports C styled pointers and provides allocations methods
to interact with them through the [runtime variable](#runtime-variable)

```cs
char *str = runtime.alloc(sizeof(char) * 3);
*str = "Hi!"; // "" returns a char[], however, the compiler and rain both do not know the difference between the 2

io.conout(str);

runtime.free(str);
```

# Libraries
Libraries are an interesting aspect of Rain. This is mainly due to the fact
that Rainbow is castable to multiple different platforms. But the first aspect
of libraries is their conventions.

Libraries in Rain are imported through defined modules. For example:

[mylib.rn](#module-definitions)
```cs
define mylib;

int Add(int a, int b)
{
    return a + b;
}
```

myprogram.rn
```cs
include mylib;

int main()
{
    int sum = Add(5, 5);
}
```

## AOT Imports
With Rainbows support for AOT compilation, linking native binaries is just as easy
as importing Rain modules:

mylib.c
```c
int Add(int a, int b) {
    return a + b;
}
```

```cs
include "./mylib.dll";

int main()
{
    int sum = Add();
}
```

## C# and Java Interop
Rainbow eventually aims to be able to compile to
run on the CLR and JVM. With that, Rain also aims
to be able to interop with those 2 languages.

## WebAssembly
Through LLVM Rain will eventually support compilation
to WebAssembly. While this feature is not supported yet,
[yendy](https://github.com/YendisFish) aims to provide
support for this feature by the first LTS release.

## Module Definitions
Module definitions in Rain are quite straightforward and
easy. They are similar to namespace in C#, except you
use the define keyword. The naming convensions are quite
simple for modules to. Simply put the name of your library
in them:

mylib.rn
```cs
define mylib;

int Add(int a, int b)
{
    return a + b;
}
```

# Intrinsic Types and Methods
Intrinsic types and met hods are those which are filled by
Rains compiler at compile time. These include special variables
like the [runtime variable](#runtime-variable) and [ref](#ref)
keyword.

## Sizeof [Method]
The sizeof method is an intrinsic method that returns the size
of a given type in bytes. This number is resolved at compile time 
instead of at runtime.

```cs
int charSize = sizeof(char);
```

## Runtime [Variable]
The runtime variable is an intrisic object that allows access to certain
runtime functions. This variable is an extremely special type because it
behaves like a global variable. It is accessible throughout the entirety
of your code.

```cs
unsafe ref byte buffer = runtime.alloc(10); // 10 byte buffer
```

## Intrinsic Overrides
[yendy](https://github.com/YendisFish) aims to provide overrides for
certain intrinsic functions and methods by the first LTS release.

# Overrides
Rain provides overrides to functions from inherited classes
and templates. Writing an override function is easy:

```cpp
template CustomTemplate
{
    int GetInt()
    {
        return 5;
    }
}

class CustomClass : CustomTemplate
{
    int GetInt() ! base.GetInt()
    {
        return 10;
    }
}
```