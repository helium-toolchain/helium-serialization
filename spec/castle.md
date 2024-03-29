# Castle Binary Format Specification

Castle is a binary-only serialization format, designed for the use-cases presented in Helium.

Goals: 
- Deserialization speed
- Low deserialization memory requirements

Non-goals:
- Disk space efficiency
- Serialization speed

## Specification

Each Castle token is built in the following structure:

> Token complexity: 1 byte
> 
> Token type: 1 byte
> 
> Token length: 4 bytes
> 
> Token name ID: 2 bytes

The token length field is statically known for primitive tokens and does not need to be deserialized, however, it is still required to be present and correct.

The name ID `FF FF` (65535) is reserved for cases where no name is given.

Depending on token complexity, some tokens may have additional fields after the 8-byte prefix:

- Tokens with a complexity of zero do not have additional fields. 
- Tokens with a complexity of one have an additional 4-byte child count field. 
- Tokens with a complexity of two have additional special structure, described below.

Each Castle file is wrapped into a root token, which starts out as `02 00 xx xx xx xx FF FF` (complexity 2, token type 0, variable length, no name ID). This must be the opening machine word of any castle file. This is followed by the length of the name array (two bytes) and two bytes indicating the amount of immediate child tokens. The aforementioned name array follows this secondary header and is constructed in the format `2-byte string length - string`. This array is allowed to have up to 65535 entries, from 0 to 65534, and is followed by the immediate child tokens of the root token.

List tokens, also of complexity 2, have a similar secondary header, consisting of one byte for the token complexity of its children (always 0 or 1), another byte for the token type of its children (must not be another list), and two bytes for the amount of child tokens. After this second header, lists store an array of 4-byte offsets for all their child tokens *from the list origin* (that is, from the point immediately after the primary list header).

Compound tokens, also of complexity 2, provide 2 bytes storing the amount of immediate children and an array of 4-byte offsets for all their child tokens *from the compound origin* (see the note on list tokens above).

Root tokens must never be child tokens of any other token, and list tokens must not be child tokens of list tokens. Only the root token, compound tokens and list tokens may have child tokens. The child tokens of a list must all be of the same type.

Arrays and strings are of complexity 1 and provide 2 bytes storing the amount of entries, followed by the entries. Those entries do not have a token header.

The immediate child tokens of lists do have a token header, but no name - they use the reserved value `FF FF` for their name instead.

**All numbers are to be read and written as little endian.**

### Token Types

Name | .NET name | ID | Payload length | Notes
---- | --------- | -- | -------------- | -----
Root | - | 80 | undefined | see explanation
Byte | Byte | 20 | 1 | -
SByte | SByte | 21 | 1 | -
Int16 | Int16 | 22 | 2 | -
UInt16 | UInt16 | 23 | 2 | -
Int32 | Int32 | 24 | 4 | -
UInt32 | UInt32 | 25 | 4 | -
Int64 | Int64 | 26 | 8 | -
UInt64 | UInt64 | 27 | 8 | -
Int128 | Int128 | 28 | 16 | -
UInt128 | UInt128 | 29 | 16 | -
Half | Half | 2A | 2 | -
Single | Single | 2B | 4 | -
Double | Double | 2C | 8 | -
DateTime | DateTimeOffset | 2D | 10 | first 8 bytes define the `DateTimeOffset.Ticks` property, latter 2 bytes store the offset from UTC in minutes.
Date | DateOnly | 2E | 4 | the 4 bytes define the `DateOnly.DayNumber` property.
Time | TimeOnly | 2F | 8 | the 8 bytes define the `TimeOnly.Ticks` property.
String | ReadOnlySpan\<Byte> | 40 | undefined | 2 bytes defining the length of the string, followed by the string in UTF-8. in libraries, this should be represented as `System.String` regardless.
String16 | String | 41 | undefined | 2 bytes defining the amount of UTF-16 characters in the string, followed by the string in UTF-16. The total binary length of the string is therefore twice the given length.
ByteArray | Byte[] | 42 | undefined | 2 bytes defining the length of the array, followed by the array
SByteArray | SByte[] | 43 | undefined | ^
Int16Array | Int16[] | 44 | undefined | ^
UInt16Array | UInt16[] | 45 | undefined | ^
Int32Array | Int32[] | 46 | undefined | ^
UInt32Array | UInt32[] | 47 | undefined | ^
Int64Array | Int64[] | 48 | undefined | ^
UInt64Array | UInt64[] | 49 | undefined | ^
Int128Array | Int128[] | 4A | undefined | ^
UInt128Array | UInt128[] | 4B | undefined | ^
HalfArray | Half[] | 4C | undefined | ^
SingleArray | Single[] | 4D | undefined | ^
DoubleArray | Double[] | 4E | undefined | ^
List | List<T> | 81 | undefined | see explanation
Compound | - | 82 | undefined | see explanation

An undefined payload length means the payload length is not statically known just from the type.