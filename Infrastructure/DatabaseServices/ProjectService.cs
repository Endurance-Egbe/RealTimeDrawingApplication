using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using View.Infrastructure;
using View.Infrastructure.Query;
using View.Models;
using View.ViewModels.ProxyModel;
using System.Collections.ObjectModel;

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
       public static ObservableCollection<string> GetAllProjects(AccountProxyModel accountProxy)
       {
            ObservableCollection<string> projects = new ObservableCollection<string>();
            List<ProjectModel> projectModels = new List<ProjectModel>();
            var sqlite = new SQLiteEFCore();
            var model = new ProjectModelEFCoreRepository(sqlite);
            projectModels = model.GetProjectModels(accountProxy.Email);
            foreach (var item in projectModels)
            {
                projects.Add(item.ProjectName);
            }
            return projects;
       }
        public static void DeleteProject(string email,ProjectProxyModel projectProxyModel)
        {
            ProjectModel projectModel = new ProjectModel();
            var sqlite = new SQLiteEFCore();
            var model = new ProjectModelEFCoreRepository(sqlite);
            projectModel = model.GetProjectModel(projectProxyModel.ProjectName);
            ShareUserService.DeleteShareUsers(email);
            DrawingComponentService.DeleteDrawings(projectProxyModel);
            model.DeleteProjectModel(projectModel);
        }
    }
}
