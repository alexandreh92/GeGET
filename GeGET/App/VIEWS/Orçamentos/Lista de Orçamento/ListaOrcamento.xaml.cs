using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BLL;
using DTO;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Threading;
using MMLib.Extensions;

namespace GeGET
{
    /// <summary>
    /// Interaction logic for ListaOrcamento.xaml
    /// </summary>
    public partial class ListaOrcamento : UserControl
    {
        #region Declarations
        Helpers helpers = new Helpers();
        ListaOrcamentosBLL bll = new ListaOrcamentosBLL();
        ListaOrcamentosDTO dto = new ListaOrcamentosDTO();
        OrcamentosBLL Orcamentosbll = new OrcamentosBLL();
        OrcamentosDTO Orcamentosdto = new OrcamentosDTO();
        DisciplinaBLL Disciplinasbll = new DisciplinaBLL();
        DisciplinaDTO Disciplinasdto = new DisciplinaDTO();
        AtividadesBLL atividadesBLL = new AtividadesBLL();
        public ObservableCollection<ListaOrcamentosDTO> listaOrcamentos;
        Thread t1;
        #endregion

        #region Initialize
        public ListaOrcamento()
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
            using (var form = new ProcurarNegocio(position))
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
                        CustomOKMessageBox.Show("Não há atividades cadastradas para este negócio.", "Atenção!");
                        //clear textboxes
                        //hide panel...
                        //hide side panel
                    }

                }
            }
            bs.Close();
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

        private void Adicionar_Click(object sender, RoutedEventArgs e)
        {
            using (var form = new ProcurarMateriais(this, dto))
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
            var result = CustomOKCancelMessageBox.Show("Você deseja mesmo atualiazar os preços para a versão atual?", "Atenção!");
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                bll.AtualizarPreco(dto);
                Thread.Sleep(200);
                listaOrcamentos = bll.LoadOrcamento(dto);
                grdItens.ItemsSource = listaOrcamentos;

            }
            
        }
    }
}
