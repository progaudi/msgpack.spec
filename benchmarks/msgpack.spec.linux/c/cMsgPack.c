#include <msgpack.h>

msgpack_sbuffer buf;

extern void init()
{
    msgpack_sbuffer_init(&buf);
}

extern void serializeIntArray()
{
    size_t size = 100;
    size_t base = 1 << 30;
    msgpack_packer * pk;

    pk = msgpack_packer_new(&buf, msgpack_sbuffer_write);

    msgpack_pack_array(pk, size);
    {
        size_t idx = 0;
        for (; idx < size; ++idx)
            msgpack_pack_uint32(pk, base-1);
    }
    msgpack_packer_free(pk);
}

extern void empty()
{
}
