using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BLL;
using DTO;
using System.Linq;
using System.Threading.Tasks;

namespace GeGET
{
    public partial class GerenciarOrcamento : UserControl, IDisposable
    {

        #region Declarations
        bool disposed = false;
        Helpers helpers = new Helpers();
        LoginDTO login = new LoginDTO();
        OrcamentistasDTO orcamentistasDTO = new OrcamentistasDTO();
        NegociosDTO dto = new NegociosDTO();
        NegociosBLL bll = new NegociosBLL();
        public ObservableCollection<OrcamentistasDTO> orcamentistas = new ObservableCollection<OrcamentistasDTO>();
        ManualResetEvent syncEvent = new ManualResetEvent(false);
        VersaoOrcamentoDTO versaoOrcamentoDTO = new VersaoOrcamentoDTO();
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

        private async void BtnPesquisa_Click(object sender, RoutedEventArgs e)
        {
            var position = Mouse.GetPosition(this);
            using (var form = new ProcurarNegocio(position))
            {
                form.Owner = Window.GetWindow(this);
                form.ShowDialog();
                if (form.DialogResult.Value && form.DialogResult.HasValue)
                {
                    dto.Id = form.Negocio_Id;
                    WaitBox wb = new WaitBox
                    {
                        Owner = Window.GetWindow(this)
                    };
                    wb.Show();
                    await Task.Run(() =>
                    {
                        dto = bll.LoadGerenciarOrcamento(dto).First();
                    });
                    

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
                    if (dto.Anotacoes != "") { txtAnotacoes.Text = dto.Anotacoes; } else { txtAnotacoes.Text = "Não existe informações adicionais para este negócio."; }
                    txtPrazo.Text = Convert.ToDateTime(dto.Prazo).ToString("dd/MM/yyyy");
                    InitializeComponents();
                    orcamentistas = bll.LoadOrcamentistaCadastrado(dto);
                    pnlOrcamentistas.ItemsSource = orcamentistas;
                    wb.Close();
                }
            }
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

        private async void BtnOrcamentista_Click(object sender, RoutedEventArgs e)
        {
            BlackScreen bs = new BlackScreen();
            var position = Mouse.GetPosition(this);
            using (var form = new ProcurarOrcamentista(position, dto))
            {
                bs.Show();
                form.ShowDialog();
                if (form.DialogResult.Value && form.DialogResult.HasValue)
                {
                    WaitBox wb = new WaitBox
                    {
                        Owner = Window.GetWindow(this)
                    };
                    wb.Show();
                    orcamentistasDTO.Id = form.Id;
                    orcamentistasDTO.Login = form.Login;
                    orcamentistasDTO.Nome_Simples = form.Nome_Simples;
                    orcamentistasDTO.Negocio_Id = dto.Id;
                    await Task.Run(() =>
                    {
                        bll.InserirOracamentista(orcamentistasDTO);
                    });
                    orcamentistas = bll.LoadOrcamentistaCadastrado(dto);
                    pnlOrcamentistas.ItemsSource = orcamentistas;
                    wb.Close();
                }
            }
            bs.Close();
        }

        private async void ButtonsDemoChip_DeleteClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as MaterialDesignThemes.Wpf.Chip;
            int index = pnlOrcamentistas.Items.IndexOf(btn.DataContext);
            orcamentistasDTO.Id = ((OrcamentistasDTO)pnlOrcamentistas.Items[index]).Id;
            orcamentistasDTO.Negocio_Id = dto.Id;
            orcamentistasDTO.Nome_Simples = ((OrcamentistasDTO)pnlOrcamentistas.Items[index]).Nome_Simples;
            WaitBox wb = new WaitBox
            {
                Owner = Window.GetWindow(this)
            };
            wb.Show();
            await Task.Run(() =>
            {
                bll.RemoverOrcamentista(orcamentistasDTO);
            });
            orcamentistas = bll.LoadOrcamentistaCadastrado(dto);
            pnlOrcamentistas.ItemsSource = orcamentistas;
            wb.Close();
        }

