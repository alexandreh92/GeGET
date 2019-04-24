using System;
using DTO;
using DAL;
using System.Data;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BLL
{
    class AdicionarItemOrcamentoBLL
    {
        #region Declarations
        AcessoBancoDados bd = new AcessoBancoDados();
        #endregion

        #region Load Itens
        public ObservableCollection<AdicionarItemOrcamentoDTO> LoadItens()
        {
            var itens = new ObservableCollection<AdicionarItemOrcamentoDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT p.id, i.descricao, p.descricao as desc_completa, un.descricao as un, p.partnumber, p.custounitario, f.rsocial FROM produto p JOIN item i ON p.DESCRICAO_ITEM_id = i.id JOIN unidade un ON un.id = i.unidade_id JOIN fornecedor f ON p.FORNECEDOR_id = f.id WHERE p.STATUS_id='1' ORDER BY p.id, i.descricao";
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
                    itens.Add(new AdicionarItemOrcamentoDTO { Id = Convert.ToInt32(item["id"]), Descricao = item["descricao"].ToString(), Descricao_Produto = item["desc_completa"].ToString().Trim('\n', '\r'), Un = item["un"].ToString(), Partnumber = item["partnumber"].ToString(), Custo_Unitario = Convert.ToDouble(item["custounitario"]), Fabricante = item["rsocial"].ToString(), Codigo_Produto = Convert.ToInt32(item["id"]).ToString("000000") });
                }
                bd.CloseConection();
            }
            return itens;
        }
        #endregion

        #region Add

        public void Add(InformacoesListaOrcamentosDTO DTO, AdicionarItemOrcamentoDTO listaDTO)
        {
            try
            {
                var query = "INSERT INTO lista_orcamento (NEGOCIO_id, PRODUTO_id, ATIVIDADES_id, preco_orc, descricao_orc) VALUES ('" + DTO.Negocio_Id + "','" + listaDTO.Id + "','" + DTO.Atividade_Id + "', (SELECT CASE WHEN icms = '0' AND ipi = '0' THEN custounitario+(custounitario*(1+ipi)-(custounitario*(1+ipi)*icms))/(1)*((0))-(custounitario*(1+ipi)*icms)+custounitario*ipi ELSE custounitario+(custounitario*(1+ipi)-(custounitario*(1+ipi)*icms))/(1-0.18)*((0.18))-(custounitario*(1+ipi)*icms)+custounitario*ipi END FROM produto WHERE id='" + listaDTO.Id + "'), '" + listaDTO.Descricao_Produto + "' )";
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