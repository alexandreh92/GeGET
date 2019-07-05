using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using BLL;
using DTO;
using MaterialDesignThemes.Wpf;

namespace GeGET
{
    /// <summary>
    /// Interaction logic for Teste.xaml
    /// </summary>
    public partial class Funil : UserControl, IDisposable
    {
        bool disposed = false;
        FunildeVendasBLL bll = new FunildeVendasBLL();
        Control teste;
        Thread t1;
        WaitBox w1;
        ManualResetEvent syncEvent = new ManualResetEvent(false);

        public Funil()
        {
            InitializeComponent();
            t1 = new Thread(Load);
            t1.Start();
        }

        private void Load()
        {

            syncEvent.Set();
            var negocios = bll.LoadNegocios();

            Dispatcher.Invoke(new Action(() =>
            {
                w1 = new WaitBox();
                w1.Show();
            }));

            Thread.Sleep(500);
            
            foreach (FunildeVendasDTO item in negocios)
            {
                Dispatcher.Invoke(DispatcherPriority.Background,
                  new Action(() =>
                  {

                      FunilControl uc = new FunilControl(item.Id,item.Descricao,item.Prazo,item.Cidade,item.Fantasia,item.Descricao_Status,item.Status_Orcamento);

                    uc.PreviewMouseLeftButtonDown += NF_MouseDownParent;

                    switch (item.Status_Orcamento)
                    {
                        case 1:
                            pnFila.Children.Add(uc);
                            break;
                        case 2:
                            pnExecucao.Children.Add(uc);
                            break;
                        case 3:
                            pnEnviados.Children.Add(uc);
                            break;
                        case 4:
                            pnNegociacao.Children.Add(uc);
                            break;
                        case 5:
                            pnVendidos.Children.Add(uc);
                            break;
                        case 6:
                            pnCancelados.Children.Add(uc);
                            break;
                    }

                  }));
        }
                
            
            
        }

        private void CloseWindow()
        {
            syncEvent.WaitOne();
            Dispatcher.Invoke(new Action(() =>
            {
                
                
                w1.Close();
            }));
        }

        private void NF_MouseDownParent(object sender, MouseButtonEventArgs e)
        {
            teste = ((Control)sender);
            DataObject dataObject = new DataObject(teste);
            DragDrop.DoDragDrop(teste, dataObject, DragDropEffects.Move);
        }

        void panel_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
        }

        private void PnFila_Drop(object sender, DragEventArgs e)
        {
            StackPanel source = (StackPanel)sender;
            var a = ((FunilControl)e.Data.GetData(typeof(FunilControl)));
            
            var element = ((FunilControl)e.Data.GetData(typeof(FunilControl))) as FrameworkElement;
            var parentcollection = (element.Parent as StackPanel).Children as UIElementCollection;


            parentcollection.Remove(a);
            
            
            source.Children.Add(a);
            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

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
