#Better Makefile for Helium Serialization. written by Kryog team
.PHONY : clean setup_linux nbt castle dotnet c cpp dotnet_nbt dotnet_castle c_nbt c_castle cpp_nbt cpp_castle

#Compilers
C_COMPILER = clang
CPP_COMPILER = clang++

#Flags
CPP_FLAGS = -std=c++17 -Ofast -fasm -fms-extensions
C_FLAGS = -std=c17 -O3 -fasm -fms-extensions

#Sources
C_SRC = src/c
CPP_SRC = src/cpp
DOTNET_COMPILER = dotnet build
DOTNET_SRC = src/csharp

#build directories
LIB_OUT_DIR = build
OBJ = obj

#add your files to OBJ, like this: $(C_CASTLE_OBJ_DIR)/filename.o (the .o is important)
C_CASTLE_OBJ_DIR = $(OBJ)/c/castle
C_CASTLE_OBJ = $(C_CASTLE_OBJ_DIR)/castle_index.o

CPP_CASTLE_OBJ_DIR = $(OBJ)/cpp/castle
CPP_CASTLE_OBJ = $(CPP_CASTLE_OBJ_DIR)/dummy.o

C_NBT_OBJ_DIR = $(OBJ)/c/nbt
C_NBT_OBJ = $(C_NBT_OBJ_DIR)/dummy.o

CPP_NBT_OBJ_DIR = $(OBJ)/cpp/nbt
CPP_NBT_OBJ = $(CPP_NBT_OBJ_DIR)/dummy.o

#idk dotnet
# todo: add more to these as more are created
DOTNET_NBT_DIRECTORIES := Helium.Serialization.Common \
	Helium.Serialization.Nbt \
	Helium.Serialization.Indexing

DOTNET_NBT_FILES := $(foreach dir, $(DOTNET_NBT_DIRECTORIES), $(wildcard $(dir)/*))

# todo: castle files here
DOTNET_CASTLE_DIRECTORIES := Helium.Serialization.Common

DOTNET_CASTLE_FILES := $(foreach dir, $(DOTNET_CASTLE_DIRECTORIES), $(wildcard $(dir)/*))

#nice aliases
all: dotnet c cpp

c: c_castle c_nbt
cpp: cpp_castle cpp_nbt

#prevent relinking when it's not needed
c_castle: $(LIB_OUT_DIR)/libheliumccastle.so
cpp_castle: $(LIB_OUT_DIR)/libheliumcppcastle.so
c_nbt: $(LIB_OUT_DIR)/libheliumcnbt.so
cpp_nbt: $(LIB_OUT_DIR)/libheliumcppnbt.so

MKD = mkdir
#OS-detection in a turing complete build system
ifeq ($(OS),Windows_NT)
	RM = del /s /q 
else
	RM = rm -rf
	MKD += -p 
endif

#setup
setup_linux: 
	@$(MKD) $(C_CASTLE_OBJ_DIR) $(CPP_CASTLE_OBJ_DIR)
	@$(MKD) $(LIB_OUT_DIR)
	@$(MKD) Ready

#Building final library 
$(LIB_OUT_DIR)/libheliumccastle.so: $(C_CASTLE_OBJ)
	@$(C_COMPILER) -shared $^ -o $@

$(LIB_OUT_DIR)/libheliumcppcastle.so: $(CPP_CASTLE_OBJ)
	@$(CPP_COMPILER) -shared $^ -o $@

$(LIB_OUT_DIR)/libheliumcnbt.so: $(C_CASTLE_OBJ)
	@$(C_COMPILER) -shared $^ -o $@

$(LIB_OUT_DIR)/libheliumcppnbt.so: $(CPP_CASTLE_OBJ)
	@$(CPP_COMPILER) -shared $^ -o $@

#Whatever dotnet does
dotnet: $(wildcard $(DOTNET_SRC/*))
	@dotnet pack -o $(LIB_OUT_DIR) -p:SymbolPackageFormat=snupkg --include-symbols --include-source

dotnet_nbt : $(DOTNET_NBT_FILES)
	@dotnet pack -o $(BUILD) ./$(DOTNET_SRC)/Helium.Serialization.Nbt -p:SymbolPackageFormat=snupkg --include-symbols --include-source

dotnet_castle: $(DOTNET_CASTLE_FILES)
	@dotnet pack -o $(BUILD) ./$(DOTNET_SRC)/Helium.Serialization.Castle -p:SymbolPackageFormat=snupkg --include-symbols --include-source 

#cleaning up :D
clean:
	$(RM) $(OBJ) $(LIB_OUT_DIR)


#CW: makefile syntax


#ignore these please, these are literal black magic
$(C_CASTLE_OBJ_DIR)/%.o: $(C_SRC)/castle/%.c
	@$(C_COMPILER) $(C_FLAGS) -c $< -o $@

$(CPP_CASTLE_OBJ_DIR)/%.o: $(CPP_SRC)/castle/%.cpp
	@$(C_COMPILER) $(CPP_FLAGS) -c $< -o $@

$(C_NBT_OBJ_DIR)/%.o: $(C_SRC)/nbt/%.c
	@$(C_COMPILER) $(C_FLAGS) -c $< -o $@

$(CPP_NBT_OBJ_DIR)/%.o: $(CPP_SRC)/nbt/%.cpp
	@$(C_COMPILER) $(CPP_FLAGS) -c $< -o $@