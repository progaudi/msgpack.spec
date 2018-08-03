using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using ProGaudi.MsgPack;

namespace msgpack.spec.linux
{
    [MemoryDiagnoser]
    [Q3Column]
    [MarkdownExporterAttribute.GitHub]
    public class NativeComparison
    {
        private const ushort length = 100;
        private const uint baseInt = 1 << 30;
        private readonly byte[] _buffer = ArrayPool<byte>.Shared.Rent(short.MaxValue);

        [Benchmark]
        public void MsgPackSpecArray()
        {
            var buffer = _buffer.AsSpan();
            var wroteSize = MsgPackSpec.WriteArray16Header(buffer, length);
            for (var i = 0u; i < length; i++)
                wroteSize += MsgPackSpec.WriteUInt32(buffer.Slice(wroteSize), baseInt);
        }

        [Benchmark]
        public void MsgPackSpecArrayMinus()
        {
            var buffer = _buffer.AsSpan();
            var wroteSize = MsgPackSpec.WriteArray16Header(buffer, length);
            for (var i = 0u; i < length; i++)
                wroteSize += MsgPackSpec.WriteUInt32(buffer.Slice(wroteSize), baseInt - i);
        }

        [Benchmark(Baseline = true)]
        public void CArray() => CNative.SerializeArray();

        [Benchmark]
        public void CArrayMinus() => CNative.SerializeArrayMinus();

        [Benchmark]
        public void CppArray() => CppNative.SerializeArray();

        [Benchmark]
        public void CppArrayMinus() => CppNative.SerializeArrayMinus();

        private static class CNative
        {
            [DllImport("libcMsgPack.so", EntryPoint = "serializeIntArray", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SerializeArray();

            [DllImport("libcMsgPack.so", EntryPoint = "serializeIntArrayMinus", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SerializeArrayMinus();

            [DllImport("libcMsgPack.so", EntryPoint = "empty", CallingConvention = CallingConvention.Cdecl)]
            public static extern void Empty();
        }

        private static class CppNative
        {
            [DllImport("libcppMsgPack.so", EntryPoint = "serializeIntArray", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SerializeArray();

            [DllImport("libcMsgPack.so", EntryPoint = "serializeIntArrayMinus", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SerializeArrayMinus();
        }
    }
}
