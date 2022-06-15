namespace Helium.Serialization.Nbt.Indexing;

using System;
using System.Runtime.InteropServices;

/// <summary>
/// Represents a detailed NBT token index, with additional data to speed up
/// specific operations, such as only deserializing the blob represented.
/// </summary>
[StructLayout(LayoutKind.Sequential, Size = 128)]
public readonly struct DetailedNbtTokenIndex
{
	/// <summary>
	/// Stores the offset from the blob origin this token starts at.
	/// </summary>
	public readonly Int32 StartOffset { get; init; }

	/// <summary>
	/// Stores the offset from the blob origin the data stored in this token starts at;
	/// after declarative bytes and the name string.
	/// </summary>
	public readonly Int32 DataStartOffset { get; init; }

	/// <summary>
	/// Stores the offset from the blob origin this token ends at.
	/// </summary>
	/// <remarks>
	/// For compound tokens, this points at the 0x00 closing byte; for all other tokens,
	/// this points at the first byte of the next token.
	/// </remarks>
	public readonly Int32 EndOffset { get; init; }

	/// <summary>
	/// Stores the length of the name string, in UTF-8. This does not include the length prefix.
	/// </summary>
	public readonly Int16 StringLength { get; init; }

	/// <summary>
	/// Stores the type of this token.
	/// </summary>
	public readonly NbtTokenType TokenType { get; init; }
}
