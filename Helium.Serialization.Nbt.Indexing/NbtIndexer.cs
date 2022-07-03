namespace Helium.Serialization.Nbt.Indexing;

using System;

/// <summary>
/// Constitutes an indexer, capable of extracting either a <seealso cref="NbtIndexTree"/>
/// or a <seealso cref="SlimNbtIndexTree"/> from the given blob.
/// </summary>
public readonly unsafe ref partial struct NbtIndexer
{
	/// <summary>
	/// The data blob this indexer is working with.
	/// </summary>
	public readonly Span<Byte> Blob { get; init; }

	/// <summary>
	/// Creates a new indexer from the given blob.
	/// </summary>
	public NbtIndexer
	(
		Span<Byte> blob
	)
	{
		this.Blob = blob;
	}
}
