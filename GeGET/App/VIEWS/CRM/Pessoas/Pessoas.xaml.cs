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
    public partial class Pessoas : UserControl, IDisposable
    {
        #region Declarations
        bool disposed = false;
        PessoasBLL bll = new PessoasBLL();
        PessoasDTO dto = new PessoasDTO();
        Helpers helpers = new Helpers();
        Thread t1;
        WaitBox wb;
        ObservableCollection<PessoasDTO> listaPessoas;
        #endregion

        #region Initialize

        public Pessoas()
        {
            InitializeComponent();
            LoadPessoas();
        }

        #endregion

        #region Methods
        private async void LoadPessoas()
        {
            wb = new WaitBox();
            wb.Owner = Window.GetWindow(this);
            wb.Show();
            await Task.Run(() => 
            {
                listaPessoas = bll.LoadPessoas();
            });
            lstClientes.ItemsSource = listaPessoas;
            wb.Close();
        }

        private void Commit()
        {
            Dispatcher.Invoke(DispatcherPriority.Background,
                  new Action(() =>
                  {
                      var Find = txtProcurar.Text.ToLower().RemoveDiacritics().Split(' ').ToList();
                      var filtered = listaPessoas.Where(descricao => Find.All(list => descricao.Nome.ToLower().RemoveDiacritics().Contains(list) || descricao.Funcao.ToLower().RemoveDiacritics().Contains(list) || descricao.Email.ToLower().Contains(list) || descricao.Celular.ToLower().Contains(list) || descricao.Telefone.ToLower().Contains(list) || descricao.Rsocial.ToLower().Contains(list)));
                      lstClientes.ItemsSource = filtered;
                  }));
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

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            helpers.Close();
        }

        private void BtnPesquisa_Click(object sender, RoutedEventArgs e)
        {
            Commit();
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

        private void BtnNegocios_Click(object sender, MouseButtonEventArgs e)
        {
            Button btn = sender as Button;
            int index = lstClientes.Items.IndexOf(btn.DataContext);
            var Id = ((EstabelecimentosDTO)lstClientes.Items[index]).Id;
            //Negociosdto.FromParent = true;
            //Negociosdto.ParentId = Id;
            helpers.OpenTab<Negocios>(sender, e, this.GetType().Name, true);
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int index = lstClientes.Items.IndexOf(btn.DataContext);
            dto = ((PessoasDTO)lstClientes.Items[index]) as PessoasDTO;
            using (var form = new EditarPessoas(dto))
            {
                form.Owner = Window.GetWindow(this);
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

        #region KeyDown
        private void TxtProcurar_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
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