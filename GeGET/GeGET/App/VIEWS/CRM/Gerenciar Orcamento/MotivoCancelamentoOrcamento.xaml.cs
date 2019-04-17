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
    public partial class MotivoCancelamentoOrcamento : Window, IDisposable
    {
        public string Motivo_Cancelamento_Id;
        public string Motivo_Cancelamento;
        MotivoCancelamentoOrcamentoBLL bll = new MotivoCancelamentoOrcamentoBLL();

        public MotivoCancelamentoOrcamento()
        {
            InitializeComponent();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            cmbMotivo.ItemsSource = bll.LoadMotivos();
            cmbMotivo.DisplayMemberPath = "Descricao";
            cmbMotivo.SelectedValuePath = "Id";
            cmbMotivo.SelectedIndex = 0;
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
            if (cmbMotivo.SelectedIndex >= 0)
            {
                DialogResult = true;
                Motivo_Cancelamento_Id = cmbMotivo.SelectedValue.ToString();
                Motivo_Cancelamento = cmbMotivo.Text;
                this.Close();
            }
        }
    }
}
