using Prism.Ioc;
using Prism.Unity;
using System.ComponentModel;
using System.Windows;
using View.ViewModels.Common;

namespace View
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        public App()
        {

        }
        
        protected override Window CreateShell()
        {
            GenericServiceLocator.ShellContainer = Container;
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}
