using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GeGET
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class PessoasControl : UserControl
    {
        public static string id;
        public PessoasControl(string Id, string Nome, string Funcao, string Telefone, string Celular, string Email)
        {
            InitializeComponent();
            txtNome.Text = Nome;
            txtFuncao.Text = Funcao;
            txtTelefone.Text = Telefone;
            txtCelular.Text = Celular;
            txtEmail.Text = Email;              
            id = Id;
        }
    }
}
