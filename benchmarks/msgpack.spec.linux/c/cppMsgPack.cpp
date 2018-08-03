#include <msgpack.hpp>

using namespace msgpack;

extern "C" void serializeIntArray()
{
    const uint32_t size = 100;
    const int64_t base = 1L << 30;
    sbuffer buffer;
    packer<sbuffer> pk(&buffer);

    pk.pack_array(size);

    int64_t idx = 0;
    for (; idx < size; ++idx)
        pk.pack(base);
}

extern "C" void serializeIntArrayMinus()
{
    const uint32_t size = 100;
    const int64_t base = 1L << 30;
    sbuffer buffer;
    packer<sbuffer> pk(&buffer);

    pk.pack_array(size);

    int64_t idx = 0;
    for (; idx < size; ++idx)
        pk.pack(base-idx);
}
