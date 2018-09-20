using MainApp.Domains;
using MainApp.Models.DTO;
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
    /// Interaction logic for PemohonView.xaml
    /// </summary>
    public partial class PemohonView : Window
    {
        public PemohonView()
        {
            InitializeComponent();
            this.DataContext = new PemohonViewModel();
            cbgender.Items.Add("Pria");
            cbgender.Items.Add("Wanita");
          
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    public class PemohonViewModel : pemohon
    {
        public PemohonCollection Source { get; }
        public CollectionView SourceView { get; set; }
        public CommandHandler NewCommand { get; }
        public CommandHandler SaveCommand { get; }

        public PemohonViewModel()
        {
            Source = Helper.GetMainViewModel().Pemohons;
            Source.OnSelected += Source_OnSelected;
            SourceView = (CollectionView)CollectionViewSource.GetDefaultView(Source);
            //Command
            NewCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = NewCommandAction };
            SaveCommand = new CommandHandler { CanExecuteAction = SaveCommandValidate, ExecuteAction = SaveCommandAction };

        }

        private void Source_OnSelected(object param)
        {
            var item = param as pemohon;
            if (item != null)
            {
                Id = item.Id;
                Nama = item.Nama;
                JenisKelamin = item.JenisKelamin;
                Alamat = item.Alamat;
            }
            else
            {
                CleanForm();
            }
        }

        private void CleanForm()
        {
            Id = 0;
            Nama = string.Empty;
            Alamat = string.Empty;
            JenisKelamin = string.Empty;

        }

        private void SaveCommandAction(object obj)
        {
            var Model = new pemohon { Alamat = Alamat, Id = Id, JenisKelamin = JenisKelamin, Nama = Nama };
            try
            {
                var IsSaved = false;
                if (Model.Id > 0)
                {
                    if (Source.Update(Model))
                        IsSaved = true;
                }
                else
                {
                    Source.Add(Model);
                    IsSaved = true;

                }


                if (IsSaved)
                {
                    Helper.Info("Data Berhasil Disimpan");
                    NewCommand.Execute(null);
                }
                else
                    Helper.Error("Gagal Disimpan");

                SourceView.Refresh();
            }
            catch (Exception)
            {
                Helper.Error("Gagal Disimpan");
            }
        }


        private bool SaveCommandValidate(object obj)
        {

            if (string.IsNullOrEmpty(Nama) || string.IsNullOrEmpty(JenisKelamin) || string.IsNullOrEmpty(Alamat))
                return false;
            return true;

        }
        private void NewCommandAction(object obj)
        {
            Source.SelectedItem = null;
            CleanForm();
        }
    }
}
