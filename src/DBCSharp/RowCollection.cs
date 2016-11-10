using System.Collections;
using System.Collections.Generic;

namespace DbcSharp
{
    public abstract class RowCollection<T> : IEnumerable<T>
    {
        public List<T> Rows { get; protected set;  }

        public IEnumerator<T> GetEnumerator()
            => this.Rows.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => this.Rows.GetEnumerator();
    }
}