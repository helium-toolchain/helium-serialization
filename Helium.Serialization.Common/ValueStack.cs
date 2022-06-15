namespace Helium.Serialization.Common;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

/// <summary>
/// Represents a variable size basic implementation of <see cref="Stack{T}"/>, suitable for use in Helium's in-memory
/// remapping of NBT data.
/// </summary>
/// <typeparam name="TItem">Specifies the type of elements in the stack.</typeparam>
public struct ValueStack<TItem> : IEnumerable<TItem>, IReadOnlyCollection<TItem>
{
	private TItem[] __items;
	private Int32 __index;
	private Int32 __capacity;
	private readonly Int32 __increment;

	#region constructors
	/// <summary>
	/// Creates a new instance with the given initial capacity and increment.
	/// </summary>
	/// <param name="capacity">Specifies the initial capacity of the backing array.</param>
	/// <param name="increment">Specifies the increment added to the backing array on every resize.</param>
	public ValueStack(Int32 capacity, Int32 increment = 16)
		: this(new TItem[capacity], increment)
	{ }

	/// <summary>
	/// Creates a new instance from the given collection and with the given increment.
	/// </summary>
	/// <param name="collection">The starting collection for the backing array. Note that the current stack index
	/// and the capacity are inferred from this collection, which may not always be desirable.</param>
	/// <param name="increment">Specifies the increment added to the backing array on every resize.</param>
	public ValueStack(IEnumerable<TItem> collection, Int32 increment = 16)
	{
		this.__items = collection.ToArray();
		// following two lines: avoid LINQ calls where possible.
		this.__index = this.__items.Length;
		this.__capacity = this.__index;
		this.__increment = increment;
	}

	/// <summary>
	/// Creates a new instance from the given information.
	/// </summary>
	/// <param name="collection">The initial collection for this ValueStack.</param>
	/// <param name="index">The current peak index of this collection.</param>
	/// <param name="capacity">The current capacity of this collection.</param>
	/// <param name="increment">Specifies the increment added to the backing array on every resize.</param>
	/// <remarks>
	/// Note that <paramref name="index"/> and <paramref name="capacity"/> <b>need</b> to be correct under any
	/// circumstances, otherwise exceptions will appear down the line and/or data loss will occur.
	/// </remarks>
	public ValueStack(IEnumerable<TItem> collection, Int32 index, Int32 capacity, Int32 increment)
	{
		this.__items = collection.ToArray();
		this.__index = index;
		this.__capacity = capacity;
		this.__increment = increment;
	}
	#endregion

	#region push
	/// <summary>
	/// Pushes an item to the stack.
	/// </summary>
	/// <param name="item">The next item to land on the stack.</param>
	/// <returns>The index at which the collection is currently at.</returns>
	public Int32 Push(TItem item)
	{
		this.resizeIfNecessary();

		this.__items[++this.__index] = item;
		return this.__index;
	}

	/// <summary>
	/// Attempts to push an item to the stack.
	/// </summary>
	/// <param name="item">The item to push on the stack.</param>
	/// <param name="index">The current index of the collection.</param>
	/// <returns>Whether or not the attempt succeeded.</returns>
	public Boolean TryPush(TItem item,

		[MaybeNullWhen(false)]
		out Int32? index)
	{
		index = null;

		try
		{
			this.resizeIfNecessary();

			this.__items[++this.__index] = item;
			index = this.__index;

			return true;
		}
		catch
		{
			return false;
		}
	}
	#endregion

	#region pop
	/// <summary>
	/// Returns the item currently at the peak of the collection.
	/// </summary>
	public TItem Pop()
	{
		return this.__items[this.__index--];
	}

	/// <summary>
	/// Attempts to return the item currently at the peak of the collection.
	/// </summary>
	/// <param name="item">The item returned. <see langword="default"/> if the item could not be returned.</param>
	/// <returns>Whether the attempt was successful.</returns>
	public Boolean TryPop(

		[MaybeNullWhen(false)]
		out TItem? item)
	{
		item = default;

		try
		{
			item = this.__items[this.__index--];
			return true;
		}
		catch
		{
			return false;
		}
	}
	#endregion

	#region peek
	/// <summary>
	/// Peeks at the current peak of the collection.
	/// </summary>
	public TItem Peek()
	{
		return this.__items[this.__index];
	}

	/// <summary>
	/// Peeks at the item <paramref name="offset"/> items away from the current peak of the collection.
	/// </summary>
	public TItem Peek(Int32 offset)
	{
		return this.__items[this.__index - offset];
	}

	/// <summary>
	/// Peeks at all items from the current peak of the collection to <paramref name="offset"/> items downwards.
	/// </summary>
	/// <param name="offset"></param>
	public IEnumerable<TItem> PeekRange(Int32 offset)
	{
		return this.__items.Take((this.__index - offset)..);
	}
	#endregion

	#region resize
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void resizeIfNecessary()
	{
		if(this.__index == this.__capacity)
		{
			resize();
		}
	}

	private void resize()
	{
		this.__capacity += this.__increment;
		TItem[] items = new TItem[this.__capacity];

		this.__items.CopyTo(items, 0);
		this.__items = items;
	}
	#endregion

	#region interface implementation
	/// <inheritdoc/>
	public Int32 Count => this.__index;

	/// <inheritdoc/>
	public IEnumerator<TItem> GetEnumerator()
	{
		return (IEnumerator<TItem>)this.__items.GetEnumerator();
	}

	/// <inheritdoc/>
	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.__items.GetEnumerator();
	}
	#endregion
}
