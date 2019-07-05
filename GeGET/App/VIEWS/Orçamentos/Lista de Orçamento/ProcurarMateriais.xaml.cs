﻿using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using BLL;
using DTO;

namespace GeGET
{
    public partial class ProcurarMateriais : Window, IDisposable
    {
        #region Declarations
        AdicionarItemOrcamentoBLL bll = new AdicionarItemOrcamentoBLL();
        AdicionarItemOrcamentoDTO dto = new AdicionarItemOrcamentoDTO();
        InformacoesListaOrcamentosDTO informacoesDTO = new InformacoesListaOrcamentosDTO();
        public ObservableCollection<AdicionarItemOrcamentoDTO> listaItens;
        ManualResetEvent syncEvent = new ManualResetEvent(false);
        WaitBox wb;
        Thread t1;
        Thread t2;
        bool disposed = false;
        string Atividade_Id;
        string Negocio_Id;
        #endregion

        #region Initialize
        public ProcurarMateriais(InformacoesListaOrcamentosDTO DTO)
        {
            InitializeComponent();
            informacoesDTO = DTO;
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            Atividade_Id = DTO.Atividade_Id;
            Negocio_Id = DTO.Id.ToString();
            t1 = new Thread(Load);
            t1.Start();
        }
        #endregion

        #region Methods
        #region On Load
        private void Load()
        {
            Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
            {
                syncEvent.Set();
                t2 = new Thread(WaitLoad);
                t2.Start();
                listaItens = bll.LoadItens();
                grdItens.ItemsSource = listaItens;
            }));
        }
        #endregion

        #region waitLoad
        private void WaitLoad()
        {
            syncEvent.WaitOne();
            Dispatcher.Invoke(new Action(() =>
            {
                progressbar.Visibility = Visibility.Collapsed;
                grdItens.Visibility = Visibility.Visible;
            }));
        }
        #endregion

        private void WaitBoxLoad()
        {
            syncEvent.WaitOne();
            Dispatcher.Invoke(new Action(() =>
            {
                wb.Close();
            }));
        }
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

            }


            new Thread(() =>
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    wb = new WaitBox
                    {
                        Owner = Window.GetWindow(this)
                    };
                    wb.Show();
                }));
                syncEvent.Set();
                foreach (var rowHandle in handles)
                {
                    Dispatcher.Invoke(DispatcherPriority.Background,
                  new Action(() =>
                  {
                      var selectedItem = grdItens.GetRow(rowHandle) as AdicionarItemOrcamentoDTO;
                      dto.Id = Convert.ToInt32(selectedItem.Id);
                      dto.Atividade_Id = Atividade_Id;
                      dto.Negocio_Id = Negocio_Id;
                      dto.Descricao_Produto = selectedItem.Descricao_Produto;
                      bll.Add(informacoesDTO,dto);
                      grdItens.UnselectAll();
                  }));
                }
                t1 = new Thread(WaitBoxLoad);
                t1.Start();
            }).Start();
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
                syncEvent.Dispose();
                bll.Dispose();
            }
            disposed = true;
        }
        #endregion
    }
}
