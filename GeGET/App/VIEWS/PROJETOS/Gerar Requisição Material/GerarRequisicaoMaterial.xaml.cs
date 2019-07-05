using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Threading;
using DevExpress.Xpf.Grid;
using BLL;
using DTO;

namespace GeGET
{
    public partial class GerarRequisicaoMaterial : UserControl, IDisposable
    {
        #region Declarations
        bool disposed = false;
        Helpers helpers = new Helpers();
        InformacoesGerarRequisicaoMaterialDTO informacoesDTO = new InformacoesGerarRequisicaoMaterialDTO();
        InformacoesGerarRequisicaoMaterialBLL informacoesBLL = new InformacoesGerarRequisicaoMaterialBLL();
        GerarRequisicaoMaterialBLL bll = new GerarRequisicaoMaterialBLL();
        GerarRequisicaoMaterialDTO dto = new GerarRequisicaoMaterialDTO();
        public ObservableCollection<GerarRequisicaoMaterialDTO> listaItens;
        ManualResetEvent syncEvent = new ManualResetEvent(false);
        ManualResetEvent syncValues = new ManualResetEvent(false);
        public ObservableCollection<GerarRequisicaoMaterialDTO> listaGerar;
        #endregion

        #region Initialize
        public GerarRequisicaoMaterial()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods

        private void InitializeComponents()
        {
            txtNumero.Text = " ";
            txtCliente.Text = "";
            txtDescricao.Text = "";
            txtVersao.Text = "";
            txtCidade.Text = "";
            txtUF.Text = "";
        }

        public void Load()
        {
            listaItens = bll.LoadLista(informacoesDTO);
            grdItens.ItemsSource = listaItens;
        }

        #endregion

        #region Events

        #region Clicks

        #region Botão de Pesquisa
        private void BtnPesquisa_Click(object sender, RoutedEventArgs e)
        {
            BlackScreen bs = new BlackScreen();
            var position = Mouse.GetPosition(this);
            using (var form = new ProcurarVenda(position))
            {
                form.Owner = Window.GetWindow(this);
                form.ShowDialog();
                if (form.DialogResult.Value && form.DialogResult.HasValue)
                {
                    informacoesDTO.Id = Convert.ToInt32(form.Venda_Id);
                    informacoesDTO.Negocio_Id = Convert.ToInt32(form.Negocio_Id);
                    informacoesDTO = informacoesBLL.LoadInformacoes(informacoesDTO).First();

                    txtNumero.Text = "P" + Convert.ToInt32(informacoesDTO.Negocio_Id).ToString("0000");
                    txtDescricao.Text = informacoesDTO.Descricao;
                    txtCidade.Text = informacoesDTO.Cidade;
                    txtUF.Text = informacoesDTO.Uf;
                    txtCliente.Text = informacoesDTO.Razao_Social;
                    txtVersao.Text = Convert.ToInt32(informacoesDTO.Versao).ToString("00");
                    Load();
                }
            }
        }
        #endregion

        #region Botão Voltar
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            helpers.OpenBack(false);
        }
        #endregion

        #region Botão Fechar
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            helpers.Close();
        }
        #endregion

        #endregion

        #region Cell Value Changed

        private void TableView_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
           var row = e.Row as GerarRequisicaoMaterialDTO;
            if (e.Column.FieldName.ToString() == "Quantidade")
            {
                if (row.Quantidade > row.Saldo)
                {
                    row.Quantidade = row.Saldo;
                }
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
                e.Handled = true;
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

        private void GerarRM_Click(object sender, RoutedEventArgs e)
        {
            int[] handles = grdItens.GetSelectedRowHandles();
            if (handles.Length > 0)
            {
                var result = CustomOKCancelMessageBox.Show("Deseja mesmo gerar uma requisição de material para todos os itens selecionados?", "Atenção!", Window.GetWindow(this));
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    listaGerar = new ObservableCollection<GerarRequisicaoMaterialDTO>();
                    foreach (var rowHandle in handles)
                    {
                        var selectedItem = grdItens.GetRow(rowHandle) as GerarRequisicaoMaterialDTO;
                        listaGerar.Add(new GerarRequisicaoMaterialDTO
                        {
                            Produto_Id = selectedItem.Produto_Id,
                            Quantidade = selectedItem.Quantidade,
                            Saldo = selectedItem.Saldo
                        });
                    }
                    var rm = bll.GerarRM(informacoesDTO, listaGerar);
                    if (rm != null && rm != "")
                    {
                        CustomOKMessageBox.Show("Requisição de Materiais Nº " + rm + " gerada com sucesso.", "Atenção!", Window.GetWindow(this));
                        Load();
                    }
                }
            }
            else
            {
                CustomOKMessageBox.Show("Você deve selecionar ao menos um item para gerar RM.", "Atenção!", Window.GetWindow(this));
            }
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
                informacoesBLL.Dispose();
                syncEvent.Dispose();
                syncValues.Dispose();
            }
            disposed = true;
        }
        #endregion
    }
}
