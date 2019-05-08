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
using DevExpress.Mvvm;
using DevExpress.Xpf.Grid;
using System.IO;
using DevExpress.XtraPrinting;
using Microsoft.Win32;

namespace GeGET
{
    public partial class ConsultaListaOrcamento : UserControl
    {
        #region Declarations
        Helpers helpers = new Helpers();

        ListaOrcamentosDTO dto = new ListaOrcamentosDTO();
        ListaOrcamentosBLL bll = new ListaOrcamentosBLL();
        InformacoesListaOrcamentosDTO informacoesDTO = new InformacoesListaOrcamentosDTO();
        InformacoesListaOrcamentosBLL informacoesBLL = new InformacoesListaOrcamentosBLL();
        public ObservableCollection<ListaOrcamentosDTO> listaOrcamentos;
        ManualResetEvent syncEvent = new ManualResetEvent(false);
        ManualResetEvent syncValues = new ManualResetEvent(false);
        #endregion

        #region Initialize
        public ConsultaListaOrcamento()
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
            txtDescricao.Text = "";
            txtVersao.Text = "";
            txtCidade.Text = "";
            txtUF.Text = "";
            cmbAtividade.ItemsSource = null;
            cmbDisciplina.ItemsSource = null;
            cmbDescricao.ItemsSource = null;
        }

        public void Load()
        {
            listaOrcamentos = bll.LoadOrcamento(informacoesDTO);
            grdItens.ItemsSource = listaOrcamentos;
            LoadSidePanel();
        }

        private void LoadSidePanel()
        {
            pnlValores.ItemsSource = bll.LoadValores(informacoesDTO);
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
                form.Owner = Window.GetWindow(this);
                form.ShowDialog();
                if (form.DialogResult.Value && form.DialogResult.HasValue)
                {
                    informacoesDTO.Id = Convert.ToInt32(form.Negocio_Id);
                    var informacoes = informacoesBLL.LoadInformacoes(informacoesDTO);

                    if (informacoes.Count > 0)
                    {
                        informacoesDTO = informacoes.First();
                        txtNumero.Text = "P" + Convert.ToInt32(informacoesDTO.Id).ToString("0000");
                        txtDescricao.Text = informacoesDTO.Descricao;
                        txtCidade.Text = informacoesDTO.Cidade;
                        txtUF.Text = informacoesDTO.Uf;
                        txtCliente.Text = informacoesDTO.Razao_Social;
                        txtVersao.Text = Convert.ToInt32(informacoesDTO.Versao).ToString("00");
                        cmbDisciplina.ItemsSource = informacoesDTO.Disciplinas;
                        cmbDisciplina.SelectedValuePath = "Id";
                        cmbDisciplina.DisplayMemberPath = "Descricao";
                        cmbDisciplina.SelectedIndex = 0;
                    }
                }
            }
            
        }

        private void Adicionar_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void AtualizarPrecos_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            helpers.OpenBack(false);
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            helpers.Close();
        }
        
        #endregion

        #region Comboboxes Selection Changed
        private void CmbDisciplina_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbDisciplina.SelectedValue != null)
            {
                var Find = cmbDisciplina.SelectedValue.ToString().ToLower();
                var filtered = informacoesDTO.Atividades.Where(descricao => descricao.Disciplina_Id == Find);
                cmbAtividade.ItemsSource = filtered;
                cmbAtividade.SelectedValuePath = "Id";
                cmbAtividade.DisplayMemberPath = "Descricao";
            }
            cmbAtividade.SelectedIndex = 0;
        }

        

        private void CmbAtividade_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbAtividade.SelectedValue != null)
            {
                var Find = cmbAtividade.SelectedValue.ToString().ToLower();
                var filtered = informacoesDTO.Descricao_Atividades.Where(descricao => descricao.Desc_Atividade_Id == Find);
                cmbDescricao.ItemsSource = filtered;
                cmbDescricao.SelectedValuePath = "Id";
                cmbDescricao.DisplayMemberPath = "Descricao";
            }
            cmbDescricao.SelectedIndex = 0;
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
                    dto.Id = material.Id;
                    dto.Quantidade = material.Quantidade;
                    bll.AtualizarQuantidade(dto);
                }
                else if (e.Column.Header.ToString() == "BDI")
                {
                    dto.Id = material.Id;
                    dto.Bdi = material.Bdi;
                    bll.AtualizarBDI(dto);
                }
                else if (e.Column.Header.ToString() == "FD")
                {
                    dto.Id = material.Id;
                    dto.Fd = material.Fd;
                    bll.AtualizarFD(dto);
                }
            }));
            LoadSidePanel();
        }

        #endregion

        #endregion

        private void CmbDescricao_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbDescricao.SelectedValue != null)
            {
                informacoesDTO.Atividade_Id = cmbDescricao.SelectedValue.ToString();
            }
            Load();
            sideExpander.Visibility = Visibility.Visible;
            CardPanel.Visibility = Visibility.Visible;
        }
    }
}
