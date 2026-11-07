using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using View.Security;

namespace RealTimeDrawingApplication.Tests
{
    [TestClass]
    public class PasswordHasherTests
    {
        [TestMethod]
        public void Hash_ReturnsAFormattedHash()
        {
            var hash = PasswordHasher.Hash("correct horse battery staple");
            Assert.AreEqual(3, hash.Split('.').Length);
        }

        [TestMethod]
        public void Hash_DoesNotStoreThePlainText()
        {
            var password = "correct horse battery staple";
            Assert.IsFalse(PasswordHasher.Hash(password).Contains(password));
        }

        [TestMethod]
        public void Hash_UsesAUniqueSalt()
        {
            Assert.AreNotEqual(PasswordHasher.Hash("same"), PasswordHasher.Hash("same"));
        }

        [TestMethod]
        public void Verify_AcceptsTheOriginalPassword()
        {
            var hash = PasswordHasher.Hash("secret");
            Assert.IsTrue(PasswordHasher.Verify("secret", hash));
        }

        [TestMethod]
        public void Verify_RejectsAnIncorrectPassword()
        {
            var hash = PasswordHasher.Hash("secret");
            Assert.IsFalse(PasswordHasher.Verify("wrong", hash));
        }

        [TestMethod]
        public void Verify_RejectsAnEmptyPassword()
        {
            var hash = PasswordHasher.Hash("secret");
            Assert.IsFalse(PasswordHasher.Verify(string.Empty, hash));
        }

        [TestMethod]
        public void Verify_RejectsAnEmptyHash()
        {
            Assert.IsFalse(PasswordHasher.Verify("secret", string.Empty));
        }

        [TestMethod]
        public void Verify_RejectsMalformedHash()
        {
            Assert.IsFalse(PasswordHasher.Verify("secret", "not-a-hash"));
        }

        [TestMethod]
        public void Verify_RejectsInvalidBase64()
        {
            Assert.IsFalse(PasswordHasher.Verify("secret", "100000.invalid.invalid"));
        }

        [TestMethod]
        public void Hash_RejectsWhitespaceOnlyPasswords()
        {
            Assert.ThrowsException<ArgumentException>(() => PasswordHasher.Hash(" "));
        }
    }
}
