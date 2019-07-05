using System;
using System.Windows;
using DTO;
using BLL;
using System.Collections.ObjectModel;
using System.Threading;

namespace GeGET
{
    public partial class AlterarBDIOrcamento : Window, IDisposable
    {
        ListaOrcamentosDTO dto = new ListaOrcamentosDTO();
        ListaOrcamentosBLL bll = new ListaOrcamentosBLL();
        ObservableCollection<MaterialDTO> listaAlterar = new ObservableCollection<MaterialDTO>();
        WaitBox wb;
        ManualResetEvent syncEvent = new ManualResetEvent(false);
        Thread t1;
        bool disposed = false;
        public AlterarBDIOrcamento(ObservableCollection<MaterialDTO> dTOs)
        {
            InitializeComponent();
            foreach (MaterialDTO item in dTOs)
            {
                listaAlterar.Add(new MaterialDTO { Id = item.Id });
            }
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Update()
        {
            new Thread(() => 
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    dto.Bdi = Convert.ToDouble(txtBDI.Text);
                    wb = new WaitBox();
                    wb.Show();
                }));
                syncEvent.Set();
                
                foreach (var item in listaAlterar)
                {
                    syncEvent.Set();
                    dto.Id = item.Id;
                    bll.AtualizarBDI(dto);
                    syncEvent.WaitOne();
                }
                t1 = new Thread(WaitBoxLoad);
                t1.Start();
            }).Start();
            
        }

        private void WaitBoxLoad()
        {
            syncEvent.WaitOne();
            Dispatcher.Invoke(new Action(() =>
            {
                wb.Close();
                this.DialogResult = true;
                this.Close();
            }));
        }

        private void BtnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            var result = CustomOKCancelMessageBox.Show("Você deseja mesmo aterar o BDI para '" + txtBDI.Text + "'% ?","Atenção!", Window.GetWindow(this));
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                Update();
            }
            
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
                syncEvent.Dispose();
                bll.Dispose();
            }
            disposed = true;
        }
        #endregion
    }
}
