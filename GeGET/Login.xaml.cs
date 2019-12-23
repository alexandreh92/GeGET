using System;
using System.Windows;
using DTO;
using BLL;
using System.Xml.Linq;
using System.Reflection;
using System.Net;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Deployment.Application;

namespace GeGET
{
    public partial class Login : Window, IDisposable
    {

        #region Declarations
        bool disposed = false;
        LoginDTO dto = new LoginDTO();
        LoginBLL bll = new LoginBLL();

        #region Updater Declarations
        private static string cVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace(".","");
        private static int oVersion;
        private static string FileLocation;
        private static string Filename;
        public static bool HaveUpdate;
        string UpdaterLocation;
        string UpdaterFilename;
        #endregion

        #endregion

        #region Initialize
        public Login()
        {
            //CheckUpdate();
            InitializeComponent();
            Remember_Data();
            // progressBar.Value = 0;
            // var bc = new BrushConverter();
            // progressBar.Background = (Brush)bc.ConvertFrom("#282828");
            txtVersion.Text = DeployVersion();
        }
        #endregion

        private string DeployVersion()
        {
            string version = null;
            try
            {
                //// get deployment version
                version = "version" + ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            }
            catch (InvalidDeploymentException)
            {
                //// you cannot read publish version when app isn't installed 
                //// (e.g. during debug)
                version = "not installed";
            }
            return version;
        }


        #region Events

        #region Clicks

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            if (!HaveUpdate)
            {
                Application.Current.Shutdown();
            }
            else
            {
                CustomOKMessageBox.Show("Você não pode fechar o programa enquanto está fazendo update!", "Atenção!", Window.GetWindow(this));
            }
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            dto.Usuario = txtLogin.Text;
            dto.Senha = Encrypt(txtPassword.Password);
            Save_Data();
            if (bll.Login(dto))
            {
                Layout mw = new Layout();
                mw.Show();
                this.Close();
            }
            else
            {
                CustomOKMessageBox.Show("Credenciais inválidas!", "Atenção!", Window.GetWindow(this));
            }
        }

        private string Encrypt(string value)
        {
            using(MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                UTF8Encoding utf8 = new UTF8Encoding();
                byte[] data = md5.ComputeHash(utf8.GetBytes(value));
                return Convert.ToBase64String(data);
            } 
        }

        #endregion

        #endregion

        #region Properties

        private void Remember_Data()
        {
            if (Properties.Settings.Default.UserName != string.Empty)
            {
                if (Properties.Settings.Default.Remme == "yes")
                {
                    txtLogin.Text = Properties.Settings.Default.UserName;
                    txtPassword.Password = Properties.Settings.Default.Password;
                    toggleRemme.IsChecked = true;
                }
                else
                {
                    txtLogin.Text = Properties.Settings.Default.UserName;
                }
            }
        }

        private void Save_Data()
        {
            if (Convert.ToBoolean(toggleRemme.IsChecked))
            {
                Properties.Settings.Default.UserName = txtLogin.Text;
                Properties.Settings.Default.Password = txtPassword.Password;
                Properties.Settings.Default.Remme = "yes";
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.UserName = txtLogin.Text;
                Properties.Settings.Default.Password = txtPassword.Password;
                Properties.Settings.Default.Remme = "no";
                Properties.Settings.Default.Save();
            }
        }

        #endregion

        private void TxtPassword_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                dto.Usuario = txtLogin.Text;
                dto.Senha = Encrypt(txtPassword.Password);
                Save_Data();
                if (bll.Login(dto))
                {
                    Layout mw = new Layout();
                    mw.Show();
                    this.Close();
                }
                else
                {
                    CustomOKMessageBox.Show("Credenciais inválidas!", "Atenção!", Window.GetWindow(this));
                }
            }
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
                bll.Dispose();
            }
            disposed = true;
        }
        #endregion
    }
}
