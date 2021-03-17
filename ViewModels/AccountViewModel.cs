using MainViews.Windows;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using View.ViewModels.Common.Event_Container;
using View.ViewModels.DatabaseServices;
using View.ViewModels.ProxyModel;
using View.Wndows;
using View.ViewModels.Common;
using View.Helper.Common.Event_Container;

namespace View.ViewModels
{
    public class AccountViewModel : BindableBase
    {
        private string email;
        private string fullName;
        private string password;
        private string comfirmPassword;
        private bool isPasswordComfirm = false;
        private bool isEmailValidated;
        //private AccountProxyModel user=new AccountProxyModel();
        //private Window window;

        public AccountViewModel()
        {

            CreateAccountCommand = new DelegateCommand(CreateAccount);
            //EventAggregator.GetEvent<CloseWindowEvent>().Publish();
            EventAggregator = GenericServiceLocator.ShellContainer.Resolve<IEventAggregator>();
        }
        public AccountProxyModel User { get; set; }
        public string Email { get => email; set { email = value; RaisePropertyChanged(); } }
        public string FullName { get => fullName; set { fullName = value; RaisePropertyChanged(); } }
        public string Password { get => password; set { password = value; RaisePropertyChanged(); } }
        public string ComfirmPassword { get => comfirmPassword; set { comfirmPassword = value; RaisePropertyChanged(); } }
        public IEventAggregator EventAggregator { get; set; }
        


        public DelegateCommand CreateAccountCommand { get; set; }

       
        public void CreateAccount()
        {
            isPasswordComfirm = PasswordValidation();
            if (isPasswordComfirm)
            {
                User = new AccountProxyModel();
                User.Email = email;
                User.FullName = fullName;
                User.Password = password;
                isEmailValidated = IsEmailValidated();

                if (isEmailValidated)
                {
                    MessageBox.Show("Email Already Exist",
                    "Error Message", MessageBoxButton.OK, MessageBoxImage.Error);

                    return;
                }
                
                EventAggregator = new EventAggregator();
                EventAggregator.GetEvent<CurrentAccountModelEvent>().Publish(User);
                AccountService.CreateAccount(User);

                MessageBox.Show("Account Created Successfully!",
                    "Success Message", MessageBoxButton.OK);
                EventAggregator.GetEvent<CloseAccountWindowEvent>().Publish();
                return;
            }

            
        }

        public bool PasswordValidation()
        {
            if (password != null && comfirmPassword != null)
            {
                if (password == comfirmPassword)
                {
                    return true;
                }
            }

            return false;
        }
        public bool IsEmailValidated()
        {
            if (email != null)
            {
                return AccountService.EmailValidations(email);
            }
            return false;
        }
        public void OpenLoginWindowProgram()
        {
            
            var loginPage = new LoginPage();

            loginPage.ShowDialog();
        }
    }
}