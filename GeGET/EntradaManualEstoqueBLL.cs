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
        LoginDTO loginDTO = new LoginDTO();

        public ObservableCollection<EntradaManualEstoqueDTO> LoadItens()
        {
            var itens = new ObservableCollection<EntradaManualEstoqueDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT p.id, i.descricao, p.descricao as desc_completa, p.custounitario, un.descricao as un, p.partnumber, f.rsocial FROM produto p JOIN item i ON p.DESCRICAO_ITEM_id = i.id JOIN unidade un ON un.id = i.unidade_id JOIN fornecedor f ON p.FORNECEDOR_id = f.id WHERE p.STATUS_id='1' AND i.mobra='0' AND p.id != '1' AND f.status_id ='1' ORDER BY i.descricao";
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
                        Custo = Convert.ToDouble(item["custounitario"]),
                        Partnumber = item["partnumber"].ToString(),
                        Fabricante = item["rsocial"].ToString(),
                        Codigo = Convert.ToInt32(item["id"]).ToString("000000")
                    });
                }
                bd.CloseConection();
            }
            return itens;
        }

        #region Inserir Produtos No Estoque

        public void InserirEstoque(ObservableCollection<EntradaManualEstoqueDTO> listaEntradaEstoque)
        {
            var dt = new DataTable();
            try
            {
                string substringEstoque = "";
                foreach (var item in listaEntradaEstoque)
                {
                    substringEstoque = substringEstoque + "('" + item.Id + "', '" + loginDTO.Id + "', + '" + item.Quantidade.ToString().Replace(",", ".") + "', '" + item.Custo.ToString().Replace(",", ".") + "', '" + item.Nota + "'),";
                }
                string query = "INSERT INTO entrada_estoque (produto_id, usuario_id, quantidade, custo, nota_fiscal) VALUES " + substringEstoque.TrimEnd(',');

                bd.Conectar();
                bd.ExecutarComandoSQL(query);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        #endregion

    }
}
