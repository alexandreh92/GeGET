using System;
using System.Windows;
using DTO;
using BLL;

namespace GeGET
{
    public partial class AnotacoesOrcamento : Window, IDisposable
    {
        bool disposed = false;
        ListaOrcamentosDTO dto = new ListaOrcamentosDTO();
        ListaOrcamentosBLL bll = new ListaOrcamentosBLL();
        public AnotacoesOrcamento(string Id, string Anotacoes, string Produto_Id)
        {
            InitializeComponent();
            dto.Id = Id;
            dto.Produto_Id = Produto_Id;
            dto.Anotacoes = Anotacoes;
            txtAnotacoes.Text = dto.Anotacoes;
            if (Produto_Id != "1")
            {
                txtAnotacoes.IsReadOnly = true;
                btnConfirmar.IsEnabled = false;
            }
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void BtnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            dto.Anotacoes = txtAnotacoes.Text.Replace("'", "''").ToUpper();
            bll.UpdateAnotacoes(dto);

            DialogResult = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtAnotacoes.Focus();
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
