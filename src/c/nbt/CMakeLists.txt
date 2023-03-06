set(
    NBT_SOURCES
    reverse_endianness.c
)

add_library(heliumnbt SHARED ${NBT_SOURCES})

target_compile_options(heliumnbt PUBLIC -O3 -Isystem -fasm -fms-extensions -march=native)
