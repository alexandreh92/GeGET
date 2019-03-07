using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GeGET
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class CustomOKCancelMessageBox : Window
    {
        public CustomOKCancelMessageBox()
        {
            InitializeComponent();
        }
        public static CustomOKCancelMessageBox MsgBoxOKCancel;
        static System.Windows.Forms.DialogResult result;

        public static DialogResult Show(string Text, string Caption)
        {
            MsgBoxOKCancel = new CustomOKCancelMessageBox();
            MsgBoxOKCancel.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MsgBoxOKCancel.MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            MsgBoxOKCancel.txtDescricao.Text = Text;
            MsgBoxOKCancel.txtTitulo.Text = Caption;
            MsgBoxOKCancel.ShowDialog();
            return result;
        }

        private void BtnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            result = System.Windows.Forms.DialogResult.OK;
            MsgBoxOKCancel.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            result = System.Windows.Forms.DialogResult.Cancel;
            MsgBoxOKCancel.Close();
        }
    }
}
