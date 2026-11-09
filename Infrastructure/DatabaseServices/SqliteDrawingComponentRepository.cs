using System;
using System.Collections.Generic;
using View.Infrastructure;
using View.Infrastructure.Query;
using View.Models;

namespace View.ViewModels.DatabaseServices
{
    public class SqliteDrawingComponentRepository : IDrawingComponentRepository, IDisposable
    {
        private readonly SQLiteEFCore sqlite = new SQLiteEFCore();

        public ProjectModel GetCurrentProject(string projectName)
        {
            return new DrawingComponentEFCoreRepository(sqlite).GetCurrentProject(projectName);
        }

        public List<DrawingComponentModel> GetDrawingComponents(ProjectModel project)
        {
            return new DrawingComponentEFCoreRepository(sqlite).GetDrawingComponents(project);
        }

        public void Create(DrawingComponentModel drawing)
        {
            new DrawingComponentEFCoreRepository(sqlite).CreateModel(drawing);
        }

        public void Delete(DrawingComponentModel drawing)
        {
            new DrawingComponentEFCoreRepository(sqlite).DeleteModel(drawing);
        }

        public void Dispose()
        {
            sqlite.Dispose();
        }
    }
}
