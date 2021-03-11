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

namespace View.Windows
{
    /// <summary>
    /// Interaction logic for CreateProject.xaml
    /// </summary>
    public partial class CreateProject : Window
    {
        public CreateProject()//AccountProxyModel accountProxy)
        {
            InitializeComponent();
           // DataContext = new CreateProjectViewModel(accountProxy);
        }
    }
}
