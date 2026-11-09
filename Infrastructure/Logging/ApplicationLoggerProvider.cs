using System;
using System.IO;

namespace View.Infrastructure.Logging
{
    public static class ApplicationLoggerProvider
    {
        public static IApplicationLogger Current { get; private set; } =
            new FileApplicationLogger(GetDefaultLogPath());

        public static void SetCurrent(IApplicationLogger logger)
        {
            Current = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private static string GetDefaultLogPath()
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "RealTimeDrawingApplication",
                "logs",
                "application.log");
        }
    }
}
