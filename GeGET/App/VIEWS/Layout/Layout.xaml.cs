using System.Windows;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using DTO;
using BLL;
using System.Reflection;
using System;
using System.Threading;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using Dragablz.Core;

namespace GeGET
{
    public partial class Layout : Window, IDisposable
    {
        #region Declarations
        bool disposed = false;
        Helpers helpers = new Helpers();
        LoginDTO Logindto = new LoginDTO();
        LayoutDTO dto = new LayoutDTO();
        MySQLDependency dependency = new MySQLDependency();
        ManualResetEvent syncEvent = new ManualResetEvent(false);
        #endregion

        #region Initialize
        public Layout()
        {
            InitializeComponent();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            #region Configura Nome
            txtNome.Text = Logindto.Primeiro_Nome + " " + Logindto.Ultimo_Sobrenome;
            #endregion
            LoadFoto();
            txtTitle.Text = txtTitle.Text + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            LoadMensagens();
            dependency.Start();
            helpers.OpenTab<Dashboard>(null, null, null, false);
            if (Logindto.Nivel < 6)
            {
                btnConfigs.Visibility = Visibility.Collapsed;
            }
        }
        #endregion

        private async void LoadFoto()
        {
            await Task.Run(() =>
            {
                Dispatcher.Invoke(DispatcherPriority.Render, new Action(() => 
                {
                    imgFoto.ImageSource = Logindto.Foto;
                }));
            });
        }

        #region Carrega Mensagens
        private async void LoadMensagens()
        {
            bool hasNewMessages = false;
            await Task.Run(() => 
            {
                hasNewMessages = dependency.HasNewMessages();
            }); 
            if (hasNewMessages)
            {
                btnNotificationIcon.Kind = PackIconKind.Bell;
                BadgeControl.Badge = dto.Quantidade;
            }
        }
        #endregion

        #region Events

        #region Clicks
        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Visible;
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
            pnUsuario.Visibility = Visibility.Visible;
        }

        private void BtnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            ButtonOpenMenu.Visibility = Visibility.Visible;
            pnUsuario.Visibility = Visibility.Collapsed;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            var result = CustomOKCancelMessageBox.Show("Você deseja mesmo fechar o programa?", "Atenção!", Window.GetWindow(this));
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                Application.Current.Shutdown();
            }
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            Logindto.Usuario = "";
            Logindto.Nome = "";
            Logindto.Id = 0;
            Logindto.Nivel = 0;
            Login login = new Login();
            login.Show();
            this.Close();
        }

        private void BtnCRM_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            helpers.OpenTab<CRM>(sender, e, null, false);
        }

        private void BtnOrcamentos_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            helpers.OpenTab<Orcamentos>(sender, e, null, false);
        }

        private void PopupBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (BadgeControl.Badge != null)
            {
                BadgeControl.Badge = null;
            }
            if (btnNotificationIcon.Kind == PackIconKind.Bell)
            {
                btnNotificationIcon.Kind = PackIconKind.BellOutline;
            }
            var position = Mouse.GetPosition(this);
            NotificacaoLayout msgs = new NotificacaoLayout();
            msgs.Left = position.X - msgs.Width;
            msgs.Top = position.Y;
            msgs.Show();
        }

        #endregion

        #region OnClose
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            dependency.Stop();
        }
        #endregion

        #endregion

        private void ListViewItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            //helpers.Open<UserControl1>(null, false);
        }


        private void BtnAccount_Click(object sender, MouseButtonEventArgs e)
        {
            helpers.OpenTab<UserPanel>(sender, e, null, false);
        }

        private void BtnSuprimentos_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            helpers.OpenTab<Suprimentos>(sender, e, null, false);
        }

        private void BtnDashBoard_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            helpers.OpenTab<Dashboard>(sender, e, null, false);
        }

        private void ColorZone_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void ColorZone_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (this.WindowState == WindowState.Maximized)
                {
                    this.WindowState = WindowState.Normal;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;
                }
            }
        }

        private void BtnProjetos_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            helpers.OpenTab<Projetos>(sender, e, null, false);

        }

        private void BtnChangelog_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            helpers.OpenTab<Changelog>(sender, e, null, false);
        }

        private void BtnLogistica_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            helpers.OpenTab<IndexLogs>(sender, e, null, false);
        }

        private void BtnSuprimentos_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void BtnRH_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void BtnEngenharia_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void BtnFinanceiro_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

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
                dependency.Dispose();
                syncEvent.Dispose();
            }
            disposed = true;
        }
        #endregion

        private void ActionTabs_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void BtnConfigs_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            helpers.OpenTab<Configuracoes>(sender, e, null, false);
        }
    }
}
