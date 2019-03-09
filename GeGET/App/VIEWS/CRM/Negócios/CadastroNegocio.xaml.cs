using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BLL;
using DTO;

namespace GeGET
{
    public partial class CadastroNegocio : UserControl
    {
        #region Declarations
        NegociosBLL bll = new NegociosBLL();
        NegociosDTO dto = new NegociosDTO();

        ClientesDTO Clientedto = new ClientesDTO();
        PrioridadeBLL Prioridadesbll = new PrioridadeBLL();
        ContatoBLL Contatobll = new ContatoBLL();
        ContatoDTO Contatodto = new ContatoDTO();
        VendedoresBLL Vendedoresbll = new VendedoresBLL();

        Helpers helpers = new Helpers();
        #endregion

        #region Initialize
        public CadastroNegocio()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods

        #region ClearControls
        private void ClearControls()
        {
           /* txtFantasia.Text = "";
            txtRazao.Text = " ";
            txtEndereco.Text = "";
            txtIE.Text = "";
            txtCNPJ.Text = "";
            txtTelefone.Text = "";
            cmbUF.SelectedIndex = -1;
            cmbCidade.SelectedIndex = -1;*/
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
           /* if (txtRazao.Text != "" && txtFantasia.Text != "" && txtEndereco.Text != "" && txtCNPJ.Text != "" && txtTelefone.Text != "" && cmbUF.SelectedIndex != -1 && cmbCidade.SelectedIndex != -1)
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
            }*/
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
        /*    Estadosdto.Id = Convert.ToInt32(cmbUF.SelectedValue);
            cmbCidade.ItemsSource = Cidadesbll.LoadCidades(Estadosdto);
            cmbCidade.DisplayMemberPath = "Cidade";
            cmbCidade.SelectedValuePath = "Id";*/
        }
        #endregion

        #region Loaded
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            txtRazao.Focus();
        }
        #endregion

        #endregion

        private void BtnPesquisaEstabelecimento_Click(object sender, RoutedEventArgs e)
        {
            if (txtRazao.Text != " " && txtRazao.Text != "")
            {
                var position = Mouse.GetPosition(this);
                using (var form = new ProcurarEstabelecimento(position, dto))
                {
                    form.ShowDialog();
                    if (form.DialogResult.Value && form.DialogResult.HasValue)
                    {
                        txtEstabelecimento.Text = form.CNPJ;
                        txtEndereco.Text = form.Endereco;
                        txtCidade.Text = form.Cidade;
                        txtUF.Text = form.UF;
                        dto.Estabelecimento_Id = form.Estabelecimento_Id;
                        cmbVendedor.ItemsSource = Vendedoresbll.LoadVendedores();
                        cmbVendedor.DisplayMemberPath = "Nome";
                        cmbVendedor.SelectedValuePath = "Id";
                        Contatodto.Id = Convert.ToInt32(dto.Cliente_Id);
                        cmbContato.ItemsSource = Contatobll.LoadContato(Contatodto);
                        cmbContato.DisplayMemberPath = "Nome";
                        cmbContato.SelectedValuePath = "Id";
                        cmbPrioridade.ItemsSource = Prioridadesbll.LoadPrioridades();
                        cmbPrioridade.DisplayMemberPath = "Descricao";
                        cmbPrioridade.SelectedValuePath = "Id";
                    }
                }
                
            }
        }
    }
}
