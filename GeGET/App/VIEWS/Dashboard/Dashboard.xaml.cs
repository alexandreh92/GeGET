using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Helpers;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DTO;
using BLL;
using LiveCharts.Wpf;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Threading;
using System.Globalization;

namespace GeGET
{
    public partial class Dashboard : UserControl, IDisposable
    {
        bool disposed = false;
        NegociosBLL bll = new NegociosBLL();
        NegociosDTO dto = new NegociosDTO();
        public ObservableCollection<NegociosDTO> listaNegocios = new ObservableCollection<NegociosDTO>();
        ManualResetEvent syncEvent = new ManualResetEvent(false);
        DashboardComercialBLL dashboardComercialBLL = new DashboardComercialBLL();
        ObservableCollection<DashboardComercialDTO> listaComercial = new ObservableCollection<DashboardComercialDTO>();
        public SeriesCollection SeriesCollection { get; set; }
        public List<string> Labels { get; set; }
        string Find;
        DashboardChartDTO ChartDTO = new DashboardChartDTO();
        public Func<double, string> Formatter { get; set; }
        WaitBox wb;


        public Dashboard()
        {
            InitializeComponent();
            DataContext = this;
            LoadData();
        }

        private async void LoadData()
        {
            wb = new WaitBox();
            wb.Show();
            wb.Owner = Window.GetWindow(this);
            wb.Show();
            await Task.Run(() => 
            {
                ChartDTO = dashboardComercialBLL.LoadCharts();
            }); 
            chart.Series = ChartDTO.SeriesCollection;
            chartlabel.Labels = ChartDTO.Labels;
            txtMedia.Text = ChartDTO.Media.ToString("N2").Replace(",", ".");
            await Task.Run(() => 
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    listaComercial = dashboardComercialBLL.Load();
                }));
            });
            grdComercial.DataContext = listaComercial;
            listaNegocios = listaComercial.First().ListaNegocios;
            Formatter = value => value.ToString("P");
            DataContext = this;
            Find = "1";
            Commit();
            wb.Close();
        }

        private void Commit()
        {
            var filtered = listaNegocios.Where(descricao => Find.Any(list => descricao.Status_Id.ToString().Contains(list)));
            lstOrcamentos.ItemsSource = filtered;
        }

        private void BtnFila_Click(object sender, RoutedEventArgs e)
        {
            Find = "1";
            Commit();
        }

        private void BtnOrcando_Click(object sender, RoutedEventArgs e)
        {
            Find = "2";
            Commit();
        }

        private void BtnEnviados_Click(object sender, RoutedEventArgs e)
        {
            Find = "3";
            Commit();
        }

        private void BtnNegociacao_Click(object sender, RoutedEventArgs e)
        {
            Find = "4";
            Commit();
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                syncEvent.Dispose();
                bll.Dispose();
                dashboardComercialBLL.Dispose();
            }
            disposed = true;
        }

        #endregion
    }
    public class BooleanToVisibilityMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.OfType<bool>().Any(b => b) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
