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

namespace GeGET
{
    /// <summary>
    /// Interaction logic for AnotacoesOrcamento.xaml
    /// </summary>
    public partial class AnotacoesOrcamento : Window, IDisposable
    {
        ListaOrcamentosDTO dto = new ListaOrcamentosDTO();
        ListaOrcamentosBLL bll = new ListaOrcamentosBLL();
        public AnotacoesOrcamento(string Id, string Anotacoes, string Produto_Id)
        {
            InitializeComponent();
            dto.Id = Id;
            dto.Produto_Id = Produto_Id;
            dto.Anotacoes = Anotacoes;
            txtAnotacoes.Text = dto.Anotacoes;
            if (Produto_Id != "1")
            {
                txtAnotacoes.IsReadOnly = true;
                btnConfirmar.IsEnabled = false;
            }
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
        }

        public void Dispose()
        {
            
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void BtnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            dto.Anotacoes = txtAnotacoes.Text.Replace("'", "''").ToUpper();
            bll.UpdateAnotacoes(dto);

            DialogResult = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtAnotacoes.Focus();
        }
    }
}
