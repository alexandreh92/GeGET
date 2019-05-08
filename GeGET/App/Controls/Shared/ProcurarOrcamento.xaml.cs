using System;
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
    public partial class ProcurarOrcamento : Window, IDisposable
    {
        #region Declarations
        NegociosBLL bll = new NegociosBLL();
        NegociosDTO dto = new NegociosDTO();
        public string Negocio_Id;
        DispatcherTimer timer = new DispatcherTimer();
        public ObservableCollection<NegociosDTO> listaNegocios;
        #endregion

        #region Initialize
        public ProcurarOrcamento(Point mouseLocation)
        {
            InitializeComponent();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            timer.Tick += new EventHandler(DispatcherTimer_Tick);
            timer.Interval = TimeSpan.FromMilliseconds(310);
            timer.Start();
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
                listaNegocios = bll.LoadOrcamentos(dto);
            });
            lstMensagens.ItemsSource = listaNegocios;
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
                      var filtered = listaNegocios.Where(descricao => Find.All(list => descricao.Razao_Social.ToLower().RemoveDiacritics().Contains(list) || descricao.Descricao.ToLower().RemoveDiacritics().Contains(list) || descricao.Endereco.ToLower().Contains(list) || descricao.Anotacoes.ToLower().Contains(list) || descricao.Vendedor.ToLower().Contains(list) || descricao.CidadeEstado.ToLower().Contains(list) || descricao.Status_Descricao.ToLower().Contains(list) || descricao.Id.ToLower().Contains(list) || descricao.Numero.ToLower().Contains(list)));
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

        #region TextChanged
        private async void TxtProcurar_TextChanged(object sender, TextChangedEventArgs e)
        {
            await Task.Run(() => Commit());
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
