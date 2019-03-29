using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DTO;
using BLL;
using System.IO;

namespace GeGET
{
    /// <summary>
    /// Interaction logic for UserPanel.xaml
    /// </summary>
    public partial class UserPanel : UserControl
    {
        LoginDTO dto = new LoginDTO();
        LoginBLL bll = new LoginBLL();
        string strName, imageName;
        bool isChanged = false;
        public UserPanel()
        {
            InitializeComponent();
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
                    dto.Senha = txtSenha.Password.Replace("'", "''");
                    passwordsave = bll.SavePassword(dto);
                    txtSenha.Password = "";
                    txtConfirmaSenha.Password = "";
                }
                else
                {
                    CustomOKMessageBox.Show("As senhas não são iguais.", "Atenção!");
                }
            }

            if (passwordsave || imagesave)
            {
                passwordsave = false;
                imagesave = false;
                CustomOKMessageBox.Show("Dados alterados com sucesso.", "Sucesso!");
            }


        }
    }
}
