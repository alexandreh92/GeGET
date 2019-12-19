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
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data;
using System.IO;

namespace GeGET
{
    public partial class CadastroLoteItem : UserControl, IDisposable
    {
        #region Declarations
        bool disposed = false;
        Helpers helpers = new Helpers();
        CadastroItensLoteBLL bll = new CadastroItensLoteBLL();
        DataTable dt;
        ManualResetEvent syncEvent = new ManualResetEvent(false);
        WaitBox wb;
        ObservableCollection<NegociosDTO> listaNegocios = new ObservableCollection<NegociosDTO>();
        #endregion

        #region Initialize
        public CadastroLoteItem()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods


        #endregion

        #region Events

        #region Clicks

        private void ExportExcel_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.DefaultExt = ".xlsx";
            openfile.Filter = "(.xlsx)|*.xlsx";
            //openfile.ShowDialog();

            var browsefile = openfile.ShowDialog();

            if (browsefile == true)
            {
                Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
                //Static File From Base Path...........
                //Microsoft.Office.Interop.Excel.Workbook excelBook = excelApp.Workbooks.Open(AppDomain.CurrentDomain.BaseDirectory + "TestExcel.xlsx", 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                //Dynamic File Using Uploader...........
                Microsoft.Office.Interop.Excel.Workbook excelBook = excelApp.Workbooks.Open(openfile.FileName.ToString(), 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                Microsoft.Office.Interop.Excel.Worksheet excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelBook.Worksheets.get_Item(1); ;
                Microsoft.Office.Interop.Excel.Range excelRange = excelSheet.UsedRange;

                dt = new DataTable();

                string strCellData = "";
                double douCellData;
                int rowCnt = 0;
                int colCnt = 0;

                for (colCnt = 1; colCnt <= excelRange.Columns.Count; colCnt++)
                {
                    string strColumn = "";
                    strColumn = (string)(excelRange.Cells[1, colCnt] as Microsoft.Office.Interop.Excel.Range).Value2;
                    dt.Columns.Add(strColumn, typeof(string));
                }

                for (rowCnt = 2; rowCnt <= excelRange.Rows.Count; rowCnt++)
                {
                    string strData = "";
                    for (colCnt = 1; colCnt <= excelRange.Columns.Count; colCnt++)
                    {
                        try
                        {
                            strCellData = (string)(excelRange.Cells[rowCnt, colCnt] as Microsoft.Office.Interop.Excel.Range).Value2;
                            strData += strCellData + "|";
                        }
                        catch (Exception ex)
                        {
                            douCellData = (excelRange.Cells[rowCnt, colCnt] as Microsoft.Office.Interop.Excel.Range).Value2;
                            strData += douCellData.ToString() + "|";
                        }
                    }
                    strData = strData.Remove(strData.Length - 1, 1);
                    dt.Rows.Add(strData.Split('|'));
                }

                grdItens.ItemsSource = dt.DefaultView;

                excelBook.Close(true, null, null);
                excelApp.Quit();
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
                syncEvent.Dispose();
            }
            disposed = true;
        }
        #endregion

        private void SendToDB_Click(object sender, RoutedEventArgs e)
        {
            if (bll.CadastroLote(dt))
            {
                CustomOKMessageBox.Show("Itens Cadastrados com sucesso!", "Sucesso!", Window.GetWindow(this));
            }
        }

        private void DownloadModelo_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.FileName = "Planilha Modelo - Cadastro de Itens em Lote";
            fileDialog.Filter = "Arquivo Microsoft Excel (*.xlsx)|*.xlsx";
            if (fileDialog.ShowDialog() == true)
            {
                using (System.IO.FileStream fs = new System.IO.FileStream(fileDialog.FileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write))
                {
                    byte[] data = Properties.Resources.modelo_itens_lote;
                    fs.Write(data, 0, data.Length);
                }
            }
        }
    }
}
