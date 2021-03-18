using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using View.ViewModels.Common;
using View.ViewModels.ProxyModel;
using Prism.Ioc;
using View.ViewModels.Common.Event_Container;
using Prism.Commands;
using View.ViewModels.DatabaseServices;
using View.Windows;
using View.Helper.Common.Event_Container;

namespace View.ViewModels
{
    public class ShareUserWindowViewModel : BindableBase
    {
        //private string userName;
        //private bool isChecked;

        public ShareUserWindowViewModel()//AccountProxyModel accountProxy )
        {

            EventAggregator = GenericServiceLocator.ShellContainer.Resolve<IEventAggregator>();
            EventAggregator.GetEvent<CurrentProjectEvent>().Subscribe(ProjectModel);
            EventAggregator.GetEvent<CurrentAccountModelEvent>().Subscribe(AccountProxy);
            EventAggregator.GetEvent<GetAllShareUserEvent>().Subscribe(LoadAllShareUser);
            AddUserCommand = new DelegateCommand(AddUser);
            ShareUser = new ObservableCollection<Users>();
        }
        public ObservableCollection<Users> ShareUser { get; set; }
        //public string UserName { get => userName; set { userName = value; RaisePropertyChanged(); } }
        public IEventAggregator EventAggregator { get; set; }
        public ProjectProxyModel ProjectProxyModel { get; set; }
        public DelegateCommand AddUserCommand { get; set; }
        public Users Users { get; set; }
        //public bool IsChecked { get => isChecked; set { isChecked = value; RaisePropertyChanged(); } }
        void ProjectModel(ProjectProxyModel projectProxy)
        {
            ProjectProxyModel = projectProxy;
        }
        void AddUser()
        {
            ShareProject shareuser = new ShareProject();
            shareuser.DataContext = new ShareUserViewModel(ProjectProxyModel);
            shareuser.ShowDialog();
        }
        void AccountProxy(AccountProxyModel accountProxy)
        {

            Users = new Users() { UserName = accountProxy.FullName, IsChecked = true };
            ShareUser.Add(Users);
            
        }
        void LoadAllShareUser(ObservableCollection<Users> users)
        {
            ShareUser = users;
        }


    }
    public class Users
    {
        public  string UserName { get; set; }
        public  bool IsChecked { get; set; }
        
    }
}
