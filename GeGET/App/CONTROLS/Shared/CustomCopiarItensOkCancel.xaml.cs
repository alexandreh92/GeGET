using System.Windows;
using System.Windows.Forms;

namespace GeGET
{
    public partial class CustomCopiarItensOkCancel : Window
    {
        public CustomCopiarItensOkCancel(Window Parent)
        {
            InitializeComponent();
            this.Owner = Parent;
        }
        public static CustomCopiarItensOkCancel MsgBoxOKCancel;
        static System.Windows.Forms.DialogResult result;

        public static DialogResult Show(string Text, string Caption, Window parent)
        {
            MsgBoxOKCancel = new CustomCopiarItensOkCancel(parent);
            MsgBoxOKCancel.btnComQtde.Focus();
            MsgBoxOKCancel.Height = parent.ActualHeight;
            MsgBoxOKCancel.Width = parent.ActualWidth;
            MsgBoxOKCancel.txtDescricao.Text = Text;
            MsgBoxOKCancel.txtTitulo.Text = Caption;
            MsgBoxOKCancel.ShowDialog();
            MsgBoxOKCancel.Owner = parent;
            return result;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            result = System.Windows.Forms.DialogResult.Cancel;
            MsgBoxOKCancel.Close();
        }

        private void BtnSemQtde_Click(object sender, RoutedEventArgs e)
        {
            result = System.Windows.Forms.DialogResult.No;
            MsgBoxOKCancel.Close();
        }

        private void BtnComQtde_Click(object sender, RoutedEventArgs e)
        {
            result = System.Windows.Forms.DialogResult.Yes;
            MsgBoxOKCancel.Close();
        }
    }
}
