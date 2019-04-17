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
    public partial class CopiarVersaoOrcamento : Window, IDisposable
    {
        public string Versao_Atividade_Id;
        public string Versao;
        VersaoOrcamentoBLL bll = new VersaoOrcamentoBLL();

        public CopiarVersaoOrcamento(NegociosDTO DTO)
        {
            InitializeComponent();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            cmbVersao.ItemsSource = bll.LoadVersaoAll(DTO);
            cmbVersao.DisplayMemberPath = "Id";
            cmbVersao.SelectedValuePath = "Id";
            cmbVersao.SelectedIndex = 0;
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
            if (cmbVersao.SelectedIndex >= 0)
            {
                DialogResult = true;
                Versao = cmbVersao.SelectedValue.ToString();
                Versao_Atividade_Id = txtVersao_Atividade_Id.Text;
                this.Close();
            }
        }
    }
}
