namespace Helium.Serialization.Nbt.Indexing;

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

// speed. using ThrowHelpers is noticeably faster than throwing directly.
internal static class ThrowHelper
{
	// throw helper methods should be annotated with those two to take away pain
	[DoesNotReturn]
	[StackTraceHidden]
	public static void ThrowOnInvalidBlob(String message)
	{
		throw new ArgumentException(message);
	}
}
