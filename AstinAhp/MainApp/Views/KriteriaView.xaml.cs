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
    /// Interaction logic for KriteriaView.xaml
    /// </summary>
    public partial class KriteriaView : Window
    {
        private KriteriaViewModel vm;

        public KriteriaView()
        {
            InitializeComponent();
            vm = new KriteriaViewModel();
            DataContext = vm;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    public class KriteriaViewModel:kriteria
    {
       
        public CollectionView SourceView { get; set; }
        public CommandHandler NewCommand { get; }
        public CommandHandler SaveCommand { get; }
        public KriteriaCollection Source { get; }
        public CommandHandler DeleteCommand { get; }
        public KriteriaViewModel()
        {
            Source = Helper.GetMainViewModel().Kriterias;
            Source.OnSelected += Source_OnSelected;
            SourceView = (CollectionView)CollectionViewSource.GetDefaultView(Source);
            //Command
            NewCommand = new CommandHandler { CanExecuteAction = x => true, ExecuteAction = NewCommandAction };
            SaveCommand = new CommandHandler { CanExecuteAction = SaveCommandValidate, ExecuteAction = SaveCommandAction };
            DeleteCommand = new CommandHandler { CanExecuteAction = x => Source.SelectedItem != null, ExecuteAction = DeleteCommandAction };
            Source.SelectedItem = null;
        }
        private void DeleteCommandAction(object obj)
        {
            try
            {
                if (Source.Remove(Source.SelectedItem))
                {
                    Helper.Info("Data Berhasil Dihapus");
                    SourceView.Refresh();
                }
                else
                {
                    throw new SystemException("Data Tidak Berhasil Dihapus");
                }
            }
            catch (Exception ex)
            {

                Helper.Error(ex.Message);
            }
        }

        private void Source_OnSelected(object param)
        {
            var item = param as kriteria;
            if (item != null)
            {
                Id = item.Id;
                Nama = item.Nama;
                Kode = item.Kode;
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
            Kode = string.Empty;

        }

        private void SaveCommandAction(object obj)
        {
            var Model = new kriteria { Id=Id, Kode=Kode, Nama = Nama };
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

            if (string.IsNullOrEmpty(Nama) || string.IsNullOrEmpty(Kode))
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
