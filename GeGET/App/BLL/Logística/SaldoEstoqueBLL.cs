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
                var query = "SELECT p.id, i.descricao, p.partnumber, un.descricao as un, f.rsocial, e.quantidade FROM estoque e JOIN produto p ON e.produto_id = p.id JOIN fornecedor f ON f.id = p.FORNECEDOR_id JOIN item i ON i.id = p.DESCRICAO_ITEM_id JOIN unidade un ON un.id = i.unidade_id;";
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
                        Id = "P" + Convert.ToInt32(dr["id"]).ToString("0000"),
                        Descricao = dr["descricao"].ToString(),
                        Partnumber = dr["partnumber"].ToString(),
                        Fabricante = dr["rsocial"].ToString(),
                        Qtde = Convert.ToDouble(dr["quantidade"]),
                        Un = dr["un"].ToString()
                    });
                }
            }
            return estoque;
        }
    }
}
