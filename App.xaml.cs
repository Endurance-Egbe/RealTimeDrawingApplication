using Prism.Ioc;
using Prism.Unity;
using System;
using System.ComponentModel;
using System.Windows;
using View.Infrastructure;
using View.ViewModels.DatabaseServices;
using View.ViewModels.Common;

namespace View
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                DatabaseInitializer.Initialize();
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "The application database could not be initialized.\r\n\r\n" +
                    exception.Message,
                    "Database initialization failed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                Shutdown(1);
                return;
            }

            base.OnStartup(e);
        }

        protected override Window CreateShell()
        {
            GenericServiceLocator.ShellContainer = Container;
            return Container.Resolve<LoginPage>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var accountService = new AccountService(new SqliteAccountRepository());
            containerRegistry.RegisterInstance(accountService);
            AccountServiceProvider.SetCurrent(accountService);
            var drawingComponentService =
                new DrawingComponentService(new SqliteDrawingComponentRepository());
            containerRegistry.RegisterInstance(drawingComponentService);
            DrawingComponentServiceProvider.SetCurrent(drawingComponentService);
        }
    }
}
