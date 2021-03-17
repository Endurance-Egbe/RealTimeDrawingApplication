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
            var getShareUser = model.GetShareUser(accountModel.Email);
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
        public static List<ShareUserProject> GetShareUsers(ProjectProxyModel projectProxyModel)
        {
            var sqlite = new SQLiteEFCore();
            var model = new ShareUserProjectEFCoreRepository(sqlite);
            var projectModel = ProjectService.GetProject(projectProxyModel.ProjectName);
            var getShareUsers = model.GetShareUsers(projectModel);
            return getShareUsers;
        }
        public static void DeleteShareUsers(ProjectProxyModel projectProxyModel)
        {
            var sqlite = new SQLiteEFCore();
            var model = new ShareUserProjectEFCoreRepository(sqlite);
            var shareUsers = GetShareUsers(projectProxyModel);
            foreach (var item in shareUsers)
            {
                model.DeleteShareUser(item);
            }
        }
    }
}
