using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace GeGET
{
    public partial class EntradaManualEstoque : UserControl, IDisposable
    {
        private bool disposed = false;
        private Helpers helpers = new Helpers();
        private EntradaManualEstoqueBLL bll = new EntradaManualEstoqueBLL();
        private EntradaManualEstoqueDTO dto = new EntradaManualEstoqueDTO();
        private ObservableCollection<EntradaManualEstoqueDTO> listaEntrada = new ObservableCollection<EntradaManualEstoqueDTO>();
        private ObservableCollection<EntradaManualEstoqueDTO> listaInserir;

        public EntradaManualEstoque()
        {
            InitializeComponent();
        }

        #region Clicks

        #region Botão Voltar
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            helpers.OpenBack(false);
        }
        #endregion

        #region Botão Fechar
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            helpers.Close();
        }
        #endregion

        #endregion

        private void AdicionarProduto_Click(object sender, RoutedEventArgs e)
        {
            using (AdicionarItemListaManual form = new AdicionarItemListaManual())
            {
                form.Owner = Window.GetWindow(this);
                form.ShowDialog();
                if (form.DialogResult.Value && form.DialogResult.HasValue)
                {
                    foreach (EntradaManualEstoqueDTO item in form.listaEntrada)
                    {
                        listaEntrada.Add(item);
                    }
                    grdItens.ItemsSource = listaEntrada;
                }
            }
        }

        private void AdicionarEstoque_Click(object sender, RoutedEventArgs e)
        {
            bool iNotify = false;
            int[] handles = grdItens.GetSelectedRowHandles();
            if (handles.Length > 0)
            {
                listaInserir = new ObservableCollection<EntradaManualEstoqueDTO>();

                foreach (int rowHandle in handles)
                {
                    dto = grdItens.GetRow(rowHandle) as EntradaManualEstoqueDTO;
                    if (dto.Quantidade != 0 && dto.Custo != 0)
                    {
                        listaInserir.Add(dto);
                    }
                    else
                    {
                        iNotify = true;
                    }
                }
                if (iNotify)
                {
                    CustomOKMessageBox.Show("Existem itens que possuem a quantidade ou o preço zerado.", "Atenção!", Window.GetWindow(this));
                }
                else
                {
                    System.Windows.Forms.DialogResult result = CustomOKCancelMessageBox.Show("Deseja mesmo dar entrada nos itens selecionados?", "Atenção!", Window.GetWindow(this));
                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        bll.InserirEstoque(listaInserir);
                        List<int> selectedRowHandles = new List<int>(grdItens.GetSelectedRowHandles());
                        IOrderedEnumerable<int> descendingOrder = selectedRowHandles.OrderByDescending(i => i);
                        grdItens.BeginDataUpdate();
                        foreach (int i in descendingOrder)
                        {
                            grdView.DeleteRow(i);
                        }
                        grdItens.EndDataUpdate();
                        CustomOKMessageBox.Show("Itens adicionados com sucesso ao estoque.", "Sucesso!", Window.GetWindow(this));
                    }
                }
            }
        }

        private void ExcluirProduto_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult result = CustomOKCancelMessageBox.Show("Deseja mesmo remover os itens selecionados?", "Atenção!", Window.GetWindow(this));
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                List<int> selectedRowHandles = new List<int>(grdItens.GetSelectedRowHandles());
                IOrderedEnumerable<int> descendingOrder = selectedRowHandles.OrderByDescending(i => i);
                grdItens.BeginDataUpdate();
                foreach (int i in descendingOrder)
                {
                    grdView.DeleteRow(i);
                }
                grdItens.EndDataUpdate();
            }
        }

        #region IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                bll.Dispose();
            }
            disposed = true;
        }
        #endregion

        private void GrdItens_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ((TableView)grdItens.View).MoveNextRow();
                ((TableView)grdItens.View).ShowEditor();
                //e.Handled = true;
            }
        }

        private void GrdView_ShownEditor(object sender, DevExpress.Xpf.Grid.EditorEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                if (this.grdView.ActiveEditor != null)
                {
                    this.grdView.ActiveEditor.SelectAll();
                }
                else
                {
                    ((TableView)grdItens.View).ShowEditor();
                }
            }), DispatcherPriority.Render);
        }
    }
}
