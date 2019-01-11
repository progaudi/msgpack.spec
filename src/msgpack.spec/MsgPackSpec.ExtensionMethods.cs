using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Text;

namespace ProGaudi.MsgPack
{
    public static partial class MsgPackSpec
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T GetFirst<T>(this ReadOnlySequence<T> ros)
        {
            if (ros.IsSingleSegment) return ros.First.Span[0];
            if (!ros.IsEmpty)
            {
                foreach (var memory in ros)
                {
                    if (!memory.IsEmpty)
                        return memory.Span[0];
                }
            }

            throw GetReadOnlySequenceIsTooShortException(1, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryRead<T>(this ReadOnlySequence<T> ros, Span<T> destination)
        {
            if (destination.Length == 0) return true;
            var index = 0;
            foreach (var memory in ros)
            {
                for (var i = 0; i < memory.Length; i++)
                {
                    destination[index++] = memory.Span[i];
                    if (index == destination.Length)
                        return true;
                }
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetIntLength<T>(this ReadOnlySequence<T> ros)
        {
            var length = ros.Length;
            if (length > int.MaxValue)
                return ThrowDataIsTooLarge(length);

            return (int)length;
        }

        [ThreadStatic]
        private static Encoder _perThreadEncoder;
        internal static Encoder GetThreadStatic(this Encoder nonDefault)
        {
            if (nonDefault != null)
            {
                nonDefault.Reset();
                return nonDefault;
            }

            var encoder = _perThreadEncoder;
            if (encoder == null)
            {
                _perThreadEncoder = encoder = DefaultEncoding.GetEncoder();
            }
            else
            {
                encoder.Reset();
            }
            return encoder;
        }

        [ThreadStatic]
        private static Decoder _perThreadDecoder;
        internal static Decoder GetThreadStatic(this Decoder nonDefault)
        {
            if (nonDefault != null)
            {
                nonDefault.Reset();
                return nonDefault;
            }

            var decoder = _perThreadDecoder;
            if (decoder == null)
            {
                _perThreadDecoder = decoder = DefaultEncoding.GetDecoder();
            }
            else
            {
                decoder.Reset();
            }
            return decoder;
        }

#if NETSTANDARD2_0 || NETFRAMEWORK
        private static unsafe void Convert(
            this Encoder encoder,
            ReadOnlySpan<char> chars,
            Span<byte> bytes,
            bool flush,
            out int charsUsed,
            out int bytesUsed,
            out bool completed)
        {
            fixed (char* charsPtr = chars)
            fixed (byte* bytesPtr = bytes)
                encoder.Convert(charsPtr, chars.Length, bytesPtr, bytes.Length, flush, out charsUsed, out bytesUsed, out completed);
        }

        private static unsafe int GetByteCount(this Encoder encoder, ReadOnlySpan<char> chars, bool flush)
        {
            if (chars.IsEmpty) return 0;

            fixed (char* charsPtr = chars)
            {
                return encoder.GetByteCount(charsPtr, chars.Length, flush);
            }
        }

        private static unsafe void Convert(
            this Decoder decoder,
            ReadOnlySpan<byte> bytes,
            Span<char> chars,
            bool flush,
            out int charsUsed,
            out int bytesUsed,
            out bool completed)
        {
            fixed (char* charsPtr = chars)
            fixed (byte* bytesPtr = bytes)
                decoder.Convert(bytesPtr, bytes.Length, charsPtr, chars.Length, flush, out charsUsed, out bytesUsed, out completed);
        }

        private static unsafe int GetChars(this Decoder decoder, ReadOnlySpan<byte> bytes, Span<char> chars, bool flush)
        {
            if (bytes.IsEmpty) return 0;

            fixed (byte* bytesPtr = bytes)
            fixed (char* charsPtr = chars)
            {
                return decoder.GetChars(bytesPtr, bytes.Length, charsPtr, chars.Length, flush);
            }
        }
#endif

#if NETSTANDARD1_4
        private static void Convert(
            this Encoder encoder,
            ReadOnlySpan<char> chars,
            Span<byte> bytes,
            bool flush,
            out int charsUsed,
            out int bytesUsed,
            out bool completed)
        {
            var (byteArray, charArray) = Allocate(bytes.Length, chars.Length);

            try
            {
                chars.CopyTo(charArray);
                encoder.Convert(charArray, 0, chars.Length, byteArray, 0, bytes.Length, flush, out charsUsed, out bytesUsed, out completed);
                var span = new Span<byte>(byteArray, 0, bytesUsed);
                span.CopyTo(bytes);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(byteArray, true);
                ArrayPool<char>.Shared.Return(charArray, true);
            }
        }

        private static int GetByteCount(this Encoder encoder, ReadOnlySpan<char> chars, bool flush)
        {
            if (chars.IsEmpty) return 0;

            var charArray = ArrayPool<char>.Shared.Rent(chars.Length);
            try
            {
                chars.CopyTo(charArray);
                return encoder.GetByteCount(charArray, 0, chars.Length, flush);
            }
            finally
            {
                ArrayPool<char>.Shared.Return(charArray, true);
            }
        }

        private static int GetChars(this Decoder decoder, ReadOnlySpan<byte> bytes, Span<char> chars, bool flush)
        {
            if (bytes.IsEmpty) return 0;

            var (byteArray, charArray) = Allocate(bytes.Length, chars.Length);
            try
            {
                bytes.CopyTo(byteArray);
                var length = decoder.GetChars(byteArray, 0, bytes.Length, charArray, 0, flush);
                var span = new Span<char>(charArray, 0, length);
                span.CopyTo(chars);
                return length;
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(byteArray, true);
                ArrayPool<char>.Shared.Return(charArray, true);
            }
        }

        private static void Convert(
            this Decoder decoder,
            ReadOnlySpan<byte> bytes,
            Span<char> chars,
            bool flush,
            out int bytesUsed,
            out int charsUsed,
            out bool completed)
        {
            var (byteArray, charArray) = Allocate(bytes.Length, chars.Length);

            try
            {
                bytes.CopyTo(byteArray);
                decoder.Convert(
                    byteArray,
                    0,
                    bytes.Length,
                    charArray,
                    0,
                    chars.Length,
                    flush,
                    out bytesUsed,
                    out charsUsed,
                    out completed);
                var span = new Span<char>(charArray, 0, charsUsed);
                span.CopyTo(chars);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(byteArray, true);
                ArrayPool<char>.Shared.Return(charArray, true);
            }
        }

        private static (byte[], char[]) Allocate(int bytesLength, int charsLength)
        {
            var bytes = ArrayPool<byte>.Shared.Rent(bytesLength);
            char[] chars;
            try
            {
                chars = ArrayPool<char>.Shared.Rent(charsLength);
            }
            catch
            {
                ArrayPool<byte>.Shared.Return(bytes, true);
                throw;
            }

            return (bytes, chars);
        }
#endif
    }
}
