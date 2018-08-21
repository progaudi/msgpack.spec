#include <msgpack.hpp>

using namespace msgpack;

extern "C" uint32_t serializeInts(uint32_t size)
{
    uint32_t base = 99000;
    sbuffer buffer;
    packer<sbuffer> pk(&buffer);

    uint32_t idx = 0;

    for (; idx < size; ++idx)
    {
        base -= 1000;
        pk.pack(base);
    }

    return idx;
}
