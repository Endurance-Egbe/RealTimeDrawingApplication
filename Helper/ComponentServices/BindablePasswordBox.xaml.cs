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

namespace View.Helper.ComponentServices
{
    /// <summary>
    /// Interaction logic for CustomPasswordBox.xaml
    /// </summary>
    public partial class BindablePasswordBox : UserControl
    {
        bool isPasswordChanging;
        public BindablePasswordBox()
        {
            InitializeComponent();
        }


        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PassworwBox.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(BindablePasswordBox), new PropertyMetadata(string.Empty, PasswordPropertyChanged));

        private static void PasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BindablePasswordBox passwordBox)
            {
                passwordBox.UpdatePassword();
            }
        }
        private void UpdatePassword()
        {
            if (!isPasswordChanging)
            {
                passworBox.Password = Password;
            }
           
        }
        private void passworBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            isPasswordChanging = true;
            Password = passworBox.Password;
            isPasswordChanging = false;
        }
    }
}
