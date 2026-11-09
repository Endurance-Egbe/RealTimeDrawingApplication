using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using View.Infrastructure.Logging;

namespace RealTimeDrawingApplication.Tests
{
    [TestClass]
    public class LoggingTests
    {
        [TestMethod]
        public void FileLogger_WritesStructuredInformationAndErrorEntries()
        {
            var path = Path.Combine(
                Path.GetTempPath(), "RealTimeDrawingApplicationTests",
                Guid.NewGuid().ToString("N"), "application.log");
            try
            {
                var logger = new FileApplicationLogger(path);

                logger.LogInformation("Test.Operation", "completed");
                logger.LogError("Test.Operation", new InvalidOperationException("failed"));

                var contents = File.ReadAllText(path);
                Assert.IsTrue(contents.Contains("\"level\":\"Information\""));
                Assert.IsTrue(contents.Contains("\"operation\":\"Test.Operation\""));
                Assert.IsTrue(contents.Contains("\"level\":\"Error\""));
                Assert.IsTrue(contents.Contains("InvalidOperationException"));
            }
            finally
            {
                var directory = Path.GetDirectoryName(path);
                if (Directory.Exists(directory))
                {
                    Directory.Delete(directory, true);
                }
            }
        }
    }
}