        private async void BtnHabilitar_Click(object sender, RoutedEventArgs e)
        {
            if (pnlOrcamentistas.Items.Count > 0)
            {
                WaitBox wb = new WaitBox
                {
                    Owner = Window.GetWindow(this)
                };
                wb.Show();
                await Task.Run(() =>
                {
                    bll.HabilitarOrcamento(dto);
                });
                dto.Status_Id = 2;
                dto.Status_Descricao = "EM ANDAMENTO";
                txtStatus.Text = dto.Status_Descricao;
                wb.Close();
                InitializeComponents();
            }
            else
            {
                CustomOKMessageBox.Show("Para habilitar o orçamento você deve cadastrar ao menos um orçamentista.","Atenção!", Window.GetWindow(this));
            }
        }

        private async void BtnEnviar_Click(object sender, RoutedEventArgs e)
        {
            if (!await bll.HasPricelessItems(dto))
            {
                var result = CustomOKCancelMessageBox.Show("Enviar o orçamento para o cliente desabilitará esta versão para alteraçoes.\nDeseja mesmo fazer isso?", "Atenção!", Window.GetWindow(this));
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    using (var form = new ValorEnviadoOrcamento())
                    {
                        form.Owner = Window.GetWindow(this);
                        form.ShowDialog();
                        if (form.DialogResult.Value && form.DialogResult.HasValue)
                        {
                            dto.Valor_Enviado = form.Valor;
                            WaitBox wb = new WaitBox
                            {
                                Owner = Window.GetWindow(this)
                            };
                            wb.Show();
                            await Task.Run(() =>
                            {
                                bll.EnviarCliente(dto);
                            });
                            dto.Status_Descricao = "ENVIADO AO CLIENTE";
                            txtStatus.Text = dto.Status_Descricao;
                            dto.Status_Id = 3;
                            wb.Close();
                            InitializeComponents();
                        }
                    }
                }
            }
            else
            {
                CustomOKMessageBox.Show("Não é possivel enviar o orçamento. Existem preços zerados no mesmo, por favor revise.", "Atenção!", Window.GetWindow(this));
            }
        }

