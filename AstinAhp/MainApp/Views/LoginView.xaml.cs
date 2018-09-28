using Ocph.DAL;
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

namespace MainApp.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        private LoginViewModel vm;

        public LoginView()
        {
            InitializeComponent();
            vm = new LoginViewModel() {WindowClose=this.Close };
            DataContext = vm;
            this.Loaded += LoginView_Loaded;
        }

        private void LoginView_Loaded(object sender, RoutedEventArgs e)
        {
            if (vm.IsFirst())
            {
                var form = new RegisterView();
                form.Show();
                this.Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            vm.Password = (sender as PasswordBox).Password;
        }
    }


    public class LoginViewModel:BaseNotify
    {
        public LoginViewModel()
        {
            LoginCommand = new CommandHandler { CanExecuteAction = LoginCommandValidation, ExecuteAction = LoginCommandAction };
        }

        private void LoginCommandAction(object obj)
        {
            try
            {
             
                using (var db = new OcphDbContext())
                {
                    var result = db.Users.Where(O => O.UserName == User).FirstOrDefault();
                    if (result == null)
                        throw new SystemException("Anda Tidak Memiliki Akses");
                    else
                    {
                        if(User==result.UserName && Password==result.Password)
                        {
                            var form = new MainWindow();
                            form.Show();
                            this.WindowClose();
                        }
                        else
                        {
                            throw new SystemException("User Atau Password Anda Salah");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.Error(ex.Message);
            }
        }

        public bool IsFirst()
        {
            using (var db = new OcphDbContext())
            {
                var result = db.Users.Select();
                if (result.Count() <= 0)
                    return true;
                else
                    return false;
            }
        }

        private bool LoginCommandValidation(object obj)
        {
            if (string.IsNullOrEmpty(User) || string.IsNullOrEmpty(Password))
                return false;
            return true;
        }

        private string user;

        public string User
        {
            get { return user; }
            set { SetProperty(ref user ,value); }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set { SetProperty(ref password ,value); }
        }

        public CommandHandler LoginCommand { get; }
        public Action WindowClose { get; internal set; }
    }
}
