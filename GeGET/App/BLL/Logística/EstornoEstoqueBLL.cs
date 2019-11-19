using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace BLL
{
    class EstornoEstoqueBLL
    {
        AcessoBancoDados bd = new AcessoBancoDados();

        public ObservableCollection<EstornoEstoqueDTO> Load(InformacoesAtendimentoDTO dTO)
        {
            var itens = new ObservableCollection<EstornoEstoqueDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT se.id, i.descricao, p.id as produto_id, p.partnumber, forn.rsocial, un.descricao as un, se.quantidade, f.nome as usuario, se.data FROM saida_estoque se JOIN produto p ON se.produto_id = p.id JOIN item i ON i.id = p.DESCRICAO_ITEM_id JOIN unidade un ON un.id = i.unidade_id JOIN usuario u ON u.id = se.usuario_id JOIN funcionario f ON f.id = u.funcionario_id JOIN fornecedor forn ON forn.id = p.fornecedor_id WHERE se.rm_id = '"+ dTO.Id +"'";
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
                    itens.Add(new EstornoEstoqueDTO
                    {
                        Id = dr["id"].ToString(),
                        Item_Id = dr["produto_id"].ToString(),
                        Descricao = dr["descricao"].ToString(),
                        Partnumber = dr["partnumber"].ToString(),
                        Fornecedor = dr["rsocial"].ToString(),
                        Un = dr["un"].ToString(),
                        Quantidade = Convert.ToDouble(dr["quantidade"]),
                        Usuario_id = dr["usuario"].ToString(),
                        Data = Convert.ToDateTime(dr["data"]).ToString("dd/MM/yyyy")
                    });
                }
            }
            return itens;
        }

        public void EstornarProdutos(ObservableCollection<EstornoEstoqueDTO> listaEstorno)
        {
            foreach (var dto in listaEstorno)
            {
                try
                {
                    var query = "DELETE FROM saida_estoque WHERE id = '"+ dto.Id +"'";
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
}
