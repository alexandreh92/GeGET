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
    class PessoasBLL
    {
        AcessoBancoDados bd = new AcessoBancoDados();
        PessoasDTO dto = new PessoasDTO();

        public List<PessoasDTO> LoadPessoas()
        {
            int i = 1;
            string Procurar = "";
            string[] ListaNegocios = dto.Pesquisa.Split(null);
            foreach (string pesq in ListaNegocios)
            {
                if (i == 1)
                {
                    Procurar = Procurar + "CONCAT(cc.nome, cc.email, cc.funcao, cc.telefone, cc.celular, cc.data, c.rsocial, c.fantasia) LIKE '%" + pesq + "%'";
                }
                else
                {
                    Procurar = Procurar + " AND CONCAT(cc.nome, cc.email, cc.funcao, cc.telefone, cc.celular, cc.data, c.rsocial, c.fantasia) LIKE '%" + pesq + "%'";
                }
                i++;
            }
            if (dto.FromParent)
            {
                Procurar = Procurar + " AND c.id = '" + dto.ParentId + "'";
            }
            var pessoas = new List<PessoasDTO>();
            var query = "SELECT cc.id, cc.nome, cc.STATUS_id, cc.email, cc.funcao, cc.telefone, cc.celular, cc.data, cc.USUARIO_id, c.id as cliente_id, c.rsocial, c.fantasia FROM contato_cliente cc join cliente c on cc.CLIENTE_id = c.id  WHERE " + Procurar +" ORDER BY cc.nome LIMIT " + dto.Inicio + "," + dto.Limite + "";
            bd.Conectar();
            var dtt = bd.RetDataTable(query);
            foreach (DataRow dr in dtt.Rows)
            {
                pessoas.Add(new PessoasDTO { Id = dr["id"].ToString(), Status_Id = dr["status_id"].ToString(), Nome = dr["nome"].ToString(), Email = dr["email"].ToString(), Funcao = dr["funcao"].ToString() + " NA " + dr["rsocial"].ToString(), Telefone = dr["telefone"].ToString(), Celular = dr["celular"].ToString() });
            }

            return pessoas;
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
