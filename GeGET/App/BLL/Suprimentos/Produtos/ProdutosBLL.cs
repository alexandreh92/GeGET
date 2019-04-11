using DAL;
using DTO;
using System.Collections.ObjectModel;
using System.Data;
using System;

namespace GeGET
{
    class ProdutosBLL
    {
        AcessoBancoDados bd = new AcessoBancoDados();

        public ObservableCollection<ProdutosDTO> LoadProdutos()
        {
            var produtos = new ObservableCollection<ProdutosDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT p.status_id, p.id, p.descricao as anotacoes, p.ncm, p.icms, p.custounitario, p.ipi, p.partnumber, i.id as item_id, f.id as fornecedor_id, i.descricao, un.descricao as un, f.rsocial FROM produto p JOIN item i ON p.DESCRICAO_ITEM_id = i.id JOIN fornecedor f ON p.FORNECEDOR_id = f.id JOIN unidade un ON un.id = i.unidade_id ORDER BY i.descricao ASC";
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
                    produtos.Add(new ProdutosDTO { Id = dr["id"].ToString(), Status_Id = dr["status_id"].ToString(), Item_Descricao = dr["descricao"].ToString(), Anotacoes = dr["anotacoes"].ToString(), Ncm = dr["ncm"].ToString(), Icms = Convert.ToDouble(dr["icms"]), Ipi = Convert.ToDouble(dr["ipi"]), Partnumber = dr["partnumber"].ToString(), Custo = Convert.ToDouble(dr["custounitario"]), Fornecedor = dr["rsocial"].ToString(), Unidade = dr["un"].ToString(), Codigo = Convert.ToInt32(dr["id"]).ToString("000000"), Item_Id = dr["item_id"].ToString(), Fornecedor_Id = dr["fornecedor_id"].ToString() });
                }
            }
            return produtos;
        }

        public void UpdateProduto(ProdutosDTO DTO)
        {
            try
            {
                var query = "UPDATE produto SET status_id='" + DTO.Status_Id + "', ncm='" + DTO.Ncm + "', custounitario='" + DTO.Custo.ToString().Replace(",", ".") + "', descricao='" + DTO.Anotacoes + "', ipi='" + DTO.Ipi.ToString().Replace(",", ".") + "', partnumber='" + DTO.Partnumber + "', icms='" + DTO.Icms.ToString().Replace(",",".") + "' WHERE id='" + DTO.Id + "'";
                bd.Conectar();
                bd.ExecutarComandoSQL(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

    }
}
