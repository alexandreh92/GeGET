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
using Microsoft.Win32;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using DevExpress.Printing.ExportHelpers;

namespace GeGET
{
    public partial class ConsultaRequisicaoMaterial : UserControl, IDisposable
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
        public ConsultaRequisicaoMaterial()
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
            listaItens = bll.LoadItensConsulta(informacoesDTO);
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
            using (var form = new ProcurarRequisicaoMaterialConsulta(position))
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

        private void ExportExcel_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog
            {
                FileName = "P" + Convert.ToInt32(informacoesDTO.Negocio_Id).ToString("0000") + "-RM-"+informacoesDTO.Id.ToString("00000"),
                Filter = "Arquivo Microsoft Excel (*.xlsx)|*.xlsx"
            };
            if (fileDialog.ShowDialog() == true)
            {
                var options = new XlsxExportOptionsEx() { ExportType = DevExpress.Export.ExportType.DataAware };
                options.CustomizeSheetHeader += options_CustomizeSheetHeader;

                grdView.ExportToXlsx(fileDialog.FileName, options );

            }
        }

        private void options_CustomizeSheetHeader(ContextEventArgs e)
        {
            // Create a new row. 
            CellObject row = new CellObject();
            // Specify row values. 
            row.Value = "RM" + informacoesDTO.Id.ToString("00000") + " - P" + informacoesDTO.Negocio_Id.ToString("0000") + " - DESCRIÇÃO: " + informacoesDTO.Descricao.ToUpper();
            // Specify row formatting. 
            XlFormattingObject rowFormatting = new XlFormattingObject();
            rowFormatting.Font = new XlCellFont { Bold = true, Size = 14 };
            rowFormatting.Alignment = new DevExpress.Export.Xl.XlCellAlignment { HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Center, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Top };
            row.Formatting = rowFormatting;
            // Add the created row to the output document. 
            e.ExportContext.AddRow(new[] { row });
            // Add an empty row to the output document. 
            e.ExportContext.AddRow();
            // Merge cells of two new rows.  
            e.ExportContext.MergeCells(new DevExpress.Export.Xl.XlCellRange(new DevExpress.Export.Xl.XlCellPosition(0, 0), new DevExpress.Export.Xl.XlCellPosition(7, 1)));
        }
    }
}
