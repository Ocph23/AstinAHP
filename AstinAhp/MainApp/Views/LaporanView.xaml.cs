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
using MainApp.Domains;
using Microsoft.Reporting.WinForms;

namespace MainApp.Views
{
    /// <summary>
    /// Interaction logic for LaporanView.xaml
    /// </summary>
    public partial class LaporanView : Window
    {
        public LaporanView()
        {
            InitializeComponent();
            MainPoject = new MainProject();
            MainPoject.Initialitation();
            MainPoject.Calculate(0);
            Loaded += LaporanView_Loaded;
        }

        public MainProject MainPoject { get; }

        private void LaporanView_Loaded(object sender, RoutedEventArgs e)
        {
            for (int x = DateTime.Now.Year; x > DateTime.Now.Year - 5; x--)
            {
                tahun.Items.Add(x);
            }

        }

        private List<ReportModel> GetRank(List<ReportModel> list)
        {
            var rank = 1;
           foreach(var item in list)
            {
                item.Keterangan = "Ranking " + rank;
                rank++;
            }

            return list;
        }

        private void tahun_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            rv.LocalReport.DataSources.Clear();
            int selectedTahun =(Int32) (sender as ComboBox).SelectedItem;
            foreach (var item in MainPoject.SubCriteria)
            {
                item.Calculate(item.Value);
            }

            List<ReportModel> list = new List<ReportModel>();
            PemohonCollection collection = new PemohonCollection();

            foreach (var item in collection)
            {
                var model = new ReportModel { ID = item.Id, Nama = item.Nama };
                DataPemohonCollecion dataPemohon = new DataPemohonCollecion(item);
                foreach (var criteria in MainPoject.SubCriteria)
                {
                    var pemohonValue = dataPemohon.Where(O => O.KriteriaId == criteria.Id && O.Tahun==selectedTahun).FirstOrDefault();
                    if (pemohonValue != null && !string.IsNullOrEmpty(pemohonValue.Value))
                    {
                        var nilai = criteria.SubCriteria.Where(O => O.Name == pemohonValue.Value).FirstOrDefault().Value;
                        model.Nilai += nilai;
                    }

                }

                if(model.Nilai>0)
                list.Add(model);

            }

            rv.ProcessingMode = ProcessingMode.Local;


            LocalReport localReport = rv.LocalReport;
            rv.SetDisplayMode(DisplayMode.PrintLayout);
            localReport.ReportPath = "Laporan.rdlc";

            ReportDataSource dsSalesOrder = new ReportDataSource();
            dsSalesOrder.Name = "DataSet1";
            dsSalesOrder.Value = GetRank(list.OrderByDescending(O => O.Nilai).ToList());

            localReport.DataSources.Add(dsSalesOrder);
            localReport.SetParameters(new ReportParameter("Tahun", selectedTahun.ToString(), true));

            rv.RefreshReport();
        }
    }


    public class ReportModel
    {
        public int ID { get; set; }
        public string Nama { get; set; }
        public double Nilai { get; set; }
        public string Keterangan { get; set; }
    }
}
