using System;
using System.Windows;

namespace GeGET
{
    public partial class ValorEnviadoOrcamento : Window, IDisposable
    {
        bool disposed = false;
        public string Valor;
        public ValorEnviadoOrcamento()
        {
            InitializeComponent();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void BtnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            decimal parse;
            if (Decimal.TryParse(txtValor.Text, out parse) && txtValor.Text != "")
            {
                var result = CustomOKCancelMessageBox.Show("Confirma o valor: " + Convert.ToDecimal(txtValor.Text).ToString("c") + " ?", "Atenção!", Window.GetWindow(this));
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    Valor = Convert.ToDecimal(txtValor.Text).ToString().Replace(",", ".");
                    DialogResult = true;
                }
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
            }
            disposed = true;
        }
        #endregion
    }
}
