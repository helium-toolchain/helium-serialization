#include "reverse_endianness.h"

#include<immintrin.h>
#include<stdint.h>

void reverse_endian_16(uint16_t *data, int32_t length) {

    uint8_t *pointer = (uint8_t*)data;
    int32_t processed = 0;

#ifdef __AVX512F__
    if(length > 64) {
        __m512i mask = _mm512_set_epi8(62, 63, 60, 61, 58, 59, 56, 57, 54, 55, 52, 53, 50, 51, 48, 49,
            46, 47, 44, 45, 42, 43, 40, 41, 38, 39, 36, 37, 34, 35, 32, 33,
            30, 31, 28, 29, 26, 27, 24, 25, 22, 23, 20, 21, 18, 19, 16, 17,
            14, 15, 12, 13, 10, 11, 8, 9, 6, 7, 4, 5, 2, 3, 0, 1);

        int32_t iterations = length / 32;

        for(int32_t i = 0; i < iterations; ++i) {
            __m512i vec = _mm512_loadu_si512(pointer + (i * 64));
#ifdef __AVX512VBMI__
            vec = _mm512_permutexvar_epi8(mask, vec);
#elif __AVX512F__
            vec = _mm512_shuffle_epi8(vec, mask);
#endif
            _mm512_storeu_si512(pointer + (i * 64), vec);
        }

        processed = iterations * 32;

        goto SCALAR_PATH;
    }
#endif

#ifdef __AVX2__

    if(length > 32) {
        __m256i mask = _mm256_set_epi8(30, 31, 28, 29, 26, 27, 24, 25, 22, 23, 20, 21, 18, 19, 16, 17,
            14, 15, 12, 13, 10, 11, 8, 9, 6, 7, 4, 5, 2, 3, 0, 1);

        int32_t iterations = length / 16;

        for(int32_t i = 0; i < iterations; ++i) {
            __m256i vec = _mm256_loadu_si256((__m256i*)(pointer + (i * 32)));
            vec = _mm256_shuffle_epi8(vec, mask);
            _mm256_storeu_si256((__m256i_u*)(pointer + (i * 32)), vec);
        }

        // because iterations = length / 16; is an idiv, not a fdiv, this actually makes sense.
        // this is equivalent to processed = length - (length % 16);
        processed = iterations * 16;
        
        goto SCALAR_PATH;
    }

#endif

#ifdef __SSSE3__
    
    if(length > 16) {
        __m128i mask = _mm_set_epi8(14, 15, 12, 13, 10, 11, 8, 9, 6, 7, 4, 5, 2, 3, 0, 1);

        int32_t iterations = length / 8;

        for(int32_t i = 0; i < iterations; ++i) {
            __m128i vec = _mm_loadu_si128((__m128i*)(pointer + (i * 16)));
            vec = _mm_shuffle_epi8(vec, mask);
            _mm_storeu_si128((__m128i_u*)(pointer + (i * 16)), vec);
        }

        processed = iterations * 8;

        goto SCALAR_PATH;
    }

#endif

SCALAR_PATH:
    // scalar logic
}