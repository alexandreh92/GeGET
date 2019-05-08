using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;

namespace GeGET
{
    class EntradaManualEstoqueBLL
    {
        AcessoBancoDados bd = new AcessoBancoDados();

        public ObservableCollection<EntradaManualEstoqueDTO> LoadItens()
        {
            var itens = new ObservableCollection<EntradaManualEstoqueDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT p.id, i.descricao, p.descricao as desc_completa, un.descricao as un, p.partnumber, f.rsocial FROM produto p JOIN item i ON p.DESCRICAO_ITEM_id = i.id JOIN unidade un ON un.id = i.unidade_id JOIN fornecedor f ON p.FORNECEDOR_id = f.id WHERE p.STATUS_id='1' AND i.mobra='0' AND p.id != '1' AND f.status_id ='1' ORDER BY i.descricao";
                bd.Conectar();
                dt = bd.RetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                foreach (DataRow item in dt.Rows)
                {
                    itens.Add(new EntradaManualEstoqueDTO
                    {
                        Id = Convert.ToInt32(item["id"]),
                        Descricao = item["descricao"].ToString(),
                        Anotacoes = item["desc_completa"].ToString(),
                        Unidade = item["un"].ToString(),
                        Partnumber = item["partnumber"].ToString(),
                        Fabricante = item["rsocial"].ToString(),
                        Codigo = Convert.ToInt32(item["id"]).ToString("000000")
                    });
                }
                bd.CloseConection();
            }
            return itens;
        }
    }
}
