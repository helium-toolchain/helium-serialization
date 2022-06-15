namespace Helium.Serialization.Nbt.Indexing;

using System;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;

/// <summary>
/// Constitutes an indexer, capable of extracting either a <seealso cref="NbtIndexTree"/>
/// or a <seealso cref="SlimNbtIndexTree"/> from the given blob.
/// </summary>
public readonly unsafe ref partial struct NbtIndexer
{
	/// <summary>
	/// The data blob this indexer is working with.
	/// </summary>
	public readonly Span<Byte> Blob { get; init; }

	/// <summary>
	/// Stores whether the current process implements SSSE3 and by extension SSE3 and older.
	/// </summary>
	/// <remarks>
	/// The .NET runtime does not support x64 platforms that do not implement at least SSSE3, ideally SSE4.2,
	/// therefore we know any x64 process will also support SSSE3. It would in practice be fine to assume
	/// any x64 process implements AVX2, but unfortunately, x64 machines without AVX2 support but supported
	/// by dotnet exist.
	/// <br/> <br/>
	/// x86 platforms which implement only MMX or SSE through SSE3 exist and some of them are supported by
	/// the dotnet runtime, but nothing about Helium is designed to run on x86 and neither is this code.
	/// </remarks>
	public readonly static Boolean Ssse3Support = RuntimeInformation.ProcessArchitecture == Architecture.X64;

	/// <summary>
	/// Stores whether the current process implements AVX2 and by extension AVX and all SSE ISEs.
	/// </summary>
	public readonly static Boolean Avx2Support = RuntimeInformation.ProcessArchitecture == Architecture.X64 && Avx2.IsSupported;

	/// <summary>
	/// Stores whether the current process implements either the Neon or the Helium SIMD ISE and therefore
	/// implies whether the current process runs on aarch64 rather than x64.
	/// </summary>
	public readonly static Boolean AdvSimdSupport = AdvSimd.IsSupported;

	/// <summary>
	/// Creates a new indexer from the given blob.
	/// </summary>
	public NbtIndexer
	(
		Span<Byte> blob
	)
	{
		this.Blob = blob;
	}
}
