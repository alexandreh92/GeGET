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
    class MovimentoSaidaBLL
    {
        AcessoBancoDados bd = new AcessoBancoDados();

        public ObservableCollection<MovimentoSaidaDTO> Load()
        {
            var dt = new DataTable();
            var collection = new ObservableCollection<MovimentoSaidaDTO>();
            try
            {
                var query = "SELECT se.id as saida, rm.id as rm, se.produto_id, i.descricao, se.quantidade, se.data, f.nome FROM saida_estoque se JOIN produto p ON p.id = se.produto_id JOIN item i ON i.id = p.DESCRICAO_ITEM_id JOIN requisicao_material rm ON rm.id = se.rm_id JOIN usuario u ON se.usuario_id = u.id JOIN funcionario f ON f.id = u.funcionario_id";
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
                    collection.Add(new MovimentoSaidaDTO
                    {
                        Id = dr["saida"].ToString(),
                        Rm_id = dr["rm"].ToString(),
                        Produto_id = Convert.ToInt32(dr["produto_id"]).ToString(),
                        Descricao = dr["descricao"].ToString(),
                        Data = Convert.ToDateTime(dr["data"]),
                        Nome = dr["nome"].ToString(),
                        Quantidade = dr["quantidade"].ToString(),
                    });
                }
            }
            return collection;
        }

        public void Excluir(ObservableCollection<MovimentoSaidaDTO> dTO)
        {
            try
            {
                string substring = "";
                foreach (MovimentoSaidaDTO dto in dTO)
                {
                    substring = substring + "OR id='" + dto.Id + "' ";
                }
                var query = "DELETE FROM saida_estoque WHERE " + substring.TrimStart(new Char[] { 'O', 'R' });

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
