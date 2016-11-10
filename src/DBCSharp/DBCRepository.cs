using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DbcSharp
{
    public abstract class DbcRepository<T> where T : DbcRepository<T>
    {
        private static readonly Type Type = typeof( T );
        private static readonly TypeInfo TypeInfo = Type.GetTypeInfo();
        private static readonly Type BaseType = typeof( IDbcFile );
        private static readonly TypeInfo BaseTypeInfo = BaseType.GetTypeInfo();

        private const BindingFlags PropertyFlags = BindingFlags.Instance | BindingFlags.Public;
        private const uint MagicId = 0x43424457; // WDBC

        public static T LoadFrom( string directory, Encoding encoding = null )
        {
            if ( directory == null )
                throw new ArgumentNullException( nameof( directory ) );

            IEnumerable<PropertyInfo> dbcFiles = GetProperties();
            T instance = Activator.CreateInstance<T>();
            foreach ( PropertyInfo property in dbcFiles )
            {
                IDbcFile dbc = (IDbcFile) Activator.CreateInstance( property.PropertyType );
                string path = Path.GetFullPath( Path.Combine( directory, dbc.FileName ) );
                using ( FileStream stream = File.Open( path, FileMode.Open, FileAccess.Read, FileShare.Read ) )
                using ( BinaryReader reader = new BinaryReader( stream, encoding ?? Encoding.UTF8, false ) )
                {
                    uint magic = reader.ReadUInt32();
                    if( magic != MagicId )
                        throw new DbcException( $"{dbc.FileName} is not a valid DBC file" );

                    DbcInfo info = new DbcInfo(
                        reader.ReadUInt32(), // record count
                        reader.ReadUInt32(), // record size
                        reader.ReadUInt32()  // string size
                    );

                    dbc.Read( reader, info );
                }

                property.GetSetMethod( true ).Invoke( instance, new object[] { dbc } );
            }

            return instance;
        }

        public void SaveTo( string directory, Encoding encoding = null )
        {
            if ( directory == null )
                throw new ArgumentNullException( nameof( directory ) );

            IEnumerable<PropertyInfo> dbcFiles = GetProperties();
            foreach ( PropertyInfo property in dbcFiles )
            {
                IDbcFile dbc = (IDbcFile) property.GetGetMethod( true ).Invoke( this, null );
                string path = Path.GetFullPath( Path.Combine( directory, dbc.FileName ) );
                using ( FileStream stream = File.Open( path, FileMode.Create, FileAccess.Write, FileShare.None ) )
                using ( BinaryWriter writer = new BinaryWriter( stream, encoding ?? Encoding.UTF8, false ) )
                {
                    dbc.Write( writer, default( DbcInfo ) ); // TODO
                }
            }
        }

        private static IEnumerable<PropertyInfo> GetProperties()
        {
            return TypeInfo.GetProperties( PropertyFlags )
                           .Where( p => BaseTypeInfo.IsAssignableFrom( p.PropertyType ) );
        }
    }
}