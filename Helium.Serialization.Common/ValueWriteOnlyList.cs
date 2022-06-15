namespace Helium.Serialization.Common;

using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Represents a write-only, strictly typed list.
/// </summary>
/// <typeparam name="TItem">The type held by this list.</typeparam>
public struct ValueWriteOnlyList<TItem> : IEnumerable<TItem>, IReadOnlyList<TItem>
{
	private TItem[] __items;
	private Int32 __index;
	private Int32 __capacity;

	/// <summary>
	/// Creates a new ValueWriteOnlyList with the specified capacity.
	/// </summary>
	public ValueWriteOnlyList
	(
		Int32 capacity
	)
	{
		this.__items = new TItem[capacity];
		this.__index = 0;
		this.__capacity = capacity;
	}

	/// <summary>
	/// Adds a new item to the ValueWriteOnlyList.
	/// </summary>
	public void Add
	(
		TItem item
	)
	{
		if(this.__index == this.__capacity)
		{
			this.resize();
		}

		this.__items[++this.__index] = item;
	}

	// resizes the list, incrementing its length by 16.
	private void resize()
	{
		this.__capacity += 16;

		TItem[] items = new TItem[this.__capacity];
		this.__items.CopyTo(items, 0);

		this.__items = items;
	}


	public TItem this[Int32 index] => this.__items[index];

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
}
