using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DbcSharp
{
    public interface IDbcFile
    {
        string FileName { get; }

        void Read( BinaryReader reader, DbcInfo info );
        void Write( BinaryWriter writer, DbcInfo info );
    }
}