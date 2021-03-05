using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using View.ViewModels;
using View.ViewModels.ProxyModel;
using View.ViewModels.ShapeServices;

namespace MainViews.Windows
{
    /// <summary>
    /// Interaction logic for MainWindowProgram.xaml
    /// </summary>
    public partial class MainWindowProgram : Window
    {
        public MainWindowProgram(AccountProxyModel accountProxyModel)
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(accountProxyModel);
            
        }

        private void BtnPropertyWindow_Click(object sender, RoutedEventArgs e)
        {
            PropMenuPopup.IsOpen = true;
        }

        private void Menubtn_Click(object sender, RoutedEventArgs e)
        {
            MenuPaneUc.Visibility = Visibility.Visible;
        }

        private void Path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //ShapeServices shapeComponent = new ShapeServices();
            FrameworkElement component = null;
            if (sender is Rectangle rectangle)
            {
                // component = ShapeService.GetDefaultControl(ControlEnum.Rectangle);
                component = new ShapeServices().GetDefaultControl(ComponentEnum.Rectangle);
            }
            else if (sender is Ellipse ellipse)
            {
                component = new ShapeServices().GetDefaultControl(ComponentEnum.Ecllipse);
            }
            else if (sender is Path path )
            {
                component = new ShapeServices().GetDefaultControl(ComponentEnum.Triangle);
            }
            else if (sender is Line line )
            {
                component = new ShapeServices().GetDefaultControl(ComponentEnum.Line);
            }
            else if (sender is TextBlock textBlock)
            {
                component = new ShapeServices().GetDefaultControl(ComponentEnum.TextBox);
            }
            if (component != null)
            {
                //e.MouseDevice.OverrideCursor = Cursors.Hand;
                DataObject dataObject = new DataObject("toolboxitem", component);
                DragDrop.DoDragDrop(component as FrameworkElement, dataObject, DragDropEffects.Copy);
            }
        }

        

        private void PropWindRb_Checked(object sender, RoutedEventArgs e)
        {
            //WindowContentControl.CurrentRadioButton = sender as RadioButton;
            //if (PropWindRb.IsChecked == true)
            //{
            //    WindowContentControl.CurrentWindow.Visibility = PropertyWindowControl.Visibility;
            //}
            //else if (ShareUserRb.IsChecked == true)
            //{
            //    WindowContentControl.CurrentWindow.Visibility = ShareUserControl.Visibility;
            //}
            //else if (ProjectWinRb.IsChecked == true)
            //{
            //    WindowContentControl.CurrentWindow.Visibility = ProjectWindowControl.Visibility;
            //}


        }
    }
}
