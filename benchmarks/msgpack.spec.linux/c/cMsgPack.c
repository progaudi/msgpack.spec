#include <msgpuck.h>

char buf[65535];

extern void serializeIntArray()
{
    const uint32_t size = 100;
    const int64_t base = 1L << 30;
    char *w = buf;

    w = mp_encode_array(w, size);
    int64_t idx = 0;
    for (; idx < size; ++idx)
        w = mp_encode_uint(w, base);
}

extern void serializeIntArrayMinus()
{
    const uint32_t size = 100;
    const int64_t base = 1L << 30;
    char *w = buf;

    w = mp_encode_array(w, size);
    int64_t idx = 0;
    for (; idx < size; ++idx)
        w = mp_encode_uint(w, base-idx);
}

extern void empty()
{
}
