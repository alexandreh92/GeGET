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
    public partial class EstablishmentControl : UserControl
    {
        public static string id;
        public EstablishmentControl(string Id, string Razao_Social, string Nome_Fantasia, string CNPJ, string Endereco, string Cidade)
        {
            InitializeComponent();
            txtRazaoSocial.Text = Razao_Social;
            txtFantasia.Text = Nome_Fantasia;
            txtCNPJ.Text = CNPJ;
            txtEndereco.Text = Endereco;
            txtCidade.Text = Cidade;
            id = Id;
        }
    }
}
