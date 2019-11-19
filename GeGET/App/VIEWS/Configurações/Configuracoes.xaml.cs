using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// 
    public partial class Configuracoes : UserControl
    {
        Helpers helpers = new Helpers();
        public Configuracoes()
        {
            InitializeComponent();
            
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            helpers.Close();
        }

       
        private void BtnCadastroUsuario_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            helpers.OpenTab<CadastroUsuario>(sender, e, this.GetType().Name, false);
        }
    }
}
