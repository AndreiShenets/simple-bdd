using System;

namespace SimpleBdd.Exceptions
{
    public class TestMethodNotFoundException : Exception
    {
        public TestMethodNotFoundException()
        {
        }

        public TestMethodNotFoundException(string message)
            : base(message)
        {
        }

        public TestMethodNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}