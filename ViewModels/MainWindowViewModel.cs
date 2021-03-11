﻿using MainViews.UserControls;
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

namespace View.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _currentWindowTitle = "Project Window";
        private UserControl s_currentWindow;//= new ProjectWindow();
        private CustomCanvasViewModel customCanvas = new CustomCanvasViewModel();
        private FrameworkElement menuPane;
        private UserControl propertyWindow ;
        private UserControl projectWindow ;
        private UserControl shareUserControl;

        public MainWindowViewModel(AccountProxyModel accountProxy)
        {

            
            propertyWindow = new PropertyWindow();
            projectWindow = new ProjectWindow();
            shareUserControl = new ShareUserControl();
            UpdateWindowCommand = new DelegateCommand<string>(UpdateWindow);
            OpenMenuPaneCommand = new DelegateCommand(OpenMenuPane);
            //EventAggregator = new EventAggregator();
            //EventAggregator.GetEvent<CurrentAccountModelEvent>().Publish(accountProxy);
            MenuPane = new MenuPane(accountProxy);
            EventAggregator = GenericServiceLocator.ShellContainer.Resolve<IEventAggregator>();
            EventAggregator.GetEvent<CanvasComponentEvent>().Subscribe(DrawingComponents);
            EventAggregator.GetEvent<ClearDrawingCanvasEvent>().Subscribe(ClearCanvasElements);
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
        public IEventAggregator EventAggregator { get; set; }

        
        private void OpenMenuPane()
        {
            menuPane.Visibility = Visibility.Visible;
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
            CustomCanvas.Children.Clear();
        }
        public IList<DrawingComponentProxyModel> DrawingComponentProxies { get; set; }
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
            DrawingComponentService.CreateDrawings(CurrentProjectModel, DrawingComponentProxies);
            MessageBox.Show("Project Saved Successfully!", "Success Message", MessageBoxButton.OK);
        }

    }
}
