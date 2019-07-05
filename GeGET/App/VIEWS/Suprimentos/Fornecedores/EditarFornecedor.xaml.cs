using System;
using System.Text.RegularExpressions;
using System.Windows;
using BLL;
using DTO;

namespace GeGET
{
    public partial class EditarFornecedor : Window, IDisposable
    {
        #region Declarations
        bool disposed = false;
        public static int id;
        FornecedoresBLL bll = new FornecedoresBLL();
        FornecedoresDTO dto = new FornecedoresDTO();
        #endregion

        #region Initialize
        public EditarFornecedor(FornecedoresDTO DTO)
        {
            InitializeComponent();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            /*txtDescricao.Text = DTO.Item_Descricao;
            txtFabricante.Text = DTO.Fornecedor;
            txtCusto.Text = DTO.Custo.ToString();
            txtAnotacoes.Text = DTO.Anotacoes;
            txtICMS.Text = (Convert.ToDouble(DTO.Icms)*100).ToString();
            txtIPI.Text = (Convert.ToDouble(DTO.Ipi)*100).ToString();
            txtNCM.Text = DTO.Ncm;
            txtPartnumber.Text = DTO.Partnumber;
            cbxStatus.IsChecked = Convert.ToBoolean(DTO.Status_Id);
            dto.Id = DTO.Id;*/
        }
        #endregion

        #region Events
        private void BtnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            
           /* dto.Partnumber = txtPartnumber.Text.ToUpper().Trim(' ').Replace("'","''");
            dto.Ncm = txtNCM.Text.ToUpper().Trim(' ').Replace("'", "''");
            dto.Custo = Convert.ToDouble(txtCusto.Text.ToUpper().Trim(' '));
            dto.Ipi = (Convert.ToDouble(txtIPI.Text.ToUpper().Trim('%')) / 100);
            dto.Icms = (Convert.ToDouble(txtICMS.Text.ToUpper().Trim('%')) / 100);
            dto.Status_Id = Convert.ToInt32(cbxStatus.IsChecked).ToString();
            dto.Anotacoes = txtAnotacoes.Text.Replace("'", "''").Replace("\n", "\r\n").ToUpper();
            bll.UpdateProduto(dto);
            DialogResult = true;
            this.Close();*/
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
        #endregion

        private void Txt_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9,]+").IsMatch(e.Text);
        }

        private void CmbUF_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void BtnPesquisa_Click(object sender, RoutedEventArgs e)
        {

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
