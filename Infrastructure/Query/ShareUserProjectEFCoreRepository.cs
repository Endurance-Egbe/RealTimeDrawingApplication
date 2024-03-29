﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using View.Models;

namespace View.Infrastructure.Query
{
    public class ShareUserProjectEFCoreRepository : EFCoreRepository<ShareUserProject>
    {
        //private UserDbContext userContext;
        public ShareUserProjectEFCoreRepository(UserDbContext _userContext) : base(_userContext)
        {
            //userContext = _userContext;
        }
        public AccountModel GetCurrentAccountModel(string email)
        {

            AccountModel model = userContext.Set<AccountModel>().FirstOrDefault(x => x.Email == email);
            if (model!=null)
            {
                return model;
            }
            return null;
        }
        public ProjectModel GetCurrentProjectModel(string projectName)
        {

            ProjectModel model = userContext.Set<ProjectModel>().FirstOrDefault(x => x.ProjectName == projectName);
            if (model != null)
            {
                return model;
            }
            return null;
        }
        public List<ShareUserProject> GetShareUsers(AccountModel accountModel)
        {
            var shareUsers = userContext.ShareUserProjects
                .Where(x => x.Users == accountModel)
                .ToList();

            return shareUsers;
        }

        public List<ShareUserProject> GetShareUsers(ProjectModel projectModel)
        {
            var shareUsers = userContext.ShareUserProjects
                .Where(x => x.Project == projectModel)
                .ToList();

            return shareUsers;
        }
        public void DeleteShareUser(ShareUserProject shareUser)
        {
            DeleteModel(shareUser);
            
        }
        public ShareUserProject GetShareUser(string email)
        {
            return userContext.Set<ShareUserProject>().FirstOrDefault(x => x.Email == email);
        }
    }
}
