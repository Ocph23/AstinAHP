using MainApp.Domains;
using MainApp.Models;
using MainApp.ViewModels;
using Ocph.DAL;
using System;
using System.Collections;
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
    /// Interaction logic for PemohonDataView.xaml
    /// </summary>
    public partial class PemohonDataView : Window
    {
        public PemohonDataView()
        {
            InitializeComponent();
            DataContext = this;
            Main = Helper.GetMainViewModel();
            Pemohons = Main.Pemohons;
            Kriterias = Main.Kriterias;
            SourceView = new List<ArrayList>();
            for (int x = DateTime.Now.Year; x > DateTime.Now.Year - 5; x--)
            {
                tahun.Items.Add(x);
            }
            Loaded += PemohonDataView_Loaded;
        }

        public MainWindowViewModel Main { get; }
        public PemohonCollection Pemohons { get; }
        public KriteriaCollection Kriterias { get; }
        public List<ArrayList> SourceView { get; private set; }
        public int SelectedTahun { get; set; }

        private void PemohonDataView_Loaded(object sender, RoutedEventArgs e)
        {
           
            SourceView.Clear();
       


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                for (var i = 0; i < Pemohons.Count; i++)
                {
                    var p = Pemohons.ToList()[i];
                    var pValue = SourceView[i];
                    int j = 1;
                    foreach (var k in Kriterias)
                    {
                        var data = p.Datas.Where(O => O.PemohonId == p.Id && O.KriteriaId == k.Id).FirstOrDefault();
                        if (data != null)
                            data.Value = pValue[j].ToString();
                        else
                            p.Datas.Add(new Models.DTO.datapemohon { Tahun = SelectedTahun, PemohonId = p.Id, KriteriaId = k.Id, Value = pValue[j].ToString() });
                        j++;

                    }
              
                    p.Datas.Update(null);
                }

            }
            catch (Exception ex)
            {

                Helper.Error(ex.Message);
            }



        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

       

        private void tahun_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            main.Columns.Clear();
            SelectedTahun =(Int32)(sender as ComboBox).SelectedItem;
            SourceView.Clear();
            var col = new DataGridTextColumn();
            col.Header = "Nama";
            col.Binding = new Binding(string.Format("[{0}]", 0));
            col.Width = 200;
            main.Columns.Add(col);
            int i = 1;
            foreach (var item in Kriterias)
            {
                var col1 = new DataGridComboBoxColumn();
                col1.Width = 120;
                col1.Header = item.Nama;
                col1.ItemsSource = item.SubKriterias;
                col1.DisplayMemberPath = "Nama";
                col1.SelectedValuePath = "Nama";
                col1.SelectedValueBinding = new Binding(string.Format("[{0}]", i));
                main.Columns.Add(col1);
                i++;
            }


            foreach (var item in Pemohons)
            {
                var list = new ArrayList();
                list.Add(item.Nama);
                foreach (var sub in Kriterias)
                {
                    var data = item.Datas.Where(O => O.PemohonId == item.Id && O.KriteriaId == sub.Id && O.Tahun== SelectedTahun).FirstOrDefault();
                    if (data != null)
                        list.Add(data.Value);
                    else
                        list.Add("");
                }
                SourceView.Add(list);
            }
            main.ItemsSource = SourceView;
        }
    }
    public class PemohonDataViewModel:BaseNotify
    {
       
    }
}
