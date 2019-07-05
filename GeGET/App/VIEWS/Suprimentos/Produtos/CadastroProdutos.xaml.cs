using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BLL;
using DTO;

namespace GeGET
{
    public partial class CadastroProdutos : UserControl, IDisposable
    {
        #region Declarations
        bool disposed = false;
        ProdutosBLL bll = new ProdutosBLL();
        ProdutosDTO dto = new ProdutosDTO();
        Helpers helpers = new Helpers();
        #endregion

        #region Initialize
        public CadastroProdutos()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods

        #region ClearControls
        private void ClearControls()
        {
            txtItem.Text = " ";
            txtDescricao.Text = "";
            txtUn.Text = "";
            txtFabricante.Text = "";
            txtPartnumber.Text = "";
            txtCusto.Text = "";
            txtIPI.Text = "";
            txtICMS.Text = "";
            txtDescricaoDetalhada.Text = "";
            txtNCM.Text = "";
        }
        #endregion

        #endregion

        #region Events

        #region Clicks
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            helpers.OpenBack(false);
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            helpers.Close();
        }

        private void BtnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            if (txtItem.Text != "" && txtFabricante.Text != "" && txtCusto.Text != "" && txtIPI.Text != "" && txtICMS.Text != "")
            {
                var result = CustomOKCancelMessageBox.Show("Deseja mesmo cadastrar este produto?", "Atenção!", Window.GetWindow(this));
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    dto.Anotacoes = txtDescricaoDetalhada.Text.Replace("'", "''").ToUpper().TrimStart(' ');
                    dto.Ncm = txtNCM.Text;
                    dto.Custo = Convert.ToDouble(txtCusto.Text);
                    dto.Icms = Convert.ToDouble(txtICMS.Text)/100;
                    dto.Ipi = Convert.ToDouble(txtIPI.Text)/100;
                    dto.Partnumber = txtPartnumber.Text.Replace("'", "''").ToUpper().TrimStart(' ');
                    if (bll.InserirProduto(dto))
                    {
                        ClearControls();
                        CustomOKMessageBox.Show("Produto cadastrado com sucesso!", "Sucesso!", Window.GetWindow(this));
                    }
                }
            }
            else
            {
                CustomOKMessageBox.Show("Campos não podem estar em branco.", "Atenção!", Window.GetWindow(this));
            }
        }

        private void BtnPesquisa_Click(object sender, RoutedEventArgs e)
        {
            var position = Mouse.GetPosition(this);
            using (var form = new ProcurarItem(position))
            {
                form.Owner = Window.GetWindow(this);
                form.ShowDialog();
                if (form.DialogResult.Value && form.DialogResult.HasValue)
                {
                    dto.Item_Id = form.Item_Id;
                    txtItem.Text = Convert.ToInt32(dto.Item_Id).ToString("000000");
                    txtDescricao.Text = form.Item_Descricao;
                    txtUn.Text = form.Item_Unidade;
                }
            }
        }

        #endregion

        #region Loaded
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            txtDescricao.Focus();
        }
        #endregion

        #endregion

        private void BtnPesquisaFabricante_Click(object sender, RoutedEventArgs e)
        {
            if (txtDescricao.Text != "")
            {
                var position = Mouse.GetPosition(this);
                using (var form = new ProcurarFornecedor(position))
                {
                    form.Owner = Window.GetWindow(this);
                    form.ShowDialog();
                    if (form.DialogResult.Value && form.DialogResult.HasValue)
                    {
                        dto.Fornecedor_Id = form.Fornecedor_Id;
                        dto.Fornecedor = form.Fornecedor_Descricao;
                        txtFabricante.Text = dto.Fornecedor;
                    }
                }
            }
        }

        private void Txt_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9,]+").IsMatch(e.Text);
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
