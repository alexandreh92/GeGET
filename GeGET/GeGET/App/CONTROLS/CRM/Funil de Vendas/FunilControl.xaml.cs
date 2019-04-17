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
    public partial class FunilControl : UserControl
    {
        private string id;
        private string status_Id;
        private string status_Descricao;

        public string Id { get => id; set => id = value; }
        public string Status_Id { get => status_Id; set => status_Id = value; }
        public string Status_Descricao { get => status_Descricao; set => status_Descricao = value; }

        public FunilControl(string Id, string Descricao, string Prazo, string Cidade, string Fantasia, string Descricao_Status, int Status_Orcamento)
        {
            InitializeComponent();
            txtNumero.Text = "p" + Convert.ToInt32(Id).ToString("0000");
            txtDescricao.Text = Descricao;
            txtPrazo.Text = Prazo;
            txtCliente.Text = Fantasia;
            txtCidade.Text = Cidade;
            id = Id;
            status_Id = Status_Orcamento.ToString();
            Status_Descricao = Descricao_Status;
        }

        
    }
}
