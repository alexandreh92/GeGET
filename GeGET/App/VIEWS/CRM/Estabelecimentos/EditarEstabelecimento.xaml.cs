using System;
using System.Windows;
using System.Windows.Input;
using BLL;
using DTO;

namespace GeGET
{
    public partial class EditarEstabelecimento : Window, IDisposable
    {
        #region Declarations
        public static string id;
        private string cliente_Id;
        EstabelecimentosDTO dto = new EstabelecimentosDTO();
        EstabelecimentosBLL bll = new EstabelecimentosBLL();
        EstadosBLL Estadosbll = new EstadosBLL();
        EstadoDTO Estadosdto = new EstadoDTO();
        CidadesBLL Cidadesbll = new CidadesBLL();
        #endregion

        #region Initialize
        public EditarEstabelecimento(EstabelecimentosDTO DTO)
        {
            InitializeComponent();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            id = DTO.Id;
            cliente_Id = DTO.Cliente_Id;
            txtRazao.Text = DTO.Razao_Social;
            txtFantasia.Text = DTO.Nome_Fantasia;
            txtEndereco.Text = DTO.Endereco;
            txtDescricao.Text = DTO.Descricao;
            cmbUF.ItemsSource = Estadosbll.LoadEstados();
            cmbUF.DisplayMemberPath = "Uf";
            cmbUF.SelectedValuePath = "Id";
            cmbUF.SelectedValue = Convert.ToInt32(DTO.UF_Id);
            Estadosdto.Id = Convert.ToInt32(cmbUF.SelectedValue);
            cmbCidade.ItemsSource = Cidadesbll.LoadCidades(Estadosdto);
            cmbCidade.DisplayMemberPath = "Cidade";
            cmbCidade.SelectedValuePath = "Id";
            cmbCidade.SelectedValue = Convert.ToInt32(DTO.Cidade_Id);
            txtCNPJ.Text = DTO.Cnpj;
            txtIE.Text = DTO.Ie;
            txtTelefone.Text = DTO.Telefone;
            cbxStatus.IsChecked = Convert.ToBoolean(DTO.Status);
        }
        #endregion

        #region Events

        #region Clicks
        private void BtnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            dto.Id = id;
            dto.Razao_Social = txtRazao.Text.Replace("'", "''").ToUpper();
            dto.Nome_Fantasia = txtFantasia.Text.Replace("'", "''").ToUpper();
            dto.Descricao = txtDescricao.Text.Replace("'", "''").ToUpper();
            dto.Cnpj = txtCNPJ.Text;
            dto.Endereco = txtEndereco.Text.Replace("'", "''");
            dto.Ie = txtIE.Text.Replace("'", "''");
            dto.Telefone = txtTelefone.Text;
            dto.Status = Convert.ToInt32(cbxStatus.IsChecked);
            dto.Cliente_Id = cliente_Id;
            dto.UF_Id = cmbUF.SelectedValue.ToString();
            dto.Cidade_Id = cmbCidade.SelectedValue.ToString();
            bll.EditarEstabelecimento(dto);
            DialogResult = true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
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
                    cliente_Id = form.new_Cliente_Id;
                }
                else
                {

                }
            }
        }
        #endregion

        #region SelectionChanged
        private void CmbUF_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cmbUF.SelectedValue != null)
            {
                Estadosdto.Id = Convert.ToInt32(cmbUF.SelectedValue);
                cmbCidade.ItemsSource = Cidadesbll.LoadCidades(Estadosdto);
                cmbCidade.DisplayMemberPath = "Cidade";
                cmbCidade.SelectedValuePath = "Id";
            }

        }
        #endregion

        #endregion

        #region IDisposable
        void IDisposable.Dispose()
        {
        }
        #endregion
    }
}
