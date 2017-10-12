using System;

namespace SimpleBdd.Exceptions
{
    public class AmbiguousTestMethodFoundException : Exception
    {
        public AmbiguousTestMethodFoundException()
        {
        }

        public AmbiguousTestMethodFoundException(string message)
            : base(message)
        {
        }

        public AmbiguousTestMethodFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}