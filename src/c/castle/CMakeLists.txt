set(
    CASTLE_SOURCES
    castle_index.c
)

add_library(heliumcastle SHARED ${CASTLE_SOURCES})

target_compile_options(heliumcastle PUBLIC -O3 -Isystem -fasm -fms-extensions -march=native)
