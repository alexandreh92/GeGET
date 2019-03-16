using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using BLL;
using DTO;
using MMLib.Extensions;

namespace GeGET
{
    public partial class ProcurarMateriais : Window, IDisposable
    {
        private ListaOrcamento m_MainForm;
        AdicionarItemOrcamentoBLL bll = new AdicionarItemOrcamentoBLL();
        AdicionarItemOrcamentoDTO dto = new AdicionarItemOrcamentoDTO();
        public ObservableCollection<AdicionarItemOrcamentoDTO> listaItens;
        ManualResetEvent syncEvent = new ManualResetEvent(false);
        Thread t1;
        Thread t2;

        string Atividade_Id;
        string Negocio_Id;

        public ProcurarMateriais(ListaOrcamento form, ListaOrcamentosDTO DTO)
        {
            m_MainForm = form;
            InitializeComponent();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            Atividade_Id = DTO.Atividade_Id;
            Negocio_Id = DTO.Id;
            t1 = new Thread(Load);
            t1.Start();
        }

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

        private void waitLoad()
        {
            syncEvent.WaitOne();
            Dispatcher.Invoke(new Action(() =>
            {
                progressbar.Visibility = Visibility.Collapsed;
                grdItens.Visibility = Visibility.Visible;
            }));
        }

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


        void IDisposable.Dispose()
        {
        }

        private void BtnLimparPesaquisa_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        #region KeyDown Show Search Textbox
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

        private void GrdItens_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_GotKeyboardFocus(Object sender, KeyboardFocusChangedEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void TxtFind_TextChanged(object sender, TextChangedEventArgs e)
        {
            t1 = new Thread(Commit);
            t1.Start();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in listaItens)
            {
                
                      if (item.IsSelected)
                      {
                    new Thread(() =>
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
                    }).Start();
                }
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
