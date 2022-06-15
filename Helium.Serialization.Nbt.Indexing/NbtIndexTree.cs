namespace Helium.Serialization.Nbt.Indexing;

using Helium.Serialization.Common;

/// <summary>
/// Represents a snapshot of a NBT token tree, with information about the current token
/// and optional information of this tokens' children.
/// </summary>
public struct NbtIndexTree
{
	/// <summary>
	/// Represents information about this current token.
	/// </summary>
	public NbtTokenIndex CurrentToken { get; init; }

	/// <summary>
	/// Potentially represents information about this tokens' child tokens.
	/// </summary>
	public ValueWriteOnlyList<NbtTokenIndex>? Children { get; init; }
}
