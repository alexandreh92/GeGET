using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using BLL;
using DTO;
using MMLib.Extensions;

namespace GeGET
{
    public partial class GrupodeFornecedores : UserControl, IDisposable
    {
        #region Declarations
        bool disposed = false;
        GrupoFornecedoresBLL bll = new GrupoFornecedoresBLL();
        GrupoFornecedoresDTO dto = new GrupoFornecedoresDTO();
        Helpers helpers = new Helpers();
        Thread t1;
        WaitBox wb;
        public ObservableCollection<GrupoFornecedoresDTO> listaGrupos;
        #endregion

        #region Initialize

        public GrupodeFornecedores()
        {
            InitializeComponent();
            LoadGrupo();
        }

        #endregion

        #region Methods
        private async void LoadGrupo()
        {
            wb = new WaitBox();
            wb.Owner = Window.GetWindow(this);
            wb.Show();
            await Task.Run(() =>
            {
                listaGrupos = bll.LoadGrupoFornecedores();
            });
            lstGrupos.ItemsSource = listaGrupos;
            wb.Close();
        }

        private void Commit()
        {
            Dispatcher.Invoke(DispatcherPriority.Background,
                  new Action(() =>
                  {
                      var Find = txtProcurar.Text.ToLower().RemoveDiacritics().Split(' ').ToList();
                      var filtered = listaGrupos.Where(descricao => Find.Any(list => descricao.Descricao.ToLower().RemoveDiacritics().Contains(list)  || descricao.Usuario.RemoveDiacritics().ToLower().Contains(list)));
                      lstGrupos.ItemsSource = filtered;
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
            helpers.OpenBack(false);
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            /*Button btn = sender as Button;
            var index = lstProdutos.Items.IndexOf(btn.DataContext);
            dto.Id = ((ProdutosDTO)lstProdutos.Items[index]).Id;
            dto.Item_Descricao = ((ProdutosDTO)lstProdutos.Items[index]).Item_Descricao;
            dto.Fornecedor = ((ProdutosDTO)lstProdutos.Items[index]).Fornecedor;
            dto.Partnumber = ((ProdutosDTO)lstProdutos.Items[index]).Partnumber;
            dto.Custo = ((ProdutosDTO)lstProdutos.Items[index]).Custo;
            dto.Ipi = ((ProdutosDTO)lstProdutos.Items[index]).Ipi;
            dto.Icms = ((ProdutosDTO)lstProdutos.Items[index]).Icms;
            dto.Ncm = ((ProdutosDTO)lstProdutos.Items[index]).Ncm;
            dto.Status_Id = ((ProdutosDTO)lstProdutos.Items[index]).Status_Id;
            dto.Anotacoes = ((ProdutosDTO)lstProdutos.Items[index]).Anotacoes;
            using (var form = new EditarProduto(dto))
            {
                form.Owner = Window.GetWindow(this);
                form.ShowDialog();
                if (form.DialogResult.Value && form.DialogResult.HasValue)
                {
                    listaProdutos = bll.LoadProdutos();
                    lstProdutos.ItemsSource = listaProdutos;
                }
            }*/
        }
        #endregion

        #region KeyDowns
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