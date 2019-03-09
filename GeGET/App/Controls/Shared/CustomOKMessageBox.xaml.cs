using System.Windows;
using System.Windows.Forms;

namespace GeGET
{
    public partial class CustomOKMessageBox : Window
    {
        public CustomOKMessageBox()
        {
            InitializeComponent();
        }
        public static CustomOKMessageBox MsgBoxOK;
        static System.Windows.Forms.DialogResult result;

        public static DialogResult Show(string Text, string Caption)
        {
            MsgBoxOK = new CustomOKMessageBox();
            MsgBoxOK.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MsgBoxOK.MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            MsgBoxOK.txtDescricao.Text = Text;
            MsgBoxOK.txtTitulo.Text = Caption;
            MsgBoxOK.ShowDialog();
            return result;
        }

        private void BtnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            result = System.Windows.Forms.DialogResult.OK;
            MsgBoxOK.Close();
        }
    }
}
