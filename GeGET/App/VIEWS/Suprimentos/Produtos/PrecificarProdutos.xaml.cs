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
using DevExpress.Xpf.Grid;
using System.Windows.Input;

namespace GeGET
{
    public partial class PrecificarProdutos : UserControl, IDisposable
    {
        #region Declarations
        bool disposed = false;
        Helpers helpers = new Helpers();
        ProdutosBLL bll = new ProdutosBLL();
        ProdutosDTO dto = new ProdutosDTO();
        ManualResetEvent syncEvent = new ManualResetEvent(false);
        Thread t1;
        WaitBox wb;
        ObservableCollection<ProdutosDTO> listaProdutos = new ObservableCollection<ProdutosDTO>();
        #endregion

        #region Initialize
        public PrecificarProdutos()
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
                    listaProdutos = bll.LoadProdutos();
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
                grdItens.ItemsSource = listaProdutos;
                wb.Close();
            }));
            
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

        private void GrdView_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            var row = e.Row as ProdutosDTO;

            new Thread(() =>
            {

                Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                {
                    if (e.Column.FieldName.ToString() == "Anotacoes")
                    {
                        dto.Id = row.Id;
                        if (row.Anotacoes != null)
                        {
                            dto.Anotacoes = row.Anotacoes.Replace("'", "''").ToUpper();
                        }
                        else
                        {
                            dto.Anotacoes = "";
                        }
                        bll.P_UpdateAnotacoes(dto);
                    }
                    else if (e.Column.FieldName.ToString() == "Partnumber")
                    {
                        dto.Id = row.Id;
                        if (row.Partnumber != null)
                        {
                            dto.Partnumber = row.Partnumber.Replace("'", "''").ToUpper();
                        }
                        else
                        {
                            dto.Partnumber = "";
                        }
                        bll.P_UpdatePartnumber(dto);
                    }
                    else if (e.Column.FieldName.ToString() == "Ncm")
                    {
                        dto.Id = row.Id;
                        if (row.Ncm != null)
                        {
                            dto.Ncm = row.Ncm.Replace("'", "''").ToUpper();
                        }
                        else
                        {
                            dto.Ncm = "";
                        }
                        bll.P_UpdateNcm(dto);
                    }
                    else if (e.Column.FieldName.ToString() == "Custo")
                    {
                        dto.Id = row.Id;
                        dto.Custo = row.Custo;
                        bll.P_UpdateCusto(dto);
                    }
                    else if (e.Column.FieldName.ToString() == "Ipi")
                    {
                        dto.Id = row.Id;
                        dto.Ipi = row.Ipi;
                        bll.P_UpdateIpi(dto);
                    }
                    else if (e.Column.FieldName.ToString() == "Icms")
                    {
                        dto.Id = row.Id;
                        dto.Icms = row.Icms;
                        bll.P_UpdateIcms(dto);
                    }
                }));
            }).Start();
        }

        private void GrdView_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ((TableView)grdItens.View).MoveNextRow();
                ((TableView)grdItens.View).ShowEditor();
            }
        }

        private void GrdView_ShownEditor(object sender, DevExpress.Xpf.Grid.EditorEventArgs e)
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
                bll.Dispose();
                syncEvent.Dispose();
            }
            disposed = true;
        }
        #endregion
    }
}
