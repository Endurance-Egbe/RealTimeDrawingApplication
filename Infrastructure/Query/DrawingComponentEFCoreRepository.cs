using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using View.Models;

namespace View.Infrastructure.Query
{
    public class DrawingComponentEFCoreRepository : EFCoreRepository<DrawingComponentModel>
    {
        //private UserDbContext userContext;
        public DrawingComponentEFCoreRepository(UserDbContext _userContext) : base(_userContext)
        {
            //userContext = _userContext;
        }
        public ProjectModel GetCurrentProject(string projectName)
        {

            ProjectModel model = userContext.Set<ProjectModel>().FirstOrDefault(x => x.ProjectName == projectName);
            return model;
        }
        public List<DrawingComponentModel> GetDrawingComponents(ProjectModel projectModel)
        {

            var components = userContext.DrawingComponentModels
                .Where(x => x.Project == projectModel)
                .ToList();

            return components;
        }
    }
}
