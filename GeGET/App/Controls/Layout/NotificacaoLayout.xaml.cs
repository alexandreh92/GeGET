using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using BLL;
using DTO;

namespace GeGET
{
    public partial class NotificacaoLayout : Window, IDisposable
    {
        #region Declarations
        bool disposed = false;
        MySQLDependency bll = new MySQLDependency();
        LayoutDTO dto = new LayoutDTO();
        LoginDTO Logindto = new LoginDTO();
        #endregion

        #region Initialize
        public NotificacaoLayout()
        {
            InitializeComponent();
            LoadMensagens();
            

        }
        #endregion

        private async void LoadMensagens()
        {
            var listaMensagens = new List<LayoutDTO>();
            await Task.Run(() =>
            {
                listaMensagens = bll.LoadMensagens();
            });
            lstMensagens.ItemsSource = listaMensagens;
            await Task.Run(() =>
            {
                bll.MarkAsRead();
            });
        }

        #region Events

        #region Deactivated
        private void Window_Deactivated(object sender, EventArgs e)
        {
            Logindto.SupressChange = false;
            this.Close();
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
}
