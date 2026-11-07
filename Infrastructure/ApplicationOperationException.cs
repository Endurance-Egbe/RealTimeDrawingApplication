using System;

namespace View.Infrastructure
{
    public class ApplicationOperationException : Exception
    {
        public ApplicationOperationException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}
