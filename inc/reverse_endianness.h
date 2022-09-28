#pragma once

#include<stdint.h>

// takes data and reverses its endianness.
void reverse_endian_16(uint16_t *data, int32_t length);

// takes data and reverses its endianness.
void reverse_endian_32(uint32_t *data, int32_t length);

// takes data and reverses its endianness.
void reverse_endian_64(uint64_t *data, int32_t length);