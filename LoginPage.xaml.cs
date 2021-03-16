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
using Prism.Events;
using Prism.Mvvm;
using View.Helper.Common.Event_Container;

namespace View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginPage : Window
    {
        public LoginPage()
        {
            InitializeComponent();
            DataContext = GenericServiceLocator.ShellContainer.Resolve<LoginViewModel>();
            EventAggregator = GenericServiceLocator.ShellContainer.Resolve<IEventAggregator>();
            EventAggregator.GetEvent<CloseWindowEvent>().Subscribe(CloseWindow);
        }
        public IEventAggregator EventAggregator  { get; set; }
        void CloseWindow()
        {
            this.Close();
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
