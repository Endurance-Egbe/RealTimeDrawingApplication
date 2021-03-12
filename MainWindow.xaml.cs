using MainViews.Windows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using View.ViewModels;
using View.ViewModels.Common;
using View.Wndows;
using Prism.Ioc;

namespace View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = GenericServiceLocator.ShellContainer.Resolve<LoginViewModel>();
            
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void txtSignUp_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
            CreateAccount accountWindow = new CreateAccount();
            accountWindow.ShowDialog();
           
        }
    }
}
