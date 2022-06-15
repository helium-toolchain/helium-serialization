namespace Helium.Serialization.Nbt.Indexing;

using System;
using System.Runtime.InteropServices;

/// <summary>
/// Represents the minimal required data for a token index.
/// </summary>
// ensure we can cast this to and from a word.
[StructLayout(LayoutKind.Sequential, Size = 64)]
public readonly struct CompactNbtTokenIndex
{
	/// <summary>
	/// Stores the offset from the blob origin at which this token starts.
	/// </summary>
	public readonly Int32 StartOffset { get; init; }

	/// <summary>
	/// Stores the offset from the blob origin at which this token ends.
	/// </summary>
	/// <remarks>
	/// For compound tokens, this stores the offset of the closing 0x00 byte;
	/// for all other tokens, this stores the offset of the first byte of the next token.
	/// This is done so a single <c>sub</c> instruction is enough to determine the memory region
	/// any given token is located in.
	/// </remarks>
	public readonly Int32 EndOffset { get; init; }
}
