using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// 
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

        private void BtnClientes_Click(object sender, MouseButtonEventArgs e)
        {
            helpers.OpenTab<Clientes>(sender, e, this.GetType().Name,false);
        }

        private void BtnEstabelecimentos_Click(object sender, MouseButtonEventArgs e)
        {
            helpers.OpenTab<Estabelecimentos>(sender, e, this.GetType().Name, false);
        }

        private void BtnNegocios_Click(object sender, MouseButtonEventArgs e)
        {
            helpers.OpenTab<Negocios>(sender, e, this.GetType().Name, false);
        }

        private void BtnPessoas_Click(object sender, MouseButtonEventArgs e)
        {
            helpers.OpenTab<Pessoas>(sender, e, this.GetType().Name, false);
        }

        private void BtnFunil_Click(object sender, MouseButtonEventArgs e)
        {
            helpers.OpenTab<Funil>(sender, e, this.GetType().Name, false);
        }

        private void BtnCadastroCliente_Click(object sender, MouseButtonEventArgs e)
        {
            helpers.OpenTab<CadastroCliente>(sender, e, this.GetType().Name, false);
        }

        private void BtnCadastroEstabelecimento_Click(object sender, MouseButtonEventArgs e)
        {
            helpers.OpenTab<CadastroEstabelecimento>(sender, e, this.GetType().Name, false);
        }

        private void BtnCadastroPessoa_Click(object sender, MouseButtonEventArgs e)
        {
            helpers.OpenTab<CadastroPessoa>(sender, e, this.GetType().Name, false);
        }

        private void BtnCadastroNegocio_Click(object sender, MouseButtonEventArgs e)
        {
            helpers.OpenTab<CadastroNegocio>(sender, e, this.GetType().Name, false);
        }

        private void BtnDashboardNegocio_Click(object sender, MouseButtonEventArgs e)
        {
            helpers.OpenTab<GerenciarOrcamento>(sender, e, this.GetType().Name, false);
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            helpers.Close();
        }

        private void BtnCadastroAtividades_Click(object sender, MouseButtonEventArgs e)
        {
            helpers.OpenTab<CadastroAtividades>(sender, e, this.GetType().Name, false);
        }

        private void BtnListaSimplificadaNegocios_Click(object sender, MouseButtonEventArgs e)
        {
            helpers.OpenTab<ListaNegociosSimplificada>(sender, e, this.GetType().Name, false);
        }

        private void BtnCopiarAtividades_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            helpers.OpenTab<CopiarAtividades>(sender, e, this.GetType().Name, false);
        }
    }
}
