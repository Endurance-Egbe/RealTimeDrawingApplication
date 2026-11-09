using System.Collections.Generic;
using View.Models;

namespace View.ViewModels.DatabaseServices
{
    public interface IProjectRepository
    {
        void Create(string email, ProjectModel project);
        List<ProjectModel> GetByAccountEmail(string email);
        ProjectModel GetByName(string projectName);
        void Delete(ProjectModel project);
    }
}
