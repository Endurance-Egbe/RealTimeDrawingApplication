using System;

namespace View.Infrastructure.Logging
{
    public interface IApplicationLogger
    {
        void LogInformation(string operation, string message);
        void LogError(string operation, Exception exception);
    }
}
