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
using System.Windows.Shapes;
using DTO;
using BLL;
using System.Collections.ObjectModel;
using System.Threading;

namespace GeGET
{
    public partial class PrecoPerdidoOrcamento : Window, IDisposable
    {
        public string Valor;
        public PrecoPerdidoOrcamento()
        {
            InitializeComponent();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
        }

        void IDisposable.Dispose()
        {
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void BtnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            decimal parse;
            if (Decimal.TryParse(txtValor.Text, out parse) && txtValor.Text != "")
            {
                var result = CustomOKCancelMessageBox.Show("Confirma o valor: " + Convert.ToDecimal(txtValor.Text).ToString("c") + " ?", "Atenção!", Window.GetWindow(this));
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    Valor = Convert.ToDecimal(txtValor.Text).ToString().Replace(",", ".");
                    DialogResult = true;
                }
            }
        }
    }
}
