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

        private void BtnProdutos_Click(object sender, MouseButtonEventArgs e)
        {
            helpers.OpenTab<Produtos>(sender, e, this.GetType().Name, false);
        }

        private void BtnItens_Click(object sender, MouseButtonEventArgs e)
        {
            helpers.OpenTab<Itens>(sender, e, this.GetType().Name, false);
        }

        private void BtnFornecedores_Click(object sender, MouseButtonEventArgs e)
        {
            helpers.OpenTab<Fornecedores>(sender, e, this.GetType().Name, false);
        }

        private void BtnGrupoFornecedores_Click(object sender, MouseButtonEventArgs e)
        {
            helpers.OpenTab<GrupodeFornecedores>(sender, e, this.GetType().Name, false);
        }

        private void BtnGrupoItens_Click(object sender, MouseButtonEventArgs e)
        {
            helpers.OpenTab<GrupodeItens>(sender, e, this.GetType().Name, false);
        }

        private void BtnCadastroFornecedores_Click(object sender, MouseButtonEventArgs e)
        {
            helpers.OpenTab<CadastroFornecedor>(sender, e, this.GetType().Name, false);
        }

        private void BtnCadastroGrupoFornecedores_Click(object sender, MouseButtonEventArgs e)
        {
            helpers.OpenTab<CadastroGrupodeFornecedores>(sender, e, this.GetType().Name, false);
        }

        private void BtnCadastroItem_Click(object sender, MouseButtonEventArgs e)
        {
            helpers.OpenTab<CadastroItens>(sender, e, this.GetType().Name, false);
        }

        private void BtnCadastroProduto_Click(object sender, MouseButtonEventArgs e)
        {
            helpers.OpenTab<CadastroProdutos>(sender, e, this.GetType().Name, false);
        }

        private void BtnCadastroGrupoItens_Click(object sender, MouseButtonEventArgs e)
        {
            helpers.OpenTab<CadastroGrupodeItens>(sender, e, this.GetType().Name, false);
        }

        private void BtnPrecificar_Click(object sender, MouseButtonEventArgs e)
        {
            helpers.OpenTab<PrecificarProdutos>(sender, e, this.GetType().Name, false);
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            helpers.Close();
        }
    }
}
