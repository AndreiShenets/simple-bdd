using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace SimpleBdd
{
    public class Table
    {
        private readonly string[] _columnNames;
        private readonly object[][] _values;

        public IEnumerable<Row> Rows => _values.Select(values => new Row(_columnNames, values));


        public Table([ItemNotNull] [NotNull] string[] columnNames, [ItemNotNull] [NotNull] object[][] values)
        {
            _columnNames = columnNames;
            _values = values;
        }
    }
}