using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using View.ViewModels.Common;
using View.ViewModels.ProxyModel;
using View.Windows;
using Prism.Ioc;
using View.ViewModels.Common.Event_Container;
using System.Windows.Media;
using View.ViewModels.Common.CustomColor;
using View.ViewModels.DatabaseServices;
using System.Windows;
using View.ViewModels.ShapeServices;
using System.Windows.Controls;
using System.Windows.Shapes;
using View.Export_Import;
using View.Helper.Common.Event_Container;

namespace View.ViewModels
{
    public class MenuPaneViewModel : BindableBase
    {
        private string accountName;
        private string accountEmail;
        private bool isOpen;

        public MenuPaneViewModel(AccountProxyModel accountProxy)
        {
            AccountName = accountProxy.FullName;
            AccountEmail = accountProxy.Email;
            AccountProxy = accountProxy;

            EventAggregator = GenericServiceLocator.ShellContainer.Resolve<IEventAggregator>();
            EventAggregator.GetEvent<CurrentProjectEvent>().Subscribe(CurrentSaveProject);
            EventAggregator.GetEvent<ActiveProjectEvent>().Subscribe(ActiveProject);
            EventAggregator.GetEvent<ExportDrawingsEvent>().Subscribe(ExportDrawing);
            //EventAggregator.GetEvent<UpdatePropertyWindow>().Subscribe(UpdatePropertyWindowEvent);
            //EventAggregator.GetEvent<ComponentPropertyPubSubEvent>().Subscribe(UpdateCanvasElementEvent);

            //DrawingComponentProxys = new List<DrawingComponentProxyModel>();
            LogOutCommand = new DelegateCommand<string>(OpenWindow);
            CreateProjectCommand = new DelegateCommand<string>(OpenWindow);
            ShareProjectCommand = new DelegateCommand<string>(OpenWindow);
            DeleteProjectCommand = new DelegateCommand<string>(OpenWindow);
            SaveProjectCommand = new DelegateCommand<string>(OpenWindow);
            ExportProjectCommand = new DelegateCommand<string>(OpenWindow);
            ImportProjectCommand = new DelegateCommand<string>(OpenWindow);
            ExportXmlCommand = new DelegateCommand<string>(ExportDrawing);
            ExportJsonCommand = new DelegateCommand<string>(ExportDrawing);
            ImportXmlCommand = new DelegateCommand<string>(ImportDrawing);
            ImportJsonCommand = new DelegateCommand<string>(ImportDrawing);

        }
        public ProjectProxyModel CurrentProjectModel { get; set; }
        public AccountProxyModel AccountProxy { get; set; }
        public string AccountName { get => accountName; set { accountName = value; RaisePropertyChanged(); } }
        public string AccountEmail { get => accountEmail; set { accountEmail = value; RaisePropertyChanged(); } }
        public int MyProperty { get; set; }
        public bool IsOpenImport { get => isOpen; set { isOpen = value; RaisePropertyChanged(); } }
        public bool IsOpenExport { get => isOpen; set { isOpen = value; RaisePropertyChanged(); } }
        public IEnumerable<DrawingComponentProxyModel> DrawingComponentProxys { get; set; }
        public IEventAggregator EventAggregator { get; set; }
        public DelegateCommand<string> LogOutCommand { get; set; }
        public DelegateCommand<string> CreateProjectCommand { get; set; }
        public DelegateCommand<string> SaveProjectCommand { get; set; }
        public DelegateCommand<string> ShareProjectCommand { get; set; }
        public DelegateCommand<string> DeleteProjectCommand { get; set; }
        public DelegateCommand<string> ImportProjectCommand { get; set; }
        public DelegateCommand<string> ExportProjectCommand { get; set; }
        public DelegateCommand<string> ExportXmlCommand { get; set; }
        public DelegateCommand<string> ExportJsonCommand { get; set; }
        public DelegateCommand<string> ImportXmlCommand { get; set; }
        public DelegateCommand<string> ImportJsonCommand { get; set; }


        private void CurrentSaveProject(ProjectProxyModel project)
        {
            CurrentProjectModel = project;
        }
        private void ActiveProject(ProjectProxyModel project)
        {
            CurrentProjectModel = project;
        }
        //public ShapeComponent Shape { get; set; }
        public CustomCanvasViewModel Canvas { get; set; }
        void OpenWindow(string parameter)
        {

            switch (parameter)
            {
                case "Create Project":
                    CreateProject project = new CreateProject();
                    project.DataContext = new CreateProjectViewModel(AccountProxy);
                    project.ShowDialog();
                    break;
                case "Share Project":
                    ShareProject shareProject = new ShareProject();
                    shareProject.DataContext = new ShareUserViewModel(CurrentProjectModel);
                    shareProject.ShowDialog();
                    break;
                case "Delete Project":
                    ProjectService.DeleteProject(CurrentProjectModel);
                    MessageBox.Show("Project Deleted Successfully!", "Delete Message", MessageBoxButton.OK);
                    break;
                case "Save Project":
                    EventAggregator.GetEvent<CanvasComponentEvent>().Publish(CurrentProjectModel);
                    
                    break;
                case "Log Out":
                    LoginPage mainWindow = new LoginPage();
                    EventAggregator.GetEvent<CloseMainWindowEvent>().Publish();
                    mainWindow.ShowDialog();
                   
                    break;
                case "Import":
                    IsOpenImport = true;
                    break;
                case "Export":
                    IsOpenExport = true;
                    break;
                default:
                    break;
            }
        }
       // public IEnumerable<DrawingComponentProxyModel> ImportedDrawingComponents { get; set; }
        void ImportDrawing(string parameter)
        {
            switch (parameter)
            {
                case "ImportXml":
                    DrawingComponentProxies = XmlService.ImportWithXml();
                    EventAggregator.GetEvent<JsonImportedProxiesEvent>().Publish(DrawingComponentProxies);
                    break;
                case "ImportJson":
                    DrawingComponentProxies = JsonService.ImportWithJson();
                    EventAggregator.GetEvent<JsonImportedProxiesEvent>().Publish(DrawingComponentProxies);
                    break;
                default:
                    break;
            }
        }
        public IEnumerable<DrawingComponentProxyModel> DrawingComponentProxies { get; set; }
        void ExportDrawing(string parameter)
        {
            switch (parameter)
            {
                case "ExportXml":
                    XmlService.ExportWithXml(CurrentProjectModel);
                    break;
                case "ExportJson":
                    JsonService.ExportWithJson(CurrentProjectModel);
                    break;
                default:
                    break;
            }
        }
        
    }
}

    

