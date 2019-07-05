using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using BLL;
using DTO;

namespace GeGET
{
    public partial class LinkarProdutoNotaFiscal : Window, IDisposable
    {
        #region Declarations
        bool disposed = false;
        LinkarProdutoNotaFiscalBLL bll = new LinkarProdutoNotaFiscalBLL();
        LinkarProdutoNotaFiscalDTO dto = new LinkarProdutoNotaFiscalDTO();
        public ObservableCollection<LinkarProdutoNotaFiscalDTO> listaItens;
        string Codigo_Barra;
        public string Descricao;
        public string Codigo_Getac;
        public string Fabricante;
        #endregion

        #region Initialize
        public LinkarProdutoNotaFiscal(EntradaNotaFiscalDTO DTO)
        {
            InitializeComponent();
            Codigo_Barra = DTO.Codigo;
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            Load();
        }
        #endregion

        #region Methods

        #region Load
        private async void Load()
        {
            await Task.Run(() =>
            {
                listaItens = bll.LoadItens();
            });
            grdItens.ItemsSource = listaItens;
            progressbar.Visibility = Visibility.Collapsed;
            grdItens.Visibility = Visibility.Visible;
        }
        #endregion

        #endregion

        #region Events

        #region Clicks

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            var handle = grdItens.GetFocusedRow();
            dto = handle as LinkarProdutoNotaFiscalDTO;
            dto.Codigo_Barra = Codigo_Barra;
            if (!bll.isEmpty(dto))
            {
                var resultEmpty = CustomOKCancelMessageBox.Show("Este produto já está linkado!\nDeseja mesmo alterar o link?", "Atenção!", Window.GetWindow(this));
                if (resultEmpty == System.Windows.Forms.DialogResult.OK)
                {
                    bll.UpdateCodigoBarra(dto);
                    Codigo_Getac = dto.Id.ToString();
                    Descricao = dto.Descricao;
                    Fabricante = dto.Fabricante;
                    DialogResult = true;
                }
            }
            else
            {
                var result = CustomOKCancelMessageBox.Show("Deseja mesmo linkar o produto da nota fiscal com este produto do sistema?", "Atenção!", Window.GetWindow(this));
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    bll.UpdateCodigoBarra(dto);
                    Codigo_Getac = dto.Id.ToString();
                    Descricao = dto.Descricao;
                    Fabricante = dto.Fabricante;
                    DialogResult = true;
                }
            }
            

            //update codigo_barras set from dto
            //passa valores para variavel publica


            /*int[] handles = grdItens.GetSelectedRowHandles();
            if (handles.Length > 0)
            {

            }


            new Thread(() =>
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    wb = new WaitBox();
                    wb.Owner = Window.GetWindow(this);
                    wb.Show();
                }));
                syncEvent.Set();
                foreach (var rowHandle in handles)
                {
                    Dispatcher.Invoke(DispatcherPriority.Background,
                  new Action(() =>
                  {
                      var selectedItem = grdItens.GetRow(rowHandle) as AdicionarItemProjetoDTO;
                      dto.Id = Convert.ToInt32(selectedItem.Id);
                      dto.Atividade_Id = Atividade_Id;
                      dto.Negocio_Id = Negocio_Id;
                      dto.Anotacoes = selectedItem.Anotacoes;
                      bll.Inserir(dto);
                      grdItens.UnselectAll();
                  }));
                }
                t1 = new Thread(WaitBoxLoad);
                t1.Start();
            }).Start();*/
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion

        #endregion

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
            }
            disposed = true;
        }
        #endregion
    }
}
