[String] $cflags = "-std=c17 -O3 -fasm -fms-extensions"
[String] $cppflags = "-std=c++17 -Ofast -fasm -fms-extensions"

[String] $build = "./build"
[String] $obj = "./obj"

[String] $c_castle_dir = "./src/c/castle"
[String] $c_nbt_dir = "./src/c/nbt"

[String] $cpp_castle_dir = "./src/cpp/castle"
[String] $cpp_nbt_dir = "./src/cpp/nbt"

[String] $c_obj_dir = "./obj/c"
[String] $cpp_obj_dir = "./obj_cpp"

function Build-DotNet {
    dotnet pack $build -p:SymbolPackageFormat=snupkg --include-symbols --include-source --no-dependencies
}

function Build-C {
    Build-CCastle
    Build-CNbt
}

function Build-Cpp {
    Build-CppCastle
    Build-CppNbt
}

function Build-DotNetCastle {
    dotnet pack -o $build src/csharp/Helium.Serialization.Castle -p:SymbolPackageFormat=snupkg --include-symbols --include-source --no-dependencies
}

function Build-DotNetNbt {
    dotnet pack -o $build src/csharp/Helium.Serialization.Nbt -p:SymbolPackageFormat=snupkg --include-symbols --include-source --no-dependencies
}

function Build-CCastle {
    clang $cflags -c $c_castle_dir/index/inc.c -o $c_obj_dir/castle_index.o
    # add all further components here
    clang -shared $c_obj_dir/castle_index.o $build/libheliumccastle.so
}

function Build-CNbt {
    clang $cflags -c $c_nbt_dir/index/inc.c -o $c_obj_dir/nbt_index.o
    # add all further components here
    clang -shared $c_obj_dir/nbt_index.o $build/libheliumcnbt.so
}

function Build-CppCastle {
    clang++ $cppflags -c $cpp_castle_dir/index/inc.c -o $cpp_obj_dir/castle_index.o 
    #add all further components here
    clang -shared $cpp_obj_dir/castle_index.o $build/libheliumcppcastle.so
}

function Build-CppNbt {
    clang++ $cppflags -c $cpp_nbt_dir/index/inc.c -o $cpp_obj_dir/nbt_index.o 
    #add all further components here
    clang -shared $cpp_obj_dir/nbt_index.o $build/libheliumcppnbt.so
}

function Remove-Artifacts {
    Remove-Item -Force -Recurse $build $obj
}

function Initialize-Build {
    New-Item $build
    New-Item $c_obj_dir
    New-Item $cpp_obj_dir 
}

if ( $args.Count -eq 0 ) {
    Build-DotNet
    Build-C
    Build-Cpp
}

if ( $args[0] -ieq "dotnet" && $args.Count -eq 1 ) {
    Build-DotNet
}

if ( $args[0] -ieq "dotnet" && $args.Count -eq 2 ) {
    if ( $args[1] -ieq "castle" ) {
        Build-DotNetCastle
    }
    if ( $args[1] -ieq "nbt" ) {
        Build-DotNetNbt
    }
}

if ( $args[0] -ieq "dotnet-castle" ) {
    Build-DotNetCastle
}

if ( $args[0] -ieq "dotnet-nbt" ) {
    Build-DotNetNbt
}

if ( $args[0] -ieq "c" && $args.Count -eq 1 ) {
    Build-C
}

if ( $args[0] -ieq "c" && $args.Count -eq 2 ) {
    if ( $args[1] -ieq "castle" ) {
        Build-CCastle
    }
    if ( $args[1] -ieq "nbt" ) {
        Build-CNbt
    }
}

if ( $args[0] -ieq "c-castle" ) {
    Build-CCastle
}

if ( $args[0] -ieq "c-nbt" ) {
    Build-CNbt
}

if ( $args[0] -ieq "cpp" && $args.Count -eq 1 ) {
    Build-Cpp
}

if ( $args[0] -ieq "cpp" && $args.Count -eq 2 ) {
    if ( $args[1] -ieq "castle" ) {
        Build-CppCastle
    }
    if ( $args[1] -ieq "nbt" ) {
        Build-CppNbt
    }
}

if ( $args[0] -ieq "cpp-castle" ) {
    Build-CppCastle
}

if ( $args[0] -ieq "cpp-nbt" ) {
    Build-CppNbt
}

if ( $args[0] -ieq "setup" ) {
    Initialize-Build
}

if ( $args[0] -ieq "clean" ) {
    Remove-Artifacts
}