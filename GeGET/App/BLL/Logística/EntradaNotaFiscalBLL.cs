using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DAL;
using DTO;

namespace BLL
{
    class EntradaNotaFiscalBLL
    {
        AcessoBancoDados bd = new AcessoBancoDados();
        LoginDTO loginDTO = new LoginDTO();

        public ObservableCollection<EntradaNotaFiscalDTO> FindProduto(EntradaNotaFiscalDTO DTO)
        {
            var find = new ObservableCollection<EntradaNotaFiscalDTO>();

            try
            {
                //PROCURA PRODUTO PELO CODIGO DE BARRAS
                var dt1 = new DataTable();
                var query1 = "select p.id, i.descricao, f.rsocial, un.descricao as un from produto p JOIN fornecedor f ON f.id = p.fornecedor_id JOIN item i ON i.id = p.descricao_item_id JOIN unidade un ON un.id = i.unidade_id where p.codigo_barra = '"+DTO.Codigo+"' LIMIT 1";
                bd.Conectar();
                dt1 = bd.RetDataTable(query1);
                //SE ACHAR O PRODUTO, ADICIONA NA LISTA
                if (dt1.Rows.Count>0)
                {
                    find.Add(new EntradaNotaFiscalDTO
                    {
                        Codigo_Getac = dt1.Rows[0]["id"].ToString(),
                        Descricao = dt1.Rows[0]["descricao"].ToString(),
                        Fabricante = dt1.Rows[0]["rsocial"].ToString(),
                        Unidade = dt1.Rows[0]["un"].ToString()
                    });
                }
                //CASO CONTRÁRIO VAI PROCURAR PELO PARTNUMBER
                else
                {
                    /*var dt2 = new DataTable();
                    var query2 = "select p.id, i.descricao, f.rsocial, un.descricao as un from produto p JOIN fornecedor f ON f.id = p.fornecedor_id JOIN item i ON i.id = p.descricao_item_id JOIN unidade un ON un.id = i.unidade_id where p.partnumber = '" + DTO.Codigo + "' LIMIT 1";
                    bd.Conectar();
                    dt2 = bd.RetDataTable(query2);
                    //SE ACHAR O PRODUTO, ADICIONA NA LISTA
                    if (dt2.Rows.Count > 0)
                    {
                        find.Add(new EntradaNotaFiscalDTO
                        {
                            Codigo_Getac = dt2.Rows[0]["id"].ToString(),
                            Descricao = dt2.Rows[0]["descricao"].ToString(),
                            Fabricante = dt2.Rows[0]["rsocial"].ToString(),
                            Unidade = dt2.Rows[0]["un"].ToString()
                        });
                    }
                    else
                    {
                        find = null;
                    }*/
                    find = null;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.ToString());
            }

            return find;
        }

        #region Verifica se o item já foi adicionado ao estoque

        public bool isPresent(EntradaNotaFiscalDTO DTO)
        {
            bool ispresent = false;
            var dt = new DataTable();
            try
            {
                var query = "SELECT id from entrada_estoque WHERE nota_fiscal = '"+ DTO.Nota +"' AND produto_id = '" +DTO.Codigo_Getac+ "';";
                bd.Conectar();
                dt = bd.RetDataTable(query);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                if (dt.Rows.Count>0)
                {
                    ispresent = true;
                }
            }
            return ispresent;
        }

        #endregion

        #region Inserir Produtos No Estoque

        public void InserirEstoque(ObservableCollection<EntradaNotaFiscalDTO> listaEntradaEstoque)
        {
            var dt = new DataTable();
            try
            {
                string substringEstoque = "";
                foreach (var item in listaEntradaEstoque)
                {
                    substringEstoque = substringEstoque + "('"+ item.Codigo_Getac +"', '"+ loginDTO.Id +"', + '"+ item.Quantidade.ToString().Replace(",",".") +"', '"+ item.Custo.ToString().Replace(",", ".") + "', '"+ item.Nota+ "', '"+ item.Chave_Acesso +"'),";
                }
                string query = "INSERT INTO entrada_estoque (produto_id, usuario_id, quantidade, custo, nota_fiscal, chave_acesso) VALUES " + substringEstoque.TrimEnd(',');

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
