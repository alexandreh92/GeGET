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
    public partial class Suprimentos : UserControl
    {
        Helpers helpers = new Helpers();
        public Suprimentos()
        {
            InitializeComponent();
        }

        private void Button_Click()
        {

        }

        private void BtnProdutos_Click(object sender, RoutedEventArgs e)
        {
            helpers.Open<Produtos>(this.GetType().Name, false);
        }

        private void BtnItens_Click(object sender, RoutedEventArgs e)
        {
            helpers.Open<Itens>(this.GetType().Name, false);
        }

        private void BtnFornecedores_Click(object sender, RoutedEventArgs e)
        {
            helpers.Open<Fornecedores>(this.GetType().Name, false);
        }

        private void BtnGrupoFornecedores_Click(object sender, RoutedEventArgs e)
        {
            helpers.Open<GrupodeFornecedores>(this.GetType().Name, false);
        }

        private void BtnGrupoItens_Click(object sender, RoutedEventArgs e)
        {
            helpers.Open<GrupodeItens>(this.GetType().Name, false);
        }

        private void BtnCadastroFornecedores_Click(object sender, RoutedEventArgs e)
        {
            helpers.Open<CadastroFornecedor>(this.GetType().Name, false);
        }

        private void BtnCadastroGrupoFornecedores_Click(object sender, RoutedEventArgs e)
        {
            helpers.Open<CadastroGrupodeFornecedores>(this.GetType().Name, false);
        }

        private void BtnCadastroItem_Click(object sender, RoutedEventArgs e)
        {
            helpers.Open<CadastroItens>(this.GetType().Name, false);
        }

        private void BtnCadastroProduto_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCadastroGrupoItens_Click(object sender, RoutedEventArgs e)
        {
            helpers.Open<CadastroGrupodeItens>(this.GetType().Name, false);
        }

        private void BtnPrecificar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            helpers.Close();
        }
    }
}
