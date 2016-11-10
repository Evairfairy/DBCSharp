namespace DbcSharp
{
    public struct DbcInfo
    {
        public uint RecordCount { get; }
        public uint RecordSize { get; }
        public uint StringSize { get; }

        public DbcInfo( uint recordCount, uint recordSize, uint stringSize )
        {
            this.RecordCount = recordCount;
            this.RecordSize = recordSize;
            this.StringSize = stringSize;
        }
    }
}