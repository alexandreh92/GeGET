using DAL;
using DTO;
using System;
using System.Collections.ObjectModel;
using System.Data;

namespace BLL
{
    internal class RelatorioProdutoBLL
    {
        private AcessoBancoDados bd = new AcessoBancoDados();

        public ObservableCollection<RelatorioProdutoDTO> ListaProdutos()
        {
            ObservableCollection<RelatorioProdutoDTO> produtos = new ObservableCollection<RelatorioProdutoDTO>();
            DataTable dt = new DataTable();
            try
            {
                string query = "SELECT p.*, i.descricao as item, f.rsocial FROM produto p JOIN fornecedor f ON f.id = p.FORNECEDOR_id JOIN item i ON i.id = p.DESCRICAO_ITEM_id";
                bd.Conectar();
                dt = bd.RetDataTable(query);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                foreach (DataRow dr in dt.Rows)
                {
                    produtos.Add(new RelatorioProdutoDTO
                    {
                        Id = dr["id"].ToString(),
                        Item_id = dr["descricao_item_id"].ToString(),
                        Ncm = dr["ncm"].ToString(),
                        Custounitario = Convert.ToDouble(dr["custounitario"]),
                        Ipi = Convert.ToDouble(dr["ipi"]),
                        Fornecedor_id = dr["fornecedor_id"].ToString(),
                        Usuario_id = dr["usuario_id"].ToString(),
                        Partnumber = dr["partnumber"].ToString(),
                        Data = dr["data"].ToString(),
                        Descricao = dr["descricao"].ToString(),
                        Icms = Convert.ToDouble(dr["icms"]),
                        Status_id = dr["status_id"].ToString(),
                        Codigo_barra = dr["codigo_barra"].ToString(),
                        Item_descricao = dr["item"].ToString(),
                        Fabricante = dr["rsocial"].ToString(),
                    });
                }
            }
            return produtos;
        }
    }
}
