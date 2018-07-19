using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using static ProGaudi.MsgPack.Light.DataCodes;

namespace ProGaudi.MsgPack.Light
{
    /// <summary>
    /// Methods for working with float
    /// </summary>
    public static partial class MsgPackBinary
    {
        /// <summary>
        /// Write float <paramref name="value"/> into <paramref name="buffer"/>.
        /// </summary>
        /// <returns>Count of bytes, written to <paramref name="buffer"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteFixFloat32(Span<byte> buffer, float value)
        {
            buffer[0] = Float32;
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

            return 5;
        }

        /// <summary>
        /// Tries to write float value into <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to write.</param>
        /// <param name="value">Value to write</param>
        /// <param name="wroteSize">Count of bytes, written to <paramref name="buffer"/>. If return value is <c>false</c>, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="buffer"/> is too small.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteFixFloat32(Span<byte> buffer, float value, out int wroteSize)
        {
            wroteSize = 5;
            if (buffer.Length < wroteSize) return false;

            WriteFixFloat32(buffer, value);

            return true;
        }

        /// <summary>
        /// Reads float value from <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="buffer"/>.</param>
        /// <returns>Float value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ReadFixFloat32(ReadOnlySpan<byte> buffer, out int readSize)
        {
            readSize = 5;
            if (buffer[0] == Float32) throw WrongCodeException(buffer[0], Float32);
            return new FloatBinary(buffer.Slice(1)).Value;
        }

        /// <summary>
        /// Tries to read from <paramref name="buffer"/>
        /// </summary>
        /// <param name="buffer">Buffer to read from.</param>
        /// <param name="value">Value, read from <paramref name="buffer"/>. If return value is false, value is unspecified.</param>
        /// <param name="readSize">Count of bytes, read from <paramref name="buffer"/>. If return value is false, value is unspecified.</param>
        /// <returns><c>true</c>, if everything is ok, <c>false</c> if <paramref name="buffer"/> is too small or <paramref name="buffer"/>[0] is not <see cref="Float32"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadFixFloat32(ReadOnlySpan<byte> buffer, out float value, out int readSize)
        {
            readSize = 5;
            var result = buffer[0] == Float32;
            var binary = new FloatBinary(buffer.Slice(1));
            value = binary.Value;
            return result;
        }

        /// <summary>
        /// Calls <see cref="WriteFixFloat32"/>. Provided for consistency with other types.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteFloat(Span<byte> buffer, float value) => WriteFixFloat32(buffer, value);

        /// <summary>
        /// Calls <see cref="TryWriteFixFloat32"/>. Provided for consistency with other types.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWriteFloat(Span<byte> buffer, float value, out int wroteSize) => TryWriteFixFloat32(buffer, value, out wroteSize);

        /// <summary>
        /// Calls <see cref="ReadFixFloat32"/>. Provided for consistency with other types.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ReadFloat(ReadOnlySpan<byte> buffer, out int readSize) => ReadFixFloat32(buffer, out readSize);

        /// <summary>
        /// Calls <see cref="TryReadFixFloat32"/>. Provided for consistency with other types.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadFloat(ReadOnlySpan<byte> buffer, out float value, out int readSize) => TryReadFixFloat32(buffer, out value, out readSize);

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
