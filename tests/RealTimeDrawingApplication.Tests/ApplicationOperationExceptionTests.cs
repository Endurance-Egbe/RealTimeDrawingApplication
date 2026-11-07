using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using View.Infrastructure;

namespace RealTimeDrawingApplication.Tests
{
    [TestClass]
    public class ApplicationOperationExceptionTests
    {
        [TestMethod]
        public void PreservesOperationMessageAndInnerException()
        {
            var cause = new InvalidOperationException("database unavailable");

            var exception = new ApplicationOperationException("Unable to save project.", cause);

            Assert.AreEqual("Unable to save project.", exception.Message);
            Assert.AreSame(cause, exception.InnerException);
        }
    }
}
