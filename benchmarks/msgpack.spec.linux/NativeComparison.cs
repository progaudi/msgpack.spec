using System;
using System.Buffers;
using BenchmarkDotNet.Attributes;
using ProGaudi.MsgPack;

namespace msgpack.spec.linux
{
    [MemoryDiagnoser]
    [DisassemblyDiagnoser]
    [Q3Column]
    [MarkdownExporterAttribute.GitHub]
    public class NativeComparison
    {
        private const ushort length = 100;
        private const uint baseInt = 1 << 30;
        private readonly byte[] _buffer = ArrayPool<byte>.Shared.Rent(short.MaxValue);

        [Benchmark]
        public int MsgPackSpecArray()
        {
            var buffer = _buffer.AsSpan();
            var wroteSize = MsgPackSpec.WriteArray16Header(buffer, length);
            for (var i = 0u; i < length; i++)
                wroteSize += MsgPackSpec.WriteUInt32(buffer.Slice(wroteSize), baseInt);

            return wroteSize;
        }

        [Benchmark]
        public int MsgPackSpecArrayMinus()
        {
            var buffer = _buffer.AsSpan();
            var wroteSize = MsgPackSpec.WriteArray16Header(buffer, length);
            for (var i = 0u; i < length; i++)
                wroteSize += MsgPackSpec.WriteUInt32(buffer.Slice(wroteSize), baseInt - i);

            return wroteSize;
        }
    }
}
