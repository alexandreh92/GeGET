using System;
using System.Windows;
using BLL;
using DTO;

namespace GeGET
{
    public partial class MensagemLayout : Window
    {
        #region Declarations
        MySQLDependency bll = new MySQLDependency();
        LayoutDTO dto = new LayoutDTO();
        LoginDTO Logindto = new LoginDTO();
        #endregion

        #region Initialize
        public MensagemLayout()
        {
            InitializeComponent();
            lstMensagens.ItemsSource = bll.LoadMensagens();
            bll.MarkAsRead();

        }
        #endregion

        #region Events

        #region Deactivated
        private void Window_Deactivated(object sender, EventArgs e)
        {
            Logindto.SupressChange = false;
            this.Close();
        }
        #endregion

        #endregion
    }
}
