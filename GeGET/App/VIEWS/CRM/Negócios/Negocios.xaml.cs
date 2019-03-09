using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using BLL;
using DTO;

namespace GeGET
{
    public partial class Negocios : UserControl
    {
        #region Declarations
        NegociosBLL bll = new NegociosBLL();
        NegociosDTO dto = new NegociosDTO();
        Helpers helpers = new Helpers();
        #endregion

        #region Initialize

        public Negocios()
        {
            InitializeComponent();
            dto.Pesquisa = "";
            LoadNegocios();
        }

        #endregion

        #region Methods
        private void LoadNegocios()
        {
            lstClientes.ItemsSource = bll.LoadNegocios();
        }

        private void Commit()
        {
            dto.Pesquisa = txtProcurar.Text.Replace("'", "''");
            LoadNegocios();
        }
        #endregion

        #region Events

        #region Keydowns
        private void CommentTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Commit();
            }
        }
        #endregion

        #region Clicks

        private void BtnPesquisa_Click(object sender, RoutedEventArgs e)
        {
            Commit();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            helpers.Close();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (dto.FromParent)
            {
                helpers.OpenBack(true);
            }
            else
            {
                helpers.OpenBack(false);
            }
        }

        private void BtnNegocios_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int index = lstClientes.Items.IndexOf(btn.DataContext);
            var Id = ((EstabelecimentosDTO)lstClientes.Items[index]).Id;
            //Negociosdto.FromParent = true;
            //Negociosdto.ParentId = Id;
            helpers.Open<Negocios>(this.GetType().Name, true);
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int index = lstClientes.Items.IndexOf(btn.DataContext);
           /* var Id = ((ClientesDTO)lstClientes.Items[index]).Id;
            var Razao_Social = ((ClientesDTO)lstClientes.Items[index]).Razao_Social;
            var Nome_Fantasia = ((ClientesDTO)lstClientes.Items[index]).Nome_Fantasia;
            var Categoria_Id = ((ClientesDTO)lstClientes.Items[index]).Categoria_Id;
            var Status = ((ClientesDTO)lstClientes.Items[index]).Status;
            var result = EditarCliente.Show(Id, Razao_Social, Nome_Fantasia, Categoria_Id, Status);
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                lstClientes.ItemsSource = bll.LoadClientes();
            }*/
        }

        #endregion

        #region Unloaded
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            dto.FromChildrenParent = false;
            dto.FromParent = false;
            dto.ParentId = "";
        }

        #endregion

        #endregion
    }
    [ValueConversion(typeof(string), typeof(Visibility))]
    public class StringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrEmpty((string)value))
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}