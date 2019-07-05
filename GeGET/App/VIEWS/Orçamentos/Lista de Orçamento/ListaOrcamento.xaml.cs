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
using DevExpress.Xpf.Grid;
using System.Threading.Tasks;

namespace GeGET
{
    public partial class ListaOrcamento : UserControl, IDisposable
    {
        #region Declarations
        bool disposed = false;
        Helpers helpers = new Helpers();

        ListaOrcamentosDTO dto = new ListaOrcamentosDTO();
        ListaOrcamentosBLL bll = new ListaOrcamentosBLL();
        InformacoesListaOrcamentosDTO informacoesDTO = new InformacoesListaOrcamentosDTO();
        InformacoesListaOrcamentosBLL informacoesBLL = new InformacoesListaOrcamentosBLL();
        public ObservableCollection<ListaOrcamentosDTO> listaOrcamentos;
        ManualResetEvent syncEvent = new ManualResetEvent(false);
        ManualResetEvent syncValues = new ManualResetEvent(false);
        WaitBox wb;
        ObservableCollection<CopiarItensOrcamentoDTO> listaCopiar;
        ObservableCollection<MaterialDTO> listaAterarBDI;
        ObservableCollection<ValoresOrcamento> listaValores;
        ObservableCollection<ListaOrcamentosDTO> listaExcluir;
        #endregion

        #region Initialize
        public ListaOrcamento()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods

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
            txtDescricao.Text = "";
            txtVersao.Text = "";
            txtCidade.Text = "";
            txtUF.Text = "";
            cmbAtividade.ItemsSource = null;
            cmbDisciplina.ItemsSource = null;
            cmbDescricao.ItemsSource = null;
        }

