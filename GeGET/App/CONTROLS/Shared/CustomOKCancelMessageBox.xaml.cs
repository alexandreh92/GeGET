using System.Windows;
using System.Windows.Forms;

namespace GeGET
{
    public partial class CustomOKCancelMessageBox : Window
    {
        public CustomOKCancelMessageBox(Window Parent)
        {
            InitializeComponent();
            this.Owner = Parent;
        }
        public static CustomOKCancelMessageBox MsgBoxOKCancel;
        static System.Windows.Forms.DialogResult result;

        public static DialogResult Show(string Text, string Caption, Window parent)
        {
            MsgBoxOKCancel = new CustomOKCancelMessageBox(parent);
            MsgBoxOKCancel.btnConfirmar.Focus();
            MsgBoxOKCancel.Height = parent.ActualHeight;
            MsgBoxOKCancel.Width = parent.ActualWidth;
            MsgBoxOKCancel.txtDescricao.Text = Text;
            MsgBoxOKCancel.txtTitulo.Text = Caption;
            MsgBoxOKCancel.ShowDialog();
            MsgBoxOKCancel.Owner = parent;
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
