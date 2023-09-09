#ifndef GC_H
#define GC_H

typedef struct StackSegment {
    void *ref;
    int size;

    int this_length;
} StackSegment;

typedef struct GCStack {
    void *mem;
    StackSegment *segments;
    int pos;
} GCStack;

typedef struct GC {
    void **reflist;
    int reflistlength;

    GCStack *stack;

    int collection_threshold;
    int total_allocated;
    int pop_count;

    int can_collect;
} GC;

#endif