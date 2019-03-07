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
    /// Interaction logic for Teste.xaml
    /// </summary>
    public partial class Teste : UserControl
    {
        Helpers helpers = new Helpers();
        public Teste()
        {
            InitializeComponent();
        }

        private void Button_Click()
        {

        }

        private void BtnClientes_Click(object sender, RoutedEventArgs e)
        {
            helpers.Open<Clientes>(this.GetType().Name,false);
        }
    }
}
