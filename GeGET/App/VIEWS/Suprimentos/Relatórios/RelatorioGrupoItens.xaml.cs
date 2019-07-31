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
using System.Threading.Tasks;

namespace GeGET
{
    public partial class RelatorioGrupoItens : UserControl, IDisposable
    {
        #region Declarations
        bool disposed = false;
        Helpers helpers = new Helpers();
        RelatorioGrupoItensBLL bll = new RelatorioGrupoItensBLL();
        public ObservableCollection<RelatorioGrupoItensDTO> listaItens;
        #endregion

        #region Initialize
        public RelatorioGrupoItens()
        {
            InitializeComponent();
            Load();
        }
        #endregion

        #region Methods


        public async void Load()
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
                return;

            if (disposing)
            {

            }
            disposed = true;
        }
        #endregion
    }
}