        private async void BtnNegociacao_Click(object sender, RoutedEventArgs e)
        {
            var result = CustomOKCancelMessageBox.Show("Você deseja mesmo marcar este orçamento como em negociação?","Atenção!", Window.GetWindow(this));
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                WaitBox wb = new WaitBox
                {
                    Owner = Window.GetWindow(this)
                };
                wb.Show();
                await Task.Run(() =>
                {
                    bll.EmNegociacao(dto);
                });
                dto.Status_Descricao = "EM NEGOCIAÇÃO";
                txtStatus.Text = dto.Status_Descricao;
                dto.Status_Id = 4;
                wb.Close();
                InitializeComponents();
            }
        }

        private async void BtnFechar_Click(object sender, RoutedEventArgs e)
        {
            var result = CustomOKCancelMessageBox.Show("Este procedimento não tem volta. Após fechar o orçamento você irá criar uma Ordem de Serviço baseada no número deste orçamento.\nDeseja continuar?","Atenção!",Window.GetWindow(this));
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                using (var form = new ValorFechadoOrcamento())
                {
                    form.Owner = Window.GetWindow(this);
                    form.ShowDialog();
                    if (form.DialogResult.Value && form.DialogResult.HasValue)
                    {
                        dto.Valor_Fechamento = form.Valor;
                        WaitBox wb = new WaitBox
                        {
                            Owner = Window.GetWindow(this)
                        };
                        wb.Show();
                        await Task.Run(() =>
                        {
                            bll.FecharNegocio(dto);
                        });
                        dto.Status_Id = 5;
                        dto.Status_Descricao = "FECHADO";
                        txtStatus.Text = dto.Status_Descricao;
                        wb.Close();
                        InitializeComponents();
                    }
                }
            }
        }

        private async void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            var result = CustomOKCancelMessageBox.Show("Este procedimento irá cancelar o orçamento, este processo não tem volta.\nDeseja continuar?", "Atenção!", Window.GetWindow(this));
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                using (var form = new MotivoCancelamentoOrcamento())
                {
                    form.Owner = Window.GetWindow(this);
                    form.ShowDialog();
                    if (form.DialogResult.HasValue && form.DialogResult.Value)
                    {
                        var presult = CustomOKCancelMessageBox.Show("Deseja adicionar o valor da concorrência?", "Atenção!", Window.GetWindow(this));
                        if (presult == System.Windows.Forms.DialogResult.OK)
                        {
                            using (var pp = new PrecoPerdidoOrcamento())
                            {
                                pp.Owner = Window.GetWindow(this);
                                pp.ShowDialog();
                                if (pp.DialogResult.HasValue && pp.DialogResult.Value)
                                {
                                    dto.Valor_Perdido = pp.Valor;
                                }
                            }
                        }
                        else
                        {
                            dto.Valor_Perdido = "0";
                        }
                        dto.Motivo_Cancelamento = form.Motivo_Cancelamento;
                        dto.Motivo_Cancelamento_Id = form.Motivo_Cancelamento_Id;
                        WaitBox wb = new WaitBox
                        {
                            Owner = Window.GetWindow(this)
                        };
                        wb.Show();
                        await Task.Run(() =>
                        {
                            bll.CancelarNegocio(dto);
                        });
                        dto.Status_Id = 5;
                        dto.Status_Descricao = "CANCELADO";
                        txtStatus.Text = dto.Status_Descricao;
                        wb.Close();
                        InitializeComponents();
                    }
                }
            }
        }

        private void BtnNovaVersão_Click(object sender, RoutedEventArgs e)
        {
            using (var form = new AdicionarVersaoOrcamento(dto))
            {
                form.Owner = Window.GetWindow(this);
                form.ShowDialog();
                if (form.DialogResult.HasValue && form.DialogResult.Value)
                {
                    dto.Versao_Id = form.Versao.ToString();
                    dto.Versao_Descricao = form.Descricao;
                    bll.AdicionarVersão(dto);
                    txtVersao.Text = form.Versao.ToString("00");
                    dto.Status_Id = 2;
                    dto.Status_Descricao = "EM ANDAMENTO";
                    InitializeComponents();
                    CustomOKMessageBox.Show("Versão " + form.Versao.ToString("00") + " cadastrada com sucesso.","Sucesso!",Window.GetWindow(this));
                    var result = CustomOKCancelMessageBox.Show("Deseja copiar os itens de alguma versão anterior para esta versão?", "Atenção!", Window.GetWindow(this));
                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        using (var cv = new CopiarVersaoOrcamento(dto))
                        {
                            cv.Owner = Window.GetWindow(this);
                            cv.ShowDialog();
                            if (cv.DialogResult.HasValue && cv.DialogResult.Value)
                            {
                                versaoOrcamentoDTO.Id = Convert.ToInt32(cv.Versao);
                                versaoOrcamentoDTO.Versao_Atividade_Id = Convert.ToInt32(cv.Versao_Atividade_Id);
                                bll.CopiarItensVersao(versaoOrcamentoDTO,dto);
                                CustomOKMessageBox.Show("Itens copiados para nova versão.","Sucesso!",Window.GetWindow(this));
                            }
                        }
                    }
                }
            }
        }

        private void BtnAterarVersao_Click(object sender, RoutedEventArgs e)
        {
            if (bll.HasEnabledVersion(dto))
            {
                using (var form = new AlterarVersaoOrcamento(dto))
            {
                form.Owner = Window.GetWindow(this);
                form.ShowDialog();
                if (form.DialogResult.HasValue && form.DialogResult.Value)
                {
                    dto.Versao_Id = form.Versao;
                    bll.AlterarVersao(dto);
                    txtVersao.Text = Convert.ToInt32(dto.Versao_Id).ToString("00");
                    InitializeComponents();
                }
            }
            }
            else
            {
                CustomOKMessageBox.Show("Este orçamento só possui uma versão ou não possui versão habilitada para edição!","Atenção!",Window.GetWindow(this));
            }
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
                bll.Dispose();
                syncEvent.Dispose();
            }
            disposed = true;
        }
        #endregion
    }
}
