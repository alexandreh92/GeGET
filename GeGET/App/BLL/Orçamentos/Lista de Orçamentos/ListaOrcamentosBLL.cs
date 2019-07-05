using System;
using System.Collections.Generic;
using DTO;
using DAL;
using System.Data;
using System.Collections.ObjectModel;
using Helpers;

namespace BLL
{
    class ListaOrcamentosBLL : IDisposable
    {
        #region Declarations
        bool disposed = false;
        AcessoBancoDados bd = new AcessoBancoDados();
        #endregion

        #region Methods

        #region Load Orçamentos
        public ObservableCollection<ListaOrcamentosDTO> LoadOrcamento(InformacoesListaOrcamentosDTO DTO)
        {
            var orcamento = new ObservableCollection<ListaOrcamentosDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT (@cnt := @cnt + 1) AS Num, t.* FROM (SELECT p.id, i.descricao, lo.descricao_orc as desc_completa, p.partnumber, un.descricao as un, f.rsocial, lo.quantidade, lo.desconto, lo.preco_orc, lo.bdi, lo.quantidade * lo.preco_orc * (1-(desconto/100)) as preco_total, CASE lo.fd WHEN '0' THEN lo.quantidade * lo.preco_orc * (1 + (lo.bdi / 100)) * (1-(desconto/100)) WHEN '1' THEN lo.quantidade * lo.preco_orc * (1-(desconto/100)) * (1 + 10 / 100) END as preco_total_bdi, lo.fd, lo.id as pk FROM lista_orcamento lo JOIN produto p ON lo.PRODUTO_id = p.id JOIN item i ON p.DESCRICAO_ITEM_id = i.id JOIN unidade un ON un.id = i.unidade_id JOIN fornecedor f ON p.FORNECEDOR_id = f.id WHERE lo.NEGOCIO_id = '"+ DTO.Id +"' AND lo.ATIVIDADES_id = '" + DTO.Atividade_Id + "') t CROSS JOIN(SELECT @cnt:= 0) AS dummy";
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
                bd.CloseConection();
            }
            return orcamento;
        }

        #endregion

        #region Load Orçamentos Exportar

