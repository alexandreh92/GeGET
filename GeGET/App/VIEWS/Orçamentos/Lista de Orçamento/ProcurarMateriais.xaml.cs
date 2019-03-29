using System;
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
    public partial class ProcurarMateriais : Window, IDisposable
    {
        #region Declarations
        AdicionarItemOrcamentoBLL bll = new AdicionarItemOrcamentoBLL();
        AdicionarItemOrcamentoDTO dto = new AdicionarItemOrcamentoDTO();
        public ObservableCollection<AdicionarItemOrcamentoDTO> listaItens;
        ManualResetEvent syncEvent = new ManualResetEvent(false);
        WaitBox wb;
        Thread t1;
        Thread t2;

        string Atividade_Id;
        string Negocio_Id;
        #endregion

        #region Initialize
        public ProcurarMateriais(ListaOrcamentosDTO DTO)
        {
            InitializeComponent();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            Atividade_Id = DTO.Atividade_Id;
            Negocio_Id = DTO.Id;
            t1 = new Thread(Load);
            t1.Start();
        }
        #endregion

        #region Methods
        #region On Load
        private void Load()
        {
            Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
            {
                syncEvent.Set();
                t2 = new Thread(waitLoad);
                t2.Start();
                listaItens = bll.LoadItens();
                grdItens.ItemsSource = listaItens;
            }));
        }
        #endregion

        #region waitLoad
        private void waitLoad()
        {
            syncEvent.WaitOne();
            Dispatcher.Invoke(new Action(() =>
            {
                progressbar.Visibility = Visibility.Collapsed;
                grdItens.Visibility = Visibility.Visible;
            }));
        }
        #endregion

        #region Commit
        private void Commit()
        {
            Dispatcher.Invoke(DispatcherPriority.Background,
                  new Action(() =>
                  {
                      var Find = txtFind.Text.ToLower().RemoveDiacritics().Split(' ').ToList();
                      var filtered = listaItens.Where(descricao => Find.Any(list => descricao.Id.ToString().Contains(list) || descricao.Descricao.ToLower().RemoveDiacritics().Contains(list) || descricao.Descricao_Produto.RemoveDiacritics().ToLower().Contains(list) || descricao.Partnumber.RemoveDiacritics().ToLower().Contains(list)));
                      grdItens.ItemsSource = filtered;
                  }));
        }
        #endregion

        private void WaitBoxLoad()
        {
            syncEvent.WaitOne();
            Dispatcher.Invoke(new Action(() =>
            {
                wb.Close();
            }));
        }
        #endregion

        #region Events

        #region Clicks
        private void BtnLimparPesaquisa_Click(object sender, RoutedEventArgs e)
        {
            txtFind.Text = "";
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            new Thread(() =>
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    wb = new WaitBox();
                    wb.Show();
                }));
                syncEvent.Set();
                foreach (var item in listaItens)
                {

                    if (item.IsSelected)
                    {


                        Dispatcher.Invoke(DispatcherPriority.Background,
                      new Action(() =>
                      {
                          dto.Id = item.Id;
                          dto.Atividade_Id = Atividade_Id;
                          dto.Negocio_Id = Negocio_Id;
                          dto.Descricao_Produto = item.Descricao_Produto;
                          bll.Add(dto);
                          item.IsSelected = false;
                      }));

                    }
                }
                t1 = new Thread(WaitBoxLoad);
                t1.Start();
            }).Start();

        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion

        #region TxtFind Text Changed
        private void TxtFind_TextChanged(object sender, TextChangedEventArgs e)
        {
            t1 = new Thread(Commit);
            t1.Start();
        }
        #endregion

        #region Grid Keydown Ctrl+F
        private void GrdItens_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F && Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (boxPesquisar.Visibility == Visibility.Collapsed)
                {
                    boxPesquisar.Visibility = Visibility.Visible;
                }
                else
                {
                    boxPesquisar.Visibility = Visibility.Collapsed;
                }
            }
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
