#include "castle_index.h"

int validate_slim_castle_index(const struct slim_castle_index index) {
    return index.token_origin_offset + index.token_length < 2147483647;
}

int validate_castle_index(const struct castle_index index) {
    // complexity is either 0, 1 or 2
    if(index.token_complexity > 2) {
        return 0;
    }
    
    // there are no greater token types than 0x22 - compound
    if(index.token_type > 0x22) {
        return 0;
    }

    // 0xFFFF is a reserved string ID, anything else is valid
    if(index.token_name_id == 0xFFFF) {
        return 0;
    }

    // if everything succeeds, return whether theposition is valid
    return validate_slim_castle_index(index.token_position);
}