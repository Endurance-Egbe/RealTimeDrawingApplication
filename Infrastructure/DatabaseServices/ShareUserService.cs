using System;
using System.Collections.Generic;
using View.Models;
using View.ViewModels.ProxyModel;
using View.Infrastructure;
using View.Infrastructure.Logging;

namespace View.ViewModels.DatabaseServices
{
    public class ShareUserService
    {
        private readonly IShareUserRepository repository;

        public bool IsUserVerified { get; private set; }
        public AccountProxyModel SharedAccount { get; private set; }

        public ShareUserService(IShareUserRepository repository)
        {
            this.repository = repository;
        }

        public void ShareProject(string email, string projectName)
        {
            try
            {
                var account = repository.GetAccount(email);
                var project = repository.GetProject(projectName);
                var existingShare = account == null || project == null
                    ? null
                    : repository.GetShare(account.Email, project);

                if (account == null || project == null || existingShare != null)
                {
                    SharedAccount = null;
                    IsUserVerified = false;
                    return;
                }

                repository.Create(new ShareUserProject
                {
                    Users = account,
                    Email = account.Email,
                    Project = project
                });
                SharedAccount = new AccountProxyModel
                {
                    FullName = account.FullName,
                    Email = account.Email
                };
                IsUserVerified = true;
                ApplicationLoggerProvider.Current.LogInformation(
                    "Project.Share", "Project shared successfully.");
            }
            catch (Exception exception)
            {
                ApplicationLoggerProvider.Current.LogError("Project.Share", exception);
                throw new ApplicationOperationException(
                    "The project could not be shared.", exception);
            }
        }

        public List<ShareUserProject> GetShareUsers(ProjectProxyModel projectProxyModel)
        {
            return repository.GetByProject(
                repository.GetProject(projectProxyModel.ProjectName));
        }

        public void DeleteShareUsers(ProjectProxyModel projectProxyModel)
        {
            foreach (var share in GetShareUsers(projectProxyModel))
            {
                repository.Delete(share);
            }
        }

        public List<ShareUserProject> GetShareUsers(AccountProxyModel accountProxyModel)
        {
            return repository.GetByAccount(repository.GetAccount(accountProxyModel.Email));
        }
    }
}
