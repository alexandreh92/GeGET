using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BLL;
using DTO;

namespace GeGET
{
    public partial class EditarEstabelecimento : Window, IDisposable
    {
        #region Declarations
        bool disposed = false;
        bool initializing;
        public static string id;
        private string cliente_Id;
        EstabelecimentosDTO dto = new EstabelecimentosDTO();
        EstabelecimentosBLL bll = new EstabelecimentosBLL();
        EstadosBLL Estadosbll = new EstadosBLL();
        EstadoDTO Estadosdto = new EstadoDTO();
        CidadesBLL Cidadesbll = new CidadesBLL();
        string CidadesID;
        #endregion

        #region Initialize
        public EditarEstabelecimento(EstabelecimentosDTO DTO)
        {
            InitializeComponent();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            initializing = true;
            id = DTO.Id;
            cliente_Id = DTO.Cliente_Id;
            txtRazao.Text = DTO.Razao_Social;
            txtFantasia.Text = DTO.Nome_Fantasia;
            txtEndereco.Text = DTO.Endereco;
            txtDescricao.Text = DTO.Descricao;
            LoadEstados();
            cmbUF.DisplayMemberPath = "Uf";
            cmbUF.SelectedValuePath = "Id";
            cmbUF.SelectedValue = Convert.ToInt32(DTO.UF_Id);
            Estadosdto.Id = Convert.ToInt32(cmbUF.SelectedValue);
            CidadesID = DTO.Cidade_Id;
            txtCNPJ.Text = DTO.Cnpj;
            txtIE.Text = DTO.Ie;
            txtTelefone.Text = DTO.Telefone;
            cbxStatus.IsChecked = Convert.ToBoolean(DTO.Status);
        }
        #endregion

        #region Methods

        private async void LoadEstados()
        {
            var listaEstados = new List<EstadoDTO>();
            await Task.Run(() =>
            {
                listaEstados = Estadosbll.LoadEstados();
            });
            cmbUF.ItemsSource = listaEstados;
        }


        private async void LoadCidades()
        {
            var listaCidades = new List<CidadesDTO>();
            await Task.Run(() =>
            {
                listaCidades = Cidadesbll.LoadCidades(Estadosdto);
            });
            cmbCidade.ItemsSource = listaCidades;
        }
        #endregion

        #region Events

        #region Clicks
        private async void BtnConfirmar_Click(object sender, RoutedEventArgs e)
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
            WaitBox wb = new WaitBox
            {
                Owner = Window.GetWindow(this)
            };
            wb.Show();
            await Task.Run(() => 
            {
                bll.EditarEstabelecimento(dto);
            });
            wb.Close();
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
            }
        }
        #endregion

        #region SelectionChanged
        private async void CmbUF_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cmbUF.SelectedValue != null)
            {
                Estadosdto.Id = Convert.ToInt32(cmbUF.SelectedValue);
                var listaCidades = new List<CidadesDTO>();
                await Task.Run(() =>
                {
                    listaCidades = Cidadesbll.LoadCidades(Estadosdto);
                });
                cmbCidade.ItemsSource = listaCidades;
                cmbCidade.DisplayMemberPath = "Cidade";
                cmbCidade.SelectedValuePath = "Id";
            }
            if (initializing)
            {
                cmbCidade.SelectedValue = Convert.ToInt32(CidadesID);
                initializing = false;
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
                Cidadesbll.Dispose();
                bll.Dispose();
                Estadosbll.Dispose();
            }
            disposed = true;
        }

        #endregion
    }
}
