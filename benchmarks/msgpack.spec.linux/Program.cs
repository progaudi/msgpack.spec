using System;
using BenchmarkDotNet.Running;

namespace msgpack.spec.linux
{
    public static class Program
    {
        public static void Main(string[] args) => BenchmarkRunner.Run<NativeComparison>();

        public static byte ThrowWrongRangeCodeException(byte code, byte min, byte max) => throw new InvalidOperationException(
            $"Wrong data code: 0x{code:x2}. Expected: 0x{min:x2} <= code <= 0x{max:x2}."
        );
    }
}
