using System;
using System.Windows;
using System.Windows.Controls;
using BLL;
using DTO;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Threading;
using DevExpress.XtraPrinting;
using Microsoft.Win32;

namespace GeGET
{
    public partial class ListaNegociosSimplificada : UserControl
    {
        #region Declarations
        Helpers helpers = new Helpers();
        NegociosBLL bll = new NegociosBLL();
        NegociosDTO dto = new NegociosDTO();
        ManualResetEvent syncEvent = new ManualResetEvent(false);
        Thread t1;
        WaitBox wb;
        ObservableCollection<NegociosDTO> listaNegocios = new ObservableCollection<NegociosDTO>();
        #endregion

        #region Initialize
        public ListaNegociosSimplificada()
        {
            InitializeComponent();
            
            new Thread(() =>
            {
                Dispatcher.Invoke( new Action(() =>
                {
                    wb = new WaitBox();
                    wb.Owner = Window.GetWindow(this);
                    wb.Show();
                }));

                syncEvent.Set();
                Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                {
                    
                    
                    listaNegocios = bll.LoadNegocios(dto);
                    t1 = new Thread(WaitLoad);
                    t1.Start();
                }));
            }).Start();
            
        }
        #endregion

        private void WaitLoad()
        {
            Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
            {
                syncEvent.WaitOne();
                Load();
                wb.Close();
            }));
            
        }

        private void Load()
        {
            grdItens.ItemsSource = listaNegocios;
        }

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

        


        #endregion


        #endregion


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
    }
}
