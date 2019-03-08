using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BLL;
using DTO;

namespace GeGET
{
    public partial class ProcurarCliente : Window, IDisposable
    {
        #region Declarations
        ClientesBLL bll = new ClientesBLL();
        ClientesDTO dto = new ClientesDTO();
        public string new_Cliente_Id;
        public string new_Razao_Social;
        public string new_Nome_Fantasia;
        #endregion

        #region Initialize
        public ProcurarCliente(Point mouseLocation)
        {
            InitializeComponent();
            dto.Pesquisa = "";
            lstMensagens.ItemsSource = bll.LoadClientes();
            Left = mouseLocation.X;
            Top = mouseLocation.Y - 50;
        }
        #endregion

        #region Methods
        private void Commit()
        {
            dto.Pesquisa = txtProcurar.Text.Replace("'", "''");
            lstMensagens.ItemsSource = bll.LoadClientes();
        }
        #endregion

        #region Events

        #region KeyDown
        private void TxtProcurar_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Commit();
            }
        }
        #endregion

        #region Clicks
        private void BtnPesquisa_Click(object sender, RoutedEventArgs e)
        {
            Commit();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int index = lstMensagens.Items.IndexOf(btn.DataContext);
            new_Cliente_Id = ((ClientesDTO)lstMensagens.Items[index]).Id;
            new_Razao_Social = ((ClientesDTO)lstMensagens.Items[index]).Razao_Social;
            new_Nome_Fantasia = ((ClientesDTO)lstMensagens.Items[index]).Nome_Fantasia;
            DialogResult = true;
        }
        #endregion

        #endregion

        #region IDisposable
        void IDisposable.Dispose()
        {
        }
        #endregion
    }
}
