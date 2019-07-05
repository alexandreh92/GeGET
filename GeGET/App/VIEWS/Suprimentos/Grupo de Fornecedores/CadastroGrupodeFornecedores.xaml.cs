using System;
using System.Windows;
using System.Windows.Controls;
using BLL;
using DTO;

namespace GeGET
{
    public partial class CadastroGrupodeFornecedores : UserControl, IDisposable
    {
        #region Declarations
        bool disposed = false;
        CategoriaClienteBLL Categoriabll = new CategoriaClienteBLL();
        GrupoFornecedoresBLL bll = new GrupoFornecedoresBLL();
        GrupoFornecedoresDTO dto = new GrupoFornecedoresDTO();
        Helpers helpers = new Helpers();
        #endregion

        #region Initialize
        public CadastroGrupodeFornecedores()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods

        #region ClearControls
        private void ClearControls()
        {
            txtDescricao.Text = "";
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
            if (txtDescricao.Text != "")
            {
                var result = CustomOKCancelMessageBox.Show("Deseja mesmo cadastrar este Grupo de Fornecedor?", "Atenção!", Window.GetWindow(this));
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    dto.Descricao = txtDescricao.Text.ToUpper().TrimStart(' ').Replace("'", "''");
                    if (bll.Cadastrar(dto))
                    {
                        ClearControls();
                        CustomOKCancelMessageBox.Show("Grupo de Fornecedor cadastrado com sucesso!", "Sucesso!", Window.GetWindow(this));
                    }
                }
            }
            else
            {
                CustomOKCancelMessageBox.Show("Campos não podem estar em branco.", "Atenção!", Window.GetWindow(this));
            }
        }
        #endregion

        #endregion

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
                Categoriabll.Dispose();
                bll.Dispose();
            }
            disposed = true;
        }

        #endregion
    }
}
