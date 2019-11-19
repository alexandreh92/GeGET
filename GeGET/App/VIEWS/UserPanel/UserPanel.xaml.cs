using Microsoft.Win32;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using DTO;
using BLL;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace GeGET
{
    public partial class UserPanel : UserControl, IDisposable
    {
        bool disposed = false;
        LoginDTO dto = new LoginDTO();
        LoginBLL bll = new LoginBLL();
        string strName, imageName;
        bool isChanged = false;
        public UserPanel()
        {
            InitializeComponent();
            string nome = "";
            var quebra = dto.Nome.Split();
            foreach (string item in quebra)
            {
                nome = nome + char.ToUpper(item[0]) + item.Substring(1).ToLower() + " ";
            }
            txtNome.Text = nome;
            txtUsuario.Text = dto.Usuario;
            if (dto.Foto != null)
            {
                imgFoto.ImageSource = dto.Foto;
            }
        }

        private void BtnChangePhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Selecione uma Foto";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
        "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
        "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                strName = op.SafeFileName;
                imageName = op.FileName;
                var newimage = new BitmapImage(new Uri(op.FileName));
                imgFoto.ImageSource = newimage;
                isChanged = true;
                var targetWindow = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is Layout) as Layout;
                targetWindow.imgFoto.ImageSource = newimage;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            bool imagesave = false;
            bool passwordsave = false;

            if (isChanged)
            {
                FileStream fs = new FileStream(imageName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                dto.Photo = br.ReadBytes((int)fs.Length);
                imagesave = bll.SavePhoto(dto);
            }


            if (txtSenha.Password != "" && txtConfirmaSenha.Password != "")
            {
                if (txtSenha.Password == txtConfirmaSenha.Password)
                {
                    dto.Senha = Encrypt(txtSenha.Password.Replace("'", "''"));
                    passwordsave = bll.SavePassword(dto);
                    txtSenha.Password = "";
                    txtConfirmaSenha.Password = "";
                }
                else
                {
                    CustomOKMessageBox.Show("As senhas não são iguais.", "Atenção!", Window.GetWindow(this));
                }
            }

            if (passwordsave || imagesave)
            {
                passwordsave = false;
                imagesave = false;
                CustomOKMessageBox.Show("Dados alterados com sucesso.", "Sucesso!", Window.GetWindow(this));
            }


        }

        private string Encrypt(string value)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                UTF8Encoding utf8 = new UTF8Encoding();
                byte[] data = md5.ComputeHash(utf8.GetBytes(value));
                return Convert.ToBase64String(data);
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
