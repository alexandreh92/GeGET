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
    public partial class Logistica : UserControl
    {
        Helpers helpers = new Helpers();
        public Logistica()
        {
            InitializeComponent();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            helpers.Close();
        }

        private void BtnEntradaXML_Click(object sender, MouseButtonEventArgs e)
        {
            helpers.OpenTab<EntradaNotaFiscal>(sender, e, this.GetType().Name, false);
        }

        private void BtnEntradaManual_Click(object sender, MouseButtonEventArgs e)
        {
            helpers.OpenTab<EntradaManualEstoque>(sender, e, this.GetType().Name, false);
        }

        private void BtnAtendimento_Click(object sender, MouseButtonEventArgs e)
        {
            helpers.OpenTab<Atendimento>(sender, e, this.GetType().Name, false);
        }

        private void BtnSaldo_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            helpers.OpenTab<SaldoEstoque>(sender, e, this.GetType().Name, false);
        }

    }
}
