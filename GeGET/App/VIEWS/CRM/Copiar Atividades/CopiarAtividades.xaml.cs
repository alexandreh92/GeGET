using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BLL;
using DTO;
using System.Linq;
using System.Windows.Controls.Primitives;

namespace GeGET
{
    public partial class CopiarAtividades : UserControl, IDisposable
    {
        #region Declarations
        Helpers helpers = new Helpers();
        LoginDTO login = new LoginDTO();
        CadastroAtividadesDTO dto = new CadastroAtividadesDTO();
        CadastroAtividadeBLL bll = new CadastroAtividadeBLL();
        DescricaoAtividadesBLL descricaoAtividadesBLL = new DescricaoAtividadesBLL();
        DescricaoAtividadesDTO descricaoAtividadesDTO = new DescricaoAtividadesDTO();
        AtividadeCadastradaDTO atividadeCadastradaDTO = new AtividadeCadastradaDTO();
        ManualResetEvent syncEvent = new ManualResetEvent(false);
        bool disposed = false;
        #endregion

        #region Initialize
        public CopiarAtividades()
        {
            InitializeComponent();
        }
        #endregion


        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            helpers.Close();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            helpers.OpenBack(false);
        }

        private void BtnPesquisa_Click(object sender, RoutedEventArgs e)
        {
            var position = Mouse.GetPosition(this);
            using (var form = new ProcurarNegocio(position))
            {
                form.Owner = Window.GetWindow(this);
                form.ShowDialog();
                if (form.DialogResult.Value && form.DialogResult.HasValue)
                {
                    dto.Id = Convert.ToInt32(form.Negocio_Id);
                    
                    dto = bll.LoadOrcamento(dto).First();

                    txtNumero.Text = dto.Numero.ToUpper();
                    txtCliente.Text = dto.Razao_Social;
                    txtNumero.Text = dto.Numero.ToUpper();
                    txtCliente.Text = dto.Razao_Social;
                    txtCNPJ.Text = dto.Cnpj;
                    txtStatus.Text = dto.Status_Descricao;
                    txtVersao.Text = Convert.ToInt32(dto.Versao_Id).ToString("00");
                    txtEndereco.Text = dto.Endereco;
                    txtCidadeEstado.Text = dto.CidadeEstado;
                    txtDescricao.Text = dto.Descricao;
                    txtVendedor.Text = dto.Vendedor;
                    txtResponsavel.Text = dto.Contato_Nome;
                    descricaoAtividadesDTO.Disciplina_Id = 100;
                    InitializeComponents();
                }
            }
        }

        private void LoadAtividadesCadastradas()
        {

        }

        private void LoadCombobox()
        {

        }

        private void InitializeComponents()
        {
            
        }

        private void BtnEletrica_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnPaineis_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnMecanica_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCadastrar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CbxStatus_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void TxtDescricaoAtividade_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

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
                syncEvent.Dispose();
                bll.Dispose();
                descricaoAtividadesBLL.Dispose();
            }
            disposed = true;
        }

        #endregion
    }
}
