using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using BLL;
using DTO;

namespace GeGET
{
    public partial class EditarCliente : Window, IDisposable
    {
        #region Declarations
        bool disposed = false;
        string id;
        ClientesBLL bll = new ClientesBLL();
        ClientesDTO dto = new ClientesDTO();
        CategoriaClienteBLL Categoriabll = new CategoriaClienteBLL();
        WaitBox wb;
        #endregion

        #region Initialize
        public EditarCliente(ClientesDTO DTO)
        {
            InitializeComponent();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            txtRazao.Text = DTO.Razao_Social;
            txtFantasia.Text = DTO.Nome_Fantasia;
            cmbCategoria.ItemsSource = Categoriabll.LoadCategoriaCliente();
            cmbCategoria.DisplayMemberPath = "Descricao";
            cmbCategoria.SelectedValuePath = "Id";
            cmbCategoria.SelectedValue = DTO.Categoria_Id;
            cbxStatus.IsChecked = Convert.ToBoolean(DTO.Status);
            id = DTO.Id;
        }
        #endregion

        #region Events
        private async void BtnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            dto.Id = id.ToString();
            dto.Razao_Social = txtRazao.Text.Replace("'", "''").ToUpper();
            dto.Nome_Fantasia = txtFantasia.Text.Replace("'", "''").ToUpper();
            dto.Categoria_Id = Convert.ToInt32(cmbCategoria.SelectedValue);
            dto.Status = Convert.ToInt32(cbxStatus.IsChecked);
            wb = new WaitBox
            {
                Owner = Window.GetWindow(this)
            };
            wb.Show();
            var isSucceed = await bll.UpdateClientes(dto);
            if (isSucceed)
            {
                Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                {
                    wb.Close();
                    DialogResult = true;
                }));
            }
            
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
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
                Categoriabll.Dispose();
            }
            disposed = true;
        }

        #endregion
    }
}
