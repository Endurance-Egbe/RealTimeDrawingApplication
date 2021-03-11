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
    public class ProjectService
    {
        //public static bool isProjectNameInDatabase;
        public static void CreateProject(string email, ProjectProxyModel projectProxyModel)
        {
            ProjectModel projectModel = new ProjectModel();
            projectModel.ProjectName = projectProxyModel.ProjectName;
            //string currentProjectName = projectModel.ProjectName;
            var sqlite = new SQLiteEFCore();
            var model = new ProjectModelEFCoreRepository(sqlite);
            model.CreateProjectModel(email, projectModel);
            
        }
       
    }
}
