using LiveCharts;
using LiveCharts.Wpf;
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

namespace GeGET
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public Window1()
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Values = new ChartValues<int> {5,4,6,3},
                    Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#90FFFFFF")),
                    Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")),
                    PointGeometry = null,
                    
                },
                new LineSeries
                {
                    Values = new ChartValues<int> {12,5,3,15},
                    Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#77000000")),
                    Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000")),
                    PointGeometry = null
                }
                
            };

            Labels = new[] { "Jan", "Fev", "Abr", "Mai" };
            YFormatter = value => value.ToString();

            DataContext = this;

        }
    }
}
