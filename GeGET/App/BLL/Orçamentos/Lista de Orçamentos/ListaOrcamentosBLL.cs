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
using Helpers;

namespace BLL
{
    class ListaOrcamentosBLL
    {
        #region Declarations
        AcessoBancoDados bd = new AcessoBancoDados();
        #endregion

        #region Methods

        #region Load Orçamentos
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
                    orcamento.Add(new ListaOrcamentosDTO { Id = dr["pk"].ToString(), Produto_Id = dr["id"].ToString(), Codigo_Produto = Convert.ToInt32(dr["id"]).ToString("000000"), Descricao = dr["descricao"].ToString(), Partnumber = dr["partnumber"].ToString(), Anotacoes = dr["desc_completa"].ToString(), Un = dr["un"].ToString(), Quantidade = Convert.ToDouble(dr["quantidade"]), Fabricante = dr["rsocial"].ToString(), Custo_Total = Convert.ToDouble(dr["preco_total"]), Bdi = Convert.ToDouble(dr["bdi"]), Preco_Unitario = Convert.ToDouble(dr["preco_orc"]), Preco_Total = Convert.ToDouble(dr["preco_total_bdi"]), Fd = Convert.ToInt32(dr["fd"]) });
                }
            }
            return orcamento;
        }

        #endregion

        #region Load Dataset Orçamentos
        public ObservableCollection<ListaOrcamentosDTO> LoadDatasetOrcamento(ListaOrcamentosDTO DTO)
        {
            var orcamento = new ObservableCollection<ListaOrcamentosDTO>();
            var listaQuery = new List<QueryHelper>();
            var ds = new DataSet();
            listaQuery.Add(new QueryHelper { Table = "listaOrcamento", Sql = "" });
            return orcamento;
        }
        #endregion

        #region Atualizar Preco

        public void AtualizarPreco(ListaOrcamentosDTO DTO)
        {
            try
            {
                var query = "UPDATE lista_orcamento lo JOIN produto p ON lo.PRODUTO_id = p.id JOIN atividade a ON lo.ATIVIDADES_id = a.id JOIN versao_atividade va ON a.VERSAO_ATIVIDADE_id = va.id JOIN negocio n ON lo.NEGOCIO_id = n.id SET lo.preco_orc = CASE WHEN p.icms = '0' AND p.ipi = '0' THEN p.custounitario+(p.custounitario*(1+p.ipi)-(p.custounitario*(1+p.ipi)*p.icms))/(1)*((0))-(p.custounitario*(1+p.ipi)*p.icms)+p.custounitario*p.ipi ELSE p.custounitario+(p.custounitario*(1+p.ipi)-(p.custounitario*(1+p.ipi)*p.icms))/(1-0.18)*((0.18))-(p.custounitario*(1+p.ipi)*p.icms)+p.custounitario*p.ipi END WHERE lo.PRODUTO_id = p.id AND lo.NEGOCIO_id = '" + DTO.Id + "' AND n.versao_valida = va.VERSAO_id";
                bd.Conectar();
                bd.ExecutarComandoSQL(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        #endregion

        #region Excluir

        public void Excluir(AdicionarItemOrcamentoDTO dTO)
        {
            try
            {
                var query = "DELETE FROM lista_orcamento WHERE id = '"+ dTO.Id +"'";
                bd.Conectar();
                bd.ExecutarComandoSQL(query);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.ToString());
            }
        }

        #endregion

        #region Atualizar Quantidade

        public void AtualizarQuantidade(MaterialDTO dto)
        {
            try
            {
                var query = "UPDATE lista_orcamento SET quantidade = '"+dto.Quantidade.ToString().Replace(",", ".") + "' WHERE id = '"+dto.Id+"'";
                bd.Conectar();
                bd.ExecutarComandoSQL(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        #endregion

        #region Atualizar BDI

        public void AtualizarBDI(MaterialDTO dto)
        {
            try
            {
                var query = "UPDATE lista_orcamento SET bdi = '" + dto.Bdi.ToString().Replace(",", ".") + "' WHERE id = '" + dto.Id + "'";
                bd.Conectar();
                bd.ExecutarComandoSQL(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        #endregion

        #region Atualizar Faturamento Direto

        public void AtualizarFD(MaterialDTO dto)
        {
            try
            {
                var query = "UPDATE lista_orcamento SET fd = '" + dto.Fd + "' WHERE id = '" + dto.Id + "'";
                bd.Conectar();
                bd.ExecutarComandoSQL(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        #endregion

        #region LoadValores

        public ObservableCollection<ValoresOrcamento> LoadValores(ListaOrcamentosDTO dto)
        {
            ObservableCollection<ValoresOrcamento> valores = new ObservableCollection<ValoresOrcamento>();
            DataTable dt = new DataTable();
            try
            {
                var query = "SELECT (SELECT COALESCE(SUM(lo.quantidade*lo.preco_orc * (1-(desconto/100))), 0) as total from lista_orcamento lo JOIN produto p ON lo.PRODUTO_id = p.id JOIN item i ON p.DESCRICAO_ITEM_id = i.id WHERE lo.NEGOCIO_id = '" + dto.Id + "' AND lo.ATIVIDADES_id = '" + dto.Atividade_Id + "' AND i.mobra = '0') as atividade_total_materiais," +
                            "(SELECT COALESCE(SUM(CASE lo.fd WHEN '0' THEN lo.quantidade * lo.preco_orc * (1-(desconto/100)) * (1+(bdi/100)) WHEN '1' THEN lo.quantidade * (1-(desconto/100)) * lo.preco_orc * (1+ 10/100) END),0) as total_bdi from lista_orcamento lo JOIN produto p ON lo.PRODUTO_id = p.id JOIN item i ON p.DESCRICAO_ITEM_id = i.id WHERE lo.NEGOCIO_id = '" + dto.Id + "' AND lo.ATIVIDADES_id = '" + dto.Atividade_Id + "' AND i.mobra = '0') as atividade_total_materiais_bdi," +
                            "(SELECT COALESCE(SUM(lo.quantidade * lo.preco_orc * (1-(desconto/100))), 0) from lista_orcamento lo JOIN produto p ON lo.PRODUTO_id = p.id JOIN item i ON p.DESCRICAO_ITEM_id = i.id WHERE lo.NEGOCIO_id = '" + dto.Id + "' AND lo.ATIVIDADES_id = '" + dto.Atividade_Id + "' AND i.mobra = '1') as atividade_total_mobra," +
                            "(SELECT COALESCE(SUM(CASE lo.fd WHEN '0' THEN lo.quantidade * lo.preco_orc * (1-(desconto/100)) * (1 + (bdi / 100)) WHEN '1' THEN lo.quantidade * (1-(desconto/100)) * lo.preco_orc * (1 + 10 / 100) END), 0) from lista_orcamento lo JOIN produto p ON lo.PRODUTO_id = p.id JOIN item i ON p.DESCRICAO_ITEM_id = i.id WHERE lo.NEGOCIO_id = '" + dto.Id + "' AND lo.ATIVIDADES_id = '" + dto.Atividade_Id + "' AND i.mobra = '1') as atividade_total_mobra_bdi," +
                            "(SELECT COALESCE(SUM(lo.quantidade*lo.preco_orc * (1-(desconto/100))), 0) from lista_orcamento lo JOIN produto p ON lo.PRODUTO_id = p.id JOIN item i ON p.DESCRICAO_ITEM_id = i.id WHERE lo.NEGOCIO_id = '" + dto.Id + "' AND lo.ATIVIDADES_id = '" + dto.Atividade_Id + "') as atividade_total," +
                            "(SELECT COALESCE(SUM(CASE lo.fd WHEN '0' THEN lo.quantidade * lo.preco_orc * (1-(desconto/100)) * (1+(bdi/100)) WHEN '1' THEN lo.quantidade * (1-(desconto/100)) * lo.preco_orc * (1+ 10/100) END),0) from lista_orcamento lo JOIN produto p ON lo.PRODUTO_id = p.id JOIN item i ON p.DESCRICAO_ITEM_id = i.id WHERE lo.NEGOCIO_id = '" + dto.Id + "' AND lo.ATIVIDADES_id = '" + dto.Atividade_Id + "') as atividade_total_bdi," +
                            "(SELECT COALESCE(SUM(CASE lo.fd WHEN '0' THEN lo.quantidade * lo.preco_orc * (1-(desconto/100)) * (1+(bdi/100)) WHEN '1' THEN lo.quantidade * (1-(desconto/100)) * lo.preco_orc * (1+ 10/100) END),0) from lista_orcamento lo JOIN produto p ON lo.PRODUTO_id = p.id JOIN item i ON p.DESCRICAO_ITEM_id = i.id WHERE lo.NEGOCIO_id = '" + dto.Id + "' AND lo.ATIVIDADES_id = '" + dto.Atividade_Id + "' AND lo.fd ='1') as atividade_total_fd," +
                            "(SELECT COALESCE(SUM(CASE lo.fd WHEN '0' THEN lo.quantidade * lo.preco_orc * (1-(desconto/100)) * (1+(bdi/100)) WHEN '1' THEN lo.quantidade * (1-(desconto/100)) * lo.preco_orc * (1+ 10/100) END),0) from lista_orcamento lo JOIN produto p ON lo.PRODUTO_id = p.id JOIN item i ON p.DESCRICAO_ITEM_id = i.id WHERE lo.NEGOCIO_id = '" + dto.Id + "' AND lo.ATIVIDADES_id = '" + dto.Atividade_Id + "' AND lo.fd ='0') as atividade_total_faturado," +
                            "(SELECT COALESCE(SUM(lo.quantidade * lo.preco_orc * (1-(desconto/100))), 0) FROM lista_orcamento lo JOIN produto p ON lo.PRODUTO_id = p.id JOIN item i ON p.DESCRICAO_ITEM_id = i.id JOIN atividade a ON lo.ATIVIDADES_id = a.id JOIN versao_atividade va ON a.VERSAO_ATIVIDADE_id = va.id JOIN negocio n ON lo.NEGOCIO_id = n.id WHERE lo.NEGOCIO_id = '" + dto.Id + "' AND n.versao_valida = va.VERSAO_id AND i.mobra = '0') as negocio_total_materiais," +
                            "(SELECT COALESCE(SUM(CASE lo.fd WHEN '0' THEN lo.quantidade * lo.preco_orc * (1-(desconto/100)) * (1 + (bdi / 100)) WHEN '1' THEN lo.quantidade * (1-(desconto/100)) * lo.preco_orc * (1 + 10 / 100) END), 0) FROM lista_orcamento lo JOIN produto p ON lo.PRODUTO_id = p.id JOIN item i ON p.DESCRICAO_ITEM_id = i.id JOIN atividade a ON lo.ATIVIDADES_id = a.id JOIN versao_atividade va ON a.VERSAO_ATIVIDADE_id = va.id JOIN negocio n ON lo.NEGOCIO_id = n.id WHERE lo.NEGOCIO_id = '" + dto.Id + "' AND n.versao_valida = va.VERSAO_id AND i.mobra = '0') as negocio_total_materiais_bdi," +
                            "(SELECT COALESCE(SUM(lo.quantidade * lo.preco_orc * (1-(desconto/100))), 0) FROM lista_orcamento lo JOIN produto p ON lo.PRODUTO_id = p.id JOIN item i ON p.DESCRICAO_ITEM_id = i.id JOIN atividade a ON lo.ATIVIDADES_id = a.id JOIN versao_atividade va ON a.VERSAO_ATIVIDADE_id = va.id JOIN negocio n ON lo.NEGOCIO_id = n.id WHERE lo.NEGOCIO_id = '" + dto.Id + "' AND n.versao_valida = va.VERSAO_id and i.mobra = '1') AS negocio_total_mobra," +
                            "(SELECT COALESCE(SUM(CASE lo.fd WHEN '0' THEN lo.quantidade * lo.preco_orc * (1-(desconto/100)) * (1 + (bdi / 100)) WHEN '1' THEN lo.quantidade * (1-(desconto/100)) * lo.preco_orc * (1 + 10 / 100) END), 0) FROM lista_orcamento lo JOIN produto p ON lo.PRODUTO_id = p.id JOIN item i ON p.DESCRICAO_ITEM_id = i.id JOIN atividade a ON lo.ATIVIDADES_id = a.id JOIN versao_atividade va ON a.VERSAO_ATIVIDADE_id = va.id JOIN negocio n ON lo.NEGOCIO_id = n.id WHERE lo.NEGOCIO_id = '" + dto.Id + "' AND n.versao_valida = va.VERSAO_id and i.mobra = '1') as negocio_total_mobra_bdi," +
                            "(SELECT COALESCE(SUM(CASE lo.fd WHEN '0' THEN lo.quantidade * lo.preco_orc * (1-(desconto/100)) * (1 + (bdi / 100)) WHEN '1' THEN lo.quantidade * (1-(desconto/100)) * lo.preco_orc * (1 + 10 / 100) END), 0) FROM lista_orcamento lo JOIN produto p ON lo.PRODUTO_id = p.id JOIN item i ON p.DESCRICAO_ITEM_id = i.id JOIN atividade a ON lo.ATIVIDADES_id = a.id JOIN versao_atividade va ON a.VERSAO_ATIVIDADE_id = va.id JOIN negocio n ON lo.NEGOCIO_id = n.id WHERE lo.NEGOCIO_id = '" + dto.Id + "' AND n.versao_valida = va.VERSAO_id and lo.fd = '1') as negocio_total_fd," +
                            "(SELECT COALESCE(SUM(CASE lo.fd WHEN '0' THEN lo.quantidade * lo.preco_orc * (1-(desconto/100)) * (1 + (bdi / 100)) WHEN '1' THEN lo.quantidade * (1-(desconto/100)) * lo.preco_orc * (1 + 10 / 100) END), 0) FROM lista_orcamento lo JOIN produto p ON lo.PRODUTO_id = p.id JOIN item i ON p.DESCRICAO_ITEM_id = i.id JOIN atividade a ON lo.ATIVIDADES_id = a.id JOIN versao_atividade va ON a.VERSAO_ATIVIDADE_id = va.id JOIN negocio n ON lo.NEGOCIO_id = n.id WHERE lo.NEGOCIO_id = '" + dto.Id + "' AND n.versao_valida = va.VERSAO_id and lo.fd = '0') as negocio_total_faturado," +
                            "(SELECT COALESCE(SUM(lo.quantidade * lo.preco_orc * (1-(desconto/100))), 0) FROM lista_orcamento lo JOIN produto p ON lo.PRODUTO_id = p.id JOIN item i ON p.DESCRICAO_ITEM_id = i.id JOIN atividade a ON lo.ATIVIDADES_id = a.id JOIN versao_atividade va ON a.VERSAO_ATIVIDADE_id = va.id JOIN negocio n ON lo.NEGOCIO_id = n.id WHERE lo.NEGOCIO_id = '" + dto.Id + "' AND n.versao_valida = va.VERSAO_id) as negocio_total," +
                            "(SELECT COALESCE(SUM(CASE lo.fd WHEN '0' THEN lo.quantidade * lo.preco_orc * (1-(desconto/100)) * (1 + (bdi / 100)) WHEN '1' THEN lo.quantidade * (1-(desconto/100)) * lo.preco_orc * (1 + 10 / 100) END), 0) FROM lista_orcamento lo JOIN produto p ON lo.PRODUTO_id = p.id JOIN item i ON p.DESCRICAO_ITEM_id = i.id JOIN atividade a ON lo.ATIVIDADES_id = a.id JOIN versao_atividade va ON a.VERSAO_ATIVIDADE_id = va.id JOIN negocio n ON lo.NEGOCIO_id = n.id WHERE lo.NEGOCIO_id = '" + dto.Id + "' AND n.versao_valida = va.VERSAO_id) as negocio_total_bdi";
                bd.Conectar();
                dt = bd.RetDataTable(query);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                valores.Add(new ValoresOrcamento { Atividade_Total_Itens = Convert.ToDouble(dt.Rows[0]["atividade_total_materiais"]), Atividade_Total_Itens_BDI = Convert.ToDouble(dt.Rows[0]["atividade_total_materiais_bdi"]), Atividade_Total_Mobra = Convert.ToDouble(dt.Rows[0]["atividade_total_mobra"]), Atividade_Total_Mobra_BDI = Convert.ToDouble(dt.Rows[0]["atividade_total_mobra_bdi"]), Atividade_Total_Atividade = Convert.ToDouble(dt.Rows[0]["atividade_total"]), Atividade_Total_Atividade_BDI = Convert.ToDouble(dt.Rows[0]["atividade_total_bdi"]), Atividade_Total_FD = Convert.ToDouble(dt.Rows[0]["atividade_total_fd"]), Atividade_Total_Faturado = Convert.ToDouble(dt.Rows[0]["atividade_total_faturado"]), Negocio_Total_Itens = Convert.ToDouble(dt.Rows[0]["negocio_total_materiais"]), Negocio_Total_Itens_BDI = Convert.ToDouble(dt.Rows[0]["negocio_total_materiais_bdi"]), Negocio_Total_Mobra = Convert.ToDouble(dt.Rows[0]["negocio_total_mobra"]), Negocio_Total_Mobra_BDI = Convert.ToDouble(dt.Rows[0]["negocio_total_mobra_bdi"]), Negocio_Total_Atividade = Convert.ToDouble(dt.Rows[0]["negocio_total"]), Negocio_Total_Atividade_BDI = Convert.ToDouble(dt.Rows[0]["negocio_total_bdi"]), Negocio_Total_Faturado = Convert.ToDouble(dt.Rows[0]["negocio_total_faturado"]), Negocio_Total_FD = Convert.ToDouble(dt.Rows[0]["negocio_total_fd"]) });
            }

            return valores;
        }


        #endregion

        #endregion
    }
}
