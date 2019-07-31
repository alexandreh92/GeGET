using BLL;
using DevExpress.XtraPrinting;
using DTO;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GeGET
{
    public partial class RelatorioItens : UserControl, IDisposable
    {
        #region Declarations
        private bool disposed = false;
        private Helpers helpers = new Helpers();
        private RelatorioItensBLL bll = new RelatorioItensBLL();
        public ObservableCollection<RelatorioItensDTO> listaItens;
        #endregion

        #region Initialize
        public RelatorioItens()
        {
            InitializeComponent();
            Load();
        }
        #endregion

        #region Methods


        private async void Load()
        {
            await Task.Run(() =>
            {
                listaItens = bll.ListaItens();
            });
            grdItens.ItemsSource = listaItens;
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
                FileName = "Relatório de Grupo de Itens",
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
            {
                return;
            }

            if (disposing)
            {

            }
            disposed = true;
        }
        #endregion
    }
}
