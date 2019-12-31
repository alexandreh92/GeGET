using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;

namespace BLL
{
    class SaldoEstoqueBLL
    {
        AcessoBancoDados bd = new AcessoBancoDados();

        public ObservableCollection<SaldoEstoqueDTO> Load()
        {
            var estoque = new ObservableCollection<SaldoEstoqueDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT p.id as produto_id,i.id as item_id,i.descricao,p.descricao AS desc_completa,un.descricao AS un,p.partnumber,p.custounitario,COALESCE(e.quantidade,0) AS estoque,f.id as fabricante_id,f.rsocial FROM produto p JOIN item i ON p.DESCRICAO_ITEM_id=i.id JOIN unidade un ON un.id=i.unidade_id JOIN fornecedor f ON p.FORNECEDOR_id=f.id LEFT OUTER JOIN estoque e ON e.produto_id=p.id WHERE p.STATUS_id='1' AND i.mobra='0' AND p.id !='1' AND f.status_id='1' ORDER BY i.descricao";
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
                    estoque.Add(new SaldoEstoqueDTO
                    {
                        Produto_Id = Convert.ToInt32(dr["produto_id"]).ToString("00000"),
                        Item_Id = Convert.ToInt32(dr["item_id"]).ToString("00000"),
                        Fabricante_Id = Convert.ToInt32(dr["fabricante_id"]).ToString(),
                        Estoque = Convert.ToDouble(dr["estoque"]),
                        Descricao = dr["descricao"].ToString(),
                        Partnumber = dr["partnumber"].ToString(),
                        Fabricante = dr["rsocial"].ToString(),
                        Un = dr["un"].ToString()
                    });
                }
            }
            return estoque;
        }
    }
}
