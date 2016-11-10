using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DbcSharp;

namespace Sandbox
{
    // For testing the library until proper unit tests are a thing
    internal static class Program
    {
        private static void Main( string[] args )
        {
            var test = TestRepository.LoadFrom( "./dbc" );
        }
    }

    internal sealed class AchievementEntry
    {
        // Fields
    }

    internal sealed class AchievementsFile : RowCollection<AchievementEntry>, IDbcFile
    {
        public string FileName { get; } = "Achievement.dbc";

        public void Read( BinaryReader reader, DbcInfo info )
        {
            this.Rows = new List<AchievementEntry>( (int)info.RecordCount );
        }

        public void Write( BinaryWriter writer, DbcInfo info )
        {
        }
    }

    internal sealed class TestRepository : DbcRepository<TestRepository>
    {
        public AchievementsFile Achievements { get; private set; }
    }
}