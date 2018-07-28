using System;
using System.Buffers;

namespace ProGaudi.MsgPack
{
    internal sealed class FixedMemoryArrayPool : MemoryPool<byte>
    {
        protected override void Dispose(bool disposing) { }

        public override IMemoryOwner<byte> Rent(int bufferSize = -1)
        {
            if (bufferSize == -1)
                bufferSize = 4096;
            else if ((uint)bufferSize > MaxBufferSize)
                throw new ArgumentOutOfRangeException(nameof(bufferSize));

            return new FixedMemoryPoolBuffer(bufferSize);
        }

        public override int MaxBufferSize { get; } = int.MaxValue;

        private sealed class FixedMemoryPoolBuffer : IMemoryOwner<byte>
        {
            private byte[] _array;
            private readonly int _size;

            public FixedMemoryPoolBuffer(int size)
            {
                _array = ArrayPool<byte>.Shared.Rent(size);
                _size = size;
            }

            public Memory<byte> Memory
            {
                get
                {
                    var array = _array;
                    if (array == null)
                    {
                        throw new ObjectDisposedException(nameof(FixedMemoryPoolBuffer));
                    }

                    return new Memory<byte>(array, 0, _size);
                }
            }

            public void Dispose()
            {
                var array = _array;
                if (array == null) return;

                _array = null;
                ArrayPool<byte>.Shared.Return(array);
            }
        }
    }
}
