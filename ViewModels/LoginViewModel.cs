using MainViews.Windows;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using View.ViewModels.DatabaseServices;
using View.ViewModels.ProxyModel;
using View.ViewModels.Common;
using View.Helper.Common.Event_Container;
using Prism.Ioc;

namespace View.ViewModels
{
    public class LoginViewModel:BindableBase
    {
        private string email;       
        private string password;
         
        //private AccountProxyModel user=new AccountProxyModel();
        //private Window window;

        public LoginViewModel(IEventAggregator eventAggregator)
        {
            //EventAggregator = eventAggregator;
            LoginAccountCommand = new DelegateCommand(LoginAccount);
            EventAggregator = eventAggregator;

        }
        public AccountProxyModel User { get; set; }
        public string Email { get => email; set { email = value; RaisePropertyChanged(); } }
        public string Password { get => password; set { password = value; RaisePropertyChanged(); } }
        public DelegateCommand LoginAccountCommand { get; set; }
        public IEventAggregator EventAggregator { get; set; }
        public List<string> ProjectNames { get; set; }


        

        public void ListOfProjects(string ProjectName)
        {
            ProjectNames.Add(ProjectName);
        }
        public void LoginAccount()
        {
                bool isEmailPasswordValidated;
                User = AccountService.Authenticate(email, password);
                isEmailPasswordValidated = User != null;

                if (isEmailPasswordValidated)
                {
                
                //EventAggregator.GetEvent<CurrentAccountModelEvent>().Publish(User);
                MessageBox.Show("Account Login Successfully!",
                    "Success Message", MessageBoxButton.OK);
                    OpenMainWindowProgram();

                    return;
                }
            
            MessageBox.Show("Account Unable to Verify\r\nSignUp to Create an Account",
                "Error Message", MessageBoxButton.OK,MessageBoxImage.Error);

            
        }
        public bool IsEmailPassworValidated(AccountProxyModel model)
        {
            return model != null;
        }
        public void OpenMainWindowProgram()
        {

            
            Window mainWindowprogram = new MainWindowProgram();
            mainWindowprogram.DataContext = new MainWindowViewModel(User);
            mainWindowprogram.ShowDialog();
            
        }
    }
    
}
