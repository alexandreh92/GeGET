using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

namespace GeGET
{
    public partial class EntradaManualEstoque : UserControl, IDisposable
    {
        bool disposed = false;
        Helpers helpers = new Helpers();
        EntradaManualEstoqueBLL bll = new EntradaManualEstoqueBLL();
        EntradaManualEstoqueDTO dto = new EntradaManualEstoqueDTO();
        ObservableCollection<EntradaManualEstoqueDTO> listaEntrada = new ObservableCollection<EntradaManualEstoqueDTO>();
        ObservableCollection<EntradaManualEstoqueDTO> listaInserir;

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
            using (var form = new AdicionarItemListaManual())
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
            var handles = grdItens.GetSelectedRowHandles();
            if (handles.Length>0)
            {
                listaInserir = new ObservableCollection<EntradaManualEstoqueDTO>();

                foreach (var rowHandle in handles)
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
                    CustomOKMessageBox.Show("Existem itens que possuem a quantidade ou o preço zerado.","Atenção!",Window.GetWindow(this));
                }
                else
                {
                    bll.InserirEstoque(listaInserir);
                    List<int> selectedRowHandles = new List<int>(grdItens.GetSelectedRowHandles());
                    var descendingOrder = selectedRowHandles.OrderByDescending(i => i);
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

        private void ExcluirProduto_Click(object sender, RoutedEventArgs e)
        {
            List<int> selectedRowHandles = new List<int>(grdItens.GetSelectedRowHandles());
            var descendingOrder = selectedRowHandles.OrderByDescending(i => i);
            grdItens.BeginDataUpdate();
            foreach (int i in descendingOrder)
            {
                grdView.DeleteRow(i);
            }
            grdItens.EndDataUpdate();
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
