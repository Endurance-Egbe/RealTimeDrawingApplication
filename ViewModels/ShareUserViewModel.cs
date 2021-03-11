using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using View.ViewModels.Common;
using View.ViewModels.Common.Event_Container;
using Prism.Ioc;
using View.ViewModels.ProxyModel;
using View.ViewModels.DatabaseServices;
using System.Windows;

namespace View.ViewModels
{
    public class ShareUserViewModel:BindableBase
    {
        private string email;

        public ShareUserViewModel(ProjectProxyModel project)
        {
            //IEventAggregator eventAggregator = new EventAggregator();
            EventAggregator = GenericServiceLocator.ShellContainer.Resolve<IEventAggregator>();
            
            CurrentProjectModel = project;
            ShareUserCommand = new DelegateCommand(ShareUser);
        }

        public string Email { get => email; set { email = value; RaisePropertyChanged(); } }
        public ProjectProxyModel CurrentProjectModel { get; set; }
        public DelegateCommand ShareUserCommand { get; set; }
        public AccountProxyModel AccountProxy { get; set; }
        public IEventAggregator EventAggregator { get; set; }
        //public string ProjectName { get; set; }

        //void CurrentProject(ProjectProxyModel currentProject)
        //{
        //    CurrentProjectModel = currentProject;
        //}
        private void ShareUser()
        {
            ShareUserService.ShareProject(email, CurrentProjectModel.ProjectName);
            if (ShareUserService.isUserVerified == true)
            {
                AccountProxy = ShareUserService.GetAccountModel;
                MessageBox.Show("Email is Verified...\n\rUser Shared Successfully", 
                    "Success Message", MessageBoxButton.OK);
                //EventAggregator.GetEvent<CurrentProjectEvent>().Publish(CurrentProjectModel);
                EventAggregator.GetEvent<CurrentAccountModelEvent>().Publish(AccountProxy);
                return;
            }
            MessageBox.Show("Email not Verified...\n\rEnter Verified Email", 
                "Error Message", MessageBoxButton.OK);
            
        }

    }
}
