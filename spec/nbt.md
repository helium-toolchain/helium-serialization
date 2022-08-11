# NBT Binary Format Specification

> Note: NBT is a format specified and designed by Mojang Studios. This is not an authoritative source on the NBT format, rather a document reflecting the specification currently implemented in this repository, as the format may and has historically changed without notice. Since there is no single document released by Mojang Studios detailing the NBT format in its entire current form, [wiki.vg](https://wiki.vg/NBT) is used as a source on the format.

This repository does not aim to support Bedrock Edition NBT as a design goal, however, contributions in that direction are welcome.

## Specification

Every NBT file is always wrapped implicitly into a compound tag, and also begin with a compound tag, meaning it always opens with `0A`.

Every tag begins with its ID, two bytes indicating the length of the name and its name. If a tag is contained within a list tag, it lacks this header.

String tags use another two bytes indicating the length of the string after their header.

Array tags use four bytes indicating the length of the array after their header, list tags use one byte signifying the type of all child tags followed by four length bytes after their header.

End tags can only appear within compounds and have no header, nor content - they are a single byte `00`.

Lists cannot contain lists.

All integers, including length prefixes, are signed integers, and all numbers are big endian.

### Tag types

Name | ID | Payload length | Notes 
---- | -- | -------------- | -----
End | 00 | 0 | Signifies the end of a compound.
SByte | 01 | 1 | -
Int16 | 02 | 2 | -
Int32 | 03 | 4 | -
Int64 | 04 | 8 | -
Single | 05 | 4 | -
Double | 06 | 8 | -
SByteArray | 07 | unknown | -
String | 08 | unknown | -
List | 09 | unknown | -
Compound | 0A | unknown | -
Int32Array | 0B | unknown | -
Int64Array | 0C | unknown | -