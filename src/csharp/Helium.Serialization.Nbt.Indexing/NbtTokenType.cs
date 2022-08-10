namespace Helium.Serialization.Nbt.Indexing;

using System;

/// <summary>
/// Represents the various NBT token types there are.
/// </summary>
/* 
 * Obviously, inheriting from Int16 here is wasteful, but it helps with memory alignment in
 * DetailedTokenIndex, which can be exactly represented by a doubleword. It also helps coreclr
 * to do less work when dealing with NbtTokenIndex, which in a worst-case scenario
 * saves us 112 bytes of stack space.
*/
public enum NbtTokenType : Int16
{
	End,
	SByte,
	Int16,
	Int32,
	Int64,
	Single,
	Double,
	ByteArray,
	String,
	List,
	Compound,
	Int32Array,
	Int64Array
}
