using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using BLL;
using DTO;

namespace GeGET
{
    public partial class CadastroUsuario : UserControl
    {
        #region Declarations
        bool disposed = false;
        CategoriaClienteBLL Categoriabll = new CategoriaClienteBLL();
        CadastroUsuarioBLL bll = new CadastroUsuarioBLL();
        CadastroUsuarioDTO dto = new CadastroUsuarioDTO();
        Helpers helpers = new Helpers();
        #endregion

        #region Initialize
        public CadastroUsuario()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods

        #region ClearControls
        private void ClearControls()
        {
            
        }
        #endregion

        #endregion

        #region Events

        #region Clicks
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            helpers.OpenBack(false);
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            helpers.Close();
        }

        private async void BtnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            if (txtLogin.Text != "" && txtPassword.Password != "" && txtPasswordConfirmation.Password != "")
            {
                if(txtPassword.Password == txtPasswordConfirmation.Password)
                {
                    var result = CustomOKCancelMessageBox.Show("Deseja mesmo cadastrar este usuário?", "Atenção!", Window.GetWindow(this));
                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        dto.Login = txtLogin.Text.Replace("'", "''").ToLower();
                        dto.Password = Encrypt(txtPassword.Password.Replace("'", "''"));
                        dto.Name = txtNome.Text.Replace("'", "''");
                        if (bll.CreateUser(dto))
                        {
                            CustomOKMessageBox.Show("Usuário cadastrado com sucesso.", "Sucesso!", Window.GetWindow(this));
                        }
                        else
                        {
                            CustomOKMessageBox.Show("Ocorreu algum erro.", "Atenção!", Window.GetWindow(this));
                        }
                    }
                }
                else
                {
                    CustomOKMessageBox.Show("As senhas não são iguais.", "Atenção!", Window.GetWindow(this));
                }
            }
            else
            {
                CustomOKMessageBox.Show("Campos não podem estar em branco.", "Atenção!", Window.GetWindow(this));
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
        #endregion

        #endregion
    }
}
