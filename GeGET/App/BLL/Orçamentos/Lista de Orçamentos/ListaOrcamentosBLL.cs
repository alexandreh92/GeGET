using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;
using System.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace BLL
{
    class ListaOrcamentosBLL
    {
        #region Declarations
        AcessoBancoDados bd = new AcessoBancoDados();
        #endregion

        #region Methods

        #region Load Clientes
        public ObservableCollection<ListaOrcamentosDTO> LoadOrcamento(ListaOrcamentosDTO DTO)
        {
            var orcamento = new ObservableCollection<ListaOrcamentosDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT (@cnt := @cnt + 1) AS Num, t.* FROM (SELECT p.id, i.descricao, lo.descricao_orc as desc_completa, p.partnumber, i.un, f.rsocial, lo.quantidade, lo.desconto, lo.preco_orc, lo.bdi, lo.quantidade * lo.preco_orc * (1-(desconto/100)) as preco_total, CASE lo.fd WHEN '0' THEN lo.quantidade * lo.preco_orc * (1 + (lo.bdi / 100)) * (1-(desconto/100)) WHEN '1' THEN lo.quantidade * lo.preco_orc * (1-(desconto/100)) * (1 + 10 / 100) END as preco_total_bdi, lo.fd, lo.id as pk FROM lista_orcamento lo JOIN produto p ON lo.PRODUTO_id = p.id JOIN item i ON p.DESCRICAO_ITEM_id = i.id JOIN fornecedor f ON p.FORNECEDOR_id = f.id WHERE lo.NEGOCIO_id = '"+ DTO.Id +"' AND lo.ATIVIDADES_id = '" + DTO.Atividade_Id + "') t CROSS JOIN(SELECT @cnt:= 0) AS dummy";
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
                    orcamento.Add(new ListaOrcamentosDTO { Id = dr["pk"].ToString(), Produto_Id = dr["id"].ToString(), Descricao = dr["descricao"].ToString(), Partnumber = dr["partnumber"].ToString(), Anotacoes = dr["desc_completa"].ToString(), Un = dr["un"].ToString(), Quantidade = Convert.ToDouble(dr["quantidade"]), Fabricante = dr["rsocial"].ToString(), Custo_Total = Convert.ToDouble(dr["preco_total"]), Bdi = Convert.ToDouble(dr["bdi"]), Preco_Unitario = Convert.ToDouble(dr["preco_orc"]), Preco_Total = Convert.ToDouble(dr["preco_total_bdi"]), Fd = Convert.ToInt32(dr["fd"]) });
                }
                bd.CloseConection();
            }
            return orcamento;
        }

        #endregion

        #region Atualizar Preco

        public void AtualizarPreco(ListaOrcamentosDTO DTO)
        {
            try
            {
                var query = "UPDATE lista_orcamento lo JOIN produto p ON lo.PRODUTO_id = p.id JOIN atividade a ON lo.ATIVIDADES_id = a.id JOIN versao_atividade va ON a.VERSAO_ATIVIDADE_id = va.id JOIN negocio n ON lo.NEGOCIO_id = n.id SET lo.preco_orc = CASE WHEN p.icms = '0' AND p.ipi = '0' THEN p.custounitario+(p.custounitario*(1+p.ipi)-(p.custounitario*(1+p.ipi)*p.icms))/(1)*((0))-(p.custounitario*(1+p.ipi)*p.icms)+p.custounitario*p.ipi ELSE p.custounitario+(p.custounitario*(1+p.ipi)-(p.custounitario*(1+p.ipi)*p.icms))/(1-0.18)*((0.18))-(p.custounitario*(1+p.ipi)*p.icms)+p.custounitario*p.ipi END WHERE lo.PRODUTO_id = p.id AND lo.NEGOCIO_id = '" + DTO.Id + "' AND n.versao_valida = va.VERSAO_id";
                Task.Run(() => bd.ExecutarComandoSQLAsync(query));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        #endregion

        #endregion
    }
}
