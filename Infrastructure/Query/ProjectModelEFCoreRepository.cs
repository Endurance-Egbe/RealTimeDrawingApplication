using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using View.Models;

namespace View.Infrastructure.Query
{
    public class ProjectModelEFCoreRepository : EFCoreRepository<ProjectModel>
    {
        private UserDbContext userContext;
        public ProjectModelEFCoreRepository(UserDbContext _userContext) : base(_userContext)
        {
            userContext = _userContext;
        }
        public void CreateProjectModel(string email, ProjectModel projectModel)
        {
            AccountModel model = userContext.Set<AccountModel>().FirstOrDefault(x => x.Email == email);
            projectModel.User = model;
            userContext.Add(projectModel);
            userContext.SaveChanges();
        }
        //public List<ProjectModel> GetProjectModels(string email)
        //{
        //    AccountModel accountModel = userContext.Set<AccountModel>().FirstOrDefault(x => x.Email == email);
        //    ProjectModel model = userContext.Set<ProjectModel>().FirstOrDefault(x => x.User == accountModel);
            
        //    if (model != null)
        //    {
        //        return model.ProjectName;
        //    }
        //    return null;
        //}
    }
}
