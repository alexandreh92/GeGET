using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BLL;
using DTO;

namespace GeGET
{
    public partial class CadastroEstabelecimento : UserControl
    {
        #region Declarations
        CidadesBLL Cidadesbll = new CidadesBLL();
        EstadosBLL Estadosbll = new EstadosBLL();
        EstadoDTO Estadosdto = new EstadoDTO();
        EstabelecimentosBLL bll = new EstabelecimentosBLL();
        EstabelecimentosDTO dto = new EstabelecimentosDTO();
        Helpers helpers = new Helpers();
        #endregion

        #region Initialize
        public CadastroEstabelecimento()
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
            txtFantasia.Text = "";
            txtRazao.Text = " ";
            txtEndereco.Text = "";
            txtIE.Text = "";
            txtCNPJ.Text = "";
            txtTelefone.Text = "";
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
            if (txtRazao.Text != "" && txtFantasia.Text != "" && txtEndereco.Text != "" && txtCNPJ.Text != "" && txtTelefone.Text != "" && cmbUF.SelectedIndex != -1 && cmbCidade.SelectedIndex != -1)
            {
                var result = CustomOKCancelMessageBox.Show("Deseja mesmo cadastrar este estabelecimento?", "Atenção!");
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    dto.Razao_Social = txtRazao.Text.Replace("'", "''").ToUpper();
                    dto.Nome_Fantasia = txtFantasia.Text.Replace("'", "''").ToUpper();
                    dto.Endereco = txtEndereco.Text.Replace("'", "''").ToUpper();
                    dto.Cnpj = txtCNPJ.Text.Replace("'", "''").ToUpper();
                    dto.Ie = txtIE.Text.Replace("'", "''").ToUpper();
                    dto.Telefone = txtTelefone.Text.Replace("'", "''").ToUpper();
                    dto.UF_Id = cmbUF.SelectedValue.ToString();
                    dto.Cidade_Id = cmbCidade.SelectedValue.ToString();
                    if (bll.CreateEstabelecimento(dto))
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
            using (var form = new ProcurarCliente(position))
            {
                form.ShowDialog();
                if (form.DialogResult.Value && form.DialogResult.HasValue)
                {
                    txtRazao.Text = form.new_Razao_Social;
                    txtFantasia.Text = form.new_Nome_Fantasia;
                    dto.Cliente_Id = form.new_Cliente_Id;
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
