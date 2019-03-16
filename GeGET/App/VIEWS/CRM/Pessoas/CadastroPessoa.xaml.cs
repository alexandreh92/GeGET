using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BLL;
using DTO;

namespace GeGET
{
    public partial class CadastroPessoa : UserControl
    {
        #region Declarations

        PessoasBLL bll = new PessoasBLL();
        PessoasDTO dto = new PessoasDTO();
        Helpers helpers = new Helpers();
        #endregion

        #region Initialize
        public CadastroPessoa()
        {
            InitializeComponent();
            cmbCategoria.ItemsSource = bll.LoadFuncoes();
            cmbCategoria.DisplayMemberPath = "Descricao";
            cmbCategoria.SelectedValuePath = "Id";
        }
        #endregion

        #region Methods

        #region ClearControls
        private void ClearControls()
        {
            txtRazao.Text = " ";
            txtNome.Text = " ";
            txtTelefone.Text = "";
            txtCelular.Text = "";
            txtEmail.Text = "";
            txtTelefone.Text = "";
            cmbCategoria.SelectedIndex = -1;
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
            if (txtRazao.Text != "" && txtNome.Text != "" && txtEmail.Text != "" && txtTelefone.Text != "" && txtCelular.Text != "" && cmbCategoria.SelectedIndex != -1)
            {
                var result = CustomOKCancelMessageBox.Show("Deseja mesmo cadastrar este estabelecimento?", "Atenção!");
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    dto.Nome = txtNome.Text.Replace("'", "''").ToUpper();
                    dto.Email = txtEmail.Text.Replace("'", "''").ToUpper();
                    dto.Telefone = txtTelefone.Text.Replace("'", "''").ToUpper();
                    dto.Celular = txtCelular.Text.Replace("'", "''").ToUpper();
                    dto.Funcao_Id = cmbCategoria.SelectedValue.ToString();
                    if (bll.CreatePessoa(dto))
                    {
                        ClearControls();
                        CustomOKMessageBox.Show("Estabelecimento cadastrado com sucesso!", "Sucesso!");
                    }
                }
            }
            else
            {
                CustomOKMessageBox.Show("Campos não podem estar em branco.", "Atenção!");
            }
        }

        private void BtnPesquisa_Click(object sender, RoutedEventArgs e)
        {
            var position = Mouse.GetPosition(this);
            BlackScreen bs = new BlackScreen();
            bs.Show();
            using (var form = new ProcurarCliente(position))
            {
                form.ShowDialog();
                if (form.DialogResult.Value && form.DialogResult.HasValue)
                {
                    txtRazao.Text = form.new_Razao_Social;
                    dto.Cliente_Id = form.new_Cliente_Id;
                }
            }
            bs.Close();
        }

        #endregion

        #region Loaded
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            txtRazao.Focus();
        }
        #endregion

        #endregion
    }
}
