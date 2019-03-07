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
    class EstabelecimentosBLL
    {
        AcessoBancoDados bd = new AcessoBancoDados();
        EstabelecimentosDTO dto = new EstabelecimentosDTO();

        public List<EstabelecimentosDTO> LoadEstabelecimentos()
        {
            int i = 1;
            string Procurar = "";
            string[] ListaClientes = dto.Pesquisa.Split(null);
            foreach (string pesq in ListaClientes)
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
                Procurar = Procurar + " AND c.id = '"+dto.ParentId+"'";
            }
            var clientes = new List<EstabelecimentosDTO>();
            var query = "SELECT e.id, e.cnpj, e.endereco, e.inscricao, e.status_id, cid.estado as id_estado, cid.id as id_cidade, e.CLIENTE_id, e.telefone, c.rsocial, c.fantasia, c.data, cid.uf, cid.cidade FROM estabelecimento e join cliente c on e.CLIENTE_id = c.id join cidades cid on e.CIDADES_id = cid.id WHERE "+Procurar+" ORDER BY c.fantasia, cid.uf, cid.cidade ASC LIMIT " + dto.Inicio + "," + dto.Limite + "";
            bd.Conectar();
            var dtt = bd.RetDataTable(query);
            foreach (DataRow dr in dtt.Rows)
            {
                clientes.Add(new EstabelecimentosDTO {Id = dr["id"].ToString(), Razao_Social = dr["rsocial"].ToString(), Nome_Fantasia = dr["fantasia"].ToString(), Cnpj = dr["cnpj"].ToString(), Status = Convert.ToInt32(dr["status_id"]), Endereco = dr["endereco"].ToString(), Cidade = dr["cidade"].ToString() + " - " + dr["uf"].ToString() });
            }

            return clientes;
        }

        public void CountRows()
        {
            try
            {
                int i = 1;
                string Procurar = "";
                string[] ListaClientes = dto.Pesquisa.Split(null);
                foreach (string pesq in ListaClientes)
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
