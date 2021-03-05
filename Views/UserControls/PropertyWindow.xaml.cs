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
using Prism.Ioc;

namespace View.UserControls
{
    /// <summary>
    /// Interaction logic for PropertyWindow.xaml
    /// </summary>
    public partial class PropertyWindow : UserControl
    {
        public PropertyWindow()
        {
            InitializeComponent();
            DataContext = GenericServiceLocator.ShellContainer.Resolve<PropertyViewModel>();
        }
    }
}
