using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using View.Infrastructure;
using View.Models;
using View.ViewModels.ProxyModel;

namespace View.ViewModels.DatabaseServices
{
    public class AccountService
    {
        public static void CreateAccount(AccountProxyModel accountProxyModel)
        {
            AccountModel accountModel = new AccountModel();
            accountModel.FullName = accountProxyModel.FullName;
            accountModel.Email = accountProxyModel.Email;
            accountModel.Password = accountProxyModel.Password;

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
            var getmodel = model.GetModel(accountModel.Email);
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
            var getmodel = model.GetModel(accountModel.Email);
            if (getmodel!=null)
            {
                if (getmodel.Email == email && getmodel.Password == password)
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
            accountModel.Password = accountProxyModel.Password;

            var sqlite = new SQLiteEFCore();
            var model = new AccountModelEFCoreRepository(sqlite);
            var getmodel = model.GetModel(accountModel.Email);
            if (getmodel!=null)
            {
                accountProxyModel.FullName = getmodel.FullName;
                accountProxyModel.Email = getmodel.Email;
                accountProxyModel.Password = getmodel.Password;
                return accountProxyModel;
            }
            
            //accountProxyModel.ProjectProxyModels = getmodel.Projects;
            return null;
        }
    }
}
