using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BLL;
using DTO;

namespace GeGET
{
    public partial class DashboardNegocios : UserControl
    {

        #region Declarations
        Helpers helpers = new Helpers();
        LoginDTO login = new LoginDTO();
        NegociosDTO dto = new NegociosDTO();
        NegociosBLL bll = new NegociosBLL();
        #endregion

        #region Initialize
        public DashboardNegocios()
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
            BlackScreen bs = new BlackScreen();
            var position = Mouse.GetPosition(this);
            using (var form = new ProcurarNegocio(position))
            {
                bs.Show();
                form.ShowDialog();
                if (form.DialogResult.Value && form.DialogResult.HasValue)
                {
                    dto.Id = form.Negocio_Id;
                    var negocios = bll.LoadGerenciarOrcamento(dto);
                    if (negocios.Count > 0)
                    {
                        foreach (NegociosDTO item in negocios)
                        {
                            dto.Id = item.Id;
                            dto.Status_Id = item.Status_Id;

                            txtNumero.Text = item.Numero.ToUpper();
                            txtCliente.Text = item.Razao_Social;
                            txtCNPJ.Text = item.Cnpj;
                            txtStatus.Text = item.Status_Descricao;
                            txtVersao.Text = Convert.ToInt32(item.Versao_Id).ToString("00");
                            txtEndereco.Text = item.Endereco;
                            txtCidadeEstado.Text = item.CidadeEstado;
                            txtDescricao.Text = item.Descricao;
                            txtVendedor.Text = item.Vendedor;
                            txtResponsavel.Text = item.Contato_Nome;
                            if (item.Anotacoes != "") { txtAnotacoes.Text = item.Anotacoes; } else { txtAnotacoes.Text = "Não existe informações adicionais para este negócio."; }
                            txtPrazo.Text = Convert.ToDateTime(item.Prazo).ToString("dd/MM/yyyy");
                        }
                        InitializeComponents();
                    }
                    else
                    {
                        CustomOKMessageBox.Show("Ocorreu algum erro. Por favor, contate um desenvolvedor.", "Atenção!");
                    }
                }
            }
            bs.Close();
        }

        private void InitializeComponents()
        {
            if (dto.Status_Id == 0 || dto.Status_Id == 5)
            {
                btnAterarVersao.IsEnabled = false;
                btnNovaVersão.IsEnabled = false;
                btnEnviar.IsEnabled = false;
                btnNegociacao.IsEnabled = false;
                btnFechar.IsEnabled = false;
                btnCancelar.IsEnabled = false;
                btnOrcamentista.IsEnabled = false;
            }
            else if (dto.Status_Id == 1)
            {
                btnAterarVersao.IsEnabled = false;
                btnNovaVersão.IsEnabled = false;
                btnEnviar.IsEnabled = false;
                btnNegociacao.IsEnabled = false;
                btnFechar.IsEnabled = false;
                btnCancelar.IsEnabled = true;
                if (login.Nivel >= 5) { btnOrcamentista.IsEnabled = true; }
                else { btnOrcamentista.IsEnabled = false; }
            }
            else if (dto.Status_Id == 2)
            {
                btnAterarVersao.IsEnabled = true;
                btnNovaVersão.IsEnabled = true;
                btnEnviar.IsEnabled = true;
                btnNegociacao.IsEnabled = false;
                btnFechar.IsEnabled = false;
                btnCancelar.IsEnabled = true;
                if (login.Nivel >= 5) { btnOrcamentista.IsEnabled = true; }
                else { btnOrcamentista.IsEnabled = false; }
            }
            else if (dto.Status_Id == 3)
            {
                btnAterarVersao.IsEnabled = true;
                btnNovaVersão.IsEnabled = true;
                btnEnviar.IsEnabled = false;
                btnNegociacao.IsEnabled = true;
                btnFechar.IsEnabled = false;
                btnCancelar.IsEnabled = true;
                if (login.Nivel >= 5) { btnOrcamentista.IsEnabled = true; }
                else { btnOrcamentista.IsEnabled = false; }
            }
            else if(dto.Status_Id == 4)
            {
                btnAterarVersao.IsEnabled = true;
                btnNovaVersão.IsEnabled = true;
                btnEnviar.IsEnabled = false;
                btnNegociacao.IsEnabled = false;
                btnFechar.IsEnabled = true;
                btnCancelar.IsEnabled = true;
                if (login.Nivel >= 5) { btnOrcamentista.IsEnabled = true; }
                else { btnOrcamentista.IsEnabled = false; }
            }
        }

        private void BtnOrcamentista_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
