using System;
using System.Buffers;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using ProGaudi.MsgPack;

namespace msgpack.spec.linux
{
    [MemoryDiagnoser]
    [DisassemblyDiagnoser(false , true)]
    [Q3Column]
    public class NativeComparison
    {
        private const uint length = 100;
        private uint baseInt = 99000;
        private readonly byte[] _buffer = ArrayPool<byte>.Shared.Rent(short.MaxValue);

        [Benchmark(Baseline = true)]
        public int Span()
        {
            var buffer = _buffer.AsSpan();
            var i = 0;
            for (; i < length; i++)
            {
                baseInt -= 1000u;
                MsgPackSpec.WriteUInt32(buffer.Slice(5 * i), baseInt);
            }

            return i;
        }

        [Benchmark]
        public int SpanConst()
        {
            var buffer = _buffer.AsSpan();
            var i = 0;
            for (; i < length; i++)
            {
                baseInt -= 1000u;
                MsgPackSpec.WriteUInt32(buffer.Slice(5 * i), length);
            }

            return i;
        }

        [Benchmark]
        public unsafe int Fixed()
        {
            fixed (byte* pointer = &_buffer[0])
            {
                var i = 0;
                for (; i < length; i++)
                {
                    baseInt -= 1000;
                    MsgPackSpecPointer.WriteUInt32(pointer, 5 * i, baseInt);
                }

                return i;
            }
        }

        [Benchmark]
        public uint C() => CNative.SerializeInts(length);

        [Benchmark]
        public uint Cpp() => CppNative.SerializeInts(length);

        private static class CNative
        {
            [DllImport("libcMsgPack.so", EntryPoint = "serializeInts", CallingConvention = CallingConvention.Cdecl)]
            public static extern uint SerializeInts(uint size);
        }

        private static class CppNative
        {
            [DllImport("libcMsgPack.so", EntryPoint = "serializeInts", CallingConvention = CallingConvention.Cdecl)]
            public static extern uint SerializeInts(uint size);
        }
    }
}
