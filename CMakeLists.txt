cmake_minimum_required(VERSION 3.10)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED true)
set(CMAKE_EXPORT_COMPILE_COMMANDS true)
set(CMAKE_CXX_STANDARD_INCLUDE_DIRECTORIES true)

project(helium_serialization VERSION 0.1.0)

include_directories("./inc")
include_directories(INTERFACE_SYSTEM_INCLUDE_DIRECTORIES)

add_subdirectory(src/c/nbt nbt)
add_subdirectory(src/c/castle castle)

