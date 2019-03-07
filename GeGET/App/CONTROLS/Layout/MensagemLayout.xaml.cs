using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BLL;
using DTO;

namespace GeGET
{
    /// <summary>
    /// Interaction logic for MensagemLayout.xaml
    /// </summary>
    public partial class MensagemLayout : Window
    {
        MySQLDependency bll = new MySQLDependency();
        LayoutDTO dto = new LayoutDTO();
        LoginDTO Logindto = new LoginDTO();
        
        public MensagemLayout()
        {
            InitializeComponent();
            lstMensagens.ItemsSource = bll.LoadMensagens();
            bll.MarkAsRead();
            
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            Logindto.SupressChange = false;
            this.Close();
        }
    }
}
