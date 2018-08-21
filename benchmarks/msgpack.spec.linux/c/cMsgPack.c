#include <msgpuck.h>

char buf[65535];

extern uint32_t serializeInts(uint32_t size)
{
    uint32_t base = 99000;
    char *w = buf;

    uint32_t idx = 0;
    for (; idx < size; ++idx)
    {
        base -= 1000;
        w = mp_encode_uint(w, base);
    }

    return idx;
}
