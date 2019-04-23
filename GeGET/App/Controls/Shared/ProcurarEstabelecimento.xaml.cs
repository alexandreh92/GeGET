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
    public partial class ProcurarEstabelecimento : Window, IDisposable
    {
        #region Declarations
        EstabelecimentosBLL bll = new EstabelecimentosBLL();
        EstabelecimentosDTO dto = new EstabelecimentosDTO();
        Thread t1;
        Thread t2;
        DispatcherTimer timer = new DispatcherTimer();
        ManualResetEvent syncEvent = new ManualResetEvent(false);
        public ObservableCollection<EstabelecimentosDTO> listaEstabelecimentos;
        public string Estabelecimento_Id;
        public string Endereco;
        public string Cidade;
        public string UF;
        public string CNPJ;
        #endregion

        #region Initialize
        public ProcurarEstabelecimento(Point mouseLocation, NegociosDTO DTO)
        {
            InitializeComponent();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            timer.Tick += new EventHandler(DispatcherTimer_Tick);
            timer.Interval = TimeSpan.FromMilliseconds(310);
            timer.Start();
            ColLeft.Width = new GridLength(mouseLocation.X, GridUnitType.Pixel);
            dto.Cliente_Id = DTO.Cliente_Id;
            
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
                         listaEstabelecimentos = bll.LoadEstabelecimentosFromClient(dto);
                         lstMensagens.ItemsSource = listaEstabelecimentos;
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
                      var filtered = listaEstabelecimentos.Where(descricao => Find.All(list => descricao.Razao_Social.ToLower().RemoveDiacritics().Contains(list) || descricao.Nome_Fantasia.ToLower().RemoveDiacritics().Contains(list) || descricao.Endereco.ToLower().Contains(list) || descricao.Cidade.ToLower().Contains(list) || descricao.Cnpj.ToLower().Contains(list)));
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
            Estabelecimento_Id = ((EstabelecimentosDTO)lstMensagens.Items[index]).Id;
            Endereco = ((EstabelecimentosDTO)lstMensagens.Items[index]).Endereco;
            Cidade = ((EstabelecimentosDTO)lstMensagens.Items[index]).Cidade;
            UF = ((EstabelecimentosDTO)lstMensagens.Items[index]).UF;
            CNPJ = ((EstabelecimentosDTO)lstMensagens.Items[index]).Cnpj;
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
