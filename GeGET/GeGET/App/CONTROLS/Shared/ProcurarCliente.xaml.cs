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
    public partial class ProcurarCliente : Window, IDisposable
    {
        #region Declarations
        ClientesBLL bll = new ClientesBLL();
        ClientesDTO dto = new ClientesDTO();
        Thread t1;
        Thread t2;
        ManualResetEvent syncEvent = new ManualResetEvent(false);
        public ObservableCollection<ClientesDTO> listaClientes;
        public string new_Cliente_Id;
        public string new_Razao_Social;
        public string new_Nome_Fantasia;
        #endregion

        #region Initialize
        public ProcurarCliente(Point mouseLocation)
        {
            InitializeComponent();
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
                         listaClientes = bll.LoadClientes();
                         lstMensagens.ItemsSource = listaClientes;
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
                      var filtered = listaClientes.Where(descricao => Find.Any(list => descricao.Razao_Social.ToLower().RemoveDiacritics().Contains(list) || descricao.Nome_Fantasia.ToLower().RemoveDiacritics().Contains(list) || descricao.Categoria.RemoveDiacritics().ToLower().Contains(list)));
                      lstMensagens.ItemsSource = filtered;
                  }));
        }
        #endregion

        #region Events

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
            new_Cliente_Id = ((ClientesDTO)lstMensagens.Items[index]).Id;
            new_Razao_Social = ((ClientesDTO)lstMensagens.Items[index]).Razao_Social;
            new_Nome_Fantasia = ((ClientesDTO)lstMensagens.Items[index]).Nome_Fantasia;
            DialogResult = true;
        }
        #endregion

        #endregion

        #region IDisposable
        void IDisposable.Dispose()
        {
        }
        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtProcurar.Focus();
        }
    }
}