        public ObservableCollection<ListaOrcamentosDTO> LoadOrcamentoExportar(ListaOrcamentosDTO DTO)
        {
            var orcamento = new ObservableCollection<ListaOrcamentosDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT (@cnt := @cnt + 1) AS Num, t.* FROM (SELECT va.versao_id as versao, dis.descricao as disciplina,da.descricao as atividade, a.descricao as descricao_atividade, p.id as codigo, i.descricao as descricao, p.partnumber as partnumber, lo.descricao_orc as descricao_detalhada, un.descricao as unidade, f.rsocial as fabricante, lo.quantidade AS qtde, lo.preco_orc preco, lo.bdi, lo.quantidade * lo.preco_orc as preco_total, CASE lo.fd WHEN '0' THEN lo.quantidade * lo.preco_orc * (1 + (lo.bdi / 100)) WHEN '1' THEN lo.quantidade * lo.preco_orc * (1 + 10 / 100) END as preco_total_bdi, lo.fd FROM lista_orcamento lo JOIN produto p ON lo.PRODUTO_id = p.id JOIN atividade a ON a.id = lo.atividades_id JOIN desc_atividades da ON da.id = a.desc_atividades_id JOIN disciplina dis ON dis.id = da.disciplina_id JOIN versao_atividade va ON va.id = a.versao_atividade_id JOIN item i ON p.DESCRICAO_ITEM_id = i.id JOIN unidade un ON i.unidade_id = un.id JOIN fornecedor f ON p.FORNECEDOR_id = f.id WHERE lo.NEGOCIO_id = '"+ DTO.Id +"' AND va.versao_id = '"+ DTO.Versao +"') t CROSS JOIN(SELECT @cnt:= 0) AS dummy";
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
                    orcamento.Add(new ListaOrcamentosDTO { Versao = Convert.ToInt32(dr["versao"]).ToString("00"), Codigo_Produto = Convert.ToInt32(dr["codigo"]).ToString("000000"), Disciplina = dr["disciplina"].ToString(), Descricao_Atividade = dr["descricao_atividade"].ToString(), Atividade = dr["atividade"].ToString(), Produto_Id = dr["codigo"].ToString(), Descricao = dr["descricao"].ToString(), Partnumber = dr["partnumber"].ToString(), Anotacoes = dr["descricao_detalhada"].ToString(), Un = dr["unidade"].ToString(), Fabricante = dr["fabricante"].ToString().ToString(), Quantidade = Convert.ToDouble(dr["qtde"]), Preco_Unitario = Convert.ToDouble(dr["preco"]), Bdi = Convert.ToDouble(dr["bdi"]), Preco_Total = Convert.ToDouble(dr["preco_total_bdi"]), Custo_Total = Convert.ToDouble(dr["preco_total"]), Fd = Convert.ToInt32(dr["fd"]) });
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

        public void AtualizarPreco(InformacoesListaOrcamentosDTO DTO)
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

        public void Excluir(ObservableCollection<ListaOrcamentosDTO> dTO)
        {
            try
            {
                string substring = "";
                foreach (ListaOrcamentosDTO dto in dTO)
                {
                    substring = substring + "OR id='"+dto.Id+"' ";
                }
                var query = "DELETE FROM lista_orcamento WHERE " + substring.TrimStart(new Char[] { 'O', 'R'});

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

        public void AtualizarQuantidade(ListaOrcamentosDTO dto)
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

        public void AtualizarBDI(ListaOrcamentosDTO dto)
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

        public void AtualizarFD(ListaOrcamentosDTO dto)
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

        public ObservableCollection<ValoresOrcamento> LoadValores(InformacoesListaOrcamentosDTO dto)
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
                            "(SELECT COALESCE(SUM(lo.quantidade * lo.preco_orc * (1-(desconto/100))), 0) FROM lista_orcamento lo JOIN produto p ON lo.PRODUTO_id = p.id JOIN item i ON p.DESCRICAO_ITEM_id = i.id JOIN atividade a ON lo.ATIVIDADES_id = a.id JOIN versao_atividade va ON a.VERSAO_ATIVIDADE_id = va.id JOIN negocio n ON lo.NEGOCIO_id = n.id WHERE lo.NEGOCIO_id = '" + dto.Id + "' AND n.versao_valida = va.VERSAO_id AND i.mobra = '0' AND a.habilitado = '1') as negocio_total_materiais," +
                            "(SELECT COALESCE(SUM(CASE lo.fd WHEN '0' THEN lo.quantidade * lo.preco_orc * (1-(desconto/100)) * (1 + (bdi / 100)) WHEN '1' THEN lo.quantidade * (1-(desconto/100)) * lo.preco_orc * (1 + 10 / 100) END), 0) FROM lista_orcamento lo JOIN produto p ON lo.PRODUTO_id = p.id JOIN item i ON p.DESCRICAO_ITEM_id = i.id JOIN atividade a ON lo.ATIVIDADES_id = a.id JOIN versao_atividade va ON a.VERSAO_ATIVIDADE_id = va.id JOIN negocio n ON lo.NEGOCIO_id = n.id WHERE lo.NEGOCIO_id = '" + dto.Id + "' AND a.habilitado = '1' AND n.versao_valida = va.VERSAO_id AND i.mobra = '0') as negocio_total_materiais_bdi," +
                            "(SELECT COALESCE(SUM(lo.quantidade * lo.preco_orc * (1-(desconto/100))), 0) FROM lista_orcamento lo JOIN produto p ON lo.PRODUTO_id = p.id JOIN item i ON p.DESCRICAO_ITEM_id = i.id JOIN atividade a ON lo.ATIVIDADES_id = a.id JOIN versao_atividade va ON a.VERSAO_ATIVIDADE_id = va.id JOIN negocio n ON lo.NEGOCIO_id = n.id WHERE lo.NEGOCIO_id = '" + dto.Id + "' AND n.versao_valida = va.VERSAO_id and i.mobra = '1') AS negocio_total_mobra," +
                            "(SELECT COALESCE(SUM(CASE lo.fd WHEN '0' THEN lo.quantidade * lo.preco_orc * (1-(desconto/100)) * (1 + (bdi / 100)) WHEN '1' THEN lo.quantidade * (1-(desconto/100)) * lo.preco_orc * (1 + 10 / 100) END), 0) FROM lista_orcamento lo JOIN produto p ON lo.PRODUTO_id = p.id JOIN item i ON p.DESCRICAO_ITEM_id = i.id JOIN atividade a ON lo.ATIVIDADES_id = a.id JOIN versao_atividade va ON a.VERSAO_ATIVIDADE_id = va.id JOIN negocio n ON lo.NEGOCIO_id = n.id WHERE lo.NEGOCIO_id = '" + dto.Id + "' AND a.habilitado = '1' AND n.versao_valida = va.VERSAO_id and i.mobra = '1') as negocio_total_mobra_bdi," +
                            "(SELECT COALESCE(SUM(CASE lo.fd WHEN '0' THEN lo.quantidade * lo.preco_orc * (1-(desconto/100)) * (1 + (bdi / 100)) WHEN '1' THEN lo.quantidade * (1-(desconto/100)) * lo.preco_orc * (1 + 10 / 100) END), 0) FROM lista_orcamento lo JOIN produto p ON lo.PRODUTO_id = p.id JOIN item i ON p.DESCRICAO_ITEM_id = i.id JOIN atividade a ON lo.ATIVIDADES_id = a.id JOIN versao_atividade va ON a.VERSAO_ATIVIDADE_id = va.id JOIN negocio n ON lo.NEGOCIO_id = n.id WHERE lo.NEGOCIO_id = '" + dto.Id + "' AND a.habilitado = '1' AND n.versao_valida = va.VERSAO_id and lo.fd = '1') as negocio_total_fd," +
                            "(SELECT COALESCE(SUM(CASE lo.fd WHEN '0' THEN lo.quantidade * lo.preco_orc * (1-(desconto/100)) * (1 + (bdi / 100)) WHEN '1' THEN lo.quantidade * (1-(desconto/100)) * lo.preco_orc * (1 + 10 / 100) END), 0) FROM lista_orcamento lo JOIN produto p ON lo.PRODUTO_id = p.id JOIN item i ON p.DESCRICAO_ITEM_id = i.id JOIN atividade a ON lo.ATIVIDADES_id = a.id JOIN versao_atividade va ON a.VERSAO_ATIVIDADE_id = va.id JOIN negocio n ON lo.NEGOCIO_id = n.id WHERE lo.NEGOCIO_id = '" + dto.Id + "' AND a.habilitado = '1' AND n.versao_valida = va.VERSAO_id and lo.fd = '0') as negocio_total_faturado," +
                            "(SELECT COALESCE(SUM(lo.quantidade * lo.preco_orc * (1-(desconto/100))), 0) FROM lista_orcamento lo JOIN produto p ON lo.PRODUTO_id = p.id JOIN item i ON p.DESCRICAO_ITEM_id = i.id JOIN atividade a ON lo.ATIVIDADES_id = a.id JOIN versao_atividade va ON a.VERSAO_ATIVIDADE_id = va.id JOIN negocio n ON lo.NEGOCIO_id = n.id WHERE lo.NEGOCIO_id = '" + dto.Id + "' AND a.habilitado = '1' AND n.versao_valida = va.VERSAO_id) as negocio_total," +
                            "(SELECT COALESCE(SUM(CASE lo.fd WHEN '0' THEN lo.quantidade * lo.preco_orc * (1-(desconto/100)) * (1 + (bdi / 100)) WHEN '1' THEN lo.quantidade * (1-(desconto/100)) * lo.preco_orc * (1 + 10 / 100) END), 0) FROM lista_orcamento lo JOIN produto p ON lo.PRODUTO_id = p.id JOIN item i ON p.DESCRICAO_ITEM_id = i.id JOIN atividade a ON lo.ATIVIDADES_id = a.id JOIN versao_atividade va ON a.VERSAO_ATIVIDADE_id = va.id JOIN negocio n ON lo.NEGOCIO_id = n.id WHERE lo.NEGOCIO_id = '" + dto.Id + "' AND a.habilitado = '1' AND n.versao_valida = va.VERSAO_id) as negocio_total_bdi";
                bd.Conectar();
                dt = bd.RetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                valores.Add(new ValoresOrcamento { Atividade_Total_Itens = Convert.ToDouble(dt.Rows[0]["atividade_total_materiais"]), Atividade_Total_Itens_BDI = Convert.ToDouble(dt.Rows[0]["atividade_total_materiais_bdi"]), Atividade_Total_Mobra = Convert.ToDouble(dt.Rows[0]["atividade_total_mobra"]), Atividade_Total_Mobra_BDI = Convert.ToDouble(dt.Rows[0]["atividade_total_mobra_bdi"]), Atividade_Total_Atividade = Convert.ToDouble(dt.Rows[0]["atividade_total"]), Atividade_Total_Atividade_BDI = Convert.ToDouble(dt.Rows[0]["atividade_total_bdi"]), Atividade_Total_FD = Convert.ToDouble(dt.Rows[0]["atividade_total_fd"]), Atividade_Total_Faturado = Convert.ToDouble(dt.Rows[0]["atividade_total_faturado"]), Negocio_Total_Itens = Convert.ToDouble(dt.Rows[0]["negocio_total_materiais"]), Negocio_Total_Itens_BDI = Convert.ToDouble(dt.Rows[0]["negocio_total_materiais_bdi"]), Negocio_Total_Mobra = Convert.ToDouble(dt.Rows[0]["negocio_total_mobra"]), Negocio_Total_Mobra_BDI = Convert.ToDouble(dt.Rows[0]["negocio_total_mobra_bdi"]), Negocio_Total_Atividade = Convert.ToDouble(dt.Rows[0]["negocio_total"]), Negocio_Total_Atividade_BDI = Convert.ToDouble(dt.Rows[0]["negocio_total_bdi"]), Negocio_Total_Faturado = Convert.ToDouble(dt.Rows[0]["negocio_total_faturado"]), Negocio_Total_FD = Convert.ToDouble(dt.Rows[0]["negocio_total_fd"]) });
            }

            return valores;
        }


        #endregion

        #region Update Anotações

        public void UpdateAnotacoes(ListaOrcamentosDTO DTO)
        {
            try
            {
                var query = "UPDATE lista_orcamento SET descricao_orc ='"+DTO.Anotacoes+"' WHERE id='"+DTO.Id+"'";
                bd.Conectar();
                bd.ExecutarComandoSQL(query);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.ToString());
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

    class InformacoesListaOrcamentosBLL : IDisposable
    {
        bool disposed = false;
        AcessoBancoDados bd = new AcessoBancoDados();

        #region Load Informações
        public ObservableCollection<InformacoesListaOrcamentosDTO> LoadInformacoes(InformacoesListaOrcamentosDTO DTO)
        {
            var informacoes = new ObservableCollection<InformacoesListaOrcamentosDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT DISTINCT n.id, n.descricao, c.rsocial, cid.cidade, cid.uf, n.versao_valida, n.STATUS_ORCAMENTO_id FROM negocio n JOIN estabelecimento e ON n.ESTABELECIMENTO_id = e.id JOIN cliente c ON e.CLIENTE_id = c.id JOIN cidades cid ON e.CIDADES_id = cid.id JOIN status_orcamento so ON n.STATUS_ORCAMENTO_id = so.id JOIN atividade a ON a.NEGOCIO_id = n.id JOIN versao_atividade va ON a.VERSAO_ATIVIDADE_id = va.id  WHERE n.id = '" + DTO.Id + "' AND va.VERSAO_id = n.versao_valida";
                bd.Conectar();
                dt = bd.RetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                informacoes.Add(new InformacoesListaOrcamentosDTO
                {
                    Id = Convert.ToInt32(dt.Rows[0]["id"]),
                    Descricao = dt.Rows[0]["descricao"].ToString(),
                    Razao_Social = dt.Rows[0]["rsocial"].ToString(),
                    Cidade = dt.Rows[0]["cidade"].ToString(),
                    Uf = dt.Rows[0]["uf"].ToString(),
                    Versao = dt.Rows[0]["versao_valida"].ToString(),
                    Disciplinas = LoadDisciplinas(DTO),
                    Atividades = LoadAtividades(DTO),
                    Descricao_Atividades = LoadAtividadesDescricao(DTO),
                    Negocio_Id = Convert.ToInt32(dt.Rows[0]["id"])
                });
            }
            return informacoes;
        }
        #endregion

        #region Load Disciplinas
        private ObservableCollection<DisciplinasOrcamentoDTO> LoadDisciplinas(InformacoesListaOrcamentosDTO DTO)
        {
            var disciplinas = new ObservableCollection<DisciplinasOrcamentoDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT DISTINCT d.descricao, d.id FROM atividade a JOIN negocio n ON a.NEGOCIO_id = n.id JOIN desc_atividades da ON a.DESC_ATIVIDADES_id = da.id JOIN disciplina d ON da.DISCIPLINA_id = d.id JOIN versao_atividade va ON a.VERSAO_ATIVIDADE_id = va.id WHERE a.NEGOCIO_id = '" + DTO.Id + "' AND va.VERSAO_id = n.versao_valida and a.habilitado='1' ORDER BY d.descricao";
                bd.Conectar();
                dt = bd.RetDataTable(query);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.ToString());
            }
            finally
            {
                if (dt.Rows.Count>0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        disciplinas.Add(new DisciplinasOrcamentoDTO
                        {
                            Id = Convert.ToInt32(dr["id"]),
                            Descricao = dr["descricao"].ToString()
                        });
                    }
                }
                else
                {
                    disciplinas = null;
                }
            }
            return disciplinas;
        }
        #endregion

        #region LoadAtividades
        private ObservableCollection<AtividadesOrcamentoDTO> LoadAtividades(InformacoesListaOrcamentosDTO DTO)
        {
            var atividades = new ObservableCollection<AtividadesOrcamentoDTO>();
            var dt = new DataTable();
            try
            {
                var query = "select distinct da.id, da.descricao, da.disciplina_id from atividade a JOIN desc_atividades da ON da.id = a.desc_atividades_id join versao_atividade va ON va.id = a.versao_atividade_id join negocio n ON a.negocio_id = n.id and n.versao_valida = va.versao_id where a.negocio_id = '" + DTO.Id + "' and a.habilitado ='1'";
                bd.Conectar();
                dt = bd.RetDataTable(query);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.ToString());
            }
            finally
            {
                if (dt.Rows.Count>0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        atividades.Add(new AtividadesOrcamentoDTO
                        {
                            Id = Convert.ToInt32(dr["id"]),
                            Descricao = dr["descricao"].ToString(),
                            Disciplina_Id = dr["disciplina_id"].ToString()
                        });
                    }
                }
                else
                {
                    atividades = null;
                }
            }
            return atividades;
        }
        #endregion

        #region Load Descricao Atividades
        private ObservableCollection<DescricaoAtividadesOrcamentoDTO> LoadAtividadesDescricao(InformacoesListaOrcamentosDTO DTO)
        {
            var atividades = new ObservableCollection<DescricaoAtividadesOrcamentoDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT da.id as descricao_atividade_id, a.descricao, a.id FROM atividade a JOIN negocio n ON a.NEGOCIO_id = n.id JOIN versao_atividade va ON a.VERSAO_ATIVIDADE_id = va.id JOIN desc_atividades da ON a.DESC_ATIVIDADES_id = da.id JOIN disciplina disc ON da.DISCIPLINA_id = disc.id WHERE a.NEGOCIO_id = '"+DTO.Id+"' AND a.habilitado='1' AND va.VERSAO_id = n.versao_valida";
                bd.Conectar();
                dt = bd.RetDataTable(query);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.ToString());
            }
            finally
            {
                if (dt.Rows.Count>0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        atividades.Add(new DescricaoAtividadesOrcamentoDTO
                        {
                            Id = Convert.ToInt32(dr["id"]),
                            Descricao = dr["descricao"].ToString(),
                            Desc_Atividade_Id = dr["descricao_atividade_id"].ToString()
                        });
                    }
                }
                else
                {
                    atividades = null;
                }
                
            }
            return atividades;
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
