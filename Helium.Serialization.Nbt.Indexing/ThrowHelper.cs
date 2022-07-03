namespace Helium.Serialization.Nbt.Indexing;

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;

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

	[DoesNotReturn]
	[StackTraceHidden]
	public static void ThrowOnInvalidEndToken()
	{
		throw new InvalidDataException("The blob contained a 0x00 compound end token outside of a compound token");
	}
}
