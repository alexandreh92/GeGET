using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using BLL;
using DTO;
using MMLib.Extensions;

namespace GeGET
{
    public partial class ProcurarOrcamentista : Window, IDisposable
    {
        #region Declarations
        NegociosBLL bll = new NegociosBLL();
        NegociosDTO dto = new NegociosDTO();
        DispatcherTimer timer = new DispatcherTimer();
        public string Negocio_Id;
        public string Id;
        public string Login;
        public string Nome_Simples;
        public ObservableCollection<OrcamentistasDTO> listaOrcamentistas;
        ManualResetEvent syncEvent = new ManualResetEvent(false);
        Thread t1;
        Thread t2;
        #endregion

        #region Initialize
        public ProcurarOrcamentista(Point mouseLocation, NegociosDTO DTO)
        {
            InitializeComponent();
            timer.Tick += new EventHandler(DispatcherTimer_Tick);
            timer.Interval = TimeSpan.FromMilliseconds(310);
            timer.Start();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            dto.Pesquisa = "";
            dto.Id = DTO.Id;
            t1 = new Thread(Load);
            t1.Start();
            Left = mouseLocation.X +240;
            Top = mouseLocation.Y - 150;
        }
        #endregion

        #region Methods

        #region Load
        private void Load()
        {

            Dispatcher.Invoke(DispatcherPriority.Background,
                     new Action(() =>
                     {
                         progressbar.Visibility = Visibility.Visible;
                         syncEvent.Set();
                         t2 = new Thread(waitLoad);
                         t2.Start();
                         listaOrcamentistas = bll.LoadOrcamentistas(dto);
                         lstMensagens.ItemsSource = listaOrcamentistas;
                     }));
        }
        #endregion

        #region Wait Load
        private void waitLoad()
        {
            syncEvent.WaitOne();
            Dispatcher.Invoke(new Action(() =>
            {
                progressbar.Visibility = Visibility.Collapsed;
                lstMensagens.Visibility = Visibility.Visible;
            }));
        }
        #endregion

        #region Commit
        private void Commit()
        {
            Dispatcher.Invoke(DispatcherPriority.Background,
                  new Action(() =>
                  {
                      var Find = txtProcurar.Text.ToLower().RemoveDiacritics().Split(' ').ToList();
                      var filtered = listaOrcamentistas.Where(descricao => Find.Any(list => descricao.Nome.ToLower().RemoveDiacritics().Contains(list) || descricao.Celular.ToLower().RemoveDiacritics().Contains(list) || descricao.Email.ToLower().Contains(list) || descricao.Setor.ToLower().Contains(list)));
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
            t1 = new Thread(Load);
            t1.Start();
        }
        #endregion

        #region TextChanged
        private void TxtProcurar_TextChanged(object sender, TextChangedEventArgs e)
        {
            t1 = new Thread(Commit);
            t1.Start();
        }
        #endregion

        #region Clicks
        private void BtnPesquisa_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int index = lstMensagens.Items.IndexOf(btn.DataContext);
            Id = ((OrcamentistasDTO)lstMensagens.Items[index]).Id;
            Login = ((OrcamentistasDTO)lstMensagens.Items[index]).Login;
            Nome_Simples = ((OrcamentistasDTO)lstMensagens.Items[index]).Nome_Simples;
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
        void IDisposable.Dispose()
        {
        }

        #endregion
    }
}
