using System;
using System.Buffers;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Columns;
using BenchmarkDotNet.Attributes.Exporters;
using ProGaudi.MsgPack;

namespace msgpack.spec.linux
{
    [MemoryDiagnoser]
    [Q3Column]
    [MarkdownExporterAttribute.GitHub]
    public class NativeComparison
    {
        private readonly byte[] _buffer = ArrayPool<byte>.Shared.Rent(short.MaxValue);
        private const ushort length = 100;
        private const int baseInt = 1 << 30;

        public NativeComparison() => Native.Init();

        [Benchmark]
        public void MsgPackSpecArray()
        {
            var buffer = _buffer.AsSpan();
            var wroteSize = MsgPackSpec.WriteArray16Header(buffer, length);
            for (var i = 0; i < length; i++)
                wroteSize += MsgPackSpec.WriteInt32(buffer.Slice(wroteSize), baseInt - i);
        }

        [Benchmark]
        public void EmptyPInvoke() => Native.Empty();

        [Benchmark]
        public void CPPArray() => Native.SerializeArray();

        private static class Native
        {
            private const string libPath = "libcMsgPack.so";

            [DllImport(libPath, EntryPoint = "empty", CallingConvention = CallingConvention.Cdecl)]
            public static extern void Empty();

            [DllImport(libPath, EntryPoint = "init", CallingConvention = CallingConvention.Cdecl)]
            public static extern void Init();

            [DllImport(libPath, EntryPoint = "serializeIntArray", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SerializeArray();
        }
    }
}
