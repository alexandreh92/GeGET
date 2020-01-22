using System;
using System.Windows;

namespace GeGET
{
    public partial class DescricaoRM : Window, IDisposable
    {
        bool disposed = false;
        public string Valor;
        public DescricaoRM()
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
            if (txtValor.Text.Length > 0)
            {
                Valor = txtValor.Text.ToString().Replace(",", ".");
                DialogResult = true;
            }
            else
            {
                CustomOKMessageBox.Show("Descrição não pode ser nula!", "Atenção!", Window.GetWindow(this));
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
