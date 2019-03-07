using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;
using System.Data;
using System.Windows;

namespace BLL
{
    class NegociosBLL
    {
        AcessoBancoDados bd = new AcessoBancoDados();
        NegociosDTO dto = new NegociosDTO();

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
                Procurar = Procurar + " AND e.id = '"+dto.ParentId+"'";
            }
            var negocios = new List<NegociosDTO>();
            var query = "SELECT n.id, n.descricao, n.anotacoes, n.prazo, n.valor_fechamento, n.data, n.data_envio, v.nome, s.descricao as status_descricao, s.id as status_id, e.cnpj, e.endereco, cid.cidade, c.id, cid.uf, c.rsocial, c.fantasia FROM negocio n JOIN vendedor v ON n.VENDEDOR_id = v.id JOIN status_orcamento s ON n.STATUS_ORCAMENTO_id = s.id JOIN estabelecimento e ON n.ESTABELECIMENTO_id = e.id JOIN cidades cid ON e.CIDADES_id = cid.id JOIN cliente c ON e.CLIENTE_id = c.id  WHERE " + Procurar +" ORDER BY n.id LIMIT " + dto.Inicio + "," + dto.Limite + "";
            bd.Conectar();
            var dtt = bd.RetDataTable(query);
            foreach (DataRow dr in dtt.Rows)
            {
                negocios.Add(new NegociosDTO { Id = dr["id"].ToString(), Numero = Convert.ToInt32(dr["id"]).ToString("0000"), Razao_Social = dr["rsocial"].ToString(), Descricao = dr["descricao"].ToString(), Anotacoes = dr["anotacoes"].ToString(), Status = Convert.ToInt32(dr["status_id"]), Endereco = dr["endereco"].ToString() + " - " + dr["cidade"].ToString() + " - " + dr["uf"].ToString(), Vendedor = dr["nome"].ToString(), Status_Descricao = dr["status_descricao"].ToString(), Status_Id = Convert.ToInt32(dr["status_id"]) });
            }

            return negocios;
        }

        public void CountRows()
        {
            try
            {
                int i = 1;
                string Procurar = "";
                string[] ListaNegocios = dto.Pesquisa.Split(null);
                foreach (string pesq in ListaNegocios)
                {
                    if (i == 1)
                    {
                        Procurar = Procurar + "CONCAT(e.cnpj, e.endereco, e.inscricao, c.rsocial, c.fantasia, cid.uf, cid.cidade) LIKE '%" + pesq + "%'";
                    }
                    else
                    {
                        Procurar = Procurar + " AND CONCAT(e.cnpj, e.endereco, e.inscricao, c.rsocial, c.fantasia, cid.uf, cid.cidade) LIKE '%" + pesq + "%'";
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
                var query = "SELECT COUNT(*) AS count_rows FROM estabelecimento e join cliente c on e.CLIENTE_id = c.id join cidades cid on e.CIDADES_id = cid.id WHERE "+Procurar+"";
                bd.Conectar();
                var dr = bd.RetDataReader(query);
                if (dr.HasRows)
                {
                    dto.TotalRows = Convert.ToInt32(dr["count_rows"]);
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.ToString());
            }
            finally
            {
                bd.CloseConection();
                dto.RowsLeft = dto.TotalRows;
            }

        }

    }
}
