cmake_minimum_required(VERSION 3.10)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED true)
set(CMAKE_EXPORT_COMPILE_COMMANDS true)
set(CMAKE_CXX_STANDARD_INCLUDE_DIRECTORIES true)

project(helium_serialization VERSION 0.1.0)

include_directories("./inc")
include_directories(INTERFACE_SYSTEM_INCLUDE_DIRECTORIES)

file(GLOB_RECURSE c_castle_sources ./src/c/castle/**)
file(GLOB_RECURSE c_nbt_sources ./src/c/nbt/**)

add_library(heliumcastle SHARED ${c_castle_sources})
add_library(heliumnbt SHARED ${c_nbt_sources})

target_compile_options(heliumcastle PUBLIC -O3 -Isystem -fasm -fms-extensions -march=native)
target_compile_options(heliumnbt PUBLIC -O3 -Isystem -fasm -fms-extensions -march=native)