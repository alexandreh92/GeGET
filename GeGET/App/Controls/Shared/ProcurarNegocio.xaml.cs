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
    public partial class ProcurarNegocio : Window, IDisposable
    {
        #region Declarations
        NegociosBLL bll = new NegociosBLL();
        NegociosDTO dto = new NegociosDTO();
        public string Negocio_Id;
        public ObservableCollection<NegociosDTO> listaNegocios;
        ManualResetEvent syncEvent = new ManualResetEvent(false);
        Thread t1;
        Thread t2;
        #endregion

        #region Initialize
        public ProcurarNegocio(Point mouseLocation)
        {
            InitializeComponent();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            ColLeft.Width = new GridLength(mouseLocation.X + 230, GridUnitType.Pixel);
            dto.Pesquisa = "";
            t1 = new Thread(Load);
            t1.Start();
            Left = mouseLocation.X +240;
            Top = mouseLocation.Y;
        }
        #endregion

        private void Load()
        {

            Dispatcher.Invoke(DispatcherPriority.Background,
                     new Action(() =>
                     {
                         progressbar.Visibility = Visibility.Visible;
                         syncEvent.Set();
                         t2 = new Thread(waitLoad);
                         t2.Start();
                         listaNegocios = bll.LoadNegocios(dto);
                         lstMensagens.ItemsSource = listaNegocios;
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


        #region Methods
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

        #region Events

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

        #endregion

        #region IDisposable
        void IDisposable.Dispose()
        {
        }
        #endregion

        
    }
}
