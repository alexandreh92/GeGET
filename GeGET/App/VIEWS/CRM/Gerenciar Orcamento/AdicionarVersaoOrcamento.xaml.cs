using System;
using System.Windows;
using DTO;
using BLL;

namespace GeGET
{
    public partial class AdicionarVersaoOrcamento : Window, IDisposable
    {
        bool disposed = false;
        public int Versao;
        public string Descricao;
        NegociosBLL bll = new NegociosBLL();

        public AdicionarVersaoOrcamento(NegociosDTO DTO)
        {
            InitializeComponent();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            Versao = bll.NewVersionNumber(DTO);
            txtVersao.Text = Versao.ToString("00");
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void BtnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            if (txtDescricao.Text != "")
            {
                Descricao = txtDescricao.Text.Replace("'", "''").ToUpper();
                DialogResult = true;
                this.Close();
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
