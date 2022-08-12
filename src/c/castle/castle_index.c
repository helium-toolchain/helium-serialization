#include "castle_index.h"

// Validates a slim castle index for length, as no other information is given to a slim castle index.
inline int validate_slim_castle_index(const struct slim_castle_index index) {
    return index.token_origin_offset + index.token_length < 2147483647;
}

// Validates a castle index, establishing whether it contains spec-compliant information.
// It is not validated in the context of the entire index tree, validate_castle_index_tree serves for that purpose.
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

// Validates a castle index along with its parent and siblings for length and token validity. Siblings and parent are not validated.
int validate_castle_index_tree
    (const struct castle_index index, const struct castle_index parent, const struct castle_index *siblings, const int32_t sibling_count) {
    // first, validate whether the token itself is valid
    if(!validate_castle_index(index)) {
        return 0;
    }

    // second, validate whether the lengths match up
    uint32_t total_length = index.token_position.token_length + 8;

    if(total_length > parent.token_position.token_length) {
        return 0;
    }

    for(int32_t i = 0; i < sibling_count; i++) {
        total_length += siblings[i].token_position.token_length + 8;

        if(total_length >= parent.token_position.token_length) {
            return 0;
        }
    }

    // these two are never equal, the combined length of all child tokens with their headers is always less than
    // the length of the parent token because of additional metadata, such as list/compound length or the root name index
    return total_length < parent.token_position.token_length;
}