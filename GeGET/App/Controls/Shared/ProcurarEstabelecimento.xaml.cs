using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BLL;
using DTO;

namespace GeGET
{
    public partial class ProcurarEstabelecimento : Window, IDisposable
    {
        #region Declarations
        EstabelecimentosBLL bll = new EstabelecimentosBLL();
        EstabelecimentosDTO dto = new EstabelecimentosDTO();
        public string Estabelecimento_Id;
        public string Endereco;
        public string Cidade;
        public string UF;
        public string CNPJ;
        #endregion

        #region Initialize
        public ProcurarEstabelecimento(Point mouseLocation, NegociosDTO DTO)
        {
            InitializeComponent();
            dto.Pesquisa = "";
            dto.Cliente_Id = DTO.Cliente_Id;
            lstMensagens.ItemsSource = bll.LoadEstabelecimentosFromClient(dto);
            Left = mouseLocation.X;
            Top = mouseLocation.Y - 50;
        }
        #endregion

        #region Methods
        private void Commit()
        {
            dto.Pesquisa = txtProcurar.Text.Replace("'", "''");
            lstMensagens.ItemsSource = bll.LoadEstabelecimentosFromClient(dto);
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
            Estabelecimento_Id = ((EstabelecimentosDTO)lstMensagens.Items[index]).Id;
            Endereco = ((EstabelecimentosDTO)lstMensagens.Items[index]).Endereco;
            Cidade = ((EstabelecimentosDTO)lstMensagens.Items[index]).Cidade;
            UF = ((EstabelecimentosDTO)lstMensagens.Items[index]).UF;
            CNPJ = ((EstabelecimentosDTO)lstMensagens.Items[index]).Cnpj;
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
