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
    public partial class FunilOld : UserControl
    {
        FunildeVendasBLL bll = new FunildeVendasBLL();
        Control teste;
        Thread t1;
        Thread t2;
        ManualResetEvent syncEvent = new ManualResetEvent(false);

        public FunilOld()
        {
            InitializeComponent();
            t1 = new Thread(Load);
            t1.Start();
        }

        private void Load()
        {
          /*  // 1 = na fila, 2 = em andamento, 3 = enviado ao cliente, 4 = negociacao, 5 = fechado, 6 = cancelado.
            syncEvent.Set();

            Dispatcher.Invoke(new Action(() =>
            {
                w1 = new Window1();
                w1.Show();
            }));


            //fazer dataset com os 6 com 6 metodos diferentes.
            Dispatcher.Invoke(DispatcherPriority.Background,
                  new Action(() =>
                  {
                      pnFila.ItemsSource = bll.LoadNegocios(1);
                      pnExecucao.ItemsSource = bll.LoadNegocios(2);
                      pnEnviados.ItemsSource = bll.LoadNegocios(3);
                      pnNegociacao.ItemsSource = bll.LoadNegocios(4);
                      pnVendidos.ItemsSource = bll.LoadNegocios(5);
                      pnCancelados.ItemsSource = bll.LoadNegocios(6);
                  }));
            


            t2 = new Thread(CloseWindow);
            t2.Start();
            /*
            syncEvent.Set();
            var negocios = bll.LoadNegocios();

            Dispatcher.Invoke(new Action(() =>
            {
                w1 = new Window1();
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
                
            */
            
        }

        private void CloseWindow()
        {
            syncEvent.WaitOne();
            Dispatcher.Invoke(new Action(() =>
            {
                /*w1.Close();*/
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
        {/*
            int target_Id;
            string source_Id;
            string Id;
            string source_name;
            //target panel
            ListBox source = (ListBox)sender;

            //MessageBox.Show(source.Name);

            var a = ((FunilControl)e.Data.GetData(typeof(FunilControl)));

            Id = a.Id;
            source_Id = a.Status_Id;
            source_name = a.Name;


            // switch em int com o nome do painel
            // bll executacmd passando id e status_id
            // guarda variavel inicial para dar refresh no primeiro painel e da switch tb
            // 

            switch (source.Name)
            {
                case "pnFila":
                    target_Id = 1;
                    break;
                case "pnExecucao":
                    target_Id = 2;
                    break;
                case "pnEnviados":
                    target_Id = 3;
                    break;
                case "pnNegociacao":
                    target_Id = 4;
                    break;
                case "pnVendidos":
                    target_Id = 5;
                    break;
                case "pnCancelados":
                    target_Id = 6;
                    break;
                default:
                    target_Id = 0;
                    break;
            }

            bll.UpdateStatus(Id, target_Id);

            ListBox teste = (ListBox)((object)source_name);

            MessageBox.Show(teste.ToString());

            switch (source_Id)
            {
                default:
                    break;
            }


            // parentcollection.Remove(a);


            // source.Children.Add(a);
            */
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
