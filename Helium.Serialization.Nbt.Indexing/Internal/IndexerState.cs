namespace Helium.Serialization.Nbt.Indexing.Internal;

using System;

// describes the progress in the current layer of the data structure.
internal record struct IndexerState
{
	// this can either be compound or list.
	public NbtTokenType CurrentToken { get; set; }

	// the children remaining for this list
	// (for compounds we're supposed to figure it out ourselves).
	public Int32 RemainingChildren { get; set; }

	// start index of this token.
	public Int32 StartIndex { get; set; }
}
