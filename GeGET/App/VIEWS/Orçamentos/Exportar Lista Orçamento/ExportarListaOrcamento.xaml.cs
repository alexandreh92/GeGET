﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BLL;
using DTO;
using System.Collections.ObjectModel;
using System.Threading;
using DevExpress.XtraPrinting;
using Microsoft.Win32;

namespace GeGET
{
    public partial class ExportarListaOrcamento : UserControl, IDisposable
    {
        #region Declarations
        bool disposed = false;
        Helpers helpers = new Helpers();
        MaterialDTO materialDTO = new MaterialDTO();
        ListaOrcamentosBLL bll = new ListaOrcamentosBLL();
        ListaOrcamentosDTO dto = new ListaOrcamentosDTO();
        OrcamentosBLL Orcamentosbll = new OrcamentosBLL();
        OrcamentosDTO Orcamentosdto = new OrcamentosDTO();
        VersaoBLL versaoBLL = new VersaoBLL();
        public ObservableCollection<ListaOrcamentosDTO> listaOrcamentos;
        ManualResetEvent syncEvent = new ManualResetEvent(false);
        ManualResetEvent syncValues = new ManualResetEvent(false);
        #endregion

        #region Initialize
        public ExportarListaOrcamento()
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
            
            txtNumero.Text = " ";
            txtCliente.Text = "";
            txtDescricao.Text = "";
            //txtVersao.Text = "";
            txtCidade.Text = "";
            txtUF.Text = "";
            cmbVersao.ItemsSource = null;
        }

        public void Load()
        {
            listaOrcamentos = bll.LoadOrcamentoExportar(dto);
            grdItens.ItemsSource = listaOrcamentos;
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
                            //txtVersao.Text = Convert.ToInt32(item.Versao_Valida).ToString("00");
                            cmbVersao.ItemsSource = versaoBLL.LoadVersao(dto);
                            cmbVersao.SelectedValuePath = "Num_Versao";
                            cmbVersao.DisplayMemberPath = "Num_Versao";
                            cmbVersao.SelectedIndex = Convert.ToInt32(item.Versao_Valida)-1;
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


       
        #endregion

        #endregion


        private void ExportExcel_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog
            {
                FileName = "P" + Convert.ToInt32(dto.Id).ToString("0000") + "-LO",
                Filter = "Arquivo Microsoft Excel (*.xlsx)|*.xlsx"
            };
            if (fileDialog.ShowDialog() == true)
            {
                grdView.ExportToXlsx(fileDialog.FileName, new XlsxExportOptionsEx() { ExportType = DevExpress.Export.ExportType.Default });

            }
        }

        private void CmbVersao_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbVersao.SelectedValue != null)
            {
                dto.Versao = cmbVersao.SelectedValue.ToString();
            }
            Load();
            CardPanel.Visibility = Visibility.Visible;
        }

        private void ExportPDF_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog
            {
                FileName = "P" + Convert.ToInt32(dto.Id).ToString("0000") + "-LO",
                Filter = "Arquivo Portable Document Format (*.pdf)|*.pdf"
            };
            if (fileDialog.ShowDialog() == true)
            {
                grdView.ExportToPdf(fileDialog.FileName, new PdfExportOptions());
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
                syncValues.Dispose();
                syncEvent.Dispose();
                bll.Dispose();
                Orcamentosbll.Dispose();
                versaoBLL.Dispose();
            }
            disposed = true;
        }
        #endregion
    }
}
