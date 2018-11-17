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

        public static int GetArrayHeaderLength(int length)
        {
            if (length <= FixArrayMaxLength)
            {
                return FixArrayHeader;
            }

            if (length <= ushort.MaxValue)
            {
                return Array16Header;
            }

            return Array32Header;
        }

        public static int GetCompatibilityBinaryHeaderLength(int length)
        {
            if (length <= FixStringMaxLength)
            {
                return FixStringHeader;
            }

            if (length <= ushort.MaxValue)
            {
                return String16Header;
            }

            return String32Header;
        }

        public static int GetStringHeaderLengthByBytesCount(int length)
        {
            if (length <= FixStringMaxLength) return FixStringHeader;

            if (length <= sbyte.MaxValue) return String8Header;

            if (length <= ushort.MaxValue) return String16Header;

            return String32Header;
        }

        public static int GetMapHeaderLength(int count)
        {
            if (count <= FixMapMaxLength)
            {
                return FixMapHeader;
            }

            if (count <= ushort.MaxValue)
            {
                return Map16Header;
            }

            return Map32Header;
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

        public static int GetHeaderLength(byte code)
        {
            if (code <= DataCodes.FixPositiveMax) return PositiveFixInt;
            if (DataCodes.FixMapMin <= code && code <= DataCodes.FixMapMax) return FixMapHeader;
            if (DataCodes.FixArrayMin <= code && code <= DataCodes.FixArrayMax) return FixArrayHeader;
            if (DataCodes.FixStringMin <= code && code <= DataCodes.FixStringMax) return FixStringHeader;
            if (DataCodes.FixNegativeMin <= code) return NegativeFixInt;
            switch (code)
            {
                case DataCodes.Nil: return Nil;

                case DataCodes.True:
                case DataCodes.False:
                    return Boolean;

                case DataCodes.Binary8: return Binary8Header;
                case DataCodes.Binary16: return Binary16Header;
                case DataCodes.Binary32: return Binary32Header;

                case DataCodes.Extension8: return Extension8Header;
                case DataCodes.Extension16: return Extension16Header;
                case DataCodes.Extension32: return Extension32Header;

                case DataCodes.Float32: return Float32;
                case DataCodes.Float64: return Float64;

                case DataCodes.Int8: return Int8;
                case DataCodes.UInt8: return UInt8;
                case DataCodes.Int16: return Int16;
                case DataCodes.UInt16: return UInt16;
                case DataCodes.Int32: return Int32;
                case DataCodes.UInt32: return UInt32;
                case DataCodes.Int64: return Int64;
                case DataCodes.UInt64: return UInt64;

                case DataCodes.FixExtension1:
                case DataCodes.FixExtension2:
                case DataCodes.FixExtension4:
                case DataCodes.FixExtension8:
                case DataCodes.FixExtension16:
                    return FixExtensionHeader;

                case DataCodes.String8: return String8Header;
                case DataCodes.String16: return String16Header;
                case DataCodes.String32: return String32Header;

                case DataCodes.Array16: return Array16Header;
                case DataCodes.Array32: return Array32Header;

                case DataCodes.Map16: return Map16Header;
                case DataCodes.Map32: return Map32Header;

                // case "NeverUsed" be here to have happy compiler
                default:
                    throw new ArgumentOutOfRangeException(nameof(code), $"Can't get header length for {DataCodes.NeverUsed}.");
            }
        }
    }
}
