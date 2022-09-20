cc=gcc
cppc=g++

cflags=-std=c17 -O3 -fasm -fms-extensions
cppflags=-std=c++17 -Ofast -fasm -fms-extensions

build=./build
obj=./obj

c_castle_dir=./src/c/castle
c_nbt_dir=./src/c/nbt

cpp_castle_dir=./src/cpp/castle
cpp_nbt_dir=./src/cpp/nbt

c_obj_dir=./obj/c
cpp_obj_dir=./obj_cpp

function dotnet () {
    dotnet pack -o $build -p:SymbolPackageFormat=snupkg --include-symbols --include-source --no-dependencies
}

function c () {
    c_castle
    c_nbt
}

function cpp () {
    cpp_castle  
    cpp_nbt
}

function dotnet_castle () {
    dotnet pack -o $build src/csharp/Helium.Serialization.Castle -p:SymbolPackageFormat=snupkg --include-symbols --include-source --no-dependencies
}

function dotnet_nbt () {
    dotnet pack -o $build src/csharp/Helium.Serialization.Castle -p:SymbolPackageFormat=snupkg --include-symbols --include-source --no-dependencies
}

function c_castle () {
    $cc $cflags -c $c_castle_dir/index/inc.c -o $c_obj_dir/castle_index.o
    # add all further components here
    $cc -shared $c_obj_dir/castle_index.o $build/libheliumccastle.so
}

function c_nbt () {
    $cc $cflags -c $c_nbt_dir/index/inc.c -o $c_obj_dir/nbt_index.o 
    # add all further components here
    $cc -shared $c_obj_dir/nbt_index.o $build/libheliumcnbt.so
}

function cpp_castle () {
    $cppc $cppflags -c $cpp_castle_dir/index/inc.c -o $cpp_obj_dir/castle_index.o
    # add all further components here
    $cc -shared $cpp_obj_dir/castle_index.o $build/libheliumcppcastle.so
}

function cpp_nbt () {
    $cppc $cppflags -c $cpp_nbt_dir/index/inc.c -o $cpp_obj_dir/castle_index.obj
    # add all further components here
    $cc -shared $cpp_obj_dir/nbt_index.o %build/libheliumcppnbt.so
}

function clean () {
    rm -rf $build $obj
}

function setup () {
    mkdir -p $build
    mkdir -p $c_obj_dir
    mkdir -p $cpp_obj_dir
}

# input parsing

if [[ $# == 0 ]]; then
    dotnet
    c
    cpp
fi

if [[ $1 == "dotnet" && $# == 1 ]]; then
    dotnet
fi

if [[ $1 == "dotnet" && $# == 2 ]]; then
    if [[ $2 == "castle" ]]; then
        dotnet_castle
    fi
    if [[ $2 == "nbt" ]]; then
        dotnet_nbt
    fi
fi

if [[ $1 == "dotnet-castle" ]]; then
    dotnet_castle
fi

if [[ $1 == "dotnet-nbt" ]]; then
    dotnet_nbt
fi

if [[ $1 == "c" && $# == 1 ]]; then
    c 
fi

if [[ $1 == "c" && $# == 2 ]]; then
    if [[$2 == "castle" ]]; then
        c_castle
    fi
    if [[$2 == "nbt" ]]; then
        c_nbt
    fi
fi

if [[ $1 == "c-castle" ]]; then
    c_castle
fi

if [[ $1 == "c-nbt" ]]; then
    c_nbt
fi

if [[ $1 == "c" && $# == 1 ]]; then
    c 
fi

if [[ $1 == "cpp" && $# == 2 ]]; then
    if [[$2 == "castle" ]]; then
        c_castle
    fi
    if [[$2 == "nbt" ]]; then
        c_nbt
    fi
fi

if [[ $1 == "cpp-castle" ]]; then
    c_castle
fi

if [[ $1 == "cpp-nbt" ]]; then
    c_nbt
fi

if [[ $1 == "setup" ]]; then
    setup
fi
