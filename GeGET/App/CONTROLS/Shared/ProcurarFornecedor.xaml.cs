﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using BLL;
using DTO;
using MMLib.Extensions;

namespace GeGET
{
    public partial class ProcurarFornecedor : Window, IDisposable
    {
        #region Declarations
        FornecedoresBLL bll = new FornecedoresBLL();
        FornecedoresDTO dto = new FornecedoresDTO();
        Thread t1;
        DispatcherTimer timer = new DispatcherTimer();
        ManualResetEvent syncEvent = new ManualResetEvent(false);
        public ObservableCollection<FornecedoresDTO> listaFornecedores;
        public string Fornecedor_Id;
        public string Fornecedor_Descricao;
        bool disposed = false;
        #endregion

        #region Initialize
        public ProcurarFornecedor(Point mouseLocation)
        {
            InitializeComponent();
            timer.Tick += new EventHandler(DispatcherTimer_Tick);
            timer.Interval = TimeSpan.FromMilliseconds(310);
            timer.Start();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            ColLeft.Width = new GridLength(mouseLocation.X, GridUnitType.Pixel);
            Left = mouseLocation.X;
            Top = mouseLocation.Y - 50;
        }
        #endregion

        #region Methods

        private async void Load()
        {
            progressbar.Visibility = Visibility.Visible;
            await Task.Run(() => 
            {
                listaFornecedores = bll.LoadFornecedores();
            });
            lstMensagens.ItemsSource = listaFornecedores;
            progressbar.Visibility = Visibility.Collapsed;
            lstMensagens.Visibility = Visibility.Visible;
        }

        private void Commit()
        {
            Dispatcher.Invoke(DispatcherPriority.Background,
                  new Action(() =>
                  {
                      var Find = txtProcurar.Text.ToLower().RemoveDiacritics().Split(' ').ToList();
                      var filtered = listaFornecedores.Where(descricao => Find.All(list => descricao.Razao_Social.ToLower().RemoveDiacritics().Contains(list) || descricao.Nome_Fantasia.ToLower().RemoveDiacritics().Contains(list) || descricao.Endereco.ToLower().Contains(list) || descricao.CidadeEstado.ToLower().Contains(list) || descricao.Cnpj.ToLower().Contains(list)));
                      lstMensagens.ItemsSource = filtered;
                  }));
        }
        #endregion

        #region Events

        #region Timer Tick
        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            Load();
        }
        #endregion

        #region Text Changed
        private void TxtProcurar_TextChanged(object sender, TextChangedEventArgs e)
        {
            t1 = new Thread(Commit);
            t1.Start();
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
            Fornecedor_Id = ((FornecedoresDTO)lstMensagens.Items[index]).Id.ToString();
            Fornecedor_Descricao = ((FornecedoresDTO)lstMensagens.Items[index]).Razao_Social;
            DialogResult = true;
        }
        #endregion

        #region Window Loaded
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtProcurar.Focus();
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
