using MainViews.UserControls;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using View.UserControls;
using View.ViewModels.Common;
using View.ViewModels.Common.Event_Container;
using Prism.Ioc;
using View.ViewModels.ProxyModel;
using View.ViewModels.Common.CustomColor;
using View.ViewModels.DatabaseServices;
using View.Helper.Common.Event_Container;
using System.Collections.ObjectModel;
using View.Windows;

namespace View.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _currentWindowTitle = "Property Window";
        private UserControl s_currentWindow = new PropertyWindow();
        private CustomCanvasViewModel customCanvas = new CustomCanvasViewModel();
        private FrameworkElement menuPane;
        private UserControl propertyWindow ;
        private UserControl projectWindow ;
        private UserControl shareUserControl;
        private bool isOpen;

        public MainWindowViewModel(AccountProxyModel accountProxy)
        {

            
            propertyWindow = new PropertyWindow();
            projectWindow = new ProjectWindow();
            shareUserControl = new ShareUserControl();
            UpdateWindowCommand = new DelegateCommand<string>(UpdateWindow);
            OpenMenuPaneCommand = new DelegateCommand(OpenMenuPane);
            OpenExportPopUpCommand = new DelegateCommand(OpenExportPopUp);
            ExportXmlCommand = new DelegateCommand<string>(ExportDrawings);
            ExportJsonCommand = new DelegateCommand<string>(ExportDrawings);
            ShareProjectCommand = new DelegateCommand(OpenShareUserProjectWindow);
            //EventAggregator = new EventAggregator();
            User = accountProxy;
            MenuPane = new MenuPane(User);
            
            EventAggregator = GenericServiceLocator.ShellContainer.Resolve<IEventAggregator>();
            EventAggregator.GetEvent<CanvasComponentEvent>().Subscribe(DrawingComponents);
            EventAggregator.GetEvent<ClearDrawingCanvasEvent>().Subscribe(ClearCanvasElements);
            EventAggregator.GetEvent<CurrentActiveProjectEvent>().Subscribe(SetCurrentProjectModel);
            EventAggregator.GetEvent<CloseWindowEvent>().Publish();
            GetAllUserProjects();
            GetAllShareUsers();
            //EventAggregator.GetEvent<CurrentAccountModelEvent>().Subscribe(CurrentUser);
        }
        public string CurrentWindowTitle { get { return _currentWindowTitle; } set { _currentWindowTitle = value; RaisePropertyChanged(); } }
        public UserControl CurrentWindow { get { return s_currentWindow; } set { s_currentWindow = value; RaisePropertyChanged(); } }
        public CustomCanvasViewModel CustomCanvas { get => customCanvas; set => customCanvas = value; }
        public FrameworkElement MenuPane { get => menuPane; set => menuPane = value; }
        public UserControl PropertyWindow { get => propertyWindow; set { propertyWindow = value; RaisePropertyChanged(); } }
        public UserControl ProjectWindow { get => projectWindow; set { projectWindow = value; RaisePropertyChanged(); } }
        public UserControl ShareUserControl { get => shareUserControl; set { shareUserControl = value; RaisePropertyChanged(); } }
        public DelegateCommand<string> UpdateWindowCommand { get; set; }
        public DelegateCommand OpenMenuPaneCommand { get; set; }
        public DelegateCommand ShareProjectCommand { get; set; }
        public DelegateCommand OpenExportPopUpCommand { get; set; }
        public DelegateCommand<string> ExportXmlCommand { get; set; }
        public DelegateCommand<string> ExportJsonCommand { get; set; }
        public IEventAggregator EventAggregator { get; set; }
        public AccountProxyModel  User { get; set; }
        public bool IsOpen { get=>isOpen; set { isOpen = value; RaisePropertyChanged(); } }

        void OpenExportPopUp()
        {
            IsOpen = true;
        }
        void ExportDrawings(string parameter)
        {
            EventAggregator.GetEvent<ExportDrawingsEvent>().Publish(parameter);
        }
        void OpenShareUserProjectWindow()
        {
            if (CurrentProjectModel!=null)
            {
                ShareProject shareProject = new ShareProject();
                shareProject.DataContext = new ShareUserViewModel(CurrentProjectModel);
                shareProject.ShowDialog();
            }
            
        }
        private void OpenMenuPane()
        {
            menuPane.Visibility = Visibility.Visible;
            
        }
        //public ProjectProxyModel CurrentProject { get; set; }
        void SetCurrentProjectModel(ProjectProxyModel projectProxyModel)
        {
            CurrentProjectModel = projectProxyModel;
        }
        void UpdateWindow(string property)
        {
            if (property == "Property Window")
            {
                CurrentWindow = PropertyWindow;
                CurrentWindow.Visibility = Visibility.Visible;
            }
            else if (property == "Project Window")
            {
                CurrentWindow = ProjectWindow;
                CurrentWindow.Visibility = Visibility.Visible;
            }
            else
            {
                CurrentWindow = ShareUserControl;
                CurrentWindow.Visibility = Visibility.Visible;
            }
            CurrentWindowTitle = property;
        }
        private void ClearCanvasElements()
        {
            if (CurrentProjectModel!=null)
            {
                CustomCanvas.Children.Clear();
            }
            
        }
        public List<DrawingComponentProxyModel> DrawingComponentProxies { get; set; }
        public ProjectProxyModel CurrentProjectModel { get; set; }
        public void DrawingComponents(ProjectProxyModel currentProject)
        {
            //FrameworkElement component = null;
           
            if (CustomCanvas != null&& currentProject!=null)
            {
                CurrentProjectModel = currentProject;
                DrawingComponentProxies = new List<DrawingComponentProxyModel>();
                foreach (var item in CustomCanvas.Children)
                {
                    var property = (IPropertyWindow)item;
                    var component = ComponentProxyModel(property);
                    DrawingComponentProxies.Add(component);
                }
                SaveProject();
            }
            else
            {
                MessageBox.Show("You Havn't Created a Project Yet!", "Error Message", MessageBoxButton.OK);
            }
            
        }
        private DrawingComponentProxyModel ComponentProxyModel(IPropertyWindow property)
        {
            DrawingComponentProxyModel drawingComponent = new DrawingComponentProxyModel();
            drawingComponent.Title = property.Title;
            drawingComponent.X = property.X;
            drawingComponent.Y = property.Y;
            drawingComponent.Width = property.Width;
            drawingComponent.Height = property.Height;
            drawingComponent.LineThickness = property.LineThickness;
            drawingComponent.SelectedBorderColor = ColorCollectionsClass.ColorCollections
                        .FirstOrDefault(x => x.ColorType == property.SelectedBorderColor);
            drawingComponent.SelectedFillColor = ColorCollectionsClass.ColorCollections
                        .FirstOrDefault(x => x.ColorType == property.SelectedFillColor);
            drawingComponent.SelectedStroke = ColorCollectionsClass.ColorCollections
                        .FirstOrDefault(x => x.ColorType == property.SelectedStroke);
            return drawingComponent;

        }
        void SaveProject()
        {
            if (CurrentProjectModel!=null)
            {
                DrawingComponentService.CreateDrawings(CurrentProjectModel, DrawingComponentProxies);
                MessageBox.Show("Project Saved Successfully!", "Success Message", MessageBoxButton.OK);
                return;
            }
            EventAggregator.GetEvent<OpenWindowEvent>().Publish();
        }
        void GetAllUserProjects()
        {
            ObservableCollection<string> getAllUserProjects = new ObservableCollection<string>();
            getAllUserProjects = ProjectService.GetAllProjects(User);
            EventAggregator.GetEvent<GetAllUserProjectsEvent>().Publish(getAllUserProjects);
        }
        void GetAllShareUsers()
        {
            ObservableCollection<Users> allShareUser = new ObservableCollection<Users>();
            var getAllShareUser = ShareUserService.GetShareUsers(User);
            if (getAllShareUser!=null)
            {
                foreach (var item in getAllShareUser)
                {
                    Users users = new Users();
                    users.UserName = item.Users.FullName;
                    allShareUser.Add(users);

                }
                EventAggregator.GetEvent<GetAllShareUserEvent>().Publish(allShareUser);
            }
            
        }
    }
}

