using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MainApp
{
   public class Helper
    {
        public static ViewModels.MainWindowViewModel GetMainViewModel()
        {
            var mainWindow = App.Current.Windows[0] as MainWindow;
            return mainWindow.DataContext as ViewModels.MainWindowViewModel;
        }

        internal static void Info(string v)
        {
            MessageBox.Show(v, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        internal static void Error(string v)
        {
            MessageBox.Show(v, "Info", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
