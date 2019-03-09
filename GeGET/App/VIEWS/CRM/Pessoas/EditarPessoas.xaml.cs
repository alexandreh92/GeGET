using System;
using System.Windows;
using System.Windows.Input;
using BLL;
using DTO;

namespace GeGET
{
    public partial class EditarPessoas : Window, IDisposable
    {
        #region Declarations
        public static string id;
        private string cliente_Id;
        PessoasDTO dto = new PessoasDTO();
        PessoasBLL bll = new PessoasBLL();
        EstadosBLL Estadosbll = new EstadosBLL();
        CidadesBLL Cidadesbll = new CidadesBLL();
        #endregion

        #region Initialize
        public EditarPessoas(PessoasDTO DTO)
        {
            InitializeComponent();
            txtNome.Text = DTO.Nome;
            txtEmail.Text = DTO.Email;
            txtRazao.Text = DTO.Rsocial;
            id = DTO.Id;
            cliente_Id = DTO.Cliente_Id;
            txtTelefone.Text = DTO.Telefone;
            txtCelular.Text = DTO.Celular;
            cmbFuncao.ItemsSource = bll.LoadFuncoes();
            cmbFuncao.DisplayMemberPath = "Descricao";
            cmbFuncao.SelectedValuePath = "Id";
            cmbFuncao.SelectedValue = Convert.ToInt32(DTO.Funcao_Id);
            cbxStatus.IsChecked = Convert.ToBoolean(DTO.Status_Id);
        }
        #endregion

        #region Events

        #region Clicks
        private void BtnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            dto.Id = id;
            dto.Cliente_Id = cliente_Id;
            dto.Email = txtEmail.Text.Replace("'", "''");
            dto.Celular = txtCelular.Text;
            dto.Telefone = txtTelefone.Text;
            dto.Nome = txtNome.Text.Replace("'", "''").ToUpper();
            dto.Funcao_Id = cmbFuncao.SelectedValue.ToString();
            dto.Status_Id = Convert.ToInt32(cbxStatus.IsChecked).ToString();
            bll.UpdatePessoas(dto);
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
                    txtNome.Text = form.new_Razao_Social;
                    cliente_Id = form.new_Cliente_Id;
                }
                else
                {

                }
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
