using System;
using System.Collections.Generic;
using DTO;
using DAL;
using System.Data;

namespace BLL
{
    class NegociosBLL
    {
        #region Declarations
        AcessoBancoDados bd = new AcessoBancoDados();
        NegociosDTO dto = new NegociosDTO();
        #endregion

        #region Methods
        #region Load Negocios
        public List<NegociosDTO> LoadNegocios()
        {
            int i = 1;
            string Procurar = "";
            string[] ListaNegocios = dto.Pesquisa.Split(null);
            foreach (string pesq in ListaNegocios)
            {
                if (i == 1)
                {
                    Procurar = Procurar + "CONCAT(n.id, n.descricao, n.anotacoes, v.nome, s.descricao, s.id, e.cnpj, e.endereco, cid.cidade, cid.uf, c.rsocial) LIKE '%" + pesq + "%'";
                }
                else
                {
                    Procurar = Procurar + " AND CONCAT(n.id, n.descricao, n.anotacoes, v.nome, s.descricao, s.id, e.cnpj, e.endereco, cid.cidade, cid.uf, c.rsocial) LIKE '%" + pesq + "%'";
                }
                i++;
            }
            if (dto.FromParent)
            {
                Procurar = Procurar + " AND c.id = '" + dto.ParentId + "'";
            }
            else if (dto.FromChildrenParent)
            {
                Procurar = Procurar + " AND e.id = '" + dto.ParentId + "'";
            }
            var negocios = new List<NegociosDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT n.id, n.descricao, n.anotacoes, n.prazo, n.valor_fechamento, n.data, n.data_envio, v.nome, s.descricao as status_descricao, s.id as status_id, e.cnpj, e.endereco, cid.cidade, c.id, cid.uf, c.rsocial, c.fantasia FROM negocio n JOIN vendedor v ON n.VENDEDOR_id = v.id JOIN status_orcamento s ON n.STATUS_ORCAMENTO_id = s.id JOIN estabelecimento e ON n.ESTABELECIMENTO_id = e.id JOIN cidades cid ON e.CIDADES_id = cid.id JOIN cliente c ON e.CLIENTE_id = c.id  WHERE " + Procurar + " ORDER BY n.id";
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
                    negocios.Add(new NegociosDTO { Id = dr["id"].ToString(), Numero = "p" + Convert.ToInt32(dr["id"]).ToString("0000"), Razao_Social = dr["rsocial"].ToString(), Descricao = dr["descricao"].ToString(), Anotacoes = dr["anotacoes"].ToString(), Status = Convert.ToInt32(dr["status_id"]), Endereco = dr["endereco"].ToString() + " - " + dr["cidade"].ToString() + " - " + dr["uf"].ToString(), Vendedor = dr["nome"].ToString(), Status_Descricao = dr["status_descricao"].ToString(), Status_Id = Convert.ToInt32(dr["status_id"]) });
                }
                bd.CloseConection();
            }
            return negocios;
        }
        #endregion

        #endregion
    }
}
