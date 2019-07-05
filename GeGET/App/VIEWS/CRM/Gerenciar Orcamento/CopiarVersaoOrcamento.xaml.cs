using System;
using System.Windows;
using DTO;
using BLL;

namespace GeGET
{
    public partial class CopiarVersaoOrcamento : Window, IDisposable
    {
        bool disposed = false;
        public string Versao_Atividade_Id;
        public string Versao;
        VersaoOrcamentoBLL bll = new VersaoOrcamentoBLL();

        public CopiarVersaoOrcamento(NegociosDTO DTO)
        {
            InitializeComponent();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            cmbVersao.ItemsSource = bll.LoadVersaoAll(DTO);
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
                Versao_Atividade_Id = txtVersao_Atividade_Id.Text;
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
