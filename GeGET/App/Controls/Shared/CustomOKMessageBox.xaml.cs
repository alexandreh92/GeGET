using System.Windows;
using System.Windows.Forms;

namespace GeGET
{
    public partial class CustomOKMessageBox : Window
    {
        public CustomOKMessageBox(Window Parent)
        {
            InitializeComponent();
            this.Owner = Parent;
        }
        public static CustomOKMessageBox MsgBoxOK;
        static System.Windows.Forms.DialogResult result;

        public static DialogResult Show(string Text, string Caption, Window parent)
        {
            MsgBoxOK = new CustomOKMessageBox(parent);
            MsgBoxOK.Height = parent.ActualHeight;
            MsgBoxOK.Width = parent.ActualWidth;
            MsgBoxOK.txtDescricao.Text = Text;
            MsgBoxOK.txtTitulo.Text = Caption;
            MsgBoxOK.ShowDialog();
            MsgBoxOK.Owner = parent;
            return result;
        }

        private void BtnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            result = System.Windows.Forms.DialogResult.OK;
            MsgBoxOK.Close();
        }
    }
}
