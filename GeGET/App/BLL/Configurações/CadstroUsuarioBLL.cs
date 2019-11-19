using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;
using MySql.Data.MySqlClient;
using System.Windows;

namespace BLL
{
    class CadastroUsuarioBLL
    {
        AcessoBancoDados bd = new AcessoBancoDados();

        public bool CreateUser(CadastroUsuarioDTO dTO)
        {
            var success = false;
            var query_funcionario = "INSERT INTO funcionario (nome) VALUES ('"+ dTO.Name +"')";
            try
            {
                bd.Conectar();
                bd.ExecutarComandoSQL(query_funcionario);
            }
            catch (Exception)
            { 
                throw;
            }
            finally
            {
                try
                {
                    var select_funcionario = "SELECT id FROM funcionario WHERE nome = '"+ dTO.Name + "' ORDER BY id DESC LIMIT 1;";
                    bd.Conectar();
                    var dr = bd.RetDataReader(select_funcionario);
                    dTO.Id = dr[0].ToString();
                    try
                    {
                        var query_usuario = "INSERT INTO usuario (login, senha, funcionario_id) VALUES ('" + dTO.Login + "', '" + dTO.Password + "', '" + dTO.Id + "'); CREATE TABLE `mensagem_" + dTO.Login.ToLower() +"` (`id` int(11) NOT NULL AUTO_INCREMENT,`USUARIO_id` int(11) NOT NULL,`USUARIO_FROM_id` int(11) DEFAULT NULL, `mensagem` longtext NOT NULL, `lida` tinyint(1) NOT NULL DEFAULT '0', `data` datetime NOT NULL, `NEGOCIO_id` int(11) DEFAULT NULL, `descricao` varchar(200) NOT NULL, PRIMARY KEY(`id`,`USUARIO_id`)); ";
                        bd.Conectar();
                        bd.ExecutarComandoSQL(query_usuario);
                        success = true;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                
            }
            return success;
        }
    }
}
