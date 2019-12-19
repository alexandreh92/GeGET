using DAL;
using DTO;
using System.Collections.ObjectModel;
using System.Data;
using System;

namespace BLL
{
    class ProdutosBLL : IDisposable
    {
        bool disposed = false;
        AcessoBancoDados bd = new AcessoBancoDados();
        LoginDTO loginDTO = new LoginDTO();

        #region Load Produtos
        public ObservableCollection<ProdutosDTO> LoadProdutos()
        {
            var produtos = new ObservableCollection<ProdutosDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT p.status_id, p.id, p.descricao as anotacoes, p.ncm, p.icms, p.custounitario, p.ipi, p.partnumber, i.id as item_id, f.id as fornecedor_id, i.descricao, un.descricao as un, f.rsocial FROM produto p JOIN item i ON p.DESCRICAO_ITEM_id = i.id JOIN fornecedor f ON p.FORNECEDOR_id = f.id JOIN unidade un ON un.id = i.unidade_id ORDER BY i.descricao ASC";
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
                    produtos.Add(new ProdutosDTO
                    { Id = dr["id"].ToString(),
                        Status_Id = dr["status_id"].ToString(),
                        Item_Descricao = dr["descricao"].ToString(),
                        Anotacoes = dr["anotacoes"].ToString(), Ncm = dr["ncm"].ToString(),
                        Icms = Convert.ToDouble(dr["icms"]), Ipi = Convert.ToDouble(dr["ipi"]),
                        Partnumber = dr["partnumber"].ToString(), Custo = Convert.ToDouble(dr["custounitario"]),
                        Fornecedor = dr["rsocial"].ToString(), Unidade = dr["un"].ToString(),
                        Codigo = Convert.ToInt32(dr["id"]).ToString("000000"),
                        Item_Id = dr["item_id"].ToString(),
                        Fornecedor_Id = dr["fornecedor_id"].ToString() });
                }
            }
            return produtos;
        }
        #endregion

        #region Load Procurar Produtos 
        public ObservableCollection<ProdutosDTO> LoadProcurarProdutos()
        {
            var produtos = new ObservableCollection<ProdutosDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT p.status_id, p.id, p.descricao as anotacoes, p.ncm, p.icms, p.custounitario, p.ipi, p.partnumber, i.id as item_id, f.id as fornecedor_id, i.descricao, un.descricao as un, f.rsocial FROM produto p JOIN item i ON p.DESCRICAO_ITEM_id = i.id JOIN fornecedor f ON p.FORNECEDOR_id = f.id JOIN unidade un ON un.id = i.unidade_id WHERE i.status_id = '1' AND f.status_id = '1' ORDER BY i.descricao ASC";
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
                    produtos.Add(new ProdutosDTO
                    {
                        Id = dr["id"].ToString(),
                        Status_Id = dr["status_id"].ToString(),
                        Item_Descricao = dr["descricao"].ToString(),
                        Anotacoes = dr["anotacoes"].ToString(),
                        Ncm = dr["ncm"].ToString(),
                        Icms = Convert.ToDouble(dr["icms"]),
                        Ipi = Convert.ToDouble(dr["ipi"]),
                        Partnumber = dr["partnumber"].ToString(),
                        Custo = Convert.ToDouble(dr["custounitario"]),
                        Fornecedor = dr["rsocial"].ToString(),
                        Unidade = dr["un"].ToString(),
                        Codigo = Convert.ToInt32(dr["id"]).ToString("000000"),
                        Item_Id = dr["item_id"].ToString(),
                        Fornecedor_Id = dr["fornecedor_id"].ToString()
                    });
                }
            }
            return produtos;
        }
        #endregion

        #region Update Produto
        public void UpdateProduto(ProdutosDTO DTO)
        {
            try
            {
                var query = "UPDATE produto SET status_id='" + DTO.Status_Id + "', ncm='" + DTO.Ncm + "', custounitario='" + DTO.Custo.ToString().Replace(",", ".") + "', descricao='" + DTO.Anotacoes + "', ipi='" + DTO.Ipi.ToString().Replace(",", ".") + "', partnumber='" + DTO.Partnumber + "', icms='" + DTO.Icms.ToString().Replace(",", ".") + "' WHERE id='" + DTO.Id + "'";
                bd.Conectar();
                bd.ExecutarComandoSQL(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        #endregion

        #region Inserir Produto

        public bool InserirProduto(ProdutosDTO DTO)
        {
            bool success = false;
            try
            {
                var query = "INSERT INTO produto (descricao, DESCRICAO_ITEM_id, ncm, custounitario, ipi, FORNECEDOR_id, USUARIO_id, partnumber, icms) VALUES ('" + DTO.Anotacoes + "','" + DTO.Item_Id + "', '" + DTO.Ncm + "', '" + DTO.Custo.ToString().Replace(",", ".") + "', '" + DTO.Ipi.ToString().Replace(",", ".") + "', '" + DTO.Fornecedor_Id + "', '" + loginDTO.Id + "', '" + DTO.Partnumber + "', '" + DTO.Icms.ToString().Replace(",", ".") + "')";
                bd.Conectar();
                bd.ExecutarComandoSQL(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                success = true;
            }
            return success;
        }

        #endregion

        #region Update Anotações Precificar

        public void P_UpdateAnotacoes(ProdutosDTO DTO)
        {
            try
            {
                var query = "UPDATE produto SET descricao='"+DTO.Anotacoes+"' WHERE id='"+DTO.Id+"'";
                bd.Conectar();
                bd.ExecutarComandoSQL(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        #endregion

        #region Update Partnumber Precificar

        public void P_UpdatePartnumber(ProdutosDTO DTO)
        {
            try
            {
                var query = "UPDATE produto SET partnumber='" + DTO.Partnumber + "' WHERE id='" + DTO.Id + "'";
                bd.Conectar();
                bd.ExecutarComandoSQL(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        #endregion

        #region Update NCM Precificar

        public void P_UpdateNcm(ProdutosDTO DTO)
        {
            try
            {
                var query = "UPDATE produto SET ncm='" + DTO.Ncm + "' WHERE id='" + DTO.Id + "'";
                bd.Conectar();
                bd.ExecutarComandoSQL(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        #endregion

        #region Update Custo Precificar

        public void P_UpdateCusto(ProdutosDTO DTO)
        {
            try
            {
                var query = "UPDATE produto SET custounitario='" + DTO.Custo.ToString().Replace(",",".") + "' WHERE id='" + DTO.Id + "'";
                bd.Conectar();
                bd.ExecutarComandoSQL(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        #endregion

        #region Update Ipi Precificar

        public void P_UpdateIpi(ProdutosDTO DTO)
        {
            try
            {
                var query = "UPDATE produto SET ipi='" + DTO.Ipi.ToString().Replace(",", ".") + "' WHERE id='" + DTO.Id + "'";
                bd.Conectar();
                bd.ExecutarComandoSQL(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        #endregion

        #region Update Ipi Precificar

        public void P_UpdateIcms(ProdutosDTO DTO)
        {
            try
            {
                var query = "UPDATE produto SET icms='" + DTO.Icms.ToString().Replace(",", ".") + "' WHERE id='" + DTO.Id + "'";
                bd.Conectar();
                bd.ExecutarComandoSQL(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        #endregion

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                bd.Dispose();
            }
            disposed = true;
        }

        #endregion
    }
}
