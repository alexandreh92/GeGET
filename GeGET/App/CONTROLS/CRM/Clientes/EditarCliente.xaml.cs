using System;
using System.Windows;
using System.Windows.Forms;
using BLL;
using DTO;

namespace GeGET
{
    public partial class EditarCliente : Window
    {
        #region Declarations
        public static EditarCliente EditCliente;
        static System.Windows.Forms.DialogResult result;
        public static int id;
        ClientesBLL bll = new ClientesBLL();
        ClientesDTO dto = new ClientesDTO();
        #endregion

        #region Initialize
        public EditarCliente()
        {
            InitializeComponent();
        }
        #endregion



        public static DialogResult Show(string Id, string Razao_Social, string Nome_Fantasia, int Categoria_Id, int Status_Id)
        {
            CategoriaClienteBLL bll = new CategoriaClienteBLL();
            EditCliente = new EditarCliente();
            id = Convert.ToInt32(Id);
            EditCliente.txtRazao.Text = Razao_Social;
            EditCliente.txtFantasia.Text = Nome_Fantasia;
            EditCliente.cmbCategoria.ItemsSource = bll.LoadCategoriaCliente();
            EditCliente.cmbCategoria.DisplayMemberPath = "Descricao";
            EditCliente.cmbCategoria.SelectedValuePath = "Id";
            EditCliente.cmbCategoria.SelectedValue = Categoria_Id;
            EditCliente.cbxStatus.IsChecked = Convert.ToBoolean(Status_Id);
            EditCliente.ShowDialog();
            return result;
        }

        private void BtnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            dto.Id = id.ToString();
            dto.Razao_Social = txtRazao.Text.Replace("'", "''").ToUpper();
            dto.Nome_Fantasia = txtFantasia.Text.Replace("'", "''").ToUpper();
            dto.Categoria_Id = Convert.ToInt32(cmbCategoria.SelectedValue);
            dto.Status = Convert.ToInt32(cbxStatus.IsChecked);
            bll.UpdateClientes(dto);
            result = System.Windows.Forms.DialogResult.OK;
            EditCliente.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            result = System.Windows.Forms.DialogResult.Cancel;
            EditCliente.Close();
        }
    }
}
