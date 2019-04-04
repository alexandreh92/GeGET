using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using BLL;
using DTO;

namespace GeGET
{
    public partial class GerenciarOrcamento : UserControl
    {

        #region Declarations
        Helpers helpers = new Helpers();
        LoginDTO login = new LoginDTO();
        OrcamentistasDTO orcamentistasDTO = new OrcamentistasDTO();
        NegociosDTO dto = new NegociosDTO();
        NegociosBLL bll = new NegociosBLL();
        public ObservableCollection<OrcamentistasDTO> orcamentistas = new ObservableCollection<OrcamentistasDTO>();
        ManualResetEvent syncEvent = new ManualResetEvent(false);
        WaitBox wb;
        #endregion

        #region Initialize
        public GerenciarOrcamento()
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
                    dto.Id = form.Negocio_Id;
                    var negocios = bll.LoadGerenciarOrcamento(dto);
                    if (negocios.Count > 0)
                    {
                        new Thread(() =>
                        {
                            Dispatcher.Invoke(new Action(() =>
                            {
                                wb = new WaitBox();
                                wb.Show();
                            }));
                            syncEvent.Set();

                            
                            foreach (NegociosDTO item in negocios)
                            {
                                Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
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
                                   InitializeComponents();
                                   orcamentistas = bll.LoadOrcamentistaCadastrado(dto);
                                   pnlOrcamentistas.ItemsSource = orcamentistas;

                               }));
                            }
                        }).Start();
                        
                        Thread t1 = new Thread(WaitBoxLoad);
                        t1.Start();
                        
                    }
                    else
                    {
                        CustomOKMessageBox.Show("Ocorreu algum erro. Por favor, contate um desenvolvedor.", "Atenção!", Window.GetWindow(this));
                    }
                }
            }
        }

        private void WaitBoxLoad()
        {
            syncEvent.WaitOne();
            Dispatcher.Invoke(new Action(() =>
            {
                wb.Close();
            }));
        }

        private void InitializeComponents()
        {
            if (dto.Status_Id == 0 || dto.Status_Id == 5)
            {
                btnHabilitar.IsEnabled = false;
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
                btnHabilitar.IsEnabled = true;
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
                btnHabilitar.IsEnabled = false;
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
                btnHabilitar.IsEnabled = false;
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
                btnHabilitar.IsEnabled = false;
                btnAterarVersao.IsEnabled = true;
                btnNovaVersão.IsEnabled = true;
                btnEnviar.IsEnabled = false;
                btnNegociacao.IsEnabled = false;
                btnFechar.IsEnabled = true;
                btnCancelar.IsEnabled = true;
                if (login.Nivel >= 5) { btnOrcamentista.IsEnabled = true; }
                else { btnOrcamentista.IsEnabled = false; }
            }
            gridOptions.Visibility = Visibility.Visible;
        }

        private void BtnOrcamentista_Click(object sender, RoutedEventArgs e)
        {
            BlackScreen bs = new BlackScreen();
            var position = Mouse.GetPosition(this);
            using (var form = new ProcurarOrcamentista(position, dto))
            {
                bs.Show();
                form.ShowDialog();
                if (form.DialogResult.Value && form.DialogResult.HasValue)
                {
                    orcamentistasDTO.Id = form.Id;
                    orcamentistasDTO.Login = form.Login;
                    orcamentistasDTO.Nome_Simples = form.Nome_Simples;
                    orcamentistasDTO.Negocio_Id = dto.Id;
                    bll.InserirOracamentista(orcamentistasDTO);
                    orcamentistas = bll.LoadOrcamentistaCadastrado(dto);
                    pnlOrcamentistas.ItemsSource = orcamentistas;
                }
            }
            bs.Close();
        }

        private void ButtonsDemoChip_DeleteClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as MaterialDesignThemes.Wpf.Chip;
            int index = pnlOrcamentistas.Items.IndexOf(btn.DataContext);
            orcamentistasDTO.Id = ((OrcamentistasDTO)pnlOrcamentistas.Items[index]).Id;
            orcamentistasDTO.Negocio_Id = dto.Id;
            orcamentistasDTO.Nome_Simples = ((OrcamentistasDTO)pnlOrcamentistas.Items[index]).Nome_Simples;
            bll.RemoverOrcamentista(orcamentistasDTO);
            orcamentistas = bll.LoadOrcamentistaCadastrado(dto);
            pnlOrcamentistas.ItemsSource = orcamentistas;
        }

        private void BtnHabilitar_Click(object sender, RoutedEventArgs e)
        {
            if (pnlOrcamentistas.Items.Count > 0)
            {
                dto.Status_Descricao = "NA FILA";
                dto.Status_Id = 2;
                bll.HabilitarOrcamento(dto);
                dto.Status_Descricao = "EM ANDAMENTO";
                txtStatus.Text = dto.Status_Descricao;
                InitializeComponents();
            }
            else
            {
                CustomOKMessageBox.Show("Para habilitar o orçamento você deve cadastrar ao menos um orçamentista.","Atenção!", Window.GetWindow(this));
            }
        }

        private void BtnEnviar_Click(object sender, RoutedEventArgs e)
        {
            var result = CustomOKCancelMessageBox.Show("Enviar o orçamento para o cliente desabilitará esta versão para alteraçoes.\nDeseja mesmo fazer isso?","Atenção!", Window.GetWindow(this));
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                using (var form = new ValorEnviadoOrcamento())
                {
                    form.ShowDialog();
                    if (form.DialogResult.Value && form.DialogResult.HasValue)
                    {
                        dto.Valor_Enviado = form.Valor;
                        dto.Status_Descricao = "EM ANDAMENTO";
                        bll.EnviarCliente(dto);
                        dto.Status_Descricao = "ENVIADO AO CLIENTE";
                        txtStatus.Text = dto.Status_Descricao;
                        dto.Status_Id = 3;
                        InitializeComponents();
                    }
                }    
            }
        }

        private void BtnNegociacao_Click(object sender, RoutedEventArgs e)
        {
            var result = CustomOKCancelMessageBox.Show("Você deseja mesmo marcar este orçamento como em negociação?","Atenção!", Window.GetWindow(this));
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                dto.Status_Descricao = "ENVIADO AO CLIENTE";
                bll.EmNegociacao(dto); //adicionar na query para atualizar a coluna negociado para 1
                dto.Status_Descricao = "EM NEGOCIAÇÃO";
                txtStatus.Text = dto.Status_Descricao;
                dto.Status_Id = 4;
                InitializeComponents();
            }
        }

        private void BtnFechar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
