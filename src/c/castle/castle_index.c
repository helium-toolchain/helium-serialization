#include "castle_index.h"

inline int validate_slim_castle_index(const struct slim_castle_index index) {
    return index.token_origin_offset + index.token_length < 2147483647;
}

int validate_castle_index(const struct castle_index index) {
    // complexity is either 0, 1 or 2
    if(index.token_complexity > 2) {
        return 0;
    }
    
    // there are no greater token types than 0x82 - compound
    if(index.token_type > 0x82) {
        return 0;
    }

    // only root, list and compound are of complexity 2, and they are also marked with 0x80 as flag.
    // since we already checked the token isn't greater than 0x82, we know this is valid
    if(index.token_complexity == 2 && (index.token_type & 0x80) != 0x80) {
        return 0;
    }

    // strings and arrays are of complexity 1, and marked with 0x40 as flag.
    // here, we also need to check that they aren't greater than 0x4E, the double array token
    if(index.token_complexity == 1 && ((index.token_type & 0x40) != 0x40) || index.token_type > 0x4E) {
        return 0;
    }

    // primitive values are of complexity 0, and marked with 0x20 as flag.
    // as above, we need to check that they aren't greater than 0x2F, the date token. 
    if(index.token_complexity == 0 && ((index.token_type & 0x20) != 0x20) || index.token_type > 0x2F) {
        return 0;
    }

    // 0xFFFF is a reserved string ID, anything else is valid
    if(index.token_name_id == 0xFFFF) {
        return 0;
    }

    // if everything succeeds, return whether theposition is valid
    return validate_slim_castle_index(index.token_position);
}