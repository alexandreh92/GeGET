using System;
using System.Collections.Generic;
using DTO;
using DAL;
using System.Data;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace BLL
{
    class NegociosBLL
    {
        #region Declarations
        AcessoBancoDados bd = new AcessoBancoDados();
        NegociosDTO dto = new NegociosDTO();
        LoginDTO Logindto = new LoginDTO();
        #endregion

        #region Methods
        #region Load Negocios
        public ObservableCollection<NegociosDTO> LoadNegocios(NegociosDTO DTO)
        {
            string Procurar = "";
            if (dto.FromParent)
            {
                Procurar = "WHERE c.id = '" + dto.ParentId + "'";
            }
            else if (dto.FromChildrenParent)
            {
                Procurar = "WHERE e.id = '" + dto.ParentId + "'";
            }
            var negocios = new ObservableCollection<NegociosDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT n.id, n.descricao, n.anotacoes, n.prazo, n.valor_fechamento, n.data, v.nome, s.descricao as status_descricao, s.id as status_id, e.cnpj, e.endereco, cid.cidade, e.id as estabelecimento_id, c.id as cliente_id, cid.uf, c.rsocial, c.fantasia FROM negocio n JOIN vendedor v ON n.VENDEDOR_id = v.id JOIN status_orcamento s ON n.STATUS_ORCAMENTO_id = s.id JOIN estabelecimento e ON n.ESTABELECIMENTO_id = e.id JOIN cidades cid ON e.CIDADES_id = cid.id JOIN cliente c ON e.CLIENTE_id = c.id " + Procurar + " ORDER BY n.id";
                bd.Conectar();
                dt = bd.RetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                foreach (DataRow dr in dt.Rows)
                {
                    negocios.Add(new NegociosDTO { Id = dr["id"].ToString(), Numero = "p" + Convert.ToInt32(dr["id"]).ToString("0000"), Razao_Social = dr["rsocial"].ToString(), Descricao = dr["descricao"].ToString(), Anotacoes = dr["anotacoes"].ToString(), Status = Convert.ToInt32(dr["status_id"]), Endereco = dr["endereco"].ToString() + " - " + dr["cidade"].ToString() + " - " + dr["uf"].ToString(), Vendedor = dr["nome"].ToString(), Status_Descricao = dr["status_descricao"].ToString(), Status_Id = Convert.ToInt32(dr["status_id"]), Cliente_Id = dr["cliente_id"].ToString(), Estabelecimento_Id = dr["estabelecimento_id"].ToString(), CidadeEstado = dr["cidade"].ToString() + " - " + dr["uf"].ToString() });
                }
                bd.CloseConection();
            }
            return negocios;
        }
        #endregion

        #region Load Orcamentos
        public ObservableCollection<NegociosDTO> LoadOrcamentos(NegociosDTO DTO)
        {
            string Procurar = "";
            if (dto.FromParent)
            {
                Procurar = "WHERE c.id = '" + dto.ParentId + "'";
            }
            else if (dto.FromChildrenParent)
            {
                Procurar = "WHERE e.id = '" + dto.ParentId + "'";
            }
            var negocios = new ObservableCollection<NegociosDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT n.id, n.descricao, n.anotacoes, n.prazo, n.valor_fechamento, n.data, v.nome, s.descricao as status_descricao, s.id as status_id, e.cnpj, e.endereco, cid.cidade, e.id as estabelecimento_id, c.id as cliente_id, cid.uf, c.rsocial, c.fantasia FROM negocio n JOIN vendedor v ON n.VENDEDOR_id = v.id JOIN status_orcamento s ON n.STATUS_ORCAMENTO_id = s.id JOIN estabelecimento e ON n.ESTABELECIMENTO_id = e.id JOIN cidades cid ON e.CIDADES_id = cid.id JOIN cliente c ON e.CLIENTE_id = c.id " + Procurar + " join orcamentista_negocio orc ON orc.negocio_id = n.id WHERE orc.usuario_id = '"+ Logindto.Id +"' AND n.status_orcamento_id = '2' ORDER BY n.id";
                bd.Conectar();
                dt = bd.RetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                foreach (DataRow dr in dt.Rows)
                {
                    negocios.Add(new NegociosDTO { Id = dr["id"].ToString(), Numero = "p" + Convert.ToInt32(dr["id"]).ToString("0000"), Razao_Social = dr["rsocial"].ToString(), Descricao = dr["descricao"].ToString(), Anotacoes = dr["anotacoes"].ToString(), Status = Convert.ToInt32(dr["status_id"]), Endereco = dr["endereco"].ToString() + " - " + dr["cidade"].ToString() + " - " + dr["uf"].ToString(), Vendedor = dr["nome"].ToString(), Status_Descricao = dr["status_descricao"].ToString(), Status_Id = Convert.ToInt32(dr["status_id"]), Cliente_Id = dr["cliente_id"].ToString(), Estabelecimento_Id = dr["estabelecimento_id"].ToString(), CidadeEstado = dr["cidade"].ToString() + " - " + dr["uf"].ToString() });
                }
                bd.CloseConection();
            }
            return negocios;
        }
        #endregion

        #region Load Gerenciar Orcamento
        public ObservableCollection<NegociosDTO> LoadGerenciarOrcamento(NegociosDTO DTO)
        {
            var negocios = new ObservableCollection<NegociosDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT n.id, n.descricao, n.anotacoes, cc.nome as responsavel, c.rsocial, e.cnpj, n.versao_valida, n.prazo, n.valor_fechamento, n.data, v.nome as vendedor, s.descricao as status_descricao, s.id as status_id, e.cnpj, e.endereco, cid.cidade, cid.uf FROM negocio n JOIN vendedor v ON n.VENDEDOR_id = v.id JOIN status_orcamento s ON n.STATUS_ORCAMENTO_id = s.id JOIN estabelecimento e ON n.ESTABELECIMENTO_id = e.id JOIN cidades cid ON e.CIDADES_id = cid.id JOIN contato_cliente cc ON n.contato_cliente_id = cc.id JOIN cliente c ON e.cliente_id = c.id WHERE n.id = '"+DTO.Id+"' ORDER BY n.id";
                bd.Conectar();
                dt = bd.RetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                foreach (DataRow dr in dt.Rows)
                {
                    negocios.Add(new NegociosDTO { Id = dr["id"].ToString(), Numero = "p" + Convert.ToInt32(dr["id"]).ToString("0000"), Razao_Social = dr["rsocial"].ToString(), Descricao = dr["descricao"].ToString(), Anotacoes = dr["anotacoes"].ToString(), Status = Convert.ToInt32(dr["status_id"]), Endereco = dr["endereco"].ToString() + " - " + dr["cidade"].ToString() + " - " + dr["uf"].ToString(), Vendedor = dr["vendedor"].ToString(), Status_Descricao = dr["status_descricao"].ToString(), Status_Id = Convert.ToInt32(dr["status_id"]), CidadeEstado = dr["cidade"].ToString() + " - " + dr["uf"].ToString(), Cnpj = dr["cnpj"].ToString(), Contato_Nome = dr["responsavel"].ToString(), Prazo = dr["prazo"].ToString(), Versao_Id = dr["versao_valida"].ToString() });
                }
                bd.CloseConection();
            }
            return negocios;
        }
        #endregion

        #region Load Orcamentistas
        public ObservableCollection<OrcamentistasDTO> LoadOrcamentistas(NegociosDTO DTO)
        {
            var orcamentistas = new ObservableCollection<OrcamentistasDTO>();
            DataTable dt = new DataTable();
            try
            {
                var query = "select T2.id, SUBSTRING_INDEX(f.nome, ' ', 1) as primeiro_nome, SUBSTRING_INDEX(f.nome, ' ', -1) as ultimo_nome, f.email, f.nome, s.descricao as setor, T2.foto, s.descricao, f.celular, T2.login from usuario as T2 JOIN funcionario f ON f.id =T2.id JOIN setor s ON s.id = f.setor_id where not exists (select * from orcamentista_negocio as T1 where T1.USUARIO_id = T2.id AND T1.NEGOCIO_id = '" + DTO.Id + "' AND T2.STATUS_id='1') ORDER BY nome ASC";
                bd.Conectar();
                dt = bd.RetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                foreach (DataRow dr in dt.Rows)
                {
                    BitmapImage bi = new BitmapImage();

                    if (!String.IsNullOrEmpty(dr["foto"].ToString()))
                    {
                        byte[] blob = (byte[])dr["foto"];
                        MemoryStream stream = new MemoryStream();
                        stream.Write(blob, 0, blob.Length);
                        stream.Position = 0;
                        System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                        bi.BeginInit();
                        MemoryStream ms = new MemoryStream();
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                        ms.Seek(0, SeekOrigin.Begin);
                        bi.StreamSource = ms;
                        bi.EndInit();
                    }
                    else
                    {
                        Image image = new Image();
                        Uri uri = new Uri("pack://application:,,,/Resources/Images/User.png");
                        bi = new BitmapImage(uri);
                    }
                    orcamentistas.Add(new OrcamentistasDTO { Id = dr["id"].ToString(), Nome_Simples = dr["primeiro_nome"].ToString() + " " + dr["ultimo_nome"].ToString(), Email = dr["email"].ToString(), Celular = dr["celular"].ToString(), Nome = dr["nome"].ToString(), Foto = bi, Setor = dr["setor"].ToString(), Login = dr["login"].ToString() });
                }
                
            }


            return orcamentistas;
        }

        #endregion

        #region Load Orcamentista Cadastrado
        public ObservableCollection<OrcamentistasDTO> LoadOrcamentistaCadastrado(NegociosDTO DTO)
        {
            var orcamentistas = new ObservableCollection<OrcamentistasDTO>();
            DataTable dt = new DataTable();
            try
            {
                var query = "SELECT SUBSTRING_INDEX(f.nome, ' ', 1) as primeiro_nome, SUBSTRING_INDEX(f.nome, ' ', -1) as ultimo_nome, u.id, f.email, f.celular, f.nome, s.descricao as setor,u.foto, u.login FROM usuario u JOIN funcionario f ON f.id = u.funcionario_id JOIN setor s ON s.id = f.setor_id JOIN orcamentista_negocio orn ON u.id = orn.usuario_id join negocio n ON n.id = orn.negocio_id WHERE n.id ='"+ DTO.Id +"'";
                bd.Conectar();
                dt = bd.RetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                foreach (DataRow dr in dt.Rows)
                {
                    BitmapImage bi = new BitmapImage();

                    if (!String.IsNullOrEmpty(dr["foto"].ToString()))
                    {
                        byte[] blob = (byte[])dr["foto"];
                        MemoryStream stream = new MemoryStream();
                        stream.Write(blob, 0, blob.Length);
                        stream.Position = 0;
                        System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                        bi.BeginInit();
                        MemoryStream ms = new MemoryStream();
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                        ms.Seek(0, SeekOrigin.Begin);
                        bi.StreamSource = ms;
                        bi.EndInit();
                    }
                    else
                    {
                        Image image = new Image();
                        Uri uri = new Uri("pack://application:,,,/Resources/Images/User.png");
                        bi = new BitmapImage(uri);
                    }
                    orcamentistas.Add(new OrcamentistasDTO { Id = dr["id"].ToString(), Nome_Simples = dr["primeiro_nome"].ToString() + " " + dr["ultimo_nome"].ToString(), Email = dr["email"].ToString(), Celular = dr["celular"].ToString(), Nome = dr["nome"].ToString(), Foto = bi, Setor = dr["setor"].ToString(), Login = dr["login"].ToString() });
                }

            }


            return orcamentistas;
        }
        #endregion

        #region Inserir Orçamentista

        public void InserirOracamentista(OrcamentistasDTO DTO)
        {
            try
            {
                var mensagem = "◾ ORÇAMENTISTA ADICIONADO → " + DTO.Nome_Simples;
                var tipo = "ADIÇÃO DE ORÇAMENTISTA";
                var data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                var usermessage = Logindto.Nome + " ADICIONOU VOCÊ COMO ORÇAMENTISTA DO NEGÓCIO P" + Convert.ToInt32(DTO.Negocio_Id).ToString("0000") + ".";

                var query = "INSERT INTO orcamentista_negocio (NEGOCIO_id, USUARIO_id) VALUES ('" + DTO.Negocio_Id + "','" + DTO.Id + "'); INSERT INTO mensagem_" + DTO.Login + " (USUARIO_id, USUARIO_FROM_id, mensagem, data, NEGOCIO_id, descricao) VALUES ('" + DTO.Id + "','" + Logindto.Id + "', '" + usermessage + "', '" + data + "','" + DTO.Negocio_Id + "', 'Orçamento Cadastrado'); INSERT INTO log_negocios (tipo, data, descricao, USUARIO_id, NEGOCIO_id) VALUES ('" + tipo + "','" + data + "','" + mensagem + "','" + Logindto.Id + "','" + DTO.Negocio_Id + "')";
                bd.Conectar();
                bd.ExecutarComandoSQL(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        #endregion

        #region Remover Orcamentista

        public void RemoverOrcamentista(OrcamentistasDTO DTO)
        {
            try
            {
                var mensagem = "◾ ORÇAMENTISTA REMOVIDO → " + DTO.Nome_Simples;
                var tipo = "REMOÇÃO DE ORÇAMENTISTA";
                var data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                var query = "DELETE FROM orcamentista_negocio WHERE negocio_id = '" + DTO.Negocio_Id + "' AND usuario_id='" + DTO.Id + "'; INSERT INTO log_negocios (tipo, data, descricao, USUARIO_id, NEGOCIO_id) VALUES ('" + tipo + "','" + data + "','" + mensagem + "','" + Logindto.Id + "','" + DTO.Negocio_Id + "')";
                bd.Conectar();
                bd.ExecutarComandoSQL(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        #endregion

        #region HABILITAR ORÇAMENTO
        public void HabilitarOrcamento(NegociosDTO DTO)
        {
            try
            {
                var mensagem = "◾ " + DTO.Status_Descricao + " ➞ HABILITADO";
                var tipo = "STATUS ALTERADO";
                var data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                var query = "UPDATE negocio SET STATUS_ORCAMENTO_id='2' WHERE id='" + DTO.Id + "'; INSERT INTO log_negocios (tipo, data, descricao, USUARIO_id, NEGOCIO_id) VALUES ('" + tipo + "','" + data + "','" + mensagem.ToUpper() + "','" + Logindto.Id + "','" + DTO.Id + "')";
                bd.Conectar();
                bd.ExecutarComandoSQL(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        #endregion

        #region MUDAR STATUS PARA ENVIADO

        public void EnviarCliente(NegociosDTO DTO)
        {
            try
            {
                var mensagem = "◾ " + DTO.Status_Descricao + " ➞ ENVIADO AO CLIENTE";
                var tipo = "STATUS ALTERADO";
                var data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                var query = "UPDATE negocio SET STATUS_ORCAMENTO_id='3' WHERE id='" + DTO.Id + "'; INSERT INTO log_negocios (tipo, data, descricao, USUARIO_id, NEGOCIO_id) VALUES ('" + tipo + "','" + data + "','" + mensagem.ToUpper() + "','" + Logindto.Id + "','" + DTO.Id + "'); UPDATE versao_atividade, negocio set valor_envio = '"+ DTO.Valor_Enviado +"', data_envio = '"+ DateTime.Now.ToString("yyyy/MM/dd") +"' WHERE negocio_id = '"+ DTO.Id +"' AND negocio.versao_valida = versao_atividade.versao_id";
                var notify = Logindto.Nome + " ENVIOU O NEGÓCIO P" + Convert.ToInt32(DTO.Id).ToString("0000") + " PARA O CLIENTE.";
                bd.Conectar();
                bd.ExecutarComandoSQL(query);
                DataTable dt = new DataTable();
                var queryselect = "SELECT distinct usuario_id, login FROM orcamentista_negocio orn JOIN usuario u ON u.id = orn.usuario_id WHERE negocio_id = '" + DTO.Id + "' AND usuario_id != '" + Logindto.Id + "'";
                bd.Conectar();
                dt = bd.RetDataTable(queryselect);
                foreach (DataRow dr in dt.Rows)
                {
                    var user_id = dr["usuario_id"].ToString();
                    var nome = dr["login"].ToString();
                    var querynotify = "INSERT INTO mensagem_" + nome + " (USUARIO_id, USUARIO_FROM_id, mensagem, data, NEGOCIO_id, descricao) VALUES ('" + user_id + "', '" + Logindto.Id + "', '" + notify.ToUpper() + "', '" + data + "', '" + DTO.Id + "', 'Status Atualizado')";
                    bd.ExecutarComandoSQL(querynotify);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        #endregion

        #region MUDAR STATUS PARA EM NEGOCIAÇÃO

        public void EmNegociacao(NegociosDTO DTO)
        {
            try
            {
                var mensagem = "◾ " + DTO.Status_Descricao + " ➞ EM NEGOCIAÇÃO";
                var tipo = "STATUS ALTERADO";
                var data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                var query = "UPDATE negocio SET STATUS_ORCAMENTO_id='4', negociado='1' WHERE id='" + DTO.Id + "'; INSERT INTO log_negocios (tipo, data, descricao, USUARIO_id, NEGOCIO_id) VALUES ('" + tipo + "','" + data + "','" + mensagem.ToUpper() + "','" + Logindto.Id + "','" + DTO.Id + "')";
                var notify = Logindto.Nome + " MARCOU O NEGÓCIO P" + Convert.ToInt32(DTO.Id).ToString("0000") + " COMO EM NEGOCIAÇÃO.";
                bd.Conectar();
                bd.ExecutarComandoSQL(query);
                DataTable dt = new DataTable();
                var queryselect = "SELECT distinct usuario_id, login FROM orcamentista_negocio orn JOIN usuario u ON u.id = orn.usuario_id WHERE negocio_id = '" + DTO.Id + "' AND usuario_id != '" + Logindto.Id + "'";
                bd.Conectar();
                dt = bd.RetDataTable(queryselect);
                foreach (DataRow dr in dt.Rows)
                {
                    var user_id = dr["usuario_id"].ToString();
                    var nome = dr["login"].ToString();
                    var querynotify = "INSERT INTO mensagem_" + nome + " (USUARIO_id, USUARIO_FROM_id, mensagem, data, NEGOCIO_id, descricao) VALUES ('" + user_id + "', '" + Logindto.Id + "', '" + notify.ToUpper() + "', '" + data + "', '" + DTO.Id + "', 'Status Atualizado')";
                    bd.ExecutarComandoSQL(querynotify);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        #endregion

        #region MUDAR STATUS PARA FECHADO

        #endregion

        #region CANCELAR NEGÓCIO

        #endregion

        #region Create Negocios

        public bool CreateNegocios(NegociosDTO DTO)
        {
            bool sucess = false;
            try
            {
                var tipo = "NEGÓCIO CRIADO";
                var mensagem = "◾ VENDEDOR: " + DTO.Vendedor + "\n◾ RESPONSÁVEL CLIENTE: " + DTO.Contato_Nome + "\n◾ Prioridade: " + DTO.Prioridade_Descricao + "\n◾ DESCRIÇÃO: " + DTO.Descricao + "\n◾ PRAZO: " + Convert.ToDateTime(DTO.Prazo).ToString("dd/MM/yyyy");
                var data = DateTime.Now.ToString("dd/MM/yyyy");
                var query = "INSERT INTO negocio (descricao, anotacoes, data, prazo, VENDEDOR_id, ESTABELECIMENTO_id, CONTATO_CLIENTE_id, USUARIO_id, PRIORIDADE_id) VALUES ('" + DTO.Descricao + "', '" + DTO.Anotacoes + "', '"+ data +"' ,'" + DTO.Prazo + "', '" + DTO.Vendedor_Id + "', '" + DTO.Estabelecimento_Id + "','" + DTO.Contato_Id + "', '" + Logindto.Id + "', '" + DTO.Prioridade_Id + "');"
                    + "INSERT INTO versao_atividade(descricao, NEGOCIO_id, USUARIO_id) VALUES('VERSÃO INICIAL', (SELECT MAX(id) as id FROM negocio), '"+ Logindto.Id +"');"
                    + "INSERT INTO log_negocios (tipo, descricao, USUARIO_id, NEGOCIO_id) VALUES ('" + tipo + "','" + mensagem + "','" + Logindto.Id + "',(SELECT MAX(id) as id FROM negocio))";
                bd.Conectar();
                bd.ExecutarComandoSQL(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            { 
                sucess = true;
                bd.CloseConection();
            }
            if (sucess)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region RetNegocioId

        public NegociosDTO RetNegocioId()
        {
            NegociosDTO negocio = new NegociosDTO();
            try
            {
                var query = "(SELECT MAX(id) as id FROM negocio)";
                bd.Conectar();
                var rd = bd.RetDataReader(query);
                negocio.Id = rd["id"].ToString();
            }
            catch (Exception)
            {
                throw;
            }
            return negocio;
        }



        #endregion

        #endregion
    }
}
