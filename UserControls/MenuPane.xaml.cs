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
using View.Windows;
using View.Wndows;

namespace View.UserControls
{
    /// <summary>
    /// Interaction logic for MenuPane.xaml
    /// </summary>
    public partial class MenuPane : UserControl
    {
        public MenuPane()
        {
            InitializeComponent();
        }

        
        private void BtnCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void TxtCreateAccountMenu_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CreateAccount createAccount = new CreateAccount();
            createAccount.ShowDialog();
        }

        private void TxtCreateProjectMenu_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CreateProject createProject = new CreateProject();
            createProject.ShowDialog();
        }

        private void TxtShareProjectMenu_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ShareProject shareProject = new ShareProject();
            shareProject.ShowDialog();
        }
    }
}
