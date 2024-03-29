﻿using Prism.Events;
using Prism.Mvvm;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using View.ViewModels.Common;
using View.ViewModels.Common.Event_Container;
using View.ViewModels.ProxyModel;
using System.Windows;
using Prism.Commands;
using System.Windows.Controls;
using View.ViewModels.DatabaseServices;
using View.Helper.Common.Event_Container;

namespace View.ViewModels
{
    public class ProjectWindowViewModel : BindableBase
    {
        private bool isChecked;
        private ProjectProxyModel selectedProject;
        private string projectName;

        public ProjectWindowViewModel()
        {
            EventAggregator = GenericServiceLocator.ShellContainer.Resolve<IEventAggregator>();
            EventAggregator.GetEvent<CurrentProjectEvent>().Subscribe(CurrentProjectModel);
            EventAggregator.GetEvent<RemoveProjectEvent>().Subscribe(RemoveProject);
            ProjectList = new ObservableCollection<ProjectProxyModel>();
            EventAggregator.GetEvent<GetAllUserProjectsEvent>().Subscribe(SetAllProjects);
        }
        public ObservableCollection<ProjectProxyModel> ProjectList { get; set; }
        public bool IsChecked { get => isChecked; set { isChecked = value; RaisePropertyChanged(); } }
        public string ProjectName { get => projectName; set { projectName = value; RaisePropertyChanged(); } }
        public AccountProxyModel AccountProxyModel { get; set; }
        public ProjectProxyModel SelectedProject
        { 
            get => selectedProject;
            set
            { 
                selectedProject = value;
                EventAggregator.GetEvent<ClearDrawingCanvasEvent>().Publish();
                EventAggregator.GetEvent<ActiveProjectEvent>().Publish(selectedProject);
                
                GetComponents(SelectedProject);
                RaisePropertyChanged();
            }
        }
        public IEnumerable<DrawingComponentProxyModel> GetDrawingComponents {get;set;}
        public IEventAggregator EventAggregator { get; set; }
        
        //public Project Project { get; set; }
        void CurrentProjectModel(ProjectProxyModel projectProxy)
        {
            SelectedProject = new ProjectProxyModel() { ProjectName = projectProxy.ProjectName};
            ProjectList.Add(SelectedProject);
            //ProjectName = SelectedProject.ProjectName;
            
        }
        void CurrentAccountModel(AccountProxyModel accountProxy)
        {
            AccountProxyModel = accountProxy;
        }
        void GetComponents(ProjectProxyModel project)
        {
            if (project!=null)
            {
                GetDrawingComponents = DrawingComponentService.GetDrawings(project);
                EventAggregator.GetEvent<DrawingComponentEvent>().Publish(GetDrawingComponents);
            }
          
        }
        void SetAllProjects(ObservableCollection<string> projectNames)
        {

            foreach (var item in projectNames)
            {
                ProjectProxyModel projects = new ProjectProxyModel();
                projects.ProjectName = item;
                ProjectList.Add(projects);
            }
        }
        void RemoveProject(ProjectProxyModel projectProxyModel)
        {
            ProjectList.Remove(projectProxyModel);
        }
    }
    //public class Project
    //{
    //    public string ProjectName { get; set; }
       
    //}
}
