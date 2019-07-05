using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using BLL;
using DTO;
using MMLib.Extensions;

namespace GeGET
{
    public partial class Estabelecimentos : UserControl, IDisposable
    {
        #region Declarations
        bool disposed = false;
        EstabelecimentosBLL bll = new EstabelecimentosBLL();
        EstabelecimentosDTO dto = new EstabelecimentosDTO();
        NegociosDTO Negociosdto = new NegociosDTO();
        Helpers helpers = new Helpers();
        Thread t1;
        WaitBox wb;
        public ObservableCollection<EstabelecimentosDTO> listaEstabelecimentos;
        #endregion

        #region Initialize

        public Estabelecimentos()
        {
            InitializeComponent();
            LoadEstabelecimentos();
        }

        #endregion

        #region Methods
        private async void LoadEstabelecimentos()
        {
            wb = new WaitBox
            {
                Owner = Window.GetWindow(this)
            };
            wb.Show();
            await Task.Run(() => 
            {
                listaEstabelecimentos = bll.LoadEstabelecimentos();
            });
            lstClientes.ItemsSource = listaEstabelecimentos;
            wb.Close();
        }

        private async void Commit()
        {
            await Task.Run(() => 
            {
                Dispatcher.Invoke(DispatcherPriority.Background,
                  new Action(() =>
                  {
                      var Find = txtProcurar.Text.ToLower().RemoveDiacritics().Split(' ').ToList();
                      var filtered = listaEstabelecimentos.Where(descricao => Find.All(list => descricao.Razao_Social.ToLower().RemoveDiacritics().Contains(list) || descricao.Nome_Fantasia.ToLower().RemoveDiacritics().Contains(list) || descricao.Endereco.ToLower().Contains(list) || descricao.Cidade.ToLower().Contains(list) || descricao.Cnpj.ToLower().Contains(list)));
                      lstClientes.ItemsSource = filtered;
                  }));
            });
        }
        #endregion

        #region Events

        #region Text Changed
        private void TxtProcurar_TextChanged(object sender, TextChangedEventArgs e)
        {
            t1 = new Thread(Commit);
            t1.Start();
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
            dto.Descricao = ((EstabelecimentosDTO)lstClientes.Items[index]).Descricao;
            using (var form = new EditarEstabelecimento(dto))
            {
                form.Owner = Window.GetWindow(this);
                form.ShowDialog();
                if (form.DialogResult.Value && form.DialogResult.HasValue)
                {
                    lstClientes.ItemsSource = bll.LoadEstabelecimentos();
                }
            }
        }

        private void BtnOpenFolder_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Unloaded
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            dto.FromParent = false;
            dto.ParentId = "";
        }
        #endregion

        #region KeyDown

        private void TxtProcurar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                Commit();
            }
        }

        #endregion

        #endregion

        #region IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                bll.Dispose();
            }
            disposed = true;
        }
        #endregion
    }
}