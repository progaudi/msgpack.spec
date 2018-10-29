using System;

using JetBrains.Annotations;

namespace ProGaudi.MsgPack
{
    [PublicAPI]
    public static class DataLengths
    {
        public const int FixArrayHeader = 1;

        public const int Array16Header = 3;

        public const int Array32Header = 5;

        public const int Binary8Header = 2;

        public const int Binary16Header = 3;

        public const int Binary32Header = 5;

        public const int Boolean = 1;

        public const int Float64 = 9;

        public const int Extension8Header = 3;

        public const int Extension16Header = 4;

        public const int Extension32Header = 6;

        public const int FixExtensionHeader = 2;

        public const int TimeStamp32 = 6;

        public const int TimeStamp64 = 10;

        public const int TimeStamp96 = 15;

        public const int Float32 = 5;

        public const int Int16 = 3;

        public const int Int32 = 5;

        public const int Int64 = 9;

        public const int Int8 = 2;

        public const int FixMapHeader = 1;

        public const int Map16Header = 3;

        public const int Map32Header = 5;

        public const int NegativeFixInt = 1;

        public const int Nil = 1;

        public const int PositiveFixInt = 1;

        public const int FixStringHeader = 1;

        public const int String8Header = 2;

        public const int String16Header = 3;

        public const int String32Header = 5;

        public const int UInt8 = 2;

        public const int UInt16 = 3;

        public const int UInt32 = 5;

        public const int UInt64 = 9;

        public const byte FixMapMaxLength = DataCodes.FixMapMax - DataCodes.FixMapMin;

        public const byte FixArrayMaxLength = DataCodes.FixArrayMax - DataCodes.FixArrayMin;

        public const byte FixStringMaxLength = DataCodes.FixStringMax - DataCodes.FixStringMin;

        public static int GetBinaryHeaderLength(int length)
        {
            if (length <= byte.MaxValue)
            {
                return Binary8Header;
            }

            if (length <= ushort.MaxValue)
            {
                return Binary16Header;
            }

            return Binary32Header;
        }

        public static int GetCompatibilityBinaryHeaderLength(int length)
        {
            if (length <= 31)
            {
                return FixStringHeader;
            }

            if (length <= ushort.MaxValue)
            {
                return String16Header;
            }

            return String32Header;
        }

        public static (int min, int max) GetMinAndMaxLength(byte code)
        {
            if (code <= DataCodes.FixPositiveMax) return (1, 1);
            if (DataCodes.FixMapMin <= code && code <= DataCodes.FixMapMax) return (code - DataCodes.FixMapMin, code - DataCodes.FixMapMin);
            if (DataCodes.FixArrayMin <= code && code <= DataCodes.FixArrayMax) return (code - DataCodes.FixArrayMin, code - DataCodes.FixArrayMin);
            if (DataCodes.FixStringMin <= code && code <= DataCodes.FixStringMax) return (code - DataCodes.FixStringMin, code - DataCodes.FixStringMin);
            if (DataCodes.FixNegativeMin <= code) return (1, 1);
            switch (code)
            {
                case DataCodes.Nil:
                case DataCodes.True:
                case DataCodes.False:
                    return (1, 1);

                case DataCodes.Binary8:
                    return (0, byte.MaxValue);
                case DataCodes.Binary16:
                    return (0, ushort.MaxValue);
                case DataCodes.Binary32:
                    return (0, int.MaxValue);

                case DataCodes.Extension8:
                    return (0, byte.MaxValue);
                case DataCodes.Extension16:
                    return (0, ushort.MaxValue);
                case DataCodes.Extension32:
                    return (0, int.MaxValue);

                case DataCodes.Float32:
                    return (4, 4);
                case DataCodes.Float64:
                    return (8, 8);

                case DataCodes.Int8:
                case DataCodes.UInt8:
                    return (1, 1);
                case DataCodes.Int16:
                case DataCodes.UInt16:
                    return (2, 2);
                case DataCodes.Int32:
                case DataCodes.UInt32:
                    return (4, 4);
                case DataCodes.Int64:
                case DataCodes.UInt64:
                    return (8, 8);

                case DataCodes.FixExtension1:
                    return (1, 1);
                case DataCodes.FixExtension2:
                    return (2, 2);
                case DataCodes.FixExtension4:
                    return (4, 4);
                case DataCodes.FixExtension8:
                    return (8, 8);
                case DataCodes.FixExtension16:
                    return (16, 16);

                case DataCodes.String8:
                    return (0, byte.MaxValue);
                case DataCodes.String16:
                    return (0, ushort.MaxValue);
                case DataCodes.String32:
                    return (0, int.MaxValue);

                case DataCodes.Array16:
                    return (0, ushort.MaxValue);
                case DataCodes.Array32:
                    return (0, int.MaxValue);

                case DataCodes.Map16:
                    return (0, ushort.MaxValue);
                case DataCodes.Map32:
                    return (0, int.MaxValue);

                // case "NeverUsed" be here to have happy compiler
                default:
                    throw new ArgumentOutOfRangeException(nameof(code), $"Can't get length for {DataCodes.NeverUsed}.");
            }
        }
    }
}
