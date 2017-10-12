using JetBrains.Annotations;
using SimpleBdd.Exceptions;

namespace SimpleBdd
{
    public class Row
    {
        [NotNull]
        private readonly string[] _columnNames;
        [NotNull]
        private readonly object[] _values;


        public Row([ItemNotNull] [NotNull] string[] columnNames, [NotNull] object[] values)
        {
            _columnNames = columnNames;
            _values = values;
        }

        /// <exception cref="T:SimpleBdd.Exceptions.ColumnNameNotFoundException"></exception>
        /// <exception cref="T:System.IndexOutOfRangeException"></exception>
        public T GetValue<T>([NotNull] string columnName)
        {
            for (int i = 0; i < _columnNames.Length; i++)
            {
                if (string.Equals(columnName, _columnNames[i]))
                {
                    return GetValue<T>(i);
                }
            }

            throw new ColumnNameNotFoundException($"Column with name '{columnName}' hasn't been found.");
        }

        /// <exception cref="T:System.IndexOutOfRangeException"></exception>
        public T GetValue<T>(int columnIndex)
        {
            return (T)_values[columnIndex];
        }
    }
}