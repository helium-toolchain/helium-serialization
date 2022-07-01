namespace Helium.Serialization.Nbt.Indexing;

using System;
using System.Buffers.Binary;

using Helium.Serialization.Common;
using Helium.Serialization.Nbt.Indexing.Internal;

public readonly unsafe ref partial struct NbtIndexer
{
	public readonly SlimNbtIndexTree IndexWholeTree()
	{
		// the root token that wraps the entire compound
		SlimNbtIndexTree root = new()
		{
			CurrentToken = new()
			{
				StartOffset = 0,
				EndOffset = this.Blob.Length
			}
		};

		// a stack to keep track of whatever it is that we're currently deserializing
		ValueStack<IndexerState> stateStack = new(16);

		// the index we're currently constructing
		SlimNbtTokenIndex currentIndex;

		// the length of the string we're currently working on
		Int16 stringLength = 0;

		// the length of the array we're currently working on
		Int32 arrayLength = 0;

		// the current offset from the start and the offset we started the current token at
		Int32 offset = 0, startOffset = 0;

		// the spans we represent our data with
		Span<Byte> workingRegister, controlRegister, referenceRegister;

		stateStack.Push(new()
		{
			CurrentToken = NbtTokenType.Compound,
			RemainingChildren = -1
		});

		// main parsing loop
		while(true)
		{
			startOffset = offset;

			// increment offset after this operation has completed
			workingRegister = this.Blob.Slice(offset++, 1);

			// this could be a switch statement, but an if chain is faster here. 

			if(workingRegister[0] == (Byte)NbtTokenType.Compound)
			{
				IndexerState state = new()
				{
					CurrentToken = NbtTokenType.Compound,
					StartIndex = startOffset
				};

				stateStack.Push(state);

				continue;
			}

			if(workingRegister[0] == (Byte)NbtTokenType.List)
			{
				referenceRegister = this.Blob.Slice(offset, 4);
				offset += 4;

				IndexerState state = new()
				{
					CurrentToken = NbtTokenType.List,
					StartIndex = startOffset,
					RemainingChildren = BinaryPrimitives.ReadInt32BigEndian(referenceRegister)
				};

				stateStack.Push(state);

				continue;
			}
		}

		return root;
	}
}
