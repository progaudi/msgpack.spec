using System;

namespace ProGaudi.MsgPack
{
    /// <summary>
    /// Timestamp structure for storing UTC date and time in msgpack
    /// </summary>
    public readonly struct Timestamp
    {
#pragma warning disable IDE1006 // Naming Styles
        private const long TicksPerSecond = 10000000;
        private const long MaxNanoSeconds = 999999999;
        private static readonly long EpochSecondsAD = new DateTimeOffset(1970, 1, 1, 0, 0, 0, 0, TimeSpan.Zero).UtcTicks / TicksPerSecond;
#pragma warning restore IDE1006 // Naming Styles

        public Timestamp(DateTimeOffset dto)
        {
            var ticks = dto.UtcTicks;
            Seconds = ticks / TicksPerSecond;
            NanoSeconds = (uint) (ticks % TicksPerSecond * 100);
        }

        public Timestamp(long seconds, uint nanoSeconds)
        {
            if (nanoSeconds > MaxNanoSeconds)
                throw new ArgumentOutOfRangeException(nameof(nanoSeconds), $"Nanoseconds can't be larger than {MaxNanoSeconds}");
            Seconds = seconds;
            NanoSeconds = nanoSeconds;
        }

        public Timestamp(long unixSeconds)
        {
            NanoSeconds = 0;
            Seconds = unixSeconds + EpochSecondsAD;
        }

        public Timestamp(ulong epoch64)
        {
            NanoSeconds = (uint) (epoch64 >> 34);
            Seconds = (long) (epoch64 & 0x00000003ffffffffUL);
        }

        public long Seconds { get; }

        public uint NanoSeconds { get; }

        public uint EpochSeconds => (uint) (Seconds - EpochSecondsAD);

        public ulong Epoch64 => unchecked(((ulong)NanoSeconds << 34) | (ulong)Seconds);

        public static explicit operator Timestamp(DateTimeOffset dto) => new Timestamp(dto);

        public static explicit operator Timestamp(DateTime dto) => new Timestamp(dto);

        public static implicit operator DateTimeOffset(Timestamp ts) => new DateTimeOffset(
            ts.Seconds * TicksPerSecond + ts.NanoSeconds / 100,
            TimeSpan.Zero
        );
    }
}
