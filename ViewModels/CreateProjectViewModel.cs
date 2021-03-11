using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using View.ViewModels.Common;
using View.ViewModels.Common.Event_Container;
using View.ViewModels.DatabaseServices;
using View.ViewModels.ProxyModel;
using Prism.Ioc;
using View.UserControls;
using View.Windows;

namespace View.ViewModels
{
   
    public class CreateProjectViewModel:BindableBase
    {
        private string projectName;

        public CreateProjectViewModel(AccountProxyModel accountProxy)
        {

            EventAggregator = GenericServiceLocator.ShellContainer.Resolve<IEventAggregator>();
            //EventAggregator.GetEvent<CurrentAccountModelEvent>().Subscribe(CurrentAccount);
            CurrentUser = accountProxy;
            CreateProjectCommand = new DelegateCommand(CreateProject);
        }
        public string ProjectName { get => projectName; set { projectName = value; RaisePropertyChanged(); } }
        public DelegateCommand CreateProjectCommand { get; set; }
        public ProjectProxyModel CurrentProject { get; set; }
        public AccountProxyModel CurrentUser { get; set; }
        public IEventAggregator EventAggregator {  get; set; }
        //void CurrentAccount(AccountProxyModel currentUser)
        //{
        //    CurrentUser = currentUser;
        //}
        public void CreateProject()
        {
            //EventAggregator = new EventAggregator();
           
            CurrentProject = new ProjectProxyModel();
            CurrentProject.ProjectName = projectName;
           
            ProjectService.CreateProject(CurrentUser.Email, CurrentProject);
            EventAggregator.GetEvent<ClearDrawingCanvasEvent>().Publish();
            EventAggregator.GetEvent<CurrentProjectEvent>().Publish(CurrentProject);

            MessageBox.Show("Project Added to the database",
                "Success Message", MessageBoxButton.OK);
            //bool isProjectNameInDatabase = ProjectService.isProjectNameInDatabase;
            //if (isProjectNameInDatabase)
            //{
            //    MessageBox.Show("Project Already Exist",
            //   "Error Message", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
            //else
            //{
            //    MessageBox.Show("Project Added to the database",
            //    "Success Message", MessageBoxButton.OK);
            //}

        }
    }
}
