using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using BLL;
using DTO;

namespace GeGET
{
    /// <summary>
    /// Interaction logic for Teste.xaml
    /// </summary>
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
            Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
            {
                lstClientes.ItemsSource = bll.LoadEstabelecimentos();
            }));
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
            var Id = ((EstabelecimentosDTO)lstClientes.Items[index]).Id;
            Negociosdto.FromParent = true;
            Negociosdto.ParentId = Id;
            helpers.Open<Negocios>(this.GetType().Name, true);
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int index = lstClientes.Items.IndexOf(btn.DataContext);
            var Id = ((EstabelecimentosDTO)lstClientes.Items[index]).Id;
            var Cliente_Id = ((EstabelecimentosDTO)lstClientes.Items[index]).Cliente_Id;
            var Razao_Social = ((EstabelecimentosDTO)lstClientes.Items[index]).Razao_Social;
            var Nome_Fantasia = ((EstabelecimentosDTO)lstClientes.Items[index]).Nome_Fantasia;
            var Endereco = ((EstabelecimentosDTO)lstClientes.Items[index]).Endereco;
            var UF_Id = ((EstabelecimentosDTO)lstClientes.Items[index]).UF_Id;
            var Cidade_Id = ((EstabelecimentosDTO)lstClientes.Items[index]).Cidade_Id;
            var CNPJ = ((EstabelecimentosDTO)lstClientes.Items[index]).Cnpj;
            var IE = ((EstabelecimentosDTO)lstClientes.Items[index]).Ie;
            var Telefone = ((EstabelecimentosDTO)lstClientes.Items[index]).Telefone;
            var Status = ((EstabelecimentosDTO)lstClientes.Items[index]).Status;
            using (var form = new EditarEstabelecimento(Id, Cliente_Id, Razao_Social, Nome_Fantasia, Endereco, UF_Id, Cidade_Id, CNPJ, IE, Telefone, Status))
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