using System.Collections.Generic;
using View.Models;

namespace View.ViewModels.DatabaseServices
{
    public interface IShareUserRepository
    {
        AccountModel GetAccount(string email);
        ProjectModel GetProject(string projectName);
        ShareUserProject GetShare(string email, ProjectModel project);
        List<ShareUserProject> GetByProject(ProjectModel project);
        List<ShareUserProject> GetByAccount(AccountModel account);
        void Create(ShareUserProject share);
        void Delete(ShareUserProject share);
    }
}
