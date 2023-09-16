# Rain (1.0 [Experimental])
Rain language mockups for version 1

# Table Of Contents
- [Top Level Statements](#top-level-statements)
- [Pointers and Reference Types](#pointers-and-reference-types)
- [Library Imports](#library-imports)

# Top Level Statements
Top level statements incude [structs](#structs), [classes](#classes), [methods](#methods),
and [IL](#il).

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

### Constructors
Constructors provide a way to define initialization code
for classes and structs.

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

### Struct/Class Getters/Setters

## Methods

## IL

# Pointers and Reference Types

## Ref

### Fixed Refs

## C Styled Pointers

# Library Imports

# Intrinsic Types and Methods

# Overrides