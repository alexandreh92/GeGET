using System.Windows;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using DTO;
using BLL;
using System.Reflection;

namespace GeGET
{
    public partial class Layout : Window
    {
        #region Declarations
        Helpers helpers = new Helpers();
        LoginDTO Logindto = new LoginDTO();
        LayoutDTO dto = new LayoutDTO();
        MySQLDependency dependency = new MySQLDependency();
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
            txtTitle.Text = txtTitle.Text + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            LoadMensagens();
            dependency.Start();
        }
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
            var result = CustomOKCancelMessageBox.Show("Você deseja mesmo fechar o programa?", "Atenção!");
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
            MensagemLayout msgs = new MensagemLayout();
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

        #region DragWindow
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }
        #endregion

        #endregion
    }
}
