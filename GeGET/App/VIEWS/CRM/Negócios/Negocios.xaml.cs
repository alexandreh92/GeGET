using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using BLL;
using DTO;
using MMLib.Extensions;

namespace GeGET
{
    public partial class Negocios : UserControl, IDisposable
    {
        #region Declarations
        bool disposed = false;
        NegociosBLL bll = new NegociosBLL();
        NegociosDTO dto = new NegociosDTO();
        Helpers helpers = new Helpers();
        Thread t1;
        WaitBox wb;
        public ObservableCollection<NegociosDTO> listaNegocios;
        #endregion

        #region Initialize

        public Negocios()
        {
            InitializeComponent();
            dto.Pesquisa = "";
            LoadNegocios();
        }

        #endregion

        #region Methods
        private async void LoadNegocios()
        {
            wb = new WaitBox();
            wb.Owner= Window.GetWindow(this);
            wb.Show();
            await Task.Run(() => 
            {
                listaNegocios = bll.LoadNegocios(dto);
            });
            lstClientes.ItemsSource = listaNegocios;
            wb.Close();
        }

        private async void Commit()
        {
            await Task.Run(() => 
            {
                Dispatcher.Invoke(DispatcherPriority.Background,
                  new Action(() =>
                  {
                      var Find = txtProcurar.Text.ToLower().RemoveDiacritics().Split(' ').ToList();
                      var filtered = listaNegocios.Where(descricao => Find.All(list => descricao.Razao_Social.ToLower().RemoveDiacritics().Contains(list) || descricao.Descricao.ToLower().RemoveDiacritics().Contains(list) || descricao.Endereco.ToLower().Contains(list) || descricao.Anotacoes.ToLower().Contains(list) || descricao.Vendedor.ToLower().Contains(list) || descricao.CidadeEstado.ToLower().Contains(list) || descricao.Status_Descricao.ToLower().Contains(list) || descricao.Id.ToLower().Contains(list) || descricao.Numero.ToLower().Contains(list)));
                      lstClientes.ItemsSource = filtered;
                  }));
            });
        }
        #endregion

        #region Events

        #region Text Changed
        private void TxtProcurar_TextChanged(object sender, TextChangedEventArgs e)
        {
            t1 = new Thread(Commit);
            t1.Start();
        }
        #endregion

        #region Clicks

        private void BtnPesquisa_Click(object sender, RoutedEventArgs e)
        {
            Commit();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            helpers.Close();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (dto.FromParent)
            {
                helpers.OpenBack(true);
            }
            else
            {
                helpers.OpenBack(false);
            }
        }

        private void BtnNegocios_Click(object sender, MouseButtonEventArgs e)
        {
            Button btn = sender as Button;
            int index = lstClientes.Items.IndexOf(btn.DataContext);
            var Id = ((EstabelecimentosDTO)lstClientes.Items[index]).Id;
            //Negociosdto.FromParent = true;
            //Negociosdto.ParentId = Id;
            helpers.OpenTab<Negocios>(sender, e, this.GetType().Name, true);
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int index = lstClientes.Items.IndexOf(btn.DataContext);
           /* var Id = ((ClientesDTO)lstClientes.Items[index]).Id;
            var Razao_Social = ((ClientesDTO)lstClientes.Items[index]).Razao_Social;
            var Nome_Fantasia = ((ClientesDTO)lstClientes.Items[index]).Nome_Fantasia;
            var Categoria_Id = ((ClientesDTO)lstClientes.Items[index]).Categoria_Id;
            var Status = ((ClientesDTO)lstClientes.Items[index]).Status;
            var result = EditarCliente.Show(Id, Razao_Social, Nome_Fantasia, Categoria_Id, Status);
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                lstClientes.ItemsSource = bll.LoadClientes();
            }*/
        }

        #endregion

        #region Unloaded
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            dto.FromChildrenParent = false;
            dto.FromParent = false;
            dto.ParentId = "";
        }

        #endregion

        #region KeyDown
        private void TxtProcurar_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                Commit();
            }
        }
        #endregion

        #endregion

        #region IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                bll.Dispose();
            }
            disposed = true;
        }
        #endregion
    }
    #region Converter
    [ValueConversion(typeof(string), typeof(Visibility))]
    public class StringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrEmpty((string)value))
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
}