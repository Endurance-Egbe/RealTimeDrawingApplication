using System;
using System.Collections.ObjectModel;
using View.Models;
using View.ViewModels.ProxyModel;
using View.Infrastructure;
using View.Infrastructure.Logging;

namespace View.ViewModels.DatabaseServices
{
    public class ProjectService
    {
        private readonly IProjectRepository repository;
        private readonly DrawingComponentService drawingComponentService;
        private readonly ShareUserService shareUserService;

        public ProjectService(
            IProjectRepository repository,
            DrawingComponentService drawingComponentService = null,
            ShareUserService shareUserService = null)
        {
            this.repository = repository;
            this.drawingComponentService = drawingComponentService;
            this.shareUserService = shareUserService;
        }

        public void CreateProject(string email, ProjectProxyModel projectProxyModel)
        {
            try
            {
                repository.Create(email, new ProjectModel
                {
                    ProjectName = projectProxyModel.ProjectName
                });
                ApplicationLoggerProvider.Current.LogInformation(
                    "Project.Create", "Project created successfully.");
            }
            catch (Exception exception)
            {
                ApplicationLoggerProvider.Current.LogError("Project.Create", exception);
                throw new ApplicationOperationException(
                    "The project could not be created.", exception);
            }
        }

        public ObservableCollection<string> GetAllProjects(AccountProxyModel accountProxy)
        {
            var projects = new ObservableCollection<string>();
            var projectModels = repository.GetByAccountEmail(accountProxy.Email);
            if (projectModels == null)
            {
                return projects;
            }

            foreach (var project in projectModels)
            {
                projects.Add(project.ProjectName);
            }

            return projects;
        }

        public void DeleteProject(string email, ProjectProxyModel projectProxyModel)
        {
            drawingComponentService?.DeleteDrawings(projectProxyModel);
            shareUserService?.DeleteShareUsers(projectProxyModel);
            repository.Delete(repository.GetByName(projectProxyModel.ProjectName));
        }

        public ProjectModel GetProject(string projectName)
        {
            return repository.GetByName(projectName);
        }
    }
}
