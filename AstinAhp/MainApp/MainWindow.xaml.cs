using System.Windows;

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

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            var form = new Views.PemohonDataView();
            form.ShowDialog();
        }

        private void keluar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void report_Click(object sender, RoutedEventArgs e)
        {
            var form = new Views.LaporanView();
            form.ShowDialog();
        }
    }
}
