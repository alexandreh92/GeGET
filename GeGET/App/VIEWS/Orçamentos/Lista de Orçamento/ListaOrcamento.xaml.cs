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

namespace GeGET
{
    public partial class ListaOrcamento : UserControl
    {
        #region Declarations
        Helpers helpers = new Helpers();
        MaterialDTO materialDTO = new MaterialDTO();
        ListaOrcamentosBLL bll = new ListaOrcamentosBLL();
        ListaOrcamentosDTO dto = new ListaOrcamentosDTO();
        AdicionarItemOrcamentoDTO adicionarItemOrcamentoDTO = new AdicionarItemOrcamentoDTO();
        OrcamentosBLL Orcamentosbll = new OrcamentosBLL();
        OrcamentosDTO Orcamentosdto = new OrcamentosDTO();
        DisciplinaBLL Disciplinasbll = new DisciplinaBLL();
        DisciplinaDTO Disciplinasdto = new DisciplinaDTO();
        AtividadesBLL atividadesBLL = new AtividadesBLL();
        public ObservableCollection<ListaOrcamentosDTO> listaOrcamentos;
        ManualResetEvent syncEvent = new ManualResetEvent(false);
        ManualResetEvent syncValues = new ManualResetEvent(false);
        Thread t1;
        WaitBox wb;
        ObservableCollection<CopiarItensOrcamentoDTO> listaCopiar;
        ObservableCollection<MaterialDTO> listaAterarBDI;
        #endregion

        #region Initialize
        public ListaOrcamento()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods

        private void InitializeComponents()
        {
            if (CardPanel.Visibility != Visibility.Collapsed)
            {
                CardPanel.Visibility = Visibility.Collapsed;
            }
            if (sideExpander.Visibility != Visibility.Collapsed)
            {
                sideExpander.Visibility = Visibility.Collapsed;
            }
            txtNumero.Text = " ";
            txtCliente.Text = "";
            txtCategoria.Text = "";
            txtDescricao.Text = "";
            txtVersao.Text = "";
            txtCidade.Text = "";
            txtUF.Text = "";
            cmbAtividade.ItemsSource = null;
            cmbDisciplina.ItemsSource = null;
        }

        private void WaitBoxLoad()
        {
            syncEvent.WaitOne();
            Dispatcher.Invoke(new Action(() =>
            {
                grdItens.UnselectAll();
                Load();
                wb.Close();
            }));
        }

        public void Load()
        {
            listaOrcamentos = bll.LoadOrcamento(dto);
            grdItens.ItemsSource = listaOrcamentos;
            LoadSidePanel();
        }

        private void LoadSidePanel()
        {
            pnlValores.ItemsSource = bll.LoadValores(dto);
        }

        #endregion

        #region Events

        #region Clicks
        private void BtnPesquisa_Click(object sender, RoutedEventArgs e)
        {
            BlackScreen bs = new BlackScreen();
            var position = Mouse.GetPosition(this);
            using (var form = new ProcurarOrcamentoCadastrado(position))
            {
                form.Owner = Window.GetWindow(this);
                form.ShowDialog();
                if (form.DialogResult.Value && form.DialogResult.HasValue)
                {
                    dto.Id = form.Negocio_Id;
                    var loadtext = Orcamentosbll.LoadTextBoxes(dto);
                    if (loadtext.Count > 0)
                    {
                        foreach (OrcamentosDTO item in loadtext)
                        {
                            txtNumero.Text = "P" + Convert.ToInt32(dto.Id).ToString("0000");
                            txtDescricao.Text = item.Descricao;
                            txtCidade.Text = item.Cidade;
                            txtUF.Text = item.UF;
                            txtCliente.Text = item.Razao_Social;
                            txtVersao.Text = Convert.ToInt32(item.Versao_Valida).ToString("00");
                            cmbDisciplina.ItemsSource = Disciplinasbll.LoadDisciplinas(dto);
                            cmbDisciplina.SelectedValuePath = "Id";
                            cmbDisciplina.DisplayMemberPath = "Descricao";
                            cmbDisciplina.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        bs.Close();
                        CustomOKMessageBox.Show("Não há atividades cadastradas para este negócio.", "Atenção!", Window.GetWindow(this));
                        InitializeComponents();
                    }

                }
            }
        }

        private void Adicionar_Click(object sender, RoutedEventArgs e)
        {
            using (var form = new ProcurarMateriais(dto))
            {
                form.Owner = Window.GetWindow(this);
                form.ShowDialog();
                if (form.DialogResult.Value && form.DialogResult.HasValue)
                {

                }
                Load();
            }
        }

        private void AtualizarPrecos_Click(object sender, RoutedEventArgs e)
        {
            var result = CustomOKCancelMessageBox.Show("Você deseja mesmo atualiazar os preços para a versão atual?", "Atenção!", Window.GetWindow(this));
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                bll.AtualizarPreco(dto);
                Load();
                CustomOKMessageBox.Show("Preços atualizados com sucesso.","Atualizado!", Window.GetWindow(this));
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            helpers.OpenBack(false);
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            helpers.Close();
        }

        private void Excluir_Click(object sender, RoutedEventArgs e)
        {
            int[] handles = grdItens.GetSelectedRowHandles();

            if (handles.Length > 0)
            {
                var result = CustomOKCancelMessageBox.Show("Deseja mesmo excluir todos os itens selecionados?", "Atenção!", Window.GetWindow(this));
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    new Thread(() =>
                    {
                        Dispatcher.Invoke(new Action(() =>
                        {
                            wb = new WaitBox();
                            wb.Show();
                        }));
                        syncEvent.Set();
                        foreach (var rowHandle in handles)
                        {
                            Dispatcher.Invoke(DispatcherPriority.Background,
                               new Action(() =>
                               {
                                   var selectedItem = grdItens.GetRow(rowHandle) as ListaOrcamentosDTO;
                                   adicionarItemOrcamentoDTO.Id = Convert.ToInt32( selectedItem.Id );
                                   bll.Excluir(adicionarItemOrcamentoDTO);
                               }));
                        }
                        t1 = new Thread(WaitBoxLoad);
                        t1.Start();
                    }).Start();
                }
            }
            else
            {
                CustomOKMessageBox.Show("Você deve selecionar ao menos um item para excluir.", "Atenção!", Window.GetWindow(this));
            }
        }

