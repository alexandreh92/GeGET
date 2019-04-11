using System;
using System.Windows;
using System.Windows.Controls;
using BLL;
using DTO;

namespace GeGET
{
    public partial class CadastroGrupodeItens : UserControl
    {
        #region Declarations
        CategoriaClienteBLL Categoriabll = new CategoriaClienteBLL();
        GrupoItensBLL bll = new GrupoItensBLL();
        GrupoItensDTO dto = new GrupoItensDTO();
        Helpers helpers = new Helpers();
        #endregion

        #region Initialize
        public CadastroGrupodeItens()
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
                var result = CustomOKCancelMessageBox.Show("Deseja mesmo cadastrar este Grupo de Item?", "Atenção!", Window.GetWindow(this));
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    dto.Descricao = txtDescricao.Text.ToUpper().TrimStart(' ').Replace("'", "''");
                    if (bll.Cadastrar(dto))
                    {
                        ClearControls();
                        CustomOKCancelMessageBox.Show("Grupo de Item cadastrado com sucesso!", "Sucesso!", Window.GetWindow(this));
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
    }
}
