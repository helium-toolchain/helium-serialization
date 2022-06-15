namespace Helium.Serialization.Nbt.Indexing;

using Helium.Serialization.Common;

/// <summary>
/// Represents a snapshot of a NBT token tree, with information about the current token
/// and optional information of this tokens' children, while using less memory than
/// <see cref="NbtIndexTree"/>, in exchange for less useful information.
/// </summary>
public struct SlimNbtIndexTree
{
	/// <summary>
	/// Represents information about this current token.
	/// </summary>
	public SlimNbtTokenIndex CurrentToken { get; init; }

	/// <summary>
	/// Potentially represents information about this tokens' child tokens.
	/// </summary>
	public ValueWriteOnlyList<SlimNbtTokenIndex>? Children { get; init; }
}
