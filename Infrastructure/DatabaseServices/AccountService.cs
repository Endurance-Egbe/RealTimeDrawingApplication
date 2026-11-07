using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using View.Infrastructure;
using View.Models;
using View.ViewModels.ProxyModel;
using View.Security;

namespace View.ViewModels.DatabaseServices
{
    public class AccountService
    {
        public static void CreateAccount(AccountProxyModel accountProxyModel)
        {
            AccountModel accountModel = new AccountModel();
            accountModel.FullName = accountProxyModel.FullName;
            accountModel.Email = accountProxyModel.Email;
            accountModel.PasswordHash = PasswordHasher.Hash(accountProxyModel.Password);

            var sqlite = new SQLiteEFCore();
            var model = new AccountModelEFCoreRepository(sqlite);
            model.CreateModel(accountModel);
        }
        public static bool EmailValidations(string email)
        {
            AccountModel accountModel = new AccountModel();
            accountModel.Email = email;
            var sqlite = new SQLiteEFCore();
            var model = new AccountModelEFCoreRepository(sqlite);
            var getmodel = model.GetModelByEmail(accountModel.Email);
            if (getmodel != null)
            {
                return true;
            }
            return false;
        }
        public static bool EmailPasswordValidation(string email, string password)
        {
            AccountModel accountModel = new AccountModel();
            accountModel.Email = email;
            var sqlite = new SQLiteEFCore();
            var model = new AccountModelEFCoreRepository(sqlite);
            var getmodel = model.GetModelByEmail(accountModel.Email);
            if (getmodel!=null)
            {
                if (getmodel.Email == email && PasswordHasher.Verify(password, getmodel.PasswordHash))
                {
                    return true;
                }
            }
            
            return false;
        }
        public static AccountProxyModel GetAccount(AccountProxyModel accountProxyModel)
        {
            AccountModel accountModel = new AccountModel();
            
            accountModel.Email = accountProxyModel.Email;
            var sqlite = new SQLiteEFCore();
            var model = new AccountModelEFCoreRepository(sqlite);
            var getmodel = model.GetModelByEmail(accountModel.Email);
            if (getmodel!=null)
            {
                accountProxyModel.FullName = getmodel.FullName;
                accountProxyModel.Email = getmodel.Email;
                return accountProxyModel;
            }
            
            //accountProxyModel.ProjectProxyModels = getmodel.Projects;
            return null;
        }

        public static AccountProxyModel Authenticate(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            var sqlite = new SQLiteEFCore();
            var model = new AccountModelEFCoreRepository(sqlite);
            var account = model.GetModelByEmail(email.Trim());
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
        public static AccountModel GetAccountModel(AccountProxyModel accountProxyModel)
        {
            AccountModel accountModel = new AccountModel();

            accountModel.Email = accountProxyModel.Email;
            var sqlite = new SQLiteEFCore();
            var model = new AccountModelEFCoreRepository(sqlite);
            var getmodel = model.GetModelByEmail(accountModel.Email);
            
            return getmodel;
        }
    }
}
