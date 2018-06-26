using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ProGaudi.MsgPack.Light
{
    /// <summary>
    /// Methods for working with float
    /// </summary>
    public static partial class MsgPackBinary
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteFixFloat32(in Span<byte> buffer, float value) => TryWriteFixFloat32(buffer, value, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteFixFloat32(in Span<byte> buffer, float value, out int wroteSize)
        {
            wroteSize = 5;
            buffer[0] = DataCodes.Float32;
            var binary = new FloatBinary(value);

            if (BitConverter.IsLittleEndian)
            {
                buffer[1] = binary.Byte3;
                buffer[2] = binary.Byte2;
                buffer[3] = binary.Byte1;
                buffer[4] = binary.Byte0;
            }
            else
            {
                buffer[1] = binary.Byte0;
                buffer[2] = binary.Byte1;
                buffer[3] = binary.Byte2;
                buffer[4] = binary.Byte3;
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ReadFixFloat32(in ReadOnlySpan<byte> buffer, out int readSize) => TryReadFixFloat32(buffer, out var result, out readSize)
            ? result
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadFixFloat32(in ReadOnlySpan<byte> buffer, out float value, out int readSize)
        {
            readSize = 5;
            var result = buffer[0] == DataCodes.Float32;
            var binary = new FloatBinary(buffer.Slice(1));
            value = binary.Value;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteFloat(in Span<byte> buffer, float value) => WriteFixFloat32(buffer, value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteFloat(in Span<byte> buffer, float value, out int wroteSize) => TryWriteFixFloat32(buffer, value, out wroteSize);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ReadFloat(in ReadOnlySpan<byte> buffer, out int readSize) => ReadFixFloat32(buffer, out readSize);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadFloat(in ReadOnlySpan<byte> buffer, out float value, out int readSize) => TryReadFixFloat32(buffer, out value, out readSize);

        [StructLayout(LayoutKind.Explicit)]
        private struct FloatBinary
        {
            [FieldOffset(0)]
            public readonly float Value;

            [FieldOffset(0)]
            public readonly byte Byte0;

            [FieldOffset(1)]
            public readonly byte Byte1;

            [FieldOffset(2)]
            public readonly byte Byte2;

            [FieldOffset(3)]
            public readonly byte Byte3;

            public FloatBinary(float f)
            {
                this = default;
                Value = f;
            }

            public FloatBinary(ReadOnlySpan<byte> bytes)
            {
                Value = 0;
                if (BitConverter.IsLittleEndian)
                {
                    Byte0 = bytes[3];
                    Byte1 = bytes[2];
                    Byte2 = bytes[1];
                    Byte3 = bytes[0];
                }
                else
                {
                    Byte0 = bytes[0];
                    Byte1 = bytes[1];
                    Byte2 = bytes[2];
                    Byte3 = bytes[3];
                }
            }
        }
    }
}
