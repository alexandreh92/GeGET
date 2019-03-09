using System.Windows;
using System.Windows.Forms;

namespace GeGET
{
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
