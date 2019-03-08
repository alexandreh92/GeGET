﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using DAL;
using Devart.Data.MySql;
using DTO;
using GeGET;

namespace BLL
{
    class MySQLDependency
    {
        AcessoBancoDados bd = new AcessoBancoDados();
        LayoutDTO dto = new LayoutDTO();
        LoginDTO Logindto = new LoginDTO();

        //ComponentResourceManager resources = new ComponentResourceManager(typeof(ResourceForm));

        public void Start()
        {
            var query = "SELECT * FROM mensagem_" + Logindto.Usuario + "";
            bd.ConectarDevArt();
            var dependency = bd.Dependency(query);
            dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);
            bd.StartDependency();
        }

        public void Stop()
        {
            bd.StopDependency();
        }

        private void dependency_OnChange(object sender, MySqlTableChangeEventArgs e)
        {
            if (!Logindto.SupressChange)
            {
                int count = 0;
                try
                {
                    var query = "SELECT (SELECT COUNT(*) from mensagem_" + Logindto.Usuario + " WHERE lida='0') as quantidade, f.nome, descricao, mensagem, negocio_id FROM mensagem_" + Logindto.Usuario + " mu JOIN usuario u ON u.id = mu.usuario_from_id JOIN funcionario f ON f.id = u.funcionario_id ORDER BY mu.data DESC LIMIT 1";
                    bd.Conectar();
                    var dr = bd.RetDataReader(query);
                    dto.Nome = dr["nome"].ToString();
                    dto.Descricao = dr["descricao"].ToString();
                    dto.Mensagem = dr["mensagem"].ToString();
                    dto.Negocio = Convert.ToInt32(dr["negocio_id"]).ToString("0000");
                    count = Convert.ToInt32(dr["quantidade"]);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => 
                    {
                        TaskbarMessage ws = new TaskbarMessage(dto.Descricao, dto.Mensagem);
                        ws.Show();
                        var targetWindow = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is Layout) as Layout;
                        targetWindow.btnNotificationIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Bell;
                        if (count > 0)
                        {
                            targetWindow.BadgeControl.Badge = count;
                        }
                    }
                    ));
                }
            }
        }

        public bool HasNewMessages()
        {
            try
            {
                var query = "SELECT COUNT(*) as quantidade from mensagem_" + Logindto.Usuario + " WHERE lida='0'";
                bd.Conectar();
                var dr = bd.RetDataReader(query);
                if (Convert.ToInt32(dr["quantidade"]) > 0)
                {
                    dto.Quantidade = Convert.ToInt32(dr["quantidade"]);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                bd.CloseConection();
            }
        }


        public List<LayoutDTO> LoadMensagens()
        {
            var mensagens = new List<LayoutDTO>();
            try
            {
                var query = "SELECT f.nome, mu.id as msg_id, lida, descricao, mu.data, mensagem, negocio_id FROM mensagem_" + Logindto.Usuario + " mu JOIN usuario u ON u.id = mu.usuario_from_id JOIN funcionario f ON f.id = u.funcionario_id ORDER BY mu.data DESC";
                bd.Conectar();
                var dt = bd.RetDataTable(query);
                foreach (DataRow dr in dt.Rows)
                {
                    mensagens.Add(new LayoutDTO { Id = dr["msg_id"].ToString(), Descricao = dr["descricao"].ToString(), Mensagem = dr["mensagem"].ToString(), Negocio = Convert.ToInt32(dr["negocio_id"]).ToString("0000"), Nome = dr["nome"].ToString(), Lida = dr["lida"].ToString(), Data = Convert.ToDateTime(dr["data"]).ToString("dd/MM/yyyy") + " - " + Convert.ToDateTime(dr["data"]).ToString("HH:mm") });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                bd.CloseConection();
            }
            return mensagens;
        }


        public void MarkAsRead()
        {
            Logindto.SupressChange = true;
            try
            {
                var query = "UPDATE mensagem_" + Logindto.Usuario + " SET lida='1' WHERE lida='0'";
                bd.Conectar();
                bd.ExecutarComandoSQL(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                bd.CloseConection();
            }
        }

    }
}