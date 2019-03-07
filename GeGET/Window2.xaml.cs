using System.Windows;
using DTO;
using BLL;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace GeGET
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    /// 
    public partial class Window2 : Window
    {

        FunildeVendasDTO dto = new FunildeVendasDTO();
        FunildeVendasBLL bll = new FunildeVendasBLL();

        public ObservableCollection<FunildeVendasDTO> Lista { get; set; }

        public Window2()
        {
           
            InitializeComponent();

            //List1.ItemsSource = bll.LoadNegocios();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int index = List1.Items.IndexOf(btn.DataContext);
            var teste = ((FunildeVendasDTO)List1.Items[index]).Id;
            MessageBox.Show(teste);
        }
    }
}
