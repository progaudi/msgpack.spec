using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ProGaudi.MsgPack.Light
{
    /// <summary>
    /// Methods for working with double
    /// </summary>
    public static partial class MsgPackBinary
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteFixFloat64(Span<byte> buffer, double value) => TryWriteFixFloat64(buffer, value, out var wroteSize)
            ? wroteSize
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteFixFloat64(Span<byte> buffer, double value, out int wroteSize)
        {
            wroteSize = 9;
            buffer[0] = DataCodes.Float64;
            var binary = new DoubleBinary(value);

            if (BitConverter.IsLittleEndian)
            {
                buffer[1] = binary.Byte7;
                buffer[2] = binary.Byte6;
                buffer[3] = binary.Byte5;
                buffer[4] = binary.Byte4;
                buffer[5] = binary.Byte3;
                buffer[6] = binary.Byte2;
                buffer[7] = binary.Byte1;
                buffer[8] = binary.Byte0;
            }
            else
            {
                buffer[1] = binary.Byte0;
                buffer[2] = binary.Byte1;
                buffer[3] = binary.Byte2;
                buffer[4] = binary.Byte3;
                buffer[5] = binary.Byte4;
                buffer[6] = binary.Byte5;
                buffer[7] = binary.Byte6;
                buffer[8] = binary.Byte7;
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ReadFixFloat64(ReadOnlySpan<byte> buffer, out int readSize) => TryReadFixFloat64(buffer, out var result, out readSize)
            ? result
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadFixFloat64(ReadOnlySpan<byte> buffer, out double value, out int readSize)
        {
            readSize = 9;
            var result = buffer[0] == DataCodes.Float64;
            var binary = new DoubleBinary(buffer.Slice(1));
            value = binary.Value;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteDouble(Span<byte> buffer, double value) => WriteFixFloat64(buffer, value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteDouble(Span<byte> buffer, double value, out int wroteSize) => TryWriteFixFloat64(buffer, value, out wroteSize);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ReadDouble(ReadOnlySpan<byte> buffer, out int readSize) => TryReadDouble(buffer, out var value, out readSize)
            ? value
            : throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadDouble(ReadOnlySpan<byte> buffer, out double value, out int readSize)
        {
            var code = buffer[0];
            switch (code)
            {
                case DataCodes.Float32:
                    var result = TryReadFixFloat32(buffer, out var floatValue, out readSize);
                    value = floatValue;
                    return result;
                case DataCodes.Float64:
                    return TryReadFixFloat64(buffer, out value, out readSize);
                default:
                    value = default;
                    readSize = default;
                    return false;
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct DoubleBinary
        {
            [FieldOffset(0)]
            public readonly double Value;

            [FieldOffset(0)]
            public readonly byte Byte0;

            [FieldOffset(1)]
            public readonly byte Byte1;

            [FieldOffset(2)]
            public readonly byte Byte2;

            [FieldOffset(3)]
            public readonly byte Byte3;

            [FieldOffset(4)]
            public readonly byte Byte4;

            [FieldOffset(5)]
            public readonly byte Byte5;

            [FieldOffset(6)]
            public readonly byte Byte6;

            [FieldOffset(7)]
            public readonly byte Byte7;

            public DoubleBinary(double f)
            {
                this = default;
                Value = f;
            }

            public DoubleBinary(ReadOnlySpan<byte> bytes)
            {
                Value = 0;
                if (BitConverter.IsLittleEndian)
                {
                    Byte0 = bytes[7];
                    Byte1 = bytes[6];
                    Byte2 = bytes[5];
                    Byte3 = bytes[4];
                    Byte4 = bytes[3];
                    Byte5 = bytes[2];
                    Byte6 = bytes[1];
                    Byte7 = bytes[0];
                }
                else
                {
                    Byte0 = bytes[0];
                    Byte1 = bytes[1];
                    Byte2 = bytes[2];
                    Byte3 = bytes[3];
                    Byte4 = bytes[4];
                    Byte5 = bytes[5];
                    Byte6 = bytes[6];
                    Byte7 = bytes[7];
                }
            }
        }
    }
}
