using System;
using View.Models;
using View.ViewModels.ProxyModel;
using View.Security;
using View.Infrastructure;
using View.Infrastructure.Logging;

namespace View.ViewModels.DatabaseServices
{
    public class AccountService
    {
        private readonly IAccountRepository accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public void CreateAccount(AccountProxyModel accountProxyModel)
        {
            try
            {
                accountRepository.Create(new AccountModel
                {
                    FullName = accountProxyModel.FullName,
                    Email = accountProxyModel.Email,
                    PasswordHash = PasswordHasher.Hash(accountProxyModel.Password)
                });
                ApplicationLoggerProvider.Current.LogInformation(
                    "Account.Create", "Account created successfully.");
            }
            catch (Exception exception)
            {
                ApplicationLoggerProvider.Current.LogError("Account.Create", exception);
                throw new ApplicationOperationException(
                    "The account could not be created.", exception);
            }
        }

        public bool EmailValidations(string email)
        {
            return accountRepository.GetByEmail(email) != null;
        }

        public bool EmailPasswordValidation(string email, string password)
        {
            var account = accountRepository.GetByEmail(email);
            return account != null
                && account.Email == email
                && PasswordHasher.Verify(password, account.PasswordHash);
        }

        public AccountProxyModel GetAccount(AccountProxyModel accountProxyModel)
        {
            var account = accountRepository.GetByEmail(accountProxyModel.Email);
            if (account != null)
            {
                accountProxyModel.FullName = account.FullName;
                accountProxyModel.Email = account.Email;
                return accountProxyModel;
            }

            return null;
        }

        public AccountProxyModel Authenticate(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            var account = accountRepository.GetByEmail(email.Trim());
            if (account == null || !PasswordHasher.Verify(password, account.PasswordHash))
            {
                return null;
            }

            return new AccountProxyModel
            {
                Email = account.Email,
                FullName = account.FullName
            };
        }

        public AccountModel GetAccountModel(AccountProxyModel accountProxyModel)
        {
            return accountRepository.GetByEmail(accountProxyModel.Email);
        }
    }
}