        private void WaitBoxLoad()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                grdItens.UnselectAll();
                Load();
            }));
        }

        public async void Load()
        {
            wb = new WaitBox
            {
                Owner = Window.GetWindow(this)
            };
            wb.Show();
            await Task.Run(() =>
            {
                listaOrcamentos = bll.LoadOrcamento(informacoesDTO);
            });
            grdItens.ItemsSource = listaOrcamentos;
            LoadSidePanel();
            wb.Close();
        }

        private void LoadSidePanel()
        {
            listaValores = bll.LoadValores(informacoesDTO);
            pnlValores.ItemsSource = listaValores;
            
        }

        #endregion

        #region Events

        #region Clicks
        private async void BtnPesquisa_Click(object sender, RoutedEventArgs e)
        {
            BlackScreen bs = new BlackScreen();
            var position = Mouse.GetPosition(this);
            using (var form = new ProcurarOrcamentoCadastrado(position))
            {
                form.Owner = Window.GetWindow(this);
                form.ShowDialog();
                if (form.DialogResult.Value && form.DialogResult.HasValue)
                {
                    informacoesDTO.Id = Convert.ToInt32(form.Negocio_Id);
                    var informacoes = new ObservableCollection<InformacoesListaOrcamentosDTO>();
                    wb = new WaitBox
                    {
                        Owner = Window.GetWindow(this)
                    };
                    wb.Show();
                    await Task.Run(() => 
                    {
                        informacoes = informacoesBLL.LoadInformacoes(informacoesDTO);
                    });
                    wb.Close();
                    if (informacoes.Count > 0)
                    {
                        informacoesDTO = informacoes.First();
                        txtNumero.Text = "P" + Convert.ToInt32(informacoesDTO.Id).ToString("0000");
                        txtDescricao.Text = informacoesDTO.Descricao;
                        txtCidade.Text = informacoesDTO.Cidade;
                        txtUF.Text = informacoesDTO.Uf;
                        txtCliente.Text = informacoesDTO.Razao_Social;
                        txtVersao.Text = Convert.ToInt32(informacoesDTO.Versao).ToString("00");
                        cmbDisciplina.ItemsSource = informacoesDTO.Disciplinas;
                        cmbDisciplina.SelectedValuePath = "Id";
                        cmbDisciplina.DisplayMemberPath = "Descricao";
                        cmbDisciplina.SelectedIndex = 0;
                    }
                    
                }
            }
        }

        private void Adicionar_Click(object sender, RoutedEventArgs e)
        {
            using (var form = new ProcurarMateriais(informacoesDTO))
            {
                form.Owner = Window.GetWindow(this);
                form.ShowDialog();
                if (form.DialogResult.Value && form.DialogResult.HasValue)
                {

                }
                Load();
            }
        }

        private async void AtualizarPrecos_Click(object sender, RoutedEventArgs e)
        {
            var result = CustomOKCancelMessageBox.Show("Você deseja mesmo atualiazar os preços para a versão atual?", "Atenção!", Window.GetWindow(this));
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                await Task.Run(() => 
                {
                    bll.AtualizarPreco(informacoesDTO);
                });
                Load();
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

        private async void Excluir_Click(object sender, RoutedEventArgs e)
        {
            int[] handles = grdItens.GetSelectedRowHandles();

            if (handles.Length > 0)
            {
                var result = CustomOKCancelMessageBox.Show("Deseja mesmo excluir todos os itens selecionados?", "Atenção!", Window.GetWindow(this));
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    listaExcluir = new ObservableCollection<ListaOrcamentosDTO>();
                    foreach (var rowHandle in handles)
                    {
                        var selectedItem = grdItens.GetRow(rowHandle) as ListaOrcamentosDTO;
                        listaExcluir.Add(selectedItem);
                    }
                    wb = new WaitBox
                    {
                        Owner = Window.GetWindow(this)
                    };
                    wb.Show();
                    await Task.Run(() => 
                    {
                        bll.Excluir(listaExcluir);
                    });
                    wb.Close();
                    grdItens.UnselectAll();
                    Load();
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
            int[] handles = grdItens.GetSelectedRowHandles();

            if (handles.Length > 0)
            {
                listaCopiar = new ObservableCollection<CopiarItensOrcamentoDTO>();
                foreach (var rowHandle in handles)
                {
                    var selectedItem = grdItens.GetRow(rowHandle) as ListaOrcamentosDTO;
                    listaCopiar.Add(new CopiarItensOrcamentoDTO { Id = selectedItem.Produto_Id });
                }
                using (var form = new CopiarItensOrcamento(listaCopiar, informacoesDTO))
                {
                    form.Owner = Window.GetWindow(this);
                    form.ShowDialog();
                    if (form.DialogResult.Value && form.DialogResult.HasValue)
                    {
                        Load();
                    }
                }
            }
            else
            {
                CustomOKMessageBox.Show("Você deve selecionar ao menos um item para copiar.", "Atenção!", Window.GetWindow(this));
            }
        }

        private void AlterarBDI_Click(object sender, RoutedEventArgs e)
        {
            listaAterarBDI = new ObservableCollection<MaterialDTO>();
            int[] handles = grdItens.GetSelectedRowHandles();
            if (handles.Length>0)
            {
                foreach (var rowHandle in handles)
                {
                    var selectedItem = grdItens.GetRow(rowHandle) as ListaOrcamentosDTO;
                    listaAterarBDI.Add(new MaterialDTO { Id = selectedItem.Id });
                }
                using (var form = new AlterarBDIOrcamento(listaAterarBDI))
                {
                    form.Owner = Window.GetWindow(this);
                    form.ShowDialog();
                    if (form.DialogResult.HasValue && form.DialogResult.Value)
                    {
                        Load();
                    }
                }
            }
            else
            {
                CustomOKMessageBox.Show("Você deve selecionar ao menos um item para alterar o BDI.", "Atenção!", Window.GetWindow(this));
            }
        }
        #endregion

        #region Comboboxes Selection Changed
        private void CmbDisciplina_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbDisciplina.SelectedValue != null)
            {
                var Find = cmbDisciplina.SelectedValue.ToString().ToLower();
                var filtered = informacoesDTO.Atividades.Where(descricao => descricao.Disciplina_Id == Find);
                cmbAtividade.ItemsSource = filtered;
                cmbAtividade.SelectedValuePath = "Id";
                cmbAtividade.DisplayMemberPath = "Descricao";
            }
            cmbAtividade.SelectedIndex = 0;
        }

        private void CmbAtividade_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbAtividade.SelectedValue != null)
            {
                var Find = cmbAtividade.SelectedValue.ToString().ToLower();
                var filtered = informacoesDTO.Descricao_Atividades.Where(descricao => descricao.Desc_Atividade_Id == Find);
                cmbDescricao.ItemsSource = filtered;
                cmbDescricao.SelectedValuePath = "Id";
                cmbDescricao.DisplayMemberPath = "Descricao";
            }
            cmbDescricao.SelectedIndex = 0;
        }
        #endregion

        #region Cell Value Changed

        private void TableView_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            var material = e.Row as ListaOrcamentosDTO;

            if (e.Column.Header.ToString() == "Quantidade")
            {
                if (material.Quantidade < 0)
                {
                    material.Quantidade = 0;
                }
            }

            if (e.Column.Header.ToString() == "Quantidade")
            {
                dto.Id = material.Id;
                dto.Quantidade = material.Quantidade;
                bll.AtualizarQuantidade(dto);
            }
            else if (e.Column.Header.ToString() == "BDI")
            {
                dto.Id = material.Id;
                dto.Bdi = Convert.ToDouble(material.Bdi);
                bll.AtualizarBDI(dto);
            }
            else if (e.Column.Header.ToString() == "FD")
            {
                dto.Id = material.Id;
                dto.Fd = material.Fd;
                bll.AtualizarFD(dto);
                LoadSidePanel();
            }
        }

        #endregion

        #endregion

        private void grdItens_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ((TableView)grdItens.View).MoveNextRow();
                ((TableView)grdItens.View).ShowEditor();
                //e.Handled = true;
            }
        }

        private void GrdView_ShownEditor(object sender, EditorEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                if (this.grdView.ActiveEditor != null)
                {
                    this.grdView.ActiveEditor.SelectAll();
                }
                else
                {
                    ((TableView)grdItens.View).ShowEditor();
                }
            }), DispatcherPriority.Render);
        }

        private void BtnAnotacoes_Click(object sender, RoutedEventArgs e)
        {
            var Id = grdItens.GetFocusedRowCellValue("Id").ToString();
            var Anotacoes = grdItens.GetFocusedRowCellValue("Anotacoes").ToString();
            var Produto_Id = grdItens.GetFocusedRowCellValue("Produto_Id").ToString();

            using (var form = new AnotacoesOrcamento(Id, Anotacoes, Produto_Id))
            {
                form.Owner = Window.GetWindow(this);
                form.ShowDialog();
                if (form.DialogResult.HasValue && form.DialogResult.Value)
                {
                    Load();
                }
            }
        }

        private void CmbDescricao_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbDescricao.SelectedValue != null)
            {
                informacoesDTO.Atividade_Id = cmbDescricao.SelectedValue.ToString();
                Load();
            }
            sideExpander.Visibility = Visibility.Visible;
            CardPanel.Visibility = Visibility.Visible;
        }

        #region IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                bll.Dispose();
                syncEvent.Dispose();
                syncValues.Dispose();
                informacoesBLL.Dispose();
            }
            disposed = true;
        }
        #endregion
    }
}
