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
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Events;
using View.Helper.Common.Event_Container;
using View.ViewModels.Common;

namespace View.Wndows
{
    /// <summary>
    /// Interaction logic for CreateAccount.xaml
    /// </summary>
    public partial class CreateAccount : Window
    {
        public CreateAccount()
        {
            InitializeComponent();
            DataContext = new AccountViewModel();
            EventAggregator = GenericServiceLocator.ShellContainer.Resolve<IEventAggregator>();
            EventAggregator.GetEvent<CloseAccountWindowEvent>().Subscribe(CloseWindow);
        }
        public IEventAggregator EventAggregator { get; set; }
        void CloseWindow()
        {
            this.Close();
        }
    }
}
