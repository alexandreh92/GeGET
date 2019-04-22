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
    public partial class ProcurarItem : Window, IDisposable
    {
        #region Declarations
        ItensBLL bll = new ItensBLL();
        ItensDTO dto = new ItensDTO();
        Thread t1;
        Thread t2;
        DispatcherTimer timer = new DispatcherTimer();
        ManualResetEvent syncEvent = new ManualResetEvent(false);
        public ObservableCollection<ItensDTO> listaItens;
        public string Item_Id;
        public string Item_Descricao;
        public string Item_Unidade;
        #endregion

        #region Initialize
        public ProcurarItem(Point mouseLocation)
        {
            InitializeComponent();
            timer.Tick += new EventHandler(DispatcherTimer_Tick);
            timer.Interval = TimeSpan.FromMilliseconds(310);
            timer.Start();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            ColLeft.Width = new GridLength(mouseLocation.X + 230, GridUnitType.Pixel);
            t1 = new Thread(Load);
            t1.Start();
            Left = mouseLocation.X;
            Top = mouseLocation.Y - 50;
        }
        #endregion

        #region Methods
        private void Load()
        {
            Dispatcher.Invoke(DispatcherPriority.Background,
                     new Action(() =>
                     {
                         progressbar.Visibility = Visibility.Visible;
                         syncEvent.Set();
                         t2 = new Thread(waitLoad);
                         t2.Start();
                         listaItens = bll.LoadItens();
                         lstMensagens.ItemsSource = listaItens;
                     }));

        }

        private void waitLoad()
        {
            syncEvent.WaitOne();
            Dispatcher.Invoke(new Action(() =>
            {
                progressbar.Visibility = Visibility.Collapsed;
                lstMensagens.Visibility = Visibility.Visible;
            }));
        }

        private void Commit()
        {
            Dispatcher.Invoke(DispatcherPriority.Background,
                  new Action(() =>
                  {
                      var Find = txtProcurar.Text.ToLower().RemoveDiacritics().Split(' ').ToList();
                      var filtered = listaItens.Where(descricao => Find.Any(list => descricao.Descricao.ToLower().RemoveDiacritics().Contains(list) || descricao.Un.ToString().ToLower().RemoveDiacritics().Contains(list)));
                      lstMensagens.ItemsSource = filtered;
                  }));
        }
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
            Item_Id = ((ItensDTO)lstMensagens.Items[index]).Id.ToString();
            Item_Descricao = ((ItensDTO)lstMensagens.Items[index]).Descricao;
            Item_Unidade = ((ItensDTO)lstMensagens.Items[index]).Un;
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
