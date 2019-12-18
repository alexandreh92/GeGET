using System;
using System.Collections.ObjectModel;
using System.Data;
using DAL;
using DTO;

namespace BLL
{
    class AdicionarItemProjetoBLL : IDisposable
    {
        #region Declarations
        bool disposed = false;
        AcessoBancoDados bd = new AcessoBancoDados();
        LoginDTO loginDTO = new LoginDTO();
        #endregion

        #region Methos

        #region Load Itens
        public ObservableCollection<AdicionarItemProjetoDTO> LoadItens()
        {
            var itens = new ObservableCollection<AdicionarItemProjetoDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT p.id,i.descricao,p.descricao AS desc_completa,un.descricao AS un,p.partnumber,p.custounitario,coalesce(e.quantidade,0) as estoque,f.rsocial FROM produto p JOIN item i ON p.DESCRICAO_ITEM_id=i.id JOIN unidade un ON un.id=i.unidade_id JOIN fornecedor f ON p.FORNECEDOR_id=f.id LEFT OUTER JOIN estoque e ON e.produto_id=p.id WHERE p.STATUS_id='1' AND i.mobra='0' AND p.id !='1' AND f.status_id='1' ORDER BY i.descricao";
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
                    itens.Add(new AdicionarItemProjetoDTO
                    {
                        Id = Convert.ToInt32(item["id"]),
                        Descricao = item["descricao"].ToString(),
                        Anotacoes = item["desc_completa"].ToString(),
                        Unidade = item["un"].ToString(),
                        Partnumber = item["partnumber"].ToString(),
                        Custo = Convert.ToDouble(item["custounitario"]),
                        Fabricante = item["rsocial"].ToString(),
                        Codigo = Convert.ToInt32(item["id"]).ToString("000000"),
                        Estoque = Convert.ToDouble(item["estoque"])
                    });
                }
                bd.CloseConection();
            }
            return itens;
        }
        #endregion

        #region Inserir
        public void Inserir(AdicionarItemProjetoDTO DTO)
        {
            try
            {
                var query = "INSERT INTO lista_vendas (VENDAS_id, PRODUTO_id, ATIVIDADE_id, preco, anotacoes, USUARIO_id) VALUES ('" + DTO.Negocio_Id + "','" + DTO.Id + "','" + DTO.Atividade_Id + "', (SELECT CASE WHEN icms = '0' AND ipi = '0' THEN custounitario+(custounitario*(1+ipi)-(custounitario*(1+ipi)*icms))/(1)*((0))-(custounitario*(1+ipi)*icms)+custounitario*ipi ELSE custounitario+(custounitario*(1+ipi)-(custounitario*(1+ipi)*icms))/(1-0.18)*((0.18))-(custounitario*(1+ipi)*icms)+custounitario*ipi END FROM produto WHERE id='" + DTO.Id + "'), '" + DTO.Anotacoes + "', '" + loginDTO.Id + "' )";
                bd.Conectar();
                bd.ExecutarComandoSQL(query);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

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
