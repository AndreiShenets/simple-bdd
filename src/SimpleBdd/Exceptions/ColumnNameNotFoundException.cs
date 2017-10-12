using System;

namespace SimpleBdd.Exceptions
{
    public class ColumnNameNotFoundException : Exception
    {
        public ColumnNameNotFoundException()
        {
        }

        public ColumnNameNotFoundException(string message)
            : base(message)
        {
        }

        public ColumnNameNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}