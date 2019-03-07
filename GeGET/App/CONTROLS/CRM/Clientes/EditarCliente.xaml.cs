using System;
using System.Windows;
using System.Windows.Forms;
using BLL;

namespace GeGET
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class EditarCliente : Window
    {
        public static EditarCliente EditCliente;
        static System.Windows.Forms.DialogResult result;
        public static int id;
        ClientesBLL bll = new ClientesBLL();

        public EditarCliente()
        {
            InitializeComponent();
        }

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
            var Id = id;
            var Razao_Social = txtRazao.Text.Replace("'", "''").ToUpper();
            var Nome_Fantasia = txtFantasia.Text.Replace("'", "''").ToUpper();
            var Categoria_Id = Convert.ToInt32(cmbCategoria.SelectedValue);
            var Status_Id = Convert.ToInt32(cbxStatus.IsChecked);
            bll.UpdateClientes(Id, Razao_Social, Nome_Fantasia, Categoria_Id, Status_Id);
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
