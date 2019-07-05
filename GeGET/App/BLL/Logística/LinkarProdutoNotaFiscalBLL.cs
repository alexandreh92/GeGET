using System;
using System.Collections.ObjectModel;
using System.Data;
using DTO;
using DAL;

namespace BLL
{
    class LinkarProdutoNotaFiscalBLL : IDisposable
    {
        bool disposed = false;
        AcessoBancoDados bd = new AcessoBancoDados();
        public ObservableCollection<LinkarProdutoNotaFiscalDTO> LoadItens()
        {
            var itens = new ObservableCollection<LinkarProdutoNotaFiscalDTO>();
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
                    itens.Add(new LinkarProdutoNotaFiscalDTO
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

        #region Check If Empty

        public bool isEmpty(LinkarProdutoNotaFiscalDTO DTO)
        {
            bool isempty = true;
            var dt = new DataTable();
            try
            {
                var query = "select codigo_barra from produto where id = '"+DTO.Id+"'";
                bd.Conectar();
                dt = bd.RetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                if (dt.Rows.Count > 0 && dt.Rows[0]["codigo_barra"].ToString() != "")
                {
                    isempty = false;
                }
            }
            return isempty;
        }

        #endregion

        #region Update Codigo de Barra

        public void UpdateCodigoBarra(LinkarProdutoNotaFiscalDTO DTO)
        {
            try
            {
                var query = "UPDATE produto SET codigo_barra = '' WHERE codigo_barra = '"+DTO.Codigo_Barra+"'; UPDATE produto SET codigo_barra = '"+ DTO.Codigo_Barra +"' WHERE id = '"+ DTO.Id +"'";
                bd.Conectar();
                bd.ExecutarComandoSQL(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
