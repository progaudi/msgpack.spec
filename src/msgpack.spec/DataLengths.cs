namespace ProGaudi.MsgPack
{
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
    }
}
