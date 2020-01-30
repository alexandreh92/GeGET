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
    class MovimentoEntradaBLL
    {
        AcessoBancoDados bd = new AcessoBancoDados();

        public ObservableCollection<MovimentoEntradaDTO> Load()
        {
            var dt = new DataTable();
            var collection = new ObservableCollection<MovimentoEntradaDTO>();
            try
            {
                var query = "SELECT ee.id as entrada, p.id as produto_id, i.descricao, ee.quantidade, ee.custo, ee.nota_fiscal, ee.chave_acesso, ee.data, f.nome FROM entrada_estoque ee JOIN produto p ON p.id = ee.produto_id JOIN usuario u ON u.id = ee.usuario_id JOIN funcionario f ON f.id = u.funcionario_id JOIN item i ON i.id = p.DESCRICAO_ITEM_id";
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
                    collection.Add(new MovimentoEntradaDTO
                    {
                        Id = dr["entrada"].ToString(),
                        Produto_id = Convert.ToInt32(dr["produto_id"]).ToString(),
                        Descricao = dr["descricao"].ToString(),
                        Data = Convert.ToDateTime(dr["data"]),
                        Nome = dr["nome"].ToString(),
                        Quantidade = dr["quantidade"].ToString(),
                        Custo = Convert.ToDouble(dr["custo"]),
                        Nota_fiscal = dr["nota_fiscal"].ToString(),
                        Chave_acesso = dr["chave_acesso"].ToString()
                    });
                }
            }
            return collection;
        }

        public void Excluir(ObservableCollection<MovimentoEntradaDTO> dTO)
        {
            try
            {
                string substring = "";
                foreach (MovimentoEntradaDTO dto in dTO)
                {
                    substring = substring + "OR id='" + dto.Id + "' ";
                }
                var query = "DELETE FROM entrada_estoque WHERE " + substring.TrimStart(new Char[] { 'O', 'R' });

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
