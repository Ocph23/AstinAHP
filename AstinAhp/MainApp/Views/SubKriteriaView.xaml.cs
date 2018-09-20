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
    /// Interaction logic for SubKriteriaView.xaml
    /// </summary>
    public partial class SubKriteriaView : Window
    {
        public SubKriteriaView()
        {
            InitializeComponent();
            DataContext = new SubKriteriaViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }


    public class SubKriteriaViewModel:subkriteria
    {
        public CommandHandler NewCommand { get; }
        public CommandHandler SaveCommand { get; }
        public KriteriaCollection Source { get; }

        public SubKriteriaViewModel()
        {
            Source = Helper.GetMainViewModel().Kriterias;
            Source.OnSelected += Source_OnSelected;
            foreach(var item in Source)
            {
                item.SubKriterias.OnSelected += SubKriterias_OnSelected;
            }
            NewCommand = new CommandHandler { CanExecuteAction = NewCommandValidation, ExecuteAction = NewCommandAction };
            SaveCommand = new CommandHandler { CanExecuteAction = SaveCommandValidate, ExecuteAction = SaveCommandAction };
        }

        private bool NewCommandValidation(object obj)
        {
            if (Source.SelectedItem != null && Source.SelectedItem.SubKriterias.SelectedItem != null)
                return true;
            else
                return false;
        }

        private void Source_OnSelected(object param)
        {
            var item = param as kriteria;
            
           if(item!=null)
            {
                item.SubKriterias.SelectedItem = null;
            }
            CleanForm();

        }

        private void SubKriterias_OnSelected(object param)
        {
            var item = param as subkriteria;
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
            var Model = new subkriteria { Id = Id, KriteriaId=KriteriaId, Kode = Kode, Nama = Nama };
            try
            {
                var IsSaved = false;
                if (Model.Id > 0)
                {
                    if (Source.SelectedItem.SubKriterias.Update(Model))
                        IsSaved = true;
                }
                else
                {
                    Source.SelectedItem.SubKriterias.Add(Model);
                    IsSaved = true;

                }


                if (IsSaved)
                {
                    Helper.Info("Data Berhasil Disimpan");
                    NewCommand.Execute(null);
                }
                else
                    Helper.Error("Gagal Disimpan");

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
            if(Source.SelectedItem != null && Source.SelectedItem.SubKriterias.SelectedItem!=null)
            Source.SelectedItem.SubKriterias.SelectedItem = null;
            CleanForm();
        }
    }
}
