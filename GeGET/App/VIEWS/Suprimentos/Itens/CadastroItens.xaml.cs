using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BLL;
using DTO;

namespace GeGET
{
    public partial class CadastroItens : UserControl
    {
        #region Declarations
        ItensBLL bll = new ItensBLL();
        ItensDTO dto = new ItensDTO();
        UnidadeBLL unidadeBLL = new UnidadeBLL();
        Helpers helpers = new Helpers();
        #endregion

        #region Initialize
        public CadastroItens()
        {
            InitializeComponent();
            cmbUn.ItemsSource = unidadeBLL.LoadUnidades();
            cmbUn.SelectedValuePath = "Id";
            cmbUn.DisplayMemberPath = "Descricao";
        }
        #endregion

        #region Methods

        #region ClearControls
        private void ClearControls()
        {
            txtGrupo.Text = " ";
            txtDescricao.Text = "";
            txtDescricaoGrupo.Text = "";
            cmbUn.SelectedIndex = -1;
            cbxStatus.IsChecked = false;
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
            if (txtGrupo.Text != "" && txtDescricao.Text != "" && cmbUn.SelectedIndex != -1)
            {
                var result = CustomOKCancelMessageBox.Show("Deseja mesmo cadastrar este item?", "Atenção!", Window.GetWindow(this));
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    dto.Descricao = txtDescricao.Text.Replace("'", "''").TrimStart(' ').ToUpper();
                    dto.Un = cmbUn.SelectedValue.ToString();
                    dto.Mobra = Convert.ToInt32(cbxStatus.IsChecked);
                    if (bll.CadastrarItem(dto))
                    {
                        ClearControls();
                        CustomOKMessageBox.Show("Fornecedor cadastrado com sucesso!", "Sucesso!", Window.GetWindow(this));
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
            BlackScreen bs = new BlackScreen();
            using (var form = new ProcurarGrupoItens(position))
            {
                form.Owner = Window.GetWindow(this);
                form.ShowDialog();
                if (form.DialogResult.Value && form.DialogResult.HasValue)
                {
                    dto.Grupo_Id = Convert.ToInt32(form.Grupo_Id);
                    txtGrupo.Text = dto.Grupo_Id.ToString();
                    txtDescricaoGrupo.Text = form.Grupo_Descricao;
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
    }
}
