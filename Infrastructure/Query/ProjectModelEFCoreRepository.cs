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
        //private UserDbContext userContext;
        public ProjectModelEFCoreRepository(UserDbContext _userContext) : base(_userContext)
        {
            //userContext = _userContext;
        }
        public void CreateProjectModel(string email, ProjectModel projectModel)
        {
            AccountModel model = userContext.Set<AccountModel>().FirstOrDefault(x => x.Email == email);
            projectModel.User = model;
            userContext.Add(projectModel);
            userContext.SaveChanges();
        }
        public List<ProjectModel> GetProjectModels(string email)
        {
            AccountModel accountModel = userContext.Set<AccountModel>().FirstOrDefault(x => x.Email == email);
            var model = userContext.Projects
               .Where(x => x.User == accountModel)
               .ToList();
            //ProjectModel model = userContext.Set<ProjectModel>().Find(x => x.User == accountModel);

            if (model != null)
            {
                return model;
            }
            return null;
        }
        public ProjectModel GetProjectModel(string projectName)
        {

            ProjectModel model = userContext.Set<ProjectModel>().FirstOrDefault(x => x.ProjectName == projectName);

                return model;
        }
        public void DeleteProjectModel(ProjectModel projectModel)
        {
            //foreach (var item in projectModel.ShapeComponents)
            //{
            //    userContext.Remove(item);
            //}
            userContext.Remove(projectModel);
            userContext.SaveChanges();
        }
    }
}
