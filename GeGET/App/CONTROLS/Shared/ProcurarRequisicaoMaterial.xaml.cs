﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using BLL;
using DTO;
using MMLib.Extensions;

namespace GeGET
{
    public partial class ProcurarRequisicaoMaterial : Window, IDisposable
    {
        #region Declarations
        bool disposed = false;
        ProcurarRequisicaoMaterialBLL bll = new ProcurarRequisicaoMaterialBLL();
        ProcurarRequisicaoMaterialDTO dto = new ProcurarRequisicaoMaterialDTO();
        public string Negocio_Id;
        DispatcherTimer timer = new DispatcherTimer();
        public string Venda_Id;
        public string RM_Id;
        public ObservableCollection<ProcurarRequisicaoMaterialDTO> listaRM;
        #endregion

        #region Initialize
        public ProcurarRequisicaoMaterial(Point mouseLocation)
        {
            InitializeComponent();
            timer.Tick += new EventHandler(DispatcherTimer_Tick);
            timer.Interval = TimeSpan.FromMilliseconds(310);
            timer.Start();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            ColLeft.Width = new GridLength(mouseLocation.X + 230, GridUnitType.Pixel);
            ColTop.Height = new GridLength(mouseLocation.Y, GridUnitType.Pixel);
            //Left = mouseLocation.X +240;
            //Top = mouseLocation.Y;
        }
        #endregion

        #region Methods

        #region Load
        private async void Load()
        {
            progressbar.Visibility = Visibility.Visible;
            await Task.Run(() =>
            {
                listaRM = bll.LoadRM();
            });
            lstMensagens.ItemsSource = listaRM;
            progressbar.Visibility = Visibility.Collapsed;
            lstMensagens.Visibility = Visibility.Visible;
        }
        #endregion

        #region Commit
        private void Commit()
        {
            Dispatcher.Invoke(DispatcherPriority.Background,
                  new Action(() =>
                  {
                      var Find = txtProcurar.Text.ToLower().RemoveDiacritics().Split(' ').ToList();
                      var filtered = listaRM.Where(descricao => Find.All(list => descricao.Razao_Social.ToLower().RemoveDiacritics().Contains(list) || descricao.Descricao.ToLower().RemoveDiacritics().Contains(list) || descricao.Endereco.ToLower().Contains(list) || descricao.Id.ToString().Contains(list) || descricao.Negocio_Id.ToString().ToLower().Contains(list) || descricao.Vendas_Id.ToString().ToLower().Contains(list)));
                      lstMensagens.ItemsSource = filtered;
                  }));
        }
        #endregion

        #endregion

        #region Events

        #region Timer Tick
        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            Load();
        }
        #endregion

        #region Window Loaded
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtProcurar.Focus();
        }
        #endregion

        #region TextChanged
        private async void TxtProcurar_TextChanged(object sender, TextChangedEventArgs e)
        {
            await Task.Run(() => Commit());
        }
        #endregion

        #region Clicks

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int index = lstMensagens.Items.IndexOf(btn.DataContext);
            Negocio_Id = ((ProcurarRequisicaoMaterialDTO)lstMensagens.Items[index]).Negocio_Id.ToString();
            Venda_Id = ((ProcurarRequisicaoMaterialDTO)lstMensagens.Items[index]).Vendas_Id.ToString();
            RM_Id = ((ProcurarRequisicaoMaterialDTO)lstMensagens.Items[index]).Id.ToString();
            DialogResult = true;
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