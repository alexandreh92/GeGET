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
    public partial class MovimentoEntrada : UserControl, IDisposable
    {
        #region Declarations
        bool disposed = false;
        Helpers helpers = new Helpers();
        MovimentoEntradaBLL bll = new MovimentoEntradaBLL();
        WaitBox wb;
        ObservableCollection<MovimentoEntradaDTO> listaSaldo = new ObservableCollection<MovimentoEntradaDTO>();
        ObservableCollection<MovimentoEntradaDTO> listaExcluir;
        #endregion

        #region Initialize
        public MovimentoEntrada()
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
                listaSaldo = bll.Load();
            });
            grdItens.ItemsSource = listaSaldo;
            wb.Close();
        }
        #endregion

        #endregion

        #region Events

        #region Clicks

        private void ExportExcel_Click(object sender, RoutedEventArgs e)
        {
            //grd_item_id.Visible = true;
            //grd_fabricante_id.Visible = true;
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.FileName = "Saldo de Estoque - " + DateTime.Now.ToString("dd-MM-yyyy");
            fileDialog.Filter = "Arquivo Microsoft Excel (*.xlsx)|*.xlsx";
            if (fileDialog.ShowDialog() == true)
            {
                grdView.ExportToXlsx(fileDialog.FileName, new XlsxExportOptionsEx() { ExportType = DevExpress.Export.ExportType.Default });

            }
            //grd_item_id.Visible = false;
            //grd_fabricante_id.Visible = false;
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
            }
            disposed = true;
        }
        #endregion

        private async void EstornarSaida_Click(object sender, RoutedEventArgs e)
        {
            int[] handles = grdItens.GetSelectedRowHandles();
            if (handles.Length > 0)
            {
                var result = CustomOKCancelMessageBox.Show("Deseja mesmo excluir todos os itens selecionados?", "Atenção!", Window.GetWindow(this));
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    listaExcluir = new ObservableCollection<MovimentoEntradaDTO>();
                    foreach (var rowHandle in handles)
                    {
                        var selectedItem = grdItens.GetRow(rowHandle) as MovimentoEntradaDTO;
                        listaExcluir.Add(selectedItem);
                    }
                    wb = new WaitBox
                    {
                        Owner = Window.GetWindow(this)
                    };
                    wb.Show();
                    await Task.Run(() =>
                    {
                        bll.Excluir(listaExcluir);
                    });
                    wb.Close();
                    grdItens.UnselectAll();
                    Load();
                }
            }
            else
            {
                CustomOKMessageBox.Show("Você deve selecionar ao menos um item para excluir.", "Atenção!", Window.GetWindow(this));
            }
        }
    }
}
