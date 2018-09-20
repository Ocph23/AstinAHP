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
    /// Interaction logic for RegisterView.xaml
    /// </summary>
    public partial class RegisterView : Window
    {
        private RegisterViewModel vm;

        public RegisterView()
        {
            InitializeComponent();
            vm = new RegisterViewModel() { WindowClose = Close };
            DataContext = vm;
        }

        private void password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            vm.Password = (sender as PasswordBox).Password;
        }

        private void confirm_PasswordChanged(object sender, RoutedEventArgs e)
        {
            vm.ConfirmPassword = (sender as PasswordBox).Password;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void loginInfo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var form = new LoginView();
            form.Show();
            this.Close();
        }
    }


    public class RegisterViewModel:BaseNotify
    {
        public RegisterViewModel()
        {
            RegisterCommand = new CommandHandler { CanExecuteAction = RegisterCommandValidation, ExecuteAction = RegisterCommandAction };
        }

        private void RegisterCommandAction(object obj)
        {
            try
            {
                if (Password != ConfirmPassword)
                    throw new SystemException("Password Tidak Sama");

                using (var db = new OcphDbContext())
                {
                    if(db.Users.Insert(new Models.DTO.user { Password=Password, UserName=User }))
                    {
                        Helper.Info("Sukses Tambah User");
                    }else
                    {
                        throw new SystemException("Gagal Tambah User");
                    }
                }

            }
            catch (Exception ex)
            {
                Helper.Error(ex.Message);
            }
        }

       
        private bool RegisterCommandValidation(object obj)
        {
            if (string.IsNullOrEmpty(User) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(ConfirmPassword))
                return false;
            return true;
        }

        private string user;

        public string User
        {
            get { return user; }
            set { SetProperty(ref user, value); }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value); }
        }

        private string confirmpassword;

        public string ConfirmPassword
        {
            get { return confirmpassword; }
            set { SetProperty(ref confirmpassword, value); }
        }

        public CommandHandler RegisterCommand { get; }
        public Action WindowClose { get; internal set; }
    }
}
