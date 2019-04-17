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
    public partial class AdicionarVersaoOrcamento : Window, IDisposable
    {
        public int Versao;
        public string Descricao;
        NegociosBLL bll = new NegociosBLL();

        public AdicionarVersaoOrcamento(NegociosDTO DTO)
        {
            InitializeComponent();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            Versao = bll.NewVersionNumber(DTO);
            txtVersao.Text = Versao.ToString("00");
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
            if (txtDescricao.Text != "")
            {
                Descricao = txtDescricao.Text.Replace("'", "''").ToUpper();
                DialogResult = true;
                this.Close();
            }
        }
    }
}
