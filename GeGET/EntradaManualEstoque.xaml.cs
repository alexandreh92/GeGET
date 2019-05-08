using System;
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
using Helpers;
using DTO;
using BLL;

namespace GeGET
{
    /// <summary>
    /// Interaction logic for EntradaManualEstoque.xaml
    /// </summary>
    public partial class EntradaManualEstoque : UserControl
    {
        Helpers helpers = new Helpers();
        ObservableCollection<EntradaManualEstoqueDTO> listaEntrada = new ObservableCollection<EntradaManualEstoqueDTO>();

        public EntradaManualEstoque()
        {
            InitializeComponent();
        }

        #region Clicks

        #region Botão Voltar
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            helpers.OpenBack(false);
        }
        #endregion

        #region Botão Fechar
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            helpers.Close();
        }
        #endregion

        #endregion

        private void AdicionarProduto_Click(object sender, RoutedEventArgs e)
        {
            using (var form = new AdicionarItemListaManual())
            {
                form.Owner = Window.GetWindow(this);
                form.ShowDialog();
                if (form.DialogResult.Value && form.DialogResult.HasValue)
                {
                    foreach (EntradaManualEstoqueDTO item in form.listaEntrada)
                    {
                        listaEntrada.Add(item);
                    }
                    grdItens.ItemsSource = listaEntrada;
                }
            }
        }

        private void AdicionarEstoque_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
