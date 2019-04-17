using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BLL;
using DTO;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Threading;
using MMLib.Extensions;
using System.Collections.Generic;

namespace GeGET
{
    public partial class ListaOrcamentoOld : UserControl
    {
        #region Declarations
        Helpers helpers = new Helpers();
        ListaOrcamentosBLL bll = new ListaOrcamentosBLL();
        ListaOrcamentosDTO dto = new ListaOrcamentosDTO();
        AdicionarItemOrcamentoDTO adicionarItemOrcamentoDTO = new AdicionarItemOrcamentoDTO();
        OrcamentosBLL Orcamentosbll = new OrcamentosBLL();
        OrcamentosDTO Orcamentosdto = new OrcamentosDTO();
        DisciplinaBLL Disciplinasbll = new DisciplinaBLL();
        DisciplinaDTO Disciplinasdto = new DisciplinaDTO();
        AtividadesBLL atividadesBLL = new AtividadesBLL();
        public ObservableCollection<ListaOrcamentosDTO> listaOrcamentos;
        ManualResetEvent syncEvent = new ManualResetEvent(false);
        Thread t1;
        WaitBox wb;
        ObservableCollection<CopiarItensOrcamentoDTO> listaCopiar;
        #endregion

        #region Initialize
        public ListaOrcamentoOld()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods

        private void Filter()
        {
            Dispatcher.Invoke(DispatcherPriority.Background,
                  new Action(() =>
                  {
                      var Find = txtFind.Text.ToLower().RemoveDiacritics().Split(' ').ToList();
                      var filtered = listaOrcamentos.Where(descricao => Find.Any(list => descricao.Descricao.ToLower().Contains(list) || descricao.Fabricante.ToLower().Contains(list)));
                      grdItens.ItemsSource = filtered;
                  }));
        }

        private void InitializeComponents()
        {
            if (CardPanel.Visibility != Visibility.Collapsed)
            {
                CardPanel.Visibility = Visibility.Collapsed;
            }
            if (sideExpander.Visibility != Visibility.Collapsed)
            {
                sideExpander.Visibility = Visibility.Collapsed;
            }
            txtNumero.Text = " ";
            txtCliente.Text = "";
            txtCategoria.Text = "";
            txtDescricao.Text = "";
            txtVersao.Text = "";
            txtCidade.Text = "";
            txtUF.Text = "";
            cmbAtividade.ItemsSource = null;
            cmbDisciplina.ItemsSource = null;
        }

        private void WaitBoxLoad()
        {
            syncEvent.WaitOne();
            Dispatcher.Invoke(new Action(() =>
            {
                listaOrcamentos = bll.LoadOrcamento(dto);
                grdItens.ItemsSource = listaOrcamentos;
                wb.Close();
            }));
        }

        private void Commit()
        {
            new Thread(() =>
            {
                Dispatcher.Invoke(DispatcherPriority.Background,
                  new Action(() =>
                  {
                      //bll.Update
                  }));

            });
        }

        #endregion

        #region Events

        #region Checkbox Checked/Unchecked
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            foreach (ListaOrcamentosDTO c in grdItens.ItemsSource)
            {
                c.IsSelected = true;
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (ListaOrcamentosDTO c in grdItens.ItemsSource)
            {
                c.IsSelected = false;
            }
        }
        #endregion

        #region Grid Itens Selected Begin Edit
        private void GrdItens_Selected(object sender, RoutedEventArgs e)
        {
            // Lookup for the source to be DataGridCell
            if (e.OriginalSource.GetType() == typeof(DataGridCell))
            {
                // Starts the Edit on the row;
                DataGrid grd = (DataGrid)sender;
                grd.BeginEdit(e);

            }
        }
        #endregion

        #region Grid Textbox Got Focus
        private void TextBox_GotKeyboardFocus(Object sender, KeyboardFocusChangedEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }
        #endregion

