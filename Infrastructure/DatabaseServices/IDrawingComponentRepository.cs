using System.Collections.Generic;
using View.Models;

namespace View.ViewModels.DatabaseServices
{
    public interface IDrawingComponentRepository
    {
        ProjectModel GetCurrentProject(string projectName);
        List<DrawingComponentModel> GetDrawingComponents(ProjectModel project);
        void Create(DrawingComponentModel drawing);
        void Delete(DrawingComponentModel drawing);
    }
}
