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
    public partial class NegociosControl : UserControl
    {
        public static string id;
        public NegociosControl(string Id, string Numero, string Razao_Social, string Endereco, string Descricao, string Vendedor, string Status, int Statud_Id)
        {
            InitializeComponent();
            txtNumero.Text = "P" + Numero;
            txtRazaoSocial.Text = Razao_Social;
            txtEndereco.Text = Endereco;
            txtDescricao.Text = Descricao;
            txtVendedor.Text = Vendedor;
            txtEndereco.Text = Endereco;
            txtStatus.Text = Status;
            starStatus.Value = Statud_Id;
            id = Id;
        }
    }
}
