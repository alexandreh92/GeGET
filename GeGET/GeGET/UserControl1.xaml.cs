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
using DTO;
using BLL;

namespace GeGET
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {

        DashboardComercialBLL bll = new DashboardComercialBLL();

        public UserControl1()
        {
            InitializeComponent();
        }

        private void lstOrcamentos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
