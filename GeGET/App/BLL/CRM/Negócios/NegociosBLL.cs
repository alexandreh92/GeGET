using System;
using System.Collections.Generic;
using DTO;
using DAL;
using System.Data;
using System.Collections.ObjectModel;

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
                var query = "SELECT n.id, n.descricao, n.anotacoes, n.prazo, n.valor_fechamento, n.data, n.data_envio, v.nome, s.descricao as status_descricao, s.id as status_id, e.cnpj, e.endereco, cid.cidade, e.id as estabelecimento_id, c.id as cliente_id, cid.uf, c.rsocial, c.fantasia FROM negocio n JOIN vendedor v ON n.VENDEDOR_id = v.id JOIN status_orcamento s ON n.STATUS_ORCAMENTO_id = s.id JOIN estabelecimento e ON n.ESTABELECIMENTO_id = e.id JOIN cidades cid ON e.CIDADES_id = cid.id JOIN cliente c ON e.CLIENTE_id = c.id " + Procurar + " ORDER BY n.id";
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
