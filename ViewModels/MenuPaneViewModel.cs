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
            //EventAggregator.GetEvent<UpdatePropertyWindow>().Subscribe(UpdatePropertyWindowEvent);
            //EventAggregator.GetEvent<ComponentPropertyPubSubEvent>().Subscribe(UpdateCanvasElementEvent);
            
            //DrawingComponentProxys = new List<DrawingComponentProxyModel>();
            LogOutCommand = new DelegateCommand<string>(OpenWindow);
            CreateProjectCommand = new DelegateCommand<string>(OpenWindow);
            ShareProjectCommand = new DelegateCommand<string>(OpenWindow);
            OpenProjectCommand = new DelegateCommand<string>(OpenWindow);
            SaveProjectCommand = new DelegateCommand<string>(OpenWindow);
            ExportProjectCommand = new DelegateCommand<string>(OpenWindow);
            ImportProjectCommand = new DelegateCommand<string>(OpenWindow);

        }
        public ProjectProxyModel CurrentProjectModel { get; set; }
        public AccountProxyModel AccountProxy { get; set; }
        public string AccountName { get => accountName; set { accountName = value; RaisePropertyChanged(); } }
        public string AccountEmail { get => accountEmail; set { accountEmail = value; RaisePropertyChanged(); } }
        public bool IsOpen { get => isOpen; set { isOpen = value; RaisePropertyChanged(); } }
        public IEnumerable<DrawingComponentProxyModel> DrawingComponentProxys { get; set; }
        public IEventAggregator EventAggregator { get; set; }
        public DelegateCommand<string> LogOutCommand { get; set; }
        public DelegateCommand<string> CreateProjectCommand { get; set; }
        public DelegateCommand<string> SaveProjectCommand { get; set; }
        public DelegateCommand<string> ShareProjectCommand { get; set; }
        public DelegateCommand<string> OpenProjectCommand { get; set; }
        public DelegateCommand<string> ImportProjectCommand { get; set; }
        public DelegateCommand<string> ExportProjectCommand { get; set; }


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
                case "Open Project":

                    break;
                case "Save Project":
                    EventAggregator.GetEvent<CanvasComponentEvent>().Publish(CurrentProjectModel);

                    break;
                case "Log Out":
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.ShowDialog();
                    break;
                case "Import":
                    IsOpen = true;
                    break;
                case "Export":
                    IsOpen = true;
                    break;
                default:
                    break;
            }
        }
        void UpdatePropertyWindowEvent(ComponentModel property)
        {
            //foreach (var component in DrawingComponentProxys)
            //{
            //    component.Title= property.Value.ToString();
            //    component.X = (double)property.Value;
            //    component.Y = (double)property.Value;
            //    component.Width = (double)property.Value;
            //    component.Height = (double)property.Value;
            //    component.StrokeThickness = (int)property.Value;
            //    component.SelectedStroke = ColorCollectionsClass.ColorCollections
            //            .FirstOrDefault(x => x.ColorType == property.Value as SolidColorBrush);
            //    component.SelectedFillColor = ColorCollectionsClass.ColorCollections
            //           .FirstOrDefault(x => x.ColorType == property.Value as SolidColorBrush);
            //    component.SelectedBorderColor = ColorCollectionsClass.ColorCollections
            //            .FirstOrDefault(x => x.ColorType == property.Value as SolidColorBrush);
            //}
            //switch (property.PropertyType)
            //{
            //    case PropertyType.Title:
            //        DrawingComponentProxy.Title = property.Value.ToString();
            //        break;
            //    case PropertyType.X:
            //        DrawingComponentProxy.X = (double)property.Value;
            //        break;
            //    case PropertyType.Y:
            //        DrawingComponentProxy.Y = (double)property.Value;
            //        break;
            //    case PropertyType.Width:
            //        DrawingComponentProxy.Width = (double)property.Value;
            //        break;
            //    case PropertyType.Height:
            //        DrawingComponentProxy.Height = (double)property.Value;
            //        break;
            //    case PropertyType.StrokeThickness:
            //        DrawingComponentProxy.StrokeThickness = (int)property.Value;
            //        break;
            //    case PropertyType.Stroke:
            //        DrawingComponentProxy.SelectedStroke = ColorCollectionsClass.ColorCollections
            //            .FirstOrDefault(x => x.ColorType == property.Value as SolidColorBrush);
            //        break;
            //    case PropertyType.FillColor:
            //        DrawingComponentProxy.SelectedFillColor = ColorCollectionsClass.ColorCollections
            //            .FirstOrDefault(x => x.ColorType == property.Value as SolidColorBrush);
            //        break;
            //    case PropertyType.BorderColor:
            //        DrawingComponentProxy.SelectedBorderColor = ColorCollectionsClass.ColorCollections
            //            .FirstOrDefault(x => x.ColorType == property.Value as SolidColorBrush);
            //        break;

            //    default:
            //        break;
        }
        
        void UpdateCanvasElementEvent(ComponentModel property)
        {
            //foreach (var component in DrawingComponentProxys)
            //{
            //    component.Title = property.Value.ToString();
            //    component.X = (double)property.Value;
            //    component.Y = (double)property.Value;
            //    component.Width = (double)property.Value;
            //    component.Height = (double)property.Value;
            //    component.StrokeThickness = (int)property.Value;
            //    component.SelectedStroke = ColorCollectionsClass.ColorCollections
            //            .FirstOrDefault(x => x.ColorType == property.Value as SolidColorBrush);
            //    component.SelectedFillColor = ColorCollectionsClass.ColorCollections
            //           .FirstOrDefault(x => x.ColorType == property.Value as SolidColorBrush);
            //    component.SelectedBorderColor = ColorCollectionsClass.ColorCollections
            //            .FirstOrDefault(x => x.ColorType == property.Value as SolidColorBrush);
            //}
        //switch (property.PropertyType)
        //{
        //    case PropertyType.Title:
        //        DrawingComponentProxy.Title = (string)property.Value;
        //        break;
        //    case PropertyType.X:
        //        DrawingComponentProxy.X = (double)property.Value;
        //        break;
        //    case PropertyType.Y:
        //        DrawingComponentProxy.Y = (double)property.Value;
        //        break;
        //    case PropertyType.Width:
        //        DrawingComponentProxy.Width = (double)property.Value;
        //        break;
        //    case PropertyType.Height:
        //        DrawingComponentProxy.Height = (double)property.Value;
        //        break;
        //    case PropertyType.StrokeThickness:
        //        DrawingComponentProxy.StrokeThickness = (int)property.Value;
        //        break;
        //    case PropertyType.Stroke:
        //        DrawingComponentProxy.SelectedStroke = ColorCollectionsClass.ColorCollections
        //            .FirstOrDefault(x => x.ColorType == property.Value as SolidColorBrush);
        //        break;
        //    case PropertyType.FillColor:
        //        DrawingComponentProxy.SelectedFillColor = ColorCollectionsClass.ColorCollections
        //            .FirstOrDefault(x => x.ColorType == property.Value as SolidColorBrush);
        //        break;
        //    case PropertyType.BorderColor:
        //        DrawingComponentProxy.SelectedBorderColor = ColorCollectionsClass.ColorCollections
        //            .FirstOrDefault(x => x.ColorType == property.Value as SolidColorBrush);
        //        break;

        //    default:
        //        break;
    }
        

            
        
        void OpenProject()
        {

        }
    }
}

    

