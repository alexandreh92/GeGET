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
            txtFantasia.Text = "";
            txtRazao.Text = " ";
            txtEndereco.Text = "";
            txtEstabelecimento.Text = "";
            txtCidade.Text = "";
            txtUF.Text = "";
            dtPicker.Text = DateTime.Today.ToString("dd/MM/yyyy");
            txtNome.Text = "";
            txtAnotacoes.Text = "";
            cmbContato.SelectedIndex = -1;
            cmbPrioridade.SelectedIndex = -1;
            cmbVendedor.SelectedIndex = -1;
        }



        #endregion

        #region Load Comboboxes
        private void LoadComboboxes()
        {
            Contatodto.Id = Convert.ToInt32(dto.Cliente_Id);
            cmbContato.ItemsSource = Contatobll.LoadContatoFromNegocios(dto);
            cmbContato.DisplayMemberPath = "Nome";
            cmbContato.SelectedValuePath = "Id";
            cmbPrioridade.ItemsSource = Prioridadesbll.LoadPrioridades();
            cmbPrioridade.DisplayMemberPath = "Descricao";
            cmbPrioridade.SelectedValuePath = "Id";
            cmbVendedor.ItemsSource = Vendedoresbll.LoadVendedores();
            cmbVendedor.DisplayMemberPath = "Nome";
            cmbVendedor.SelectedValuePath = "Id";
        }
        #endregion

        #region GetValues

        private void GetValues()
        {
            dto.Anotacoes = txtAnotacoes.Text.Replace("'","''").ToUpper();
            dto.Descricao = txtNome.Text.Replace("'", "''").ToUpper();
            dto.Prazo = Convert.ToDateTime(dtPicker.Text).ToString("yyyy/MM/dd");
            dto.Vendedor_Id = cmbVendedor.SelectedValue.ToString();
            dto.Contato_Id = cmbContato.SelectedValue.ToString();
            dto.Prioridade_Id = cmbPrioridade.SelectedValue.ToString();
            dto.Prioridade_Descricao = cmbPrioridade.Text;
            dto.Contato_Nome = cmbContato.Text;
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
            if (txtRazao.Text != "" && txtEstabelecimento.Text != "" && txtNome.Text != "" && dtPicker.Text != "" && cmbContato.SelectedIndex != -1 && cmbPrioridade.SelectedIndex != -1 && cmbVendedor.SelectedIndex != -1)
            {
                var result = CustomOKCancelMessageBox.Show("Deseja mesmo cadastrar este estabelecimento?", "Atenção!");
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    GetValues();
                    if (bll.CreateNegocios(dto))
                    {
                        var negocio = bll.RetNegocioId();
                        ClearControls();
                        CustomOKMessageBox.Show("Negocio P" + Convert.ToInt32(negocio.Id).ToString("0000") + " cadastrado com sucesso!", "Sucesso!");
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
                    txtFantasia.Text = form.new_Nome_Fantasia;
                    txtEstabelecimento.Text = " ";
                    dto.Cliente_Id = form.new_Cliente_Id;
                }
            }
            bs.Close();
        }

        private void BtnPesquisaEstabelecimento_Click(object sender, RoutedEventArgs e)
        {
            if (txtRazao.Text != " " && txtRazao.Text != "")
            {
                var position = Mouse.GetPosition(this);
                BlackScreen bs = new BlackScreen();
                bs.Show();
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
                        LoadComboboxes();
                    }
                }
                bs.Close();
            }
        }

        #endregion

        #region Loaded
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            txtRazao.Focus();
        }
        #endregion

        #endregion

        #region Selected Data Changed
        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Convert.ToDateTime(dtPicker.Text) < DateTime.Today)
            {
                dtPicker.Text = DateTime.Today.ToString("dd/MM/yyyy");
            }
        }
        #endregion
    }
}
