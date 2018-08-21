using System.Runtime.CompilerServices;
using ProGaudi.MsgPack;

namespace msgpack.spec.linux
{
    public static class MsgPackSpecPointer
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int WriteUInt32(byte* buffer, int idx, uint value)
        {
            if (value <= DataCodes.FixPositiveMax) return WritePositiveFixInt(buffer, idx, (byte) value);
            if (value <= byte.MaxValue) return WriteFixUInt8(buffer, idx, (byte) value);
            if (value <= ushort.MaxValue) return WriteFixUInt16(buffer, idx, (ushort) value);
            return WriteFixUInt32(buffer, idx, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int WritePositiveFixInt(byte* buffer, int idx, byte value)
        {
            if (value > DataCodes.FixPositiveMax) return Program.ThrowWrongRangeCodeException(value, DataCodes.FixPositiveMin, DataCodes.FixPositiveMax);

            buffer[idx] = value;
            return 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int WriteFixUInt8(byte* buffer, int idx, byte value)
        {
            buffer[idx + 1] = value;
            buffer[idx] = DataCodes.UInt8;
            return 2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int WriteFixUInt16(byte* buffer, int idx, ushort value)
        {
            Unsafe.WriteUnaligned(ref buffer[idx + 1], value);
            buffer[idx] = DataCodes.UInt16;
            return 3;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int WriteFixUInt32(byte* buffer, int idx, uint value)
        {
            Unsafe.WriteUnaligned(ref buffer[idx + 1], value);
            buffer[idx] = DataCodes.UInt32;
            return 5;
        }
    }
}
