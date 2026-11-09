using System;
using System.IO;
using System.Text;

namespace View.Infrastructure.Logging
{
    public sealed class FileApplicationLogger : IApplicationLogger
    {
        private readonly string logFilePath;
        private readonly object syncRoot = new object();

        public FileApplicationLogger(string logFilePath)
        {
            this.logFilePath = logFilePath;
        }

        public void LogInformation(string operation, string message)
        {
            Write("Information", operation, message, null);
        }

        public void LogError(string operation, Exception exception)
        {
            Write("Error", operation, exception.Message, exception);
        }

        private void Write(string level, string operation, string message, Exception exception)
        {
            var directory = Path.GetDirectoryName(logFilePath);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var entry = string.Format(
                "{{\"timestamp\":\"{0:o}\",\"level\":\"{1}\",\"operation\":\"{2}\",\"message\":\"{3}\"{4}}}",
                DateTime.UtcNow,
                Escape(level),
                Escape(operation),
                Escape(message),
                exception == null
                    ? string.Empty
                    : string.Format(",\"exception\":\"{0}\"", Escape(exception.ToString())));

            lock (syncRoot)
            {
                File.AppendAllText(logFilePath, entry + Environment.NewLine, Encoding.UTF8);
            }
        }

        private static string Escape(string value)
        {
            return (value ?? string.Empty)
                .Replace("\\", "\\\\")
                .Replace("\"", "\\\"")
                .Replace("\r", "\\r")
                .Replace("\n", "\\n");
        }
    }
}
