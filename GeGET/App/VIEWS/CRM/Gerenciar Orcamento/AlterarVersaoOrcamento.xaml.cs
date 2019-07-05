using System;
using System.Windows;
using DTO;
using BLL;

namespace GeGET
{
    public partial class AlterarVersaoOrcamento : Window, IDisposable
    {
        bool disposed = false;
        public string Versao;
        VersaoOrcamentoBLL bll = new VersaoOrcamentoBLL();

        public AlterarVersaoOrcamento(NegociosDTO DTO)
        {
            InitializeComponent();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            cmbVersao.ItemsSource = bll.LoadVersao(DTO);
            cmbVersao.DisplayMemberPath = "Id";
            cmbVersao.SelectedValuePath = "Id";
            cmbVersao.SelectedIndex = 0;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void BtnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            if (cmbVersao.SelectedIndex >= 0)
            {
                DialogResult = true;
                Versao = cmbVersao.SelectedValue.ToString();
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
