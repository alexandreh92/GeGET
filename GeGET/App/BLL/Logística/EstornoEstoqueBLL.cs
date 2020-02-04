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
                var query = "SELECT mr.id, p.id as produto_id, i.descricao, p.partnumber,  un.descricao as un, mr.quantidade, f.rsocial FROM materiais_requeridos mr JOIN produto p ON p.id = mr.produto_id JOIN item i ON i.id = p.DESCRICAO_ITEM_id JOIN fornecedor f ON f.id = p.FORNECEDOR_id JOIN unidade un ON un.id = i.unidade_id WHERE mr.requisicao_material_id = '"+ dTO.Id +"' ";
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
                    var query = "DELETE FROM materiais_requeridos WHERE id = '"+ dto.Id +"'";
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
