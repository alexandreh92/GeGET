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

namespace GeGET
{
    /// <summary>
    /// Interaction logic for Teste.xaml
    /// </summary>
    public partial class Orcamentos : UserControl
    {
        Helpers helpers = new Helpers();
        public Orcamentos()
        {
            InitializeComponent();
        }

        private void BtnListaOrcamento_Click(object sender, RoutedEventArgs e)
        {
            helpers.Open<ListaOrcamento>(this.GetType().Name, false);
        }

        private void BtnConsultaOrcamento_Click(object sender, RoutedEventArgs e)
        {
            helpers.Open<ConsultaListaOrcamento>(this.GetType().Name, false);
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            helpers.Close();
        }

        private void BtnExportarListaOrcamento_Click(object sender, RoutedEventArgs e)
        {
            helpers.Open<ExportarListaOrcamento>(this.GetType().Name, false);
        }
    }
}
