using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BLL;
using DTO;

namespace GeGET
{
    public partial class Estabelecimentos : UserControl
    {
        #region Declarations
        EstabelecimentosBLL bll = new EstabelecimentosBLL();
        EstabelecimentosDTO dto = new EstabelecimentosDTO();
        NegociosDTO Negociosdto = new NegociosDTO();
        Helpers helpers = new Helpers();
        #endregion

        #region Initialize

        public Estabelecimentos()
        {
            InitializeComponent();
            dto.Pesquisa = "";
            LoadEstabelecimentos();
        }

        #endregion

        #region Methods
        private void LoadEstabelecimentos()
        {
            lstClientes.ItemsSource = bll.LoadEstabelecimentos();
        }

        private void Commit()
        {
            dto.Pesquisa = txtProcurar.Text.Replace("'", "''");
            LoadEstabelecimentos();
        }
        #endregion

        #region Events

        #region Keydowns
        private void CommentTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Commit();
            }
        }
        #endregion

        #region Clicks

        private void BtnPesquisa_Click(object sender, RoutedEventArgs e)
        {
            Commit();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            helpers.Close();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (dto.FromParent)
            {
                helpers.OpenBack(true);
            }
            else
            {
                helpers.OpenBack(false);
            }
        }

        private void BtnNegocios_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int index = lstClientes.Items.IndexOf(btn.DataContext);
            var Id = ((EstabelecimentosDTO)lstClientes.Items[index]).Cliente_Id;
            Negociosdto.FromParent = true;
            Negociosdto.ParentId = Id;
            helpers.Open<Negocios>(this.GetType().Name, true);
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int index = lstClientes.Items.IndexOf(btn.DataContext);
            dto.Id = ((EstabelecimentosDTO)lstClientes.Items[index]).Id;
            dto.Cliente_Id = ((EstabelecimentosDTO)lstClientes.Items[index]).Cliente_Id;
            dto.Razao_Social = ((EstabelecimentosDTO)lstClientes.Items[index]).Razao_Social;
            dto.Nome_Fantasia = ((EstabelecimentosDTO)lstClientes.Items[index]).Nome_Fantasia;
            dto.Endereco = ((EstabelecimentosDTO)lstClientes.Items[index]).Endereco;
            dto.UF_Id = ((EstabelecimentosDTO)lstClientes.Items[index]).UF_Id;
            dto.Cidade_Id = ((EstabelecimentosDTO)lstClientes.Items[index]).Cidade_Id;
            dto.Cnpj = ((EstabelecimentosDTO)lstClientes.Items[index]).Cnpj;
            dto.Ie = ((EstabelecimentosDTO)lstClientes.Items[index]).Ie;
            dto.Telefone = ((EstabelecimentosDTO)lstClientes.Items[index]).Telefone;
            dto.Status = ((EstabelecimentosDTO)lstClientes.Items[index]).Status;
            using (var form = new EditarEstabelecimento(dto))
            {
                form.ShowDialog();
                if (form.DialogResult.Value && form.DialogResult.HasValue)
                {
                    lstClientes.ItemsSource = bll.LoadEstabelecimentos();
                }
            }
        }

        #endregion

        #region Unloaded
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            dto.FromParent = false;
            dto.ParentId = "";
        }
        #endregion

        #endregion
    }
}