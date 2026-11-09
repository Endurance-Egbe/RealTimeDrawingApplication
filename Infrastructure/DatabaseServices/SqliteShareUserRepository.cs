using System.Collections.Generic;
using View.Infrastructure;
using View.Infrastructure.Query;
using View.Models;

namespace View.ViewModels.DatabaseServices
{
    public class SqliteShareUserRepository : IShareUserRepository
    {
        private readonly SQLiteEFCore sqlite = new SQLiteEFCore();
        private ShareUserProjectEFCoreRepository Repository =>
            new ShareUserProjectEFCoreRepository(sqlite);

        public AccountModel GetAccount(string email) => Repository.GetCurrentAccountModel(email);
        public ProjectModel GetProject(string projectName) => Repository.GetCurrentProjectModel(projectName);
        public ShareUserProject GetShare(string email, ProjectModel project) =>
            Repository.GetShareUser(email, project);
        public List<ShareUserProject> GetByProject(ProjectModel project) =>
            Repository.GetShareUsers(project);
        public List<ShareUserProject> GetByAccount(AccountModel account) =>
            Repository.GetShareUsers(account);
        public void Create(ShareUserProject share) => Repository.CreateModel(share);
        public void Delete(ShareUserProject share) => Repository.DeleteShareUser(share);
    }
}
