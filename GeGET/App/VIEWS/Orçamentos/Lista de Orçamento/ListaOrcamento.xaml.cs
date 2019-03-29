﻿using System;
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
        ObservableCollection<ValoresOrcamento> valoresOrcamento;
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
            valoresOrcamento = bll.LoadValores(dto);
            foreach (var item in valoresOrcamento)
            {
                txt_Atividade_Total_Materiais.Text = item.Atividade_Total_Itens.ToString("c");
                txt_Atividade_Total_Materiais_BDI.Text = item.Atividade_Total_Itens_BDI.ToString("c");
                txt_Atividade_Total_Mobra.Text = item.Atividade_Total_Mobra.ToString("c");
                txt_Atividade_Total_Mobra_BDI.Text = item.Atividade_Total_Mobra_BDI.ToString("c");
                txt_Atividade_Total.Text = item.Atividade_Total_Atividade.ToString("c");
                txt_Atividade_Total_BDI.Text = item.Atividade_Total_Atividade_BDI.ToString("c");
                txt_Atividade_Faturado.Text = item.Atividade_Total_Faturado.ToString("c");
                txt_Atividade_FD.Text = item.Atividade_Total_FD.ToString("c");

                txt_Negocio_Total_Materiais.Text = item.Negocio_Total_Itens.ToString("c");
                txt_Negocio_Total_Materiais_BDI.Text = item.Negocio_Total_Itens_BDI.ToString("c");
                txt_Negocio_Total_Mobra.Text = item.Negocio_Total_Mobra.ToString("c");
                txt_Negocio_Total_Mobra_BDI.Text = item.Negocio_Total_Mobra_BDI.ToString("c");
                txt_Negocio_Total.Text = item.Negocio_Total_Atividade.ToString("c");
                txt_Negocio_Total_BDI.Text = item.Negocio_Total_Atividade_BDI.ToString("c");
                txt_Negocio_Faturado.Text = item.Negocio_Total_Faturado.ToString("c");
                txt_Negocio_FD.Text = item.Negocio_Total_FD.ToString("c");

            }
        }

        #endregion

        #region Events

        #region Clicks
        private void BtnPesquisa_Click(object sender, RoutedEventArgs e)
        {
            BlackScreen bs = new BlackScreen();
            var position = Mouse.GetPosition(this);
            using (var form = new ProcurarOrcamento(position))
            {

                bs.Show();
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
                        CustomOKMessageBox.Show("Não há atividades cadastradas para este negócio.", "Atenção!");
                        InitializeComponents();
                    }

                }
            }
            bs.Close();
        }

        private void Adicionar_Click(object sender, RoutedEventArgs e)
        {
            using (var form = new ProcurarMateriais(dto))
            {
                form.ShowDialog();
                if (form.DialogResult.Value && form.DialogResult.HasValue)
                {

                }
                Load();
            }
        }

        private void AtualizarPrecos_Click(object sender, RoutedEventArgs e)
        {
            var result = CustomOKCancelMessageBox.Show("Você deseja mesmo atualiazar os preços para a versão atual?", "Atenção!");
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                bll.AtualizarPreco(dto);
                Load();
                CustomOKMessageBox.Show("Preços atualizados com sucesso.","Atualizado!");
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
                var result = CustomOKCancelMessageBox.Show("Deseja mesmo excluir todos os itens selecionados?", "Atenção!");
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
                CustomOKMessageBox.Show("Você deve selecionar ao menos um item para excluir.", "Atenção!");
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
                    form.ShowDialog();
                }
            }
            else
            {
                CustomOKMessageBox.Show("Você deve selecionar ao menos um item para copiar.", "Atenção!");
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
                    form.ShowDialog();
                    if (form.DialogResult.HasValue && form.DialogResult.Value)
                    {
                        Load();
                    }
                }
                

            }
            else
            {
                CustomOKMessageBox.Show("Você deve selecionar ao menos um item para alterar o BDI.", "Atenção!");
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

            Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
            {
                if (e.Column.Header.ToString() == "Quantidade")
                {
                    materialDTO.Id = material.Id;
                    materialDTO.Quantidade = material.Quantidade;
                    MessageBox.Show("qtde");
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
                    bll.AtualizarFD(materialDTO);
                }
            }));
            LoadSidePanel();
        }

        #endregion

        #endregion

        private void TableView_CellValueChanging(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            var material = e.Row as ListaOrcamentosDTO;
            
            LoadSidePanel();
        }
    }
}
