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
    public partial class Pessoas : UserControl
    {
        #region Declarations
        PessoasBLL bll = new PessoasBLL();
        PessoasDTO dto = new PessoasDTO();
        Helpers helpers = new Helpers();
        #endregion

        #region Initialize

        public Pessoas()
        {
            InitializeComponent();
            dto.Pesquisa = "";
            LoadPessoas();
        }

        #endregion

        #region Methods
        private void LoadPessoas()
        {
            Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
            {
                lstClientes.ItemsSource = bll.LoadPessoas();
            }));
        }

        private void Commit()
        {
            dto.Pesquisa = txtProcurar.Text.Replace("'", "''");
            LoadPessoas();
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
            //Negociosdto.FromParent = true;
            //Negociosdto.ParentId = Id;
            helpers.Open<Negocios>(this.GetType().Name, true);
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int index = lstClientes.Items.IndexOf(btn.DataContext);
            dto.Id = ((PessoasDTO)lstClientes.Items[index]).Id;
            dto.Nome = ((PessoasDTO)lstClientes.Items[index]).Nome;
            dto.Rsocial = ((PessoasDTO)lstClientes.Items[index]).Rsocial;
            dto.Email = ((PessoasDTO)lstClientes.Items[index]).Email;
            dto.Telefone = ((PessoasDTO)lstClientes.Items[index]).Telefone;
            dto.Celular = ((PessoasDTO)lstClientes.Items[index]).Celular;
            dto.Cliente_Id = ((PessoasDTO)lstClientes.Items[index]).Cliente_Id;
            dto.Funcao_Id = ((PessoasDTO)lstClientes.Items[index]).Funcao_Id;
            dto.Status_Id = ((PessoasDTO)lstClientes.Items[index]).Status_Id;
            using (var form = new EditarPessoas(dto))
            {
                form.ShowDialog();
                if (form.DialogResult.Value && form.DialogResult.HasValue)
                {
                    lstClientes.ItemsSource = bll.LoadPessoas();
                }
            }
        }

        #endregion

        #region Unloaded
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            dto.FromChildrenParent = false;
            dto.FromParent = false;
            dto.ParentId = "";
        }
        #endregion

        #endregion
    }
}