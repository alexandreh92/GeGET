using System;
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
    public partial class RelatorioProdutos : UserControl, IDisposable
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
        public RelatorioProdutos()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods


        public void Load()
        {
            listaOrcamentos = bll.LoadOrcamentoExportar(dto);
            grdItens.ItemsSource = listaOrcamentos;
        }

        #endregion

        #region Events

        #region Clicks

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            helpers.OpenBack(false);
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            helpers.Close();
        }


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

        #endregion

        #endregion

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
