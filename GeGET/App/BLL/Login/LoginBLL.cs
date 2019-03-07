using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;
using GeGET;
using System.Windows;

namespace BLL
{
    class LoginBLL
    {
        AcessoBancoDados bd = new AcessoBancoDados();

        public bool Login(LoginDTO dto)
        {
            string usuario = dto.Usuario.Replace("'", "''");
            string senha = dto.Senha.Replace("'", "''");

            var query = "SELECT SUBSTRING_INDEX(f.nome, ' ', 1) as primeiro_nome, SUBSTRING_INDEX(f.nome, ' ', -1) as ultimo_nome, u.id, f.nome, u.nivel_usuario_id FROM usuario u JOIN funcionario f ON f.id = u.funcionario_id WHERE login='" + usuario + "' AND senha='" + senha + "' AND u.STATUS_ID='1'";

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
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.CloseConection();
            }
        }
    }
}
