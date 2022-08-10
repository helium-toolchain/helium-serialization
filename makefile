.PHONY : clean nbt castle dotnet c cpp dotnet_nbt dotnet_castle c_nbt c_castle cpp_nbt cpp_castle
DOTNET_COMPILER = dotnet build
DOTNET_SRC = src/csharp
C_COMPILER = clang
CPP_COMPILER = clang++
C_SRC = src/c
CPP_SRC = src/cpp
BUILD := build
TEMP_DIRECTORY := temp
C_NBT_OBJ_DIR := obj/c/nbt
C_CASTLE_OBJ_DIR := obj/c/castle
CPP_NBT_OBJ_DIR := obj/cpp/nbt
CPP_CASTLE_OBJ_DIR := obj/cpp/castle

# todo: add more to these as more are created
DOTNET_NBT_DIRECTORIES := Helium.Serialization.Common \
	Helium.Serialization.Nbt \
	Helium.Serialization.Indexing

DOTNET_NBT_FILES := $(foreach dir, $(DOTNET_NBT_DIRECTORIES), $(wildcard $(dir)/*))

# todo: castle files here
DOTNET_CASTLE_DIRECTORIES := Helium.Serialization.Common

DOTNET_CASTLE_FILES := $(foreach dir, $(DOTNET_CASTLE_DIRECTORIES), $(wildcard $(dir)/*))

CPP_FLAGS = -std=c++17 -Ofast -fasm -fms-extensions
C_FLAGS = -std=c17 -O3 -fasm -fms-extensions

C_NBT_FILES := $(wildcard $(C_SRC)/nbt/*.c)
C_CASTLE_FILES := $(wildcard $(C_SRC)/castle/*.c)

CPP_NBT_FILES := $(wildcard $(CPP_SRC)/nbt/*.cpp) $(C_NBT_FILES)
CPP_CASTLE_FILES := $(wildcard $(CPP_SRC)/nbt/*.cpp) $(CPP_NBT_FILES)

C_NBT_OBJECTS := $(wildcard $(C_NBT_OBJ_DIR)/*.o)
C_CASTLE_OBJECTS := $(wildcard $(C_CASTLE_OBJ_DIR)/*.o)

CPP_NBT_OBJECTS := $(wildcard $(CPP_NBT_OBJ_DIR)/*.o)
CPP_CASTLE_OBJECTS := $(wildcard $(CPP_CASTLE_OBJ_DIR)/*.o)

all : dotnet c cpp

nbt : dotnet_nbt c_nbt cpp_nbt

castle : dotnet_castle c_castle cpp_castle

# for dotnet we actually have a shared library to build
dotnet : $(wildcard $(DOTNET_SRC/*))
	@dotnet build -o $(BUILD)

c : c_nbt c_castle

cpp : cpp_nbt cpp_castle

dotnet_nbt : $(DOTNET_NBT_FILES)
	@dotnet build -o $(BUILD) ./$(DOTNET_SRC)/Helium.Serialization.Nbt

dotnet_castle: $(DOTNET_CASTLE_FILES)
	@dotnet build -o $(BUILD) ./$(DOTNET_SRC)/Helium.Serialization.Castle

c_nbt : $(C_NBT_OBJECTS)
	@$(C_COMPILER) -shared $^ -o $(BUILD)/libheliumcnbt.so

c_castle : $(C_CASTLE_OBJECTS)
	@$(C_COMPILER) -shared $^ -o $(BUILD)/libheliumccastle.so

cpp_nbt : $(CPP_NBT_OBJECTS)
	@$(C_COMPILER) -shared $^ -o $(BUILD)/libheliumcppnbt.so

cpp_castle : $(CPP_CASTLE_OBJECTS)
	@$(C_COMPILER) -shared $^ -o $(BUILD)/libheliumcppcastle.so

$(C_NBT_OBJ_DIR)/%.o : $(C_NBT_FILES)
	@$(C_COMPILER) -I$(C_NBT_FILES) $(C_FLAGS) -c $< -o $@

$(C_CASTLE_OBJ_DIR)/%.o : $(C_CASTLE_FILES)
	@$(C_COMPILER) -I$(C_CASTLE_FILES) $(C_FLAGS) -c $< -o $@

$(CPP_NBT_OBJ_DIR)/%.o : $(CPP_NBT_FILES)
	@$(CPP_COMPILER) -I$(C_NBT_FILES) $(C_FLAGS) -c $< -o $@

$(CPP_CASTLE_OBJ_DIR)/%.o : $(CPP_CASTLE_FILES)
	@$(CPP_COMPILER) -I$(CPP_CASTLE_FILES) $(CPP_FLAGS) -c $< -o $@

clean : 
	-rm -rf $(BUILD)
	-rm -rf obj