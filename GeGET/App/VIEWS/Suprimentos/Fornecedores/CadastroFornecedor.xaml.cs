using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BLL;
using DTO;

namespace GeGET
{
    public partial class CadastroFornecedor : UserControl
    {
        #region Declarations
        CidadesBLL Cidadesbll = new CidadesBLL();
        EstadosBLL Estadosbll = new EstadosBLL();
        EstadoDTO Estadosdto = new EstadoDTO();
        FornecedoresBLL bll = new FornecedoresBLL();
        FornecedoresDTO dto = new FornecedoresDTO();
        Helpers helpers = new Helpers();
        #endregion

        #region Initialize
        public CadastroFornecedor()
        {
            InitializeComponent();
            cmbUF.ItemsSource = Estadosbll.LoadEstados();
            cmbUF.DisplayMemberPath = "Uf";
            cmbUF.SelectedValuePath = "Id";
        }
        #endregion

        #region Methods

        #region ClearControls
        private void ClearControls()
        {
            txtDescricaoGrupo.Text = "";
            txtGrupo.Text = " ";
            txtFantasia.Text = "";
            txtRazao.Text = " ";
            txtEndereco.Text = "";
            txtIE.Text = "";
            txtCNPJ.Text = "";
            txtTelefone.Text = "";
            txtEmail.Text = "";
            cmbUF.SelectedIndex = -1;
            cmbCidade.SelectedIndex = -1;
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
            if (txtGrupo.Text != "" && txtRazao.Text != "" && txtFantasia.Text != "" && txtEndereco.Text != "" && txtCNPJ.Text != "" && txtTelefone.Text != "" && cmbUF.SelectedIndex != -1 && cmbCidade.SelectedIndex != -1)
            {
                var result = CustomOKCancelMessageBox.Show("Deseja mesmo cadastrar este fornecedor?", "Atenção!", Window.GetWindow(this));
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    dto.Razao_Social = txtRazao.Text.Replace("'", "''").ToUpper();
                    dto.Nome_Fantasia = txtFantasia.Text.Replace("'", "''").ToUpper();
                    dto.Endereco = txtEndereco.Text.Replace("'", "''").ToUpper();
                    dto.Cnpj = txtCNPJ.Text.Replace("'", "''").ToUpper();
                    dto.Ie = txtIE.Text.Replace("'", "''").ToUpper();
                    dto.Telefone = txtTelefone.Text.Replace("'", "''").ToUpper();
                    dto.Estado_Id = Convert.ToInt32(cmbUF.SelectedValue);
                    dto.Cidade_Id = Convert.ToInt32(cmbCidade.SelectedValue);
                    dto.Email = txtEmail.Text.Replace("'","''").TrimStart(' ');
                    if (bll.CadastrarFornecedor(dto))
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
            using (var form = new ProcurarGrupoFornecedor(position))
            {
                form.Owner = Window.GetWindow(this);
                form.ShowDialog();
                if (form.DialogResult.Value && form.DialogResult.HasValue)
                {
                    dto.Grupo_Forn_id = Convert.ToInt32(form.Grupo_Id);
                    txtGrupo.Text = dto.Grupo_Forn_id.ToString();
                    txtDescricaoGrupo.Text = form.Grupo_Descricao;
                }
            }
        }

        #endregion

        #region Selection Changed
        private void CmbUF_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Estadosdto.Id = Convert.ToInt32(cmbUF.SelectedValue);
            cmbCidade.ItemsSource = Cidadesbll.LoadCidades(Estadosdto);
            cmbCidade.DisplayMemberPath = "Cidade";
            cmbCidade.SelectedValuePath = "Id";
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
