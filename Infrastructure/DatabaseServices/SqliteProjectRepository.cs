using System.Collections.Generic;
using View.Infrastructure;
using View.Infrastructure.Query;
using View.Models;

namespace View.ViewModels.DatabaseServices
{
    public class SqliteProjectRepository : IProjectRepository
    {
        private readonly SQLiteEFCore sqlite = new SQLiteEFCore();
        private ProjectModelEFCoreRepository Repository =>
            new ProjectModelEFCoreRepository(sqlite);

        public void Create(string email, ProjectModel project) =>
            Repository.CreateProjectModel(email, project);

        public List<ProjectModel> GetByAccountEmail(string email) =>
            Repository.GetProjectModels(email);

        public ProjectModel GetByName(string projectName) =>
            Repository.GetProjectModel(projectName);

        public void Delete(ProjectModel project) =>
            Repository.DeleteProjectModel(project);
    }
}
