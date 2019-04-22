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
    public partial class ProcurarGrupoFornecedor : Window, IDisposable
    {
        #region Declarations
        GrupoFornecedoresBLL bll = new GrupoFornecedoresBLL();
        GrupoFornecedoresDTO dto = new GrupoFornecedoresDTO();
        Thread t1;
        Thread t2;
        DispatcherTimer timer = new DispatcherTimer();
        ManualResetEvent syncEvent = new ManualResetEvent(false);
        public ObservableCollection<GrupoFornecedoresDTO> listaGrupoFornecedores;
        public string Grupo_Id;
        public string Grupo_Descricao;
        #endregion

        #region Initialize
        public ProcurarGrupoFornecedor(Point mouseLocation)
        {
            InitializeComponent();
            timer.Tick += new EventHandler(DispatcherTimer_Tick);
            timer.Interval = TimeSpan.FromMilliseconds(310);
            timer.Start();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            ColLeft.Width = new GridLength(mouseLocation.X + 230, GridUnitType.Pixel);
            
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
                         listaGrupoFornecedores = bll.LoadGrupoFornecedores();
                         lstMensagens.ItemsSource = listaGrupoFornecedores;
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
                      var filtered = listaGrupoFornecedores.Where(descricao => Find.Any(list => descricao.Descricao.ToLower().RemoveDiacritics().Contains(list) || descricao.Id.ToString().ToLower().RemoveDiacritics().Contains(list)));
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
            Grupo_Id = ((GrupoFornecedoresDTO)lstMensagens.Items[index]).Id.ToString();
            Grupo_Descricao = ((GrupoFornecedoresDTO)lstMensagens.Items[index]).Descricao;
            DialogResult = true;
        }
        #endregion

        #region Window Load
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
