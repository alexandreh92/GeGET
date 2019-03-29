using System;
using DTO;
using DAL;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace BLL
{
    class LoginBLL
    {
        #region Declarations
        AcessoBancoDados bd = new AcessoBancoDados();
        #endregion

        #region Methods

        #region Login
        public bool Login(LoginDTO dto)
        {
            string usuario = dto.Usuario.Replace("'", "''");
            string senha = dto.Senha.Replace("'", "''");

            var query = "SELECT SUBSTRING_INDEX(f.nome, ' ', 1) as primeiro_nome, SUBSTRING_INDEX(f.nome, ' ', -1) as ultimo_nome, u.id, f.nome, u.nivel_usuario_id, u.foto FROM usuario u JOIN funcionario f ON f.id = u.funcionario_id WHERE login='" + usuario + "' AND senha='" + senha + "' AND u.STATUS_ID='1'";

            try
            {
                bd.Conectar();
                var rd = bd.RetDataReader(query);
                if (rd.HasRows)
                {
                    dto.Id = Convert.ToInt32(rd["id"]);
                    dto.Nome = rd["nome"].ToString();
                    dto.Nivel = Convert.ToInt32(rd["nivel_usuario_id"]);
                    dto.Primeiro_Nome = rd["primeiro_nome"].ToString();
                    dto.Ultimo_Sobrenome = rd["ultimo_nome"].ToString();

                    
                    if (!String.IsNullOrEmpty(rd["foto"].ToString()))
                    {
                        byte[] blob = (byte[])rd["foto"];
                        MemoryStream stream = new MemoryStream();
                        stream.Write(blob, 0, blob.Length);
                        stream.Position = 0;
                        System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();
                        MemoryStream ms = new MemoryStream();
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                        ms.Seek(0, SeekOrigin.Begin);
                        bi.StreamSource = ms;
                        bi.EndInit();
                        dto.Foto = bi;
                    }
                    else
                    {
                        Image image = new Image();
                        Uri uri = new Uri("pack://application:,,,/Resources/Images/User.png");
                        BitmapImage bi = new BitmapImage(uri);
                        dto.Foto = bi;
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                bd.CloseConection();
            }
        }
        #endregion

        #region Salvar Senha

        public bool SavePassword(LoginDTO dto)
        {
            try
            {
                var query = "UPDATE usuario SET senha='"+ dto.Senha +"' WHERE id = '"+ dto.Id +"'";
                bd.Conectar();
                bd.ExecutarComandoSQL(query);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        #endregion

        #region Salvar Foto

        public bool SavePhoto(LoginDTO dto)
        {
            try
            {
                var query = "UPDATE usuario SET foto=@IMG WHERE id = '" + dto.Id + "'";
                bd.Conectar();
                bd.ExecutarComandoSQLWithByteParameter(query, dto.Photo);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        #endregion

        #endregion
    }
}