        private void CopiarItens_Click(object sender, RoutedEventArgs e)
        {
            listaCopiar = new ObservableCollection<CopiarItensOrcamentoDTO>();
            int[] handles = grdItens.GetSelectedRowHandles();

            if (handles.Length > 0)
            {
                listaCopiar = new ObservableCollection<CopiarItensOrcamentoDTO>();
                foreach (var rowHandle in handles)
                {
                    var selectedItem = grdItens.GetRow(rowHandle) as ListaOrcamentosDTO;
                    listaCopiar.Add(new CopiarItensOrcamentoDTO { Id = selectedItem.Produto_Id });
                }
                using (var form = new CopiarItensOrcamento(listaCopiar, dto))
                {
                    form.Owner = Window.GetWindow(this);
                    form.ShowDialog();
                    if (form.DialogResult.Value && form.DialogResult.HasValue)
                    {
                        Load();
                    }
                }
            }
            else
            {
                CustomOKMessageBox.Show("Você deve selecionar ao menos um item para copiar.", "Atenção!", Window.GetWindow(this));
            }
        }

        private void AlterarBDI_Click(object sender, RoutedEventArgs e)
        {
            listaAterarBDI = new ObservableCollection<MaterialDTO>();
            int[] handles = grdItens.GetSelectedRowHandles();
            if (handles.Length>0)
            {
                foreach (var rowHandle in handles)
                {
                    var selectedItem = grdItens.GetRow(rowHandle) as ListaOrcamentosDTO;
                    listaAterarBDI.Add(new MaterialDTO { Id = selectedItem.Id });
                }
                using (var form = new AlterarBDIOrcamento(listaAterarBDI))
                {
                    form.Owner = Window.GetWindow(this);
                    form.ShowDialog();
                    if (form.DialogResult.HasValue && form.DialogResult.Value)
                    {
                        Load();
                    }
                }
                

            }
            else
            {
                CustomOKMessageBox.Show("Você deve selecionar ao menos um item para alterar o BDI.", "Atenção!", Window.GetWindow(this));
            }
        }

        #endregion

        #region Comboboxes Selection Changed
        private void CmbDisciplina_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Disciplinasdto.Id = Convert.ToInt32(cmbDisciplina.SelectedValue);
            cmbAtividade.ItemsSource = atividadesBLL.LoadAtividades(dto, Disciplinasdto);
            cmbAtividade.SelectedValuePath = "Id";
            cmbAtividade.DisplayMemberPath = "Descricao";
            cmbAtividade.SelectedIndex = 0;
        }

        

        private void CmbAtividade_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbAtividade.SelectedValue != null)
            {
                dto.Atividade_Id = cmbAtividade.SelectedValue.ToString();
            }
            Load();
            sideExpander.Visibility = Visibility.Visible;
            CardPanel.Visibility = Visibility.Visible;
            
        }
        #endregion

        #region Cell Value Changed

        private void TableView_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            var material = e.Row as ListaOrcamentosDTO;

            new Thread(() =>
            {

            Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
            {
                if (e.Column.Header.ToString() == "Quantidade")
                {
                    materialDTO.Id = material.Id;
                    materialDTO.Quantidade = material.Quantidade;
                    bll.AtualizarQuantidade(materialDTO);
                }
                else if (e.Column.Header.ToString() == "BDI")
                {
                    materialDTO.Id = material.Id;
                    materialDTO.Bdi = material.Bdi.ToString();
                    bll.AtualizarBDI(materialDTO);
                }
                else if (e.Column.Header.ToString() == "FD")
                {
                    materialDTO.Id = material.Id;
                    materialDTO.Fd = material.Fd;
                    new Thread(() => 
                    {
                        Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(() =>
                        {
                            bll.AtualizarFD(materialDTO);
                        }));
                    }).Start();
                    
                }
            }));
            }).Start();
            new Thread(() => 
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => 
                {
                    LoadSidePanel();
                }));
            }).Start();
        }

        #endregion

        #endregion

        private void grdItens_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ((TableView)grdItens.View).MoveNextRow();
                e.Handled = true;
            }
        }

        private void GrdView_ShownEditor(object sender, EditorEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                this.grdView.ActiveEditor.SelectAll();
            }), DispatcherPriority.Render);
        }

        private void BtnAnotacoes_Click(object sender, RoutedEventArgs e)
        {
            var Id = grdItens.GetFocusedRowCellValue("Id").ToString();
            var Anotacoes = grdItens.GetFocusedRowCellValue("Anotacoes").ToString();
            var Produto_Id = grdItens.GetFocusedRowCellValue("Produto_Id").ToString();

            using (var form = new AnotacoesOrcamento(Id, Anotacoes, Produto_Id))
            {
                form.Owner = Window.GetWindow(this);
                form.ShowDialog();
                if (form.DialogResult.HasValue && form.DialogResult.Value)
                {
                    Load();
                }
            }
        }
    }
}
