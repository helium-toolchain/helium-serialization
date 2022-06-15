namespace Helium.Serialization.Common;

using System;
using System.Collections;
using System.Collections.Generic;

public struct ValueWriteOnlyList<TItem> : IEnumerable<TItem>, IReadOnlyList<TItem>
{
	private TItem[] __items;
	private Int32 __index;
	private Int32 __capacity;

	public ValueWriteOnlyList(Int32 capacity)
	{
		this.__items = new TItem[capacity];
		this.__index = 0;
		this.__capacity = capacity;
	}

	public void Add(TItem item)
	{
		if(this.__index == this.__capacity)
		{
			this.resize();
		}

		this.__items[++this.__index] = item;
	}

	private void resize()
	{
		this.__capacity += 16;

		TItem[] items = new TItem[this.__capacity];
		this.__items.CopyTo(items, 0);

		this.__items = items;
	}

	public TItem this[Int32 index] => this.__items[index];

	public Int32 Count => this.__index;

	public IEnumerator<TItem> GetEnumerator()
	{
		return (IEnumerator<TItem>)this.__items.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.__items.GetEnumerator();
	}
}
