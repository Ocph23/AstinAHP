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

namespace MainApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new ViewModels.MainWindowViewModel();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var form = new Views.PemohonView();
            form.ShowDialog();
        }

        private void kriteriaMenu_Click(object sender, RoutedEventArgs e)
        {
            var form = new Views.KriteriaView();
            form.ShowDialog();
        }

        private void submn_Click(object sender, RoutedEventArgs e)
        {
            var form = new Views.SubKriteriaView();
            form.ShowDialog();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            var form = new Views.BobotCriteriaView();
            form.ShowDialog();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            var form = new Views.BobotSubCriteriaView();
            form.ShowDialog();
        }
    }
}
