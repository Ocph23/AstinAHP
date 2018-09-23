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
          
            foreach(var item in MainPoject.SubCriteria)
            {
                item.Calculate(item.Value);
            }

            List<ReportModel> list = new List<ReportModel>();
            PemohonCollection collection = new PemohonCollection();
           
            foreach(var item in collection)
            {
                var model = new ReportModel { ID=item.Id, Nama=item.Nama };
                DataPemohonCollecion dataPemohon = new DataPemohonCollecion(item);
              foreach(var criteria in MainPoject.SubCriteria)
                {
                    var pemohonValue = dataPemohon.Where(O => O.KriteriaId == criteria.Id).FirstOrDefault().Value;
                    var nilai = criteria.SubCriteria.Where(O => O.Name == pemohonValue).FirstOrDefault().Value;
                    model.Nilai += nilai;
                }
                list.Add(model);

            }

            rv.ProcessingMode = ProcessingMode.Local;
            

            LocalReport localReport = rv.LocalReport;
            rv.SetDisplayMode(DisplayMode.PrintLayout);
            localReport.ReportPath = "Laporan.rdlc";

            ReportDataSource dsSalesOrder = new ReportDataSource();
            dsSalesOrder.Name = "DataSet1";
            dsSalesOrder.Value = GetRank(list.OrderByDescending(O=>O.Nilai).ToList());

            localReport.DataSources.Add(dsSalesOrder);

           
            rv.RefreshReport();

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
    }


    public class ReportModel
    {
        public int ID { get; set; }
        public string Nama { get; set; }
        public double Nilai { get; set; }
        public string Keterangan { get; set; }
    }
}
