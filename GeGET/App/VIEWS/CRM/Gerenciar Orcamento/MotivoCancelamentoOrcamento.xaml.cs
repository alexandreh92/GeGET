using System;
using System.Windows;
using BLL;

namespace GeGET
{
    public partial class MotivoCancelamentoOrcamento : Window, IDisposable
    {
        bool disposed = false;
        public string Motivo_Cancelamento_Id;
        public string Motivo_Cancelamento;
        MotivoCancelamentoOrcamentoBLL bll = new MotivoCancelamentoOrcamentoBLL();

        public MotivoCancelamentoOrcamento()
        {
            InitializeComponent();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            cmbMotivo.ItemsSource = bll.LoadMotivos();
            cmbMotivo.DisplayMemberPath = "Descricao";
            cmbMotivo.SelectedValuePath = "Id";
            cmbMotivo.SelectedIndex = 0;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void BtnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            if (cmbMotivo.SelectedIndex >= 0)
            {
                DialogResult = true;
                Motivo_Cancelamento_Id = cmbMotivo.SelectedValue.ToString();
                Motivo_Cancelamento = cmbMotivo.Text;
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
