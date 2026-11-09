using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using View.Models;
using View.Security;
using View.ViewModels.DatabaseServices;
using View.ViewModels.ProxyModel;

namespace RealTimeDrawingApplication.Tests
{
    [TestClass]
    public class AccountServiceTests
    {
        [TestMethod]
        public void CreateAccount_StoresAHashedPassword()
        {
            var repository = new InMemoryAccountRepository();
            var service = new AccountService(repository);

            service.CreateAccount(new AccountProxyModel
            {
                Email = "user@example.com",
                FullName = "Test User",
                Password = "secret"
            });

            Assert.IsNotNull(repository.StoredAccount);
            Assert.AreEqual("user@example.com", repository.StoredAccount.Email);
            Assert.IsTrue(PasswordHasher.Verify("secret", repository.StoredAccount.PasswordHash));
            Assert.AreNotEqual("secret", repository.StoredAccount.PasswordHash);
        }

        [TestMethod]
        public void EmailValidations_ReturnsTrueWhenEmailExists()
        {
            var repository = new InMemoryAccountRepository();
            repository.Add("user@example.com", "secret", "Test User");

            var service = new AccountService(repository);

            Assert.IsTrue(service.EmailValidations("user@example.com"));
        }

        [TestMethod]
        public void EmailValidations_ReturnsFalseWhenEmailDoesNotExist()
        {
            var service = new AccountService(new InMemoryAccountRepository());

            Assert.IsFalse(service.EmailValidations("missing@example.com"));
        }

        [TestMethod]
        public void EmailPasswordValidation_AcceptsCorrectCredentials()
        {
            var repository = new InMemoryAccountRepository();
            repository.Add("user@example.com", "secret", "Test User");

            var service = new AccountService(repository);

            Assert.IsTrue(service.EmailPasswordValidation("user@example.com", "secret"));
        }

        [TestMethod]
        public void EmailPasswordValidation_RejectsIncorrectPassword()
        {
            var repository = new InMemoryAccountRepository();
            repository.Add("user@example.com", "secret", "Test User");

            var service = new AccountService(repository);

            Assert.IsFalse(service.EmailPasswordValidation("user@example.com", "wrong"));
        }

        [TestMethod]
        public void Authenticate_ReturnsAccountWithoutPassword()
        {
            var repository = new InMemoryAccountRepository();
            repository.Add("user@example.com", "secret", "Test User");

            var service = new AccountService(repository);
            var account = service.Authenticate(" user@example.com ", "secret");

            Assert.IsNotNull(account);
            Assert.AreEqual("user@example.com", account.Email);
            Assert.AreEqual("Test User", account.FullName);
            Assert.IsNull(account.Password);
        }

        [TestMethod]
        public void Authenticate_RejectsMissingOrInvalidCredentials()
        {
            var repository = new InMemoryAccountRepository();
            repository.Add("user@example.com", "secret", "Test User");
            var service = new AccountService(repository);

            Assert.IsNull(service.Authenticate(string.Empty, "secret"));
            Assert.IsNull(service.Authenticate("user@example.com", string.Empty));
            Assert.IsNull(service.Authenticate("user@example.com", "wrong"));
            Assert.IsNull(service.Authenticate("missing@example.com", "secret"));
        }

        private sealed class InMemoryAccountRepository : IAccountRepository
        {
            private readonly Dictionary<string, AccountModel> accounts =
                new Dictionary<string, AccountModel>();

            public AccountModel StoredAccount { get; private set; }

            public void Add(string email, string password, string fullName)
            {
                Create(new AccountModel
                {
                    Email = email,
                    FullName = fullName,
                    PasswordHash = PasswordHasher.Hash(password)
                });
            }

            public void Create(AccountModel account)
            {
                StoredAccount = account;
                accounts[account.Email] = account;
            }

            public AccountModel GetByEmail(string email)
            {
                AccountModel account;
                accounts.TryGetValue(email, out account);
                return account;
            }
        }
    }
}
