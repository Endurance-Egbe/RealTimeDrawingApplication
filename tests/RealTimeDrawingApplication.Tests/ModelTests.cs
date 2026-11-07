using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using View.Models;
using View.ViewModels.ProxyModel;

namespace RealTimeDrawingApplication.Tests
{
    [TestClass]
    public class ModelTests
    {
        [TestMethod]
        public void AccountModel_StoresIdentityAndProjects()
        {
            var project = new ProjectModel { ProjectName = "Sketch" };
            var account = new AccountModel
            {
                AccountId = 7,
                Email = "user@example.com",
                FullName = "Test User",
                PasswordHash = "hash",
                Projects = new List<ProjectModel> { project }
            };

            Assert.AreEqual(7, account.AccountId);
            Assert.AreEqual("user@example.com", account.Email);
            Assert.AreEqual("Test User", account.FullName);
            Assert.AreEqual("hash", account.PasswordHash);
            Assert.AreSame(project, account.Projects[0]);
        }

        [TestMethod]
        public void ProjectModel_StoresRelationships()
        {
            var account = new AccountModel { Email = "user@example.com" };
            var project = new ProjectModel
            {
                ProjectId = 3,
                ProjectName = "Sketch",
                User = account,
                ShareUsers = new List<ShareUserProject>(),
                ShapeComponents = new List<DrawingComponentModel>()
            };

            Assert.AreEqual(3, project.ProjectId);
            Assert.AreEqual("Sketch", project.ProjectName);
            Assert.AreSame(account, project.User);
            Assert.IsNotNull(project.ShareUsers);
            Assert.IsNotNull(project.ShapeComponents);
        }

        [TestMethod]
        public void DrawingComponentModel_ExcludesPersistenceFieldsFromJson()
        {
            var component = new DrawingComponentModel
            {
                DrawingComponentId = 11,
                Title = "Rectangle",
                X = 10,
                Y = 20,
                Width = 100,
                Height = 50,
                LineThickness = 2,
                StrokeThickness = 3,
                SelectedStroke = "Black",
                SelectedFillColor = "White",
                SelectedBorderColor = "Gray"
            };

            var json = JsonConvert.SerializeObject(component);

            StringAssert.Contains(json, "Rectangle");
            Assert.IsFalse(json.Contains("DrawingComponentId"));
            Assert.IsFalse(json.Contains("\"Project\""));
        }

        [TestMethod]
        public void ProxyModels_ExposeExpectedValues()
        {
            var account = new AccountProxyModel { Email = "user@example.com", FullName = "Test User" };
            var project = new ProjectProxyModel { ProjectName = "Sketch" };
            Assert.AreEqual("user@example.com", account.Email);
            Assert.AreEqual("Test User", account.FullName);
            Assert.AreEqual("Sketch", project.ProjectName);
        }
    }
}
