using System;
using DTO;
using DAL;
using System.Data;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows;
using System.Threading.Tasks;

namespace BLL
{
    class NegociosBLL : IDisposable
    {
        #region Declarations
        bool disposed = false;
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
                var query = "SELECT n.id, n.descricao, n.anotacoes, n.prazo, n.valor_fechamento, n.data, v.nome, s.descricao as status_descricao, s.id as status_id, e.cnpj, e.endereco, cid.cidade, e.id as estabelecimento_id, c.id as cliente_id, cid.uf, c.rsocial, c.fantasia, e.descricao as descricao_estabelecimento FROM negocio n JOIN vendedor v ON n.VENDEDOR_id = v.id JOIN status_orcamento s ON n.STATUS_ORCAMENTO_id = s.id JOIN estabelecimento e ON n.ESTABELECIMENTO_id = e.id JOIN cidades cid ON e.CIDADES_id = cid.id JOIN cliente c ON e.CLIENTE_id = c.id " + Procurar + " ORDER BY n.id";
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
                    negocios.Add(new NegociosDTO { Id = dr["id"].ToString(), Numero = "p" + Convert.ToInt32(dr["id"]).ToString("0000"), Razao_Social = dr["rsocial"].ToString(), Descricao = dr["descricao"].ToString(), Anotacoes = dr["anotacoes"].ToString(), Status = Convert.ToInt32(dr["status_id"]), Endereco = dr["endereco"].ToString() + " - " + dr["cidade"].ToString() + " - " + dr["uf"].ToString(), Vendedor = dr["nome"].ToString(), Status_Descricao = dr["status_descricao"].ToString(), Status_Id = Convert.ToInt32(dr["status_id"]), Cliente_Id = dr["cliente_id"].ToString(), Estabelecimento_Id = dr["estabelecimento_id"].ToString(), CidadeEstado = dr["cidade"].ToString() + " - " + dr["uf"].ToString(), ProgressBarValue = Convert.ToInt32(dr["status_id"])*20, Cnpj = dr["cnpj"].ToString(), Descricao_Estabelecimento = dr["descricao_estabelecimento"].ToString(), Prazo = Convert.ToDateTime(dr["prazo"]).ToString("dd/MM/yyyy"), Nome_Fantasia = dr["fantasia"].ToString() });
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
                var query = "SELECT n.id, n.descricao, n.anotacoes, n.prazo, va.locked, n.valor_fechamento, n.data, v.nome, s.descricao as status_descricao, s.id as status_id, e.cnpj, e.endereco, cid.cidade, e.id as estabelecimento_id, c.id as cliente_id, cid.uf, c.rsocial, c.fantasia, e.descricao as descricao_estabelecimento FROM negocio n JOIN vendedor v ON n.VENDEDOR_id = v.id JOIN status_orcamento s ON n.STATUS_ORCAMENTO_id = s.id JOIN estabelecimento e ON n.ESTABELECIMENTO_id = e.id JOIN cidades cid ON e.CIDADES_id = cid.id JOIN cliente c ON e.CLIENTE_id = c.id join versao_atividade va ON va.negocio_id = n.id and va.versao_id = n.versao_valida " + Procurar + " ORDER BY n.id";
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
                    negocios.Add(new NegociosDTO { Id = dr["id"].ToString(), Numero = "p" + Convert.ToInt32(dr["id"]).ToString("0000"), Razao_Social = dr["rsocial"].ToString(), Descricao = dr["descricao"].ToString(), Anotacoes = dr["anotacoes"].ToString(), Status = Convert.ToInt32(dr["status_id"]), Endereco = dr["endereco"].ToString() + " - " + dr["cidade"].ToString() + " - " + dr["uf"].ToString(), Vendedor = dr["nome"].ToString(), Status_Descricao = dr["status_descricao"].ToString(), Status_Id = Convert.ToInt32(dr["status_id"]), Cliente_Id = dr["cliente_id"].ToString(), Estabelecimento_Id = dr["estabelecimento_id"].ToString(), CidadeEstado = dr["cidade"].ToString() + " - " + dr["uf"].ToString(), Versao_Locked = Convert.ToInt32(dr["locked"]), Cnpj = dr["cnpj"].ToString(), Descricao_Estabelecimento= dr["descricao_estabelecimento"].ToString() });
                }
                bd.CloseConection();
            }
            return negocios;
        }
        #endregion

        #region Load Orcamentos Cadastrados
        public ObservableCollection<NegociosDTO> LoadOrcamentosCadastrados(NegociosDTO DTO)
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
                var query = "SELECT distinct t1.* FROM (SELECT n.id, n.descricao, n.anotacoes, n.prazo, n.valor_fechamento, n.data, v.nome, s.descricao as status_descricao, s.id as status_id, e.cnpj, e.endereco, cid.cidade, e.id as estabelecimento_id, c.id as cliente_id, cid.uf, c.rsocial, c.fantasia, e.descricao as descricao_estabelecimento FROM negocio n JOIN vendedor v ON n.VENDEDOR_id = v.id JOIN status_orcamento s ON n.STATUS_ORCAMENTO_id = s.id JOIN estabelecimento e ON n.ESTABELECIMENTO_id = e.id JOIN cidades cid ON e.CIDADES_id = cid.id JOIN cliente c ON e.CLIENTE_id = c.id join orcamentista_negocio orc ON orc.negocio_id = n.id WHERE orc.usuario_id = '"+ Logindto.Id +"' AND n.status_orcamento_id = '2') as t1" +
                            " LEFT OUTER JOIN (SELECT distinct a.id as atividade, a.negocio_id FROM atividade a JOIN versao_atividade va ON a.versao_atividade_id = va.id JOIN negocio n ON n.id = a.negocio_id AND n.versao_valida = va.versao_id JOIN orcamentista_negocio orn ON orn.negocio_id = n.id) as t2 ON t1.id = t2.negocio_id WHERE t2.negocio_id = t1.id GROUP BY t1.id HAVING COUNT(t2.atividade) > 0 ORDER BY t1.id ASC";
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
                    negocios.Add(new NegociosDTO { Id = dr["id"].ToString(), Numero = "p" + Convert.ToInt32(dr["id"]).ToString("0000"), Razao_Social = dr["rsocial"].ToString(), Descricao = dr["descricao"].ToString(), Anotacoes = dr["anotacoes"].ToString(), Status = Convert.ToInt32(dr["status_id"]), Endereco = dr["endereco"].ToString() + " - " + dr["cidade"].ToString() + " - " + dr["uf"].ToString(), Vendedor = dr["nome"].ToString(), Status_Descricao = dr["status_descricao"].ToString(), Status_Id = Convert.ToInt32(dr["status_id"]), Cliente_Id = dr["cliente_id"].ToString(), Estabelecimento_Id = dr["estabelecimento_id"].ToString(), CidadeEstado = dr["cidade"].ToString() + " - " + dr["uf"].ToString(),Cnpj = dr["cnpj"].ToString(), Descricao_Estabelecimento = dr["descricao_estabelecimento"].ToString() });
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

        #region Load Negócios Dashboard
        public ObservableCollection<NegociosDTO> LoadNegociosDashboard(NegociosDTO DTO)
        {
            var negocios = new ObservableCollection<NegociosDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT n.id, n.descricao, n.anotacoes, n.prazo, n.valor_fechamento, n.data, v.nome, s.descricao as status_descricao, s.id as status_id, e.cnpj, e.endereco, cid.cidade, e.id as estabelecimento_id, c.id as cliente_id, cid.uf, c.rsocial, c.fantasia FROM negocio n JOIN vendedor v ON n.VENDEDOR_id = v.id JOIN status_orcamento s ON n.STATUS_ORCAMENTO_id = s.id JOIN estabelecimento e ON n.ESTABELECIMENTO_id = e.id JOIN cidades cid ON e.CIDADES_id = cid.id JOIN cliente c ON e.CLIENTE_id = c.id WHERE n.status_orcamento_id != '0' AND n.status_orcamento_id != '5' ORDER BY n.id";
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
                    negocios.Add(new NegociosDTO { Id = dr["id"].ToString(), Numero = "p" + Convert.ToInt32(dr["id"]).ToString("0000"), Razao_Social = dr["rsocial"].ToString(), Descricao = dr["descricao"].ToString(), Anotacoes = dr["anotacoes"].ToString(), Status = Convert.ToInt32(dr["status_id"]), Endereco = dr["endereco"].ToString() + " - " + dr["cidade"].ToString() + " - " + dr["uf"].ToString(), Vendedor = dr["nome"].ToString(), Status_Descricao = dr["status_descricao"].ToString(), Status_Id = Convert.ToInt32(dr["status_id"]), Cliente_Id = dr["cliente_id"].ToString(), Estabelecimento_Id = dr["estabelecimento_id"].ToString(), CidadeEstado = dr["cidade"].ToString() + " - " + dr["uf"].ToString(), ProgressBarValue = Convert.ToInt32(dr["status_id"]) * 20, Nome_Fantasia = dr["fantasia"].ToString(), Prazo = Convert.ToDateTime(dr["prazo"]).ToString("dd/MM/yyyy") });
                }
                bd.CloseConection();
            }
            return negocios;
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
                var query = "UPDATE negocio SET STATUS_ORCAMENTO_id='3' WHERE id='" + DTO.Id + "'; INSERT INTO log_negocios (tipo, data, descricao, USUARIO_id, NEGOCIO_id) VALUES ('" + tipo + "','" + data + "','" + mensagem.ToUpper() + "','" + Logindto.Id + "','" + DTO.Id + "'); UPDATE negocio set valor_envio = '"+ DTO.Valor_Enviado +"', data_envio = '"+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +"' WHERE id = '"+ DTO.Id + "'; UPDATE versao_atividade set locked='1' where negocio_id = '"+DTO.Id+"'";
                var notify = Logindto.Nome + " ENVIOU O NEGÓCIO P" + Convert.ToInt32(DTO.Id).ToString("0000") + " PARA O CLIENTE.";
                bd.Conectar();
                bd.ExecutarComandoSQL(query);
                DataTable dt = new DataTable();
                var queryselect = "SELECT distinct usuario_id, login FROM orcamentista_negocio orn JOIN usuario u ON u.id = orn.usuario_id WHERE orn.negocio_id = '" + DTO.Id + "' AND usuario_id != '" + Logindto.Id + "'";
                bd.Conectar();
                dt = bd.RetDataTable(queryselect);
                foreach (DataRow dr in dt.Rows)
                {
                    var user_id = dr["usuario_id"].ToString();
                    var nome = dr["login"].ToString();
                    var querynotify = "INSERT INTO mensagem_" + nome + " (USUARIO_id, USUARIO_FROM_id, mensagem, data, NEGOCIO_id, descricao) VALUES ('" + user_id + "', '" + Logindto.Id + "', '" + notify.ToUpper() + "', '" + data + "', '" + DTO.Id + "', 'Status Atualizado')";
                    bd.Conectar();
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
                    bd.Conectar();
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
        public void FecharNegocio(NegociosDTO DTO)
        {
            try
            {
                var preco_log = Convert.ToDecimal(DTO.Valor_Fechamento).ToString("c2");
                var mensagem = "◾ " + DTO.Status_Descricao + " ➞ FECHADO\n◾ VALOR NEGOCIADO: " + preco_log;
                var tipo = "STATUS ALTERADO";
                var data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                var query = "UPDATE negocio SET valor_fechamento='" + DTO.Valor_Fechamento + "', STATUS_ORCAMENTO_id='5' WHERE id='" + DTO.Id + "'; INSERT INTO log_negocios (tipo, data, descricao, USUARIO_id, NEGOCIO_id) VALUES ('" + tipo + "','" + data + "','" + mensagem + "','" + Logindto.Id + "','" + DTO.Id + "'); INSERT INTO vendas (NEGOCIO_id, USUARIO_id, data) VALUES ('" + DTO.Id + "', '" + Logindto.Id + "', '" + data + "'); INSERT INTO lista_vendas (PRODUTO_id, VENDAS_id, ATIVIDADE_id, quantidade_orc, preco_orc, desconto_orc, anotacoes, fd, usuario_id, data) SELECT lo.PRODUTO_id, (SELECT id FROM vendas WHERE negocio_id = '" + DTO.Id + "'), (SELECT a.id FROM atividade a JOIN negocio n ON n.id = a.NEGOCIO_id JOIN versao_atividade va ON va.id = a.VERSAO_ATIVIDADE_id WHERE a.NEGOCIO_id = '" + DTO.Id + "' AND va.VERSAO_id = n.versao_valida AND a.id = lo.atividades_id), quantidade, preco_orc*(1-(desconto/100)), desconto, descricao_orc, fd, '" + Logindto.Id + "', '" + data + "' FROM lista_orcamento lo JOIN atividade a ON lo.ATIVIDADES_id = a.id JOIN negocio n ON n.id = lo.negocio_id JOIN versao_atividade va ON a.VERSAO_ATIVIDADE_id = va.id WHERE va.VERSAO_id = n.versao_valida AND lo.NEGOCIO_id = '" + DTO.Id + "' AND a.id = lo.atividades_id";
                var notify = Logindto.Nome + " MARCOU O NEGÓCIO P" + Convert.ToInt32(DTO.Id).ToString("0000") + " COMO FECHADO.";
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
                    bd.Conectar();
                    bd.ExecutarComandoSQL(querynotify);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        #endregion

        #region CANCELAR NEGÓCIO

        public void CancelarNegocio(NegociosDTO DTO)
        {
            var precoperdido = DTO.Valor_Perdido.Replace(",", ".").Replace("'","''");
            var mensagem = "◾ " + DTO.Status_Id + " ➞ CANCELADO\n◾ MOTIVO: " + DTO.Motivo_Cancelamento;
            var tipo = "STATUS ALTERADO";
            var data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var notify = Logindto.Nome + " MARCOU O NEGÓCIO P" + Convert.ToInt32(DTO.Id).ToString("0000") + " COMO CANCELADO.";
            var query = "UPDATE negocio SET STATUS_ORCAMENTO_id='0', MOTIVO_CANCELAMENTO_id='" + DTO.Motivo_Cancelamento_Id + "' WHERE id='" + DTO.Id + "'; INSERT INTO log_negocios (tipo, data, descricao, USUARIO_id, NEGOCIO_id) VALUES ('" + tipo + "','" + data + "','" + mensagem + "','" + Logindto.Id + "','" + DTO.Id + "'); INSERT INTO perdido_preco (preco, NEGOCIO_id) VALUES ('" + DTO.Valor_Perdido + "', '" + DTO.Id + "')";
            bd.Conectar();
            bd.ExecutarComandoSQL(query);
            var queryselect = "SELECT distinct usuario_id, login FROM orcamentista_negocio orn JOIN usuario u ON u.id = orn.usuario_id WHERE negocio_id = '" + DTO.Id + "' AND usuario_id != '" + Logindto.Id + "'";
            bd.Conectar();
            DataTable dt = new DataTable();
            dt = bd.RetDataTable(queryselect);
            foreach (DataRow dr in dt.Rows)
            {
                var user_id = dr["usuario_id"].ToString();
                var nome = dr["login"].ToString();
                var querynotify = "INSERT INTO mensagem_" + nome + " (USUARIO_id, USUARIO_FROM_id, mensagem, data, NEGOCIO_id, descricao) VALUES ('" + user_id + "', '" + Logindto.Id + "', '" + notify.ToUpper() + "', '" + data + "', '" + DTO.Id + "', 'Status Atualizado')";
                bd.Conectar();
                bd.ExecutarComandoSQL(querynotify);
            }
        }

        #endregion

        #region Create Negocios

        public bool CreateNegocios(NegociosDTO DTO)
        {
            bool sucess = false;
            try
            {
                var tipo = "NEGÓCIO CRIADO";
                var mensagem = "◾ VENDEDOR: " + DTO.Vendedor + "\n◾ RESPONSÁVEL CLIENTE: " + DTO.Contato_Nome + "\n◾ Prioridade: " + DTO.Prioridade_Descricao + "\n◾ DESCRIÇÃO: " + DTO.Descricao + "\n◾ PRAZO: " + Convert.ToDateTime(DTO.Prazo).ToString("dd/MM/yyyy");
                var data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                var query = "INSERT INTO negocio (descricao, anotacoes, prazo, VENDEDOR_id, ESTABELECIMENTO_id, CONTATO_CLIENTE_id, USUARIO_id, PRIORIDADE_id) VALUES ('" + DTO.Descricao + "', '" + DTO.Anotacoes + "', '" + Convert.ToDateTime(DTO.Prazo).ToString("yyyy-MM-dd HH:mm:ss") + "', '" + DTO.Vendedor_Id + "', '" + DTO.Estabelecimento_Id + "','" + DTO.Contato_Id + "', '" + Logindto.Id + "', '" + DTO.Prioridade_Id + "');"
                    + "INSERT INTO versao_atividade(descricao, NEGOCIO_id, USUARIO_id) VALUES('VERSÃO INICIAL', (SELECT MAX(id) as id FROM negocio), '"+ Logindto.Id +"');"
                    + "INSERT INTO log_negocios (tipo, descricao, USUARIO_id, NEGOCIO_id) VALUES ('" + tipo + "','" + mensagem + "','" + Logindto.Id + "',(SELECT MAX(id) as id FROM negocio))";
                bd.Conectar();
                bd.ExecutarComandoSQL(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
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

        #region Verifica se há mais que uma atividade HABIILITADA

        public bool HasEnabledVersion(NegociosDTO DTO)
        {
            int atividades;
            try
            {
                var query = "SELECT COUNT(*) as max FROM versao_atividade WHERE NEGOCIO_id ='"+DTO.Id+"' AND locked='0'";
                bd.Conectar();
                var dr = bd.RetDataReader(query);
                atividades = Convert.ToInt32(dr["max"]);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            if (atividades > 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region ALTERA VERSÃO

        public void AlterarVersao(NegociosDTO DTO)
        {
            var get_versaoatual = "SELECT n.versao_valida FROM negocio n WHERE n.id = '" + DTO.Id + "'";
            bd.Conectar();
            var dr = bd.RetDataReader(get_versaoatual);
            var versao_antiga = dr["versao_valida"].ToString();
            var mensagem = "◾ VERSÃO Nº " + Convert.ToInt32(versao_antiga).ToString("00") + " ➞ VERSÃO Nº " + Convert.ToInt32(DTO.Versao_Id).ToString("00");
            var tipo = "VERSÃO ALTERADA";
            var data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            var query = "UPDATE negocio SET STATUS_ORCAMENTO_id='2', versao_valida='"+ DTO.Versao_Id +"' WHERE id='" + DTO.Id + "'; INSERT INTO log_negocios (tipo, data, descricao, USUARIO_id, NEGOCIO_id) VALUES ('" + tipo + "','" + data + "','" + mensagem + "','" + Logindto.Id + "','" + DTO.Id + "')";
            bd.Conectar();
            bd.ExecutarComandoSQL(query);
        }
        #endregion

        #region RET NEW VERSION ID

        public int NewVersionNumber(NegociosDTO DTO)
        {
            int max;

            try
            {
                var query = "select max(versao_id)+1 as max from versao_atividade where negocio_id = '"+DTO.Id+"'";
                bd.Conectar();
                var dr = bd.RetDataReader(query);
                max = Convert.ToInt32(dr["max"]);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return max;
        }

        #endregion

        #region ADICIONA VERSÃO

        public void AdicionarVersão(NegociosDTO DTO)
        {
            try
            {
                var mensagem = "VERSÃO Nº " + Convert.ToInt32(DTO.Versao_Id).ToString("00") + " CRIADA\n◾ DESCRIÇÃO: " + DTO.Versao_Descricao;
                var tipo = "NOVA VERSÃO";
                var data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                var query = "INSERT INTO versao_atividade (VERSAO_id, descricao, NEGOCIO_id, USUARIO_id) VALUES ('" + DTO.Versao_Id + "', '" + DTO.Versao_Descricao + "', '" + DTO.Id + "', '" + Logindto.Id + "'); UPDATE negocio SET versao_valida='" + DTO.Versao_Id + "' WHERE id='" + DTO.Id + "'; INSERT INTO log_negocios (tipo, data, descricao, USUARIO_id, NEGOCIO_id) VALUES ('" + tipo + "','" + data + "','" + mensagem + "','" + Logindto.Id + "','" + DTO.Id + "'); UPDATE negocio SET STATUS_ORCAMENTO_id='2' WHERE id='" + DTO.Id + "'";
                bd.Conectar();
                bd.ExecutarComandoSQL(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        #endregion

        #region COPIAR ITENS DE VERSAO ANTERIOR

        public void CopiarItensVersao(VersaoOrcamentoDTO versaoDTO, NegociosDTO DTO)
        {
            try
            {
                var query = "INSERT INTO atividade (descricao, NEGOCIO_id, DESC_ATIVIDADES_id, VERSAO_ATIVIDADE_id, USUARIO_id, habilitado) SELECT descricao, NEGOCIO_id, DESC_ATIVIDADES_id, (select id from versao_atividade where negocio_id = '"+DTO.Id+"' and versao_id = (select max(versao_id) from versao_atividade where negocio_id = '"+DTO.Id+"')), '"+Logindto.Id+"', habilitado FROM atividade WHERE VERSAO_ATIVIDADE_id = '"+ versaoDTO.Versao_Atividade_Id +"' and NEGOCIO_id = '"+DTO.Id+ "'";
                bd.Conectar();
                bd.ExecutarComandoSQL(query);

                var select_items = "SELECT a.id, a.descricao, a.desc_atividades_id FROM atividade a JOIN versao_atividade va ON va.id = a.VERSAO_ATIVIDADE_id WHERE a.NEGOCIO_id = '" + DTO.Id + "' AND va.VERSAO_id = '" + versaoDTO.Id + "'";
                bd.Conectar();
                var dt = bd.RetDataTable(select_items);

                foreach (DataRow dr in dt.Rows)
                {
                    var descricao = dr["descricao"].ToString();
                    var desc_atividades_id = dr["desc_atividades_id"].ToString();
                    var inserir_itens = "INSERT INTO lista_orcamento (PRODUTO_id, NEGOCIO_id, ATIVIDADES_id, quantidade, descricao_orc, preco_orc, bdi, fd) SELECT lo.PRODUTO_id, lo.NEGOCIO_id, (SELECT a.id FROM atividade a JOIN versao_atividade va ON va.id = a.VERSAO_ATIVIDADE_id WHERE a.NEGOCIO_id = '" + DTO.Id + "' AND va.VERSAO_id = (select max(versao_id) from versao_atividade where negocio_id = '"+DTO.Id+"') AND a.descricao = '" + descricao + "' AND a.desc_atividades_id = '"+ desc_atividades_id +"'), quantidade, descricao_orc, preco_orc, bdi, fd FROM lista_orcamento lo JOIN atividade a ON lo.ATIVIDADES_id = a.id JOIN versao_atividade va ON a.VERSAO_ATIVIDADE_id = va.id WHERE va.VERSAO_id = '" + versaoDTO.Id + "' AND lo.NEGOCIO_id = '" + DTO.Id + "' AND a.descricao = '" + descricao + "' AND a.desc_atividades_id = '" + desc_atividades_id + "'";
                    bd.Conectar();
                    bd.ExecutarComandoSQL(inserir_itens);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        #endregion

        public Task<bool> HasPricelessItems(NegociosDTO DTO)
        {
            bool isTrue = false;
            var dt = new DataTable();
            try
            {
                var query = "SELECT lo.preco_orc FROM lista_orcamento lo JOIN negocio n ON lo.negocio_id = n.id JOIN atividade a ON a.id = lo.atividades_id JOIN versao_atividade va ON va.id = a.versao_atividade_id WHERE n.id = '" + DTO.Id + "' AND va.versao_id = n.versao_valida AND lo.preco_orc = 0";
                bd.Conectar();
                dt = bd.RetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (dt.Rows.Count > 0)
                {
                    isTrue = true;
                }
            }
            return Task.FromResult(isTrue);
        }

        #endregion

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
                bd.Dispose();
            }
            disposed = true;
        }

        #endregion
    }
}
