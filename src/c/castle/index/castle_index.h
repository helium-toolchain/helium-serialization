#ifndef __CASTLE_INDEX_H_ 
#define __CASTLE_INDEX_H_

#include<stdint.h>

struct slim_castle_index {
    uint32_t token_length;
    uint32_t token_origin_offset;
};

struct castle_index {
    struct slim_castle_index token_position;
    uint16_t token_name_id;
    uint8_t token_complexity;
    uint8_t token_type;
};

int validate_slim_castle_index(const struct slim_castle_index index);

int validate_castle_index(const struct castle_index index);

int validate_castle_index_tree(const struct castle_index index, const struct castle_index parent, const struct castle_index *siblings, const int32_t sibling_count);

#endif