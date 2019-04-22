using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using BLL;
using DTO;
using MMLib.Extensions;

namespace GeGET
{
    public partial class ProcurarOrcamentoCadastrado : Window, IDisposable
    {
        #region Declarations
        NegociosBLL bll = new NegociosBLL();
        NegociosDTO dto = new NegociosDTO();
        DispatcherTimer timer = new DispatcherTimer();
        public string Negocio_Id;
        public ObservableCollection<NegociosDTO> listaNegocios;
        ManualResetEvent syncEvent = new ManualResetEvent(false);
        Thread t1;
        Thread t2;
        #endregion

        #region Initialize
        public ProcurarOrcamentoCadastrado(Point mouseLocation)
        {
            InitializeComponent();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            timer.Tick += new EventHandler(DispatcherTimer_Tick);
            timer.Interval = TimeSpan.FromMilliseconds(310);
            timer.Start();
            dto.Pesquisa = "";
            t1 = new Thread(Load);
            t1.Start();
            ColLeft.Width = new GridLength(mouseLocation.X + 230, GridUnitType.Pixel);
            ColTop.Height = new GridLength(mouseLocation.Y, GridUnitType.Pixel);
            //Left = mouseLocation.X +240;
            //Top = mouseLocation.Y;
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
                         listaNegocios = bll.LoadOrcamentosCadastrados(dto);
                         lstMensagens.ItemsSource = listaNegocios;
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
                      var filtered = listaNegocios.Where(descricao => Find.Any(list => descricao.Razao_Social.ToLower().RemoveDiacritics().Contains(list) || descricao.Descricao.ToLower().RemoveDiacritics().Contains(list) || descricao.Endereco.ToLower().Contains(list) || descricao.Anotacoes.ToLower().Contains(list) || descricao.Vendedor.ToLower().Contains(list) || descricao.CidadeEstado.ToLower().Contains(list) || descricao.Status_Descricao.ToLower().Contains(list) || descricao.Id.ToLower().Contains(list) || descricao.Numero.ToLower().Contains(list)));
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
            Negocio_Id = ((NegociosDTO)lstMensagens.Items[index]).Id;
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
