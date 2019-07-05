using System;
using System.Windows;
using System.Windows.Controls;
using BLL;
using DTO;
using System.Collections.ObjectModel;
using System.Threading;
using DevExpress.XtraPrinting;
using Microsoft.Win32;
using System.Threading.Tasks;

namespace GeGET
{
    public partial class ListaNegociosSimplificada : UserControl, IDisposable
    {
        #region Declarations
        bool disposed = false;
        Helpers helpers = new Helpers();
        NegociosBLL bll = new NegociosBLL();
        NegociosDTO dto = new NegociosDTO();
        ManualResetEvent syncEvent = new ManualResetEvent(false);
        WaitBox wb;
        ObservableCollection<NegociosDTO> listaNegocios = new ObservableCollection<NegociosDTO>();
        #endregion

        #region Initialize
        public ListaNegociosSimplificada()
        {
            InitializeComponent();
            Load();
        }
        #endregion

        #region Methods

        #region Load Values
        private async void Load()
        {
            wb = new WaitBox();
            wb.Owner = Window.GetWindow(this);
            wb.Show();
            await Task.Run(() =>
            {
                listaNegocios = bll.LoadNegocios(dto);
            });
            grdItens.ItemsSource = listaNegocios;
            wb.Close();
        }
        #endregion

        #endregion

        #region Events

        #region Clicks

        private void ExportExcel_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.FileName = "Lista de Negócios Simplificada - " + DateTime.Now.ToString("dd-MM-yyyy");
            fileDialog.Filter = "Arquivo Microsoft Excel (*.xlsx)|*.xlsx";
            if (fileDialog.ShowDialog() == true)
            {
                grdView.ExportToXlsx(fileDialog.FileName, new XlsxExportOptionsEx() { ExportType = DevExpress.Export.ExportType.Default });

            }
        }

        private void ExportPDF_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.FileName = "P" + Convert.ToInt32(1).ToString("0000") + "-LO";
            fileDialog.Filter = "Arquivo Portable Document Format (*.pdf)|*.pdf";
            if (fileDialog.ShowDialog() == true)
            {
                grdView.ExportToPdf(fileDialog.FileName, new PdfExportOptions());
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
                bll.Dispose();
                syncEvent.Dispose();
            }
            disposed = true;
        }
        #endregion
    }
}
