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
    public partial class CRM : UserControl
    {
        Helpers helpers = new Helpers();
        public CRM()
        {
            InitializeComponent();
        }

        private void Button_Click()
        {

        }

        private void BtnClientes_Click(object sender, RoutedEventArgs e)
        {
            helpers.Open<Clientes>(this.GetType().Name,false);
        }

        private void BtnEstabelecimentos_Click(object sender, RoutedEventArgs e)
        {
            helpers.Open<Estabelecimentos>(this.GetType().Name, false);
        }

        private void BtnNegocios_Click(object sender, RoutedEventArgs e)
        {
            helpers.Open<Negocios>(this.GetType().Name, false);
        }

        private void BtnPessoas_Click(object sender, RoutedEventArgs e)
        {
            helpers.Open<Pessoas>(this.GetType().Name, false);
        }

        private void BtnFunil_Click(object sender, RoutedEventArgs e)
        {
            helpers.Open<Funil>(this.GetType().Name, false);
        }

        private void BtnCadastroCliente_Click(object sender, RoutedEventArgs e)
        {
            helpers.Open<CadastroCliente>(this.GetType().Name, false);
        }

        private void BtnCadastroEstabelecimento_Click(object sender, RoutedEventArgs e)
        {
            helpers.Open<CadastroEstabelecimento>(this.GetType().Name, false);
        }

        private void BtnCadastroPessoa_Click(object sender, RoutedEventArgs e)
        {
            helpers.Open<CadastroPessoa>(this.GetType().Name, false);
        }

        private void BtnCadastroNegocio_Click(object sender, RoutedEventArgs e)
        {
            helpers.Open<CadastroNegocio>(this.GetType().Name, false);
        }
    }
}
