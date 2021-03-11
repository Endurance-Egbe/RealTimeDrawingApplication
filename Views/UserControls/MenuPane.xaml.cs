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
using View.ViewModels.ProxyModel;
using View.Windows;
using View.Wndows;

namespace View.UserControls
{
    /// <summary>
    /// Interaction logic for MenuPane.xaml
    /// </summary>
    public partial class MenuPane : UserControl
    {
        
        public MenuPane(AccountProxyModel accountProxy)
        {
            InitializeComponent();

            DataContext = new MenuPaneViewModel(accountProxy);
        }

        
        private void BtnCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

    }
}
