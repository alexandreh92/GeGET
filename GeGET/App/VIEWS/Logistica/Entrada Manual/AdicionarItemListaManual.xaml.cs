using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace GeGET
{
    public partial class AdicionarItemListaManual : Window, IDisposable
    {
        #region Declarations
        EntradaManualEstoqueBLL bll = new EntradaManualEstoqueBLL();
        EntradaManualEstoqueDTO dto = new EntradaManualEstoqueDTO();
        public ObservableCollection<EntradaManualEstoqueDTO> listaItens;
        public ObservableCollection<EntradaManualEstoqueDTO> listaEntrada = new ObservableCollection<EntradaManualEstoqueDTO>();
        bool disposed = false;
        #endregion

        #region Initialize
        public AdicionarItemListaManual()
        {
            InitializeComponent();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            Load();
        }
        #endregion

        #region Methods
        #region Load
        private async void Load()
        {
            await Task.Run(() => 
            {
                listaItens = bll.LoadItens();
            });
            grdItens.ItemsSource = listaItens;
            progressbar.Visibility = Visibility.Collapsed;
            grdItens.Visibility = Visibility.Visible;
        }
        #endregion


        #endregion

        #region Events

        #region Clicks

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            int[] handles = grdItens.GetSelectedRowHandles();
            if (handles.Length > 0)
            {
                foreach (var rowHandle in handles)
                {
                    dto = grdItens.GetRow(rowHandle) as EntradaManualEstoqueDTO;
                    listaEntrada.Add(dto);
                }
            }
            DialogResult = true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
            }
            disposed = true;
        }

        #endregion
    }
}
