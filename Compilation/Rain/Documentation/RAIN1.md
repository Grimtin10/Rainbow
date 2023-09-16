# Rain (1.0 [Experimental])
Rain language mockups for version 1

# Table Of Contents
- [Top Level Statements](#top-level-statements)
- [Pointers and Reference Types](#pointers-and-reference-types)
- [Libraries](#libraries)
- [Intrinsic Types and Methods](#intrinsic-types-and-methods)
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

### Constructors
Constructors provide a way to define initialization code
for classes and structs.

```cs
class Car
{
    string make { get; set; }
    string model { get; set; }
}
```

### Usage of Data Types
Declaring instances of a variable/object is quite easy
in Rain. Just make usage of the ``new`` keyword:

```cs
Person p = new Person("Jeremy", 45);
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

#### Intrisic this.fields Parameter
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
Unlike other languages that support OOP; Rain only
supports static methods within classes. Mainly because
not everything within Rain is a class. But upon usage
of static classes, you will realize they behave in a
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
il(1)
{
    mov eax, 5
    push eax
}
```

If your program needs to JIT compile *and* AOT compile
you can use * in the IL arguments:

```cs
il(*)
{
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

### Fixed Refs
Automatically dereferenced ``ref``s can get annoying! So Rain
provides a fix in the form of ``fixed ref`` types. A ``fixed ref``
is not automatically dereferenced and can just be passed around:

```cs
fixed ref int x = ref 5;
int y = &5;
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

# Libraries

## Imports

### AOT Imports

## Module Definitions

# Intrinsic Types and Methods

## Runtime Variable

# Overrides