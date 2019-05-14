using System.Windows;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using DTO;
using BLL;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System;
using System.Drawing;
using System.Windows.Interop;
using System.Threading;
using System.Windows.Threading;

namespace GeGET
{
    public partial class Layout : Window
    {
        #region Declarations
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
            #region MyRegion
            txtNome.Text = Logindto.Primeiro_Nome + " " + Logindto.Ultimo_Sobrenome;
            #endregion
            imgFoto.ImageSource = Logindto.Foto;
            txtTitle.Text = txtTitle.Text + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            LoadMensagens();
            dependency.Start();
            helpers.Open<Dashboard>(null, false);
        }
        #endregion

        #region WaitLoad


        #endregion

        #region Carrega Mensagens
        private void LoadMensagens()
        {
            var hasNewMessages = dependency.HasNewMessages();
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
            helpers.Open<CRM>(null, false);
        }

        private void BtnOrcamentos_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            helpers.Open<Orcamentos>(null, false);
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


        private void BtnAccount_Click(object sender, RoutedEventArgs e)
        {
            helpers.Open<UserPanel>(null, false);
        }

        private void btnSuprimentos_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            helpers.Open<Suprimentos>(null, false);
        }

        private void BtnDashBoard_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            helpers.Open<Dashboard>(null, false);
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
            helpers.Open<Projetos>(null, false);
        }

        private void BtnChangelog_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            helpers.Open<Changelog>(null, false);
        }

        private void BtnLogistica_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            helpers.Open<Logistica>(null, false);
        }

        private void BtnSuprimentos_Selected(object sender, RoutedEventArgs e)
        {

        }
    }
}
