using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BLL;
using DTO;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Threading;
using MMLib.Extensions;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.Xpf.Grid;
using System.Threading.Tasks;

namespace GeGET
{
    public partial class Atendimento : UserControl, IDisposable
    {
        #region Declarations
        Helpers helpers = new Helpers();
        InformacoesAtendimentoDTO informacoesDTO = new InformacoesAtendimentoDTO();
        InformacoesAtendimentoBLL informacoesBLL = new InformacoesAtendimentoBLL();
        AtendimentoBLL bll = new AtendimentoBLL();
        AtendimentoDTO dto = new AtendimentoDTO();
        public ObservableCollection<AtendimentoDTO> listaItens;
        ManualResetEvent syncEvent = new ManualResetEvent(false);
        ManualResetEvent syncValues = new ManualResetEvent(false);
        public ObservableCollection<AtendimentoDTO> listaAtender;
        bool disposed = false;
        #endregion

        #region Initialize
        public Atendimento()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods

        private void InitializeComponents()
        {
            txtNumero.Text = " ";
            txtCliente.Text = "";
            txtDescricao.Text = "";
            txtVersao.Text = "";
            txtCidade.Text = "";
            txtUF.Text = "";
        }

        public void Load()
        {
            listaItens = bll.LoadItens(informacoesDTO);
            grdItens.ItemsSource = listaItens;
        }

        #endregion

        #region Events

        #region Clicks

        #region Botão de Pesquisa
        private void BtnPesquisa_Click(object sender, RoutedEventArgs e)
        {
            BlackScreen bs = new BlackScreen();
            var position = Mouse.GetPosition(this);
            using (var form = new ProcurarRequisicaoMaterial(position))
            {
                form.Owner = Window.GetWindow(this);
                form.ShowDialog();
                if (form.DialogResult.Value && form.DialogResult.HasValue)
                {
                    informacoesDTO.Id = Convert.ToInt32(form.RM_Id);
                    informacoesDTO = informacoesBLL.LoadInformacoes(informacoesDTO).First();
                    txtSolicitante.Text = informacoesDTO.Solicitante;
                    txtEndereco.Text = informacoesDTO.Endereco;
                    txtNumero.Text = "RM" + Convert.ToInt32(informacoesDTO.Id).ToString("00000");
                    txtDescricao.Text = informacoesDTO.Descricao;
                    txtCidade.Text = informacoesDTO.Cidade;
                    txtUF.Text = informacoesDTO.Uf;
                    txtCliente.Text = informacoesDTO.Razao_Social;
                    txtVersao.Text = Convert.ToInt32(informacoesDTO.Versao).ToString("00");
                    Load();
                }
            }
        }
        #endregion

        #region Botão Voltar
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            helpers.OpenBack(false);
        }
        #endregion

        #region Botão Fechar
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            helpers.Close();
        }
        #endregion

        #endregion

        #region Cell Value Changed

        private void TableView_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
           var row = e.Row as AtendimentoDTO;
            if (e.Column.FieldName.ToString() == "Quantidade")
            {
                if (row.Quantidade > row.Solicitado || row.Quantidade > row.Estoque)
                {
                    if (row.Quantidade > row.Solicitado)
                    {
                        if (row.Solicitado > row.Estoque)
                        {
                            row.Quantidade = row.Estoque;
                        }
                        else
                        {
                            row.Quantidade = row.Solicitado;
                        }
                    }
                    if (row.Quantidade > row.Estoque)
                    {
                        row.Quantidade = row.Estoque;
                    }
                }
                else if (row.Quantidade < 0)
                {
                    row.Quantidade = 0;
                }
            }
        }

        #endregion

        #endregion

        private void GrdItens_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ((TableView)grdItens.View).MoveNextRow();
                ((TableView)grdItens.View).ShowEditor();
                e.Handled = true;
            }
        }

        private void GrdView_ShownEditor(object sender, EditorEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                if (this.grdView.ActiveEditor != null)
                {
                    this.grdView.ActiveEditor.SelectAll();
                }
                else
                {
                    ((TableView)grdItens.View).ShowEditor();
                }
            }), DispatcherPriority.Render);
        }

        private async void AtenderMateriais_Click(object sender, RoutedEventArgs e)
        {
            int[] handles = grdItens.GetSelectedRowHandles();
            if (handles.Length > 0)
            {
                bool iNotify = false;
                foreach (var rowHandle in handles)
                {
                    var selectedItem = grdItens.GetRow(rowHandle) as AtendimentoDTO;
                    if (selectedItem.Quantidade == 0)
                    {
                        iNotify = true;
                    }
                }
                if (iNotify)
                {
                    CustomOKMessageBox.Show("Existem materiais na sua seleção que possuem a quantidade zerada.", "Atenção!", Window.GetWindow(this));
                }
                else
                {
                    var result = CustomOKCancelMessageBox.Show("Deseja mesmo atender os produtos selecionados?.", "Atenção!", Window.GetWindow(this));
                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        listaAtender = new ObservableCollection<AtendimentoDTO>();
                        foreach (var rowHandle in handles)
                        {
                            var selectedItem = grdItens.GetRow(rowHandle) as AtendimentoDTO;
                            listaAtender.Add(selectedItem);
                        }
                        WaitBox wb = new WaitBox
                        {
                            Owner = Window.GetWindow(this)
                        };
                        await Task.Run(() =>
                        {
                            bll.AtenderProdutos(informacoesDTO, listaAtender);
                        });
                        wb.Close();
                        CustomOKMessageBox.Show("Itens atendidos com sucesso!", "Sucesso!", Window.GetWindow(this));
                        Load();
                    }
                }
            }
            else
            {
                CustomOKMessageBox.Show("Você deve selecionar ao menos um produto para atender.","Atenção!",Window.GetWindow(this));
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
                syncEvent.Dispose();
                syncValues.Dispose();
                informacoesBLL.Dispose();
                bll.Dispose();
            }
            disposed = true;
        }

        #endregion
    }
}
