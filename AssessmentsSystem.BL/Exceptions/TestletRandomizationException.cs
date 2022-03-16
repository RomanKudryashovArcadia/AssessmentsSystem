using System;

namespace AssessmentsSystem.BL.Exceptions
{
    public class TestletRandomizationException : Exception
    {
        public TestletRandomizationException()
        {
        }

        public TestletRandomizationException(string message) : base(message)
        {
        }

        public TestletRandomizationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}