using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using View.Models;
using View.ViewModels.DatabaseServices;
using View.ViewModels.ProxyModel;

namespace RealTimeDrawingApplication.Tests
{
    [TestClass]
    public class ProjectAndShareServiceTests
    {
        [TestMethod]
        public void CreateProject_PersistsProjectNameForAccount()
        {
            var repository = new InMemoryProjectRepository();
            var service = new ProjectService(repository);

            service.CreateProject("user@example.com", new ProjectProxyModel
            {
                ProjectName = "Planning"
            });

            Assert.AreEqual("user@example.com", repository.Email);
            Assert.AreEqual("Planning", repository.Created.ProjectName);
        }

        [TestMethod]
        public void GetAllProjects_ReturnsProjectNamesForAccount()
        {
            var repository = new InMemoryProjectRepository();
            repository.Projects.Add(new ProjectModel { ProjectName = "One" });
            repository.Projects.Add(new ProjectModel { ProjectName = "Two" });
            var service = new ProjectService(repository);

            var result = service.GetAllProjects(new AccountProxyModel
            {
                Email = "user@example.com"
            });

            CollectionAssert.AreEqual(new[] { "One", "Two" }, result.ToArray());
        }

        [TestMethod]
        public void ShareProject_CreatesShareForExistingAccountAndProject()
        {
            var repository = new InMemoryShareRepository();
            var service = new ShareUserService(repository);

            service.ShareProject("user@example.com", "Planning");

            Assert.IsTrue(service.IsUserVerified);
            Assert.AreEqual("user@example.com", service.SharedAccount.Email);
            Assert.AreEqual(1, repository.Created.Count);
        }

        [TestMethod]
        public void ShareProject_RejectsDuplicateShare()
        {
            var repository = new InMemoryShareRepository();
            repository.ExistingShare = new ShareUserProject();
            var service = new ShareUserService(repository);

            service.ShareProject("user@example.com", "Planning");

            Assert.IsFalse(service.IsUserVerified);
            Assert.IsNull(service.SharedAccount);
            Assert.AreEqual(0, repository.Created.Count);
        }

        private sealed class InMemoryProjectRepository : IProjectRepository
        {
            public string Email { get; private set; }
            public ProjectModel Created { get; private set; }
            public List<ProjectModel> Projects { get; } = new List<ProjectModel>();

            public void Create(string email, ProjectModel project)
            {
                Email = email;
                Created = project;
            }

            public List<ProjectModel> GetByAccountEmail(string email) => Projects;
            public ProjectModel GetByName(string projectName) =>
                Projects.FirstOrDefault(project => project.ProjectName == projectName);
            public void Delete(ProjectModel project) { }
        }

        private sealed class InMemoryShareRepository : IShareUserRepository
        {
            private readonly AccountModel account = new AccountModel
            {
                Email = "user@example.com",
                FullName = "Test User"
            };
            private readonly ProjectModel project = new ProjectModel
            {
                ProjectName = "Planning"
            };

            public ShareUserProject ExistingShare { get; set; }
            public List<ShareUserProject> Created { get; } =
                new List<ShareUserProject>();

            public AccountModel GetAccount(string email) =>
                email == account.Email ? account : null;
            public ProjectModel GetProject(string projectName) =>
                projectName == project.ProjectName ? project : null;
            public ShareUserProject GetShare(string email, ProjectModel currentProject) =>
                ExistingShare;
            public List<ShareUserProject> GetByProject(ProjectModel currentProject) =>
                new List<ShareUserProject>();
            public List<ShareUserProject> GetByAccount(AccountModel currentAccount) =>
                new List<ShareUserProject>();
            public void Create(ShareUserProject share) => Created.Add(share);
            public void Delete(ShareUserProject share) { }
        }
    }
}
