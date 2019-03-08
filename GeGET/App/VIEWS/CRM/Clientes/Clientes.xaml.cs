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
    public partial class Clientes : UserControl
    {
        #region Declarations
        ClientesBLL bll = new ClientesBLL();
        ClientesDTO dto = new ClientesDTO();
        EstabelecimentosDTO Estabelecimentosdto = new EstabelecimentosDTO();
        NegociosDTO Negociosdto = new NegociosDTO();
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

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int index = lstClientes.Items.IndexOf(btn.DataContext);
            var Id = ((ClientesDTO)lstClientes.Items[index]).Id;
            var Razao_Social = ((ClientesDTO)lstClientes.Items[index]).Razao_Social;
            var Nome_Fantasia = ((ClientesDTO)lstClientes.Items[index]).Nome_Fantasia;
            var Categoria_Id = ((ClientesDTO)lstClientes.Items[index]).Categoria_Id;
            var Status = ((ClientesDTO)lstClientes.Items[index]).Status;
            var result = EditarCliente.Show(Id, Razao_Social, Nome_Fantasia, Categoria_Id, Status);
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                lstClientes.ItemsSource = bll.LoadClientes();
            }
        }

        #endregion

        #endregion
    }
}