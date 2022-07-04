namespace Helium.Serialization.Nbt.Indexing;

using System;
using System.Buffers.Binary;

using Helium.Serialization.Common;
using Helium.Serialization.Nbt.Indexing.Internal;

public readonly unsafe ref partial struct NbtIndexer
{
	/// <summary>
	/// Indexes the entire blob and returns a slim tree.
	/// </summary>
	public readonly SlimNbtIndexNode IndexWholeTreeSlim()
	{
		// the root token that wraps the entire compound
		SlimNbtIndexNode root = new()
		{
			CurrentToken = new()
			{
				StartOffset = 0,
				EndOffset = this.Blob.Length
			}
		};

		// a stack to keep track of whatever it is that we're currently deserializing
		ValueStack<IndexerState> stateStack = new(16);


		// our tokens to live with
		ValueStack<SlimNbtIndexNode> indexStack = new(16);
		indexStack.Push(root);

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

				SlimNbtIndexNode node = new()
				{
					Children = new()
				};

				indexStack.Peek().Children!.Value.Add(node);

				indexStack.Push(node);

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

				SlimNbtIndexNode node = new()
				{
					Children = new()
				};

				indexStack.Peek().Children!.Value.Add(node);

				indexStack.Push(node);

				continue;
			}

			if(workingRegister[0] == 0x00)
			{
				if(stateStack.Peek().CurrentToken != NbtTokenType.Compound)
				{
					ThrowHelper.ThrowOnInvalidEndToken();
				}

				IndexerState compoundState = stateStack.Pop();

				indexStack.Peek().CurrentToken = new()
				{
					StartOffset = compoundState.StartIndex,
					EndOffset = startOffset
				};

				indexStack.Pop();

				if(indexStack.Count == 0)
				{
					break;
				}
			}

			// non-special-case tokens

			// list handling

			IndexerState listState = stateStack.Peek();

			if(listState.CurrentToken == NbtTokenType.List)
			{
				listState.RemainingChildren--;

				if(listState.RemainingChildren >= 0)
				{
					stateStack.Pop();

					indexStack.Peek().CurrentToken = new()
					{
						StartOffset = listState.StartIndex,
						EndOffset = startOffset
					};

					indexStack.Pop();
				}
			}
		}

		return root;
	}
}
