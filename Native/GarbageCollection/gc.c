#include<stdio.h>
#include<stdlib.h>
#include"gc.h"

GC *new_gc() {
    GC *gc = malloc(sizeof(GC));
    gc->stack = malloc(sizeof(GCStack));
    gc->can_collect = 1;

    return gc;
}

void *gc_alloc(GC *this, int size) {

}

void gc_collect(GC *this) {
    if(this->can_collect) {

    }
}

void enable_gc(GC *this) { 
    this->can_collect = 1;
}

void disable_gc(GC *this) {
    this->can_collect = 0;
}

void recursive_free(GC *this) {

}