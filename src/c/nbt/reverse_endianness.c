#include "reverse_endianness.h"

#include<immintrin.h>
#include<stdint.h>

void reverse_endian_16(uint16_t *data, int32_t length) {

#ifdef __SSSE3__
#ifdef __AVX2__
    // avx2 logic
#endif
    // ssse3 logic
#endif
    // scalar logic
}