        #region Clicks
        private void BtnPesquisa_Click(object sender, RoutedEventArgs e)
        {
            BlackScreen bs = new BlackScreen();
            var position = Mouse.GetPosition(this);
            using (var form = new ProcurarOrcamentoCadastrado(position))
            {

                bs.Show();
                form.ShowDialog();
                if (form.DialogResult.Value && form.DialogResult.HasValue)
                {
                    dto.Id = form.Negocio_Id;
                    var loadtext = Orcamentosbll.LoadTextBoxes(dto);
                    if (loadtext.Count > 0)
                    {
                        foreach (OrcamentosDTO item in loadtext)
                        {
                            txtNumero.Text = "P" + Convert.ToInt32(dto.Id).ToString("0000");
                            txtDescricao.Text = item.Descricao;
                            txtCidade.Text = item.Cidade;
                            txtUF.Text = item.UF;
                            txtCliente.Text = item.Razao_Social;
                            txtVersao.Text = Convert.ToInt32(item.Versao_Valida).ToString("00");
                            cmbDisciplina.ItemsSource = Disciplinasbll.LoadDisciplinas(dto);
                            cmbDisciplina.SelectedValuePath = "Id";
                            cmbDisciplina.DisplayMemberPath = "Descricao";
                            cmbDisciplina.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        bs.Close();
                        CustomOKMessageBox.Show("Não há atividades cadastradas para este negócio.", "Atenção!", Window.GetWindow(this));
                        InitializeComponents();
                    }

                }
            }
            bs.Close();
        }

        private void Adicionar_Click(object sender, RoutedEventArgs e)
        {
            using (var form = new ProcurarMateriais(dto))
            {
                form.ShowDialog();
                if (form.DialogResult.Value && form.DialogResult.HasValue)
                {

                }
                Load();
            }
        }

        private void AtualizarPrecos_Click(object sender, RoutedEventArgs e)
        {
            var result = CustomOKCancelMessageBox.Show("Você deseja mesmo atualiazar os preços para a versão atual?", "Atenção!", Window.GetWindow(this));
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                bll.AtualizarPreco(dto);
                Thread.Sleep(200);
                listaOrcamentos = bll.LoadOrcamento(dto);
                grdItens.ItemsSource = listaOrcamentos;
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            helpers.OpenBack(false);
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            helpers.Close();
        }

        private void BtnLimparPesaquisa_Click(object sender, RoutedEventArgs e)
        {
            txtFind.Text = "";
        }

        private void Excluir_Click(object sender, RoutedEventArgs e)
        {
            listaCopiar = new ObservableCollection<CopiarItensOrcamentoDTO>();
            foreach (var item in listaOrcamentos)
            {
                if (item.IsSelected)
                {
                    listaCopiar.Add(new CopiarItensOrcamentoDTO { Id = item.Id });
                    item.IsSelected = false;
                }
            }

            if (listaCopiar.Count > 0)
            {
                var result = CustomOKCancelMessageBox.Show("Deseja mesmo excluir todos os itens selecionados?", "Atenção!", Window.GetWindow(this));
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    new Thread(() =>
                    {
                        Dispatcher.Invoke(new Action(() =>
                        {
                            wb = new WaitBox();
                            wb.Show();
                        }));
                        syncEvent.Set();
                        foreach (var item in listaCopiar)
                        {
                            Dispatcher.Invoke(DispatcherPriority.Background,
                               new Action(() =>
                               {
                                   adicionarItemOrcamentoDTO.Id = Convert.ToInt32(item.Id);
                                   bll.Excluir(adicionarItemOrcamentoDTO);
                               }));
                        }
                        t1 = new Thread(WaitBoxLoad);
                        t1.Start();
                    }).Start();
                }
            }
            else
            {
                CustomOKMessageBox.Show("Você deve selecionar ao menos um item para excluir.", "Atenção!", Window.GetWindow(this));
            }
        }

        private void CopiarItens_Click(object sender, RoutedEventArgs e)
        {
            listaCopiar = new ObservableCollection<CopiarItensOrcamentoDTO>();
            foreach (var item in listaOrcamentos)
            {
                if (item.IsSelected)
                {
                    listaCopiar.Add(new CopiarItensOrcamentoDTO { Id = item.Produto_Id });
                    item.IsSelected = false;
                }
            }
            if (listaCopiar.Count > 0)
            {
                using (var form = new CopiarItensOrcamento(listaCopiar, dto))
                {
                    form.ShowDialog();
                }
            }
            else
            {
                CustomOKMessageBox.Show("Você deve selecionar ao menos um item para copiar.", "Atenção!", Window.GetWindow(this));
            }

        }

        #endregion

        #region Comboboxes Selection Changed
        private void CmbDisciplina_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Disciplinasdto.Id = Convert.ToInt32(cmbDisciplina.SelectedValue);
            cmbAtividade.ItemsSource = atividadesBLL.LoadAtividades(dto, Disciplinasdto);
            cmbAtividade.SelectedValuePath = "Id";
            cmbAtividade.DisplayMemberPath = "Descricao";
            cmbAtividade.SelectedIndex = 0;
        }

        public void Load()
        {
            listaOrcamentos = bll.LoadOrcamento(dto);

            grdItens.ItemsSource = listaOrcamentos;
        }

        private void CmbAtividade_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbAtividade.SelectedValue != null)
            {
                dto.Atividade_Id = cmbAtividade.SelectedValue.ToString();
            }
            listaOrcamentos = bll.LoadOrcamento(dto);
            grdItens.ItemsSource = listaOrcamentos;
            sideExpander.Visibility = Visibility.Visible;
            CardPanel.Visibility = Visibility.Visible;
            
        }
        #endregion

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

        #region TextChanged
        private void TxtFind_TextChanged(object sender, TextChangedEventArgs e)
        {
            t1 = new Thread(Filter);
            t1.Start();
        }
        #endregion

        #endregion

        private void GrdItens_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            
           /** if (e.EditAction == DataGridEditAction.Commit)
            {
                var column = e.Column;
                var row = e.Row.GetIndex();
                var qtde = grdItens.Columns[6].GetCellContent(grdItens.Items[row]) as TextBlock;
                MessageBox.Show(qtde.Text);
            }*/
        }

        private void GrdItens_TargetUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {
            
       //     MessageBox.Show("");
        }



    }
}
