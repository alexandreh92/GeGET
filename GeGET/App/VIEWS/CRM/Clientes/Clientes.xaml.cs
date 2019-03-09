using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using BLL;
using DTO;

namespace GeGET
{
    public partial class Clientes : UserControl
    {
        #region Declarations
        ClientesBLL bll = new ClientesBLL();
        ClientesDTO dto = new ClientesDTO();
        EstabelecimentosDTO Estabelecimentosdto = new EstabelecimentosDTO();
        NegociosDTO Negociosdto = new NegociosDTO();
        PessoasDTO Pessoasdto = new PessoasDTO();
        Helpers helpers = new Helpers();

        #endregion

        #region Initialize

        public Clientes()
        {
            InitializeComponent();
            dto.Pesquisa = "";
            LoadClients();
        }

        #endregion

        #region Methods
        private void LoadClients()
        {
            Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
            {
                lstClientes.ItemsSource = bll.LoadClientes();
            }));
        }

        private void Commit()
        {
            dto.Pesquisa = txtProcurar.Text.Replace("'", "''");
            LoadClients();
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

        private void BtnEstabelecimentos_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int index = lstClientes.Items.IndexOf(btn.DataContext);
            var Id = ((ClientesDTO)lstClientes.Items[index]).Id;
            Estabelecimentosdto.FromParent = true;
            Estabelecimentosdto.ParentId = Id;
            helpers.Open<Estabelecimentos>(this.GetType().Name, true);
        }

        private void BtnNegocios_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int index = lstClientes.Items.IndexOf(btn.DataContext);
            var Id = ((ClientesDTO)lstClientes.Items[index]).Id;
            Negociosdto.FromParent = true;
            Negociosdto.ParentId = Id;
            helpers.Open<Negocios>(this.GetType().Name, true);
        }

        private void BtnPessoas_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int index = lstClientes.Items.IndexOf(btn.DataContext);
            var Id = ((ClientesDTO)lstClientes.Items[index]).Id;
            Pessoasdto.FromParent = true;
            Pessoasdto.ParentId = Id;
            helpers.Open<Pessoas>(this.GetType().Name, true);
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            var index = lstClientes.Items.IndexOf(btn.DataContext);
            dto.Id = ((ClientesDTO)lstClientes.Items[index]).Id;
            dto.Razao_Social = ((ClientesDTO)lstClientes.Items[index]).Razao_Social;
            dto.Nome_Fantasia = ((ClientesDTO)lstClientes.Items[index]).Nome_Fantasia;
            dto.Categoria_Id = ((ClientesDTO)lstClientes.Items[index]).Categoria_Id;
            dto.Status = ((ClientesDTO)lstClientes.Items[index]).Status;
            using (var form = new EditarCliente(dto))
            {
                form.ShowDialog();
                if (form.DialogResult.Value && form.DialogResult.HasValue)
                {
                    lstClientes.ItemsSource = bll.LoadClientes();
                }
            }
        }
        #endregion

        #endregion
    }
}