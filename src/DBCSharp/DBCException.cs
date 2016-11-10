using System;

namespace DbcSharp
{
    public sealed class DbcException : Exception
    {
        public DbcException( string message, Exception inner = null ) : base( message, inner ) { }
    }
}