using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using BLL;
using DTO;
using MMLib.Extensions;

namespace GeGET
{
    public partial class AdicionarItemListaManual : Window, IDisposable
    {
        #region Declarations
        EntradaManualEstoqueBLL bll = new EntradaManualEstoqueBLL();
        EntradaManualEstoqueDTO dto = new EntradaManualEstoqueDTO();
        public ObservableCollection<EntradaManualEstoqueDTO> listaItens;
        public ObservableCollection<EntradaManualEstoqueDTO> listaEntrada = new ObservableCollection<EntradaManualEstoqueDTO>();
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
        void IDisposable.Dispose()
        {
        }
        #endregion
    }
}
