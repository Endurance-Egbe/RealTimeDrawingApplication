using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using View.Infrastructure;
using View.Infrastructure.Query;
using View.Models;
using View.ViewModels.ProxyModel;

namespace View.ViewModels.DatabaseServices
{
    public class ShareUserService
    {
        public static bool isUserVerified;
        public static AccountProxyModel GetAccountModel;
        public static void ShareProject(string email, string projectName)
        {
            ShareUserProject shareModel = new ShareUserProject();
            //shareModel. = projectProxyModel.ProjectName;
            //string currentProjectName = projectModel.ProjectName;
            var sqlite = new SQLiteEFCore();
            var model = new ShareUserProjectEFCoreRepository(sqlite);
            AccountModel accountModel =model.GetCurrentAccountModel(email);
            var projectModel = model.GetCurrentProjectModel(projectName);
            var getShareUser = model.GetShareUser(accountModel);
            if (accountModel != null&& projectModel != null&& getShareUser==null)
            {
                GetAccountModel = new AccountProxyModel();
                GetAccountModel.FullName = accountModel.FullName;
                GetAccountModel.Email = accountModel.Email;
                shareModel.Users = accountModel;
                shareModel.Email = accountModel.Email;
                shareModel.Project = projectModel;
                //projectModel.ShareUsers.Add(shareModel);
                model.CreateModel(shareModel);
                isUserVerified = true;
                return;
            }
            GetAccountModel = null;
            isUserVerified = false;
        }
        public static List<ShareUserProject> GetShareUsers(string email)
        {
            var sqlite = new SQLiteEFCore();
            var model = new ShareUserProjectEFCoreRepository(sqlite);
            AccountModel accountModel = model.GetCurrentAccountModel(email);
            var getShareUsers = model.GetShareUsers(accountModel);
            return getShareUsers;
        }
        public static void DeleteShareUsers(string email)
        {
            var sqlite = new SQLiteEFCore();
            var model = new ShareUserProjectEFCoreRepository(sqlite);
            var shareUsers = GetShareUsers(email);
            foreach (var item in shareUsers)
            {
                model.DeleteShareUser(item);
            }
        }
    }
}
