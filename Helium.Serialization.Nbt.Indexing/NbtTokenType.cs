namespace Helium.Serialization.Nbt.Indexing;

using System;

/// <summary>
/// Represents the various NBT token types there are.
/// </summary>
/// <remarks>
/// END tokens (0x00) are not represented here, as the library never treats them as their own,
/// independent token. In the rare cases where we do treat 0x00 END separately, using an enum type
/// hinders the JITs ability to optimize correctly.
/// </remarks>
/* 
 * Obviously, inheriting from Int16 here is wasteful, but it helps with memory alignment in
 * DetailedTokenIndex, which can be exactly represented by a doubleword. It also helps coreclr
 * to do less work when dealing with DetailedNbtTokenIndex, which in a worst-case scenario
 * saves us 112 bytes of stack space.
*/
public enum NbtTokenType : Int16
{
	SByte = 1,
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
