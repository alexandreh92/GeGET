using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DTO;
using BLL;
using System.Collections.ObjectModel;
using System.Threading;

namespace GeGET
{
    public partial class AlterarBDIOrcamento : Window, IDisposable
    {
        MaterialDTO dto = new MaterialDTO();
        ListaOrcamentosBLL bll = new ListaOrcamentosBLL();
        ObservableCollection<MaterialDTO> listaAlterar = new ObservableCollection<MaterialDTO>();
        WaitBox wb;
        ManualResetEvent syncEvent = new ManualResetEvent(false);
        Thread t1;

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

        void IDisposable.Dispose()
        {
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
                    dto.Bdi = txtBDI.Text.Replace(",",".");
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
            var result = CustomOKCancelMessageBox.Show("Você deseja mesmo aterar o BDI para '" + txtBDI.Text + "'% ?","Atenção!");
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                Update();
            }
            
        }
    }
}
