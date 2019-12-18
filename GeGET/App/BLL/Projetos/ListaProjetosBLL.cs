using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;
using System.Windows.Forms;

namespace BLL
{
    class ListaProjetosBLL : IDisposable
    {
        bool disposed = false;
        AcessoBancoDados bd = new AcessoBancoDados();
        LoginDTO loginDTO = new LoginDTO();

        #region Load Lista

        public ObservableCollection<ListaProjetosDTO> LoadLista(InformacoesListaProjetosDTO DTO)
        {
            var projetos = new ObservableCollection<ListaProjetosDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT (@cnt := @cnt + 1) AS num, t.* FROM (SELECT p.id, coalesce(e.produto_id,0) as estoque, i.descricao, p.partnumber, un.descricao as un, f.rsocial,lv.quantidade_orc, lv.preco_orc, lv.anotacoes, lv.quantidade, lv.quantidade * lv.preco as preco_total, lv.preco, lv.fd, lv.id as pk FROM lista_vendas lv JOIN produto p ON lv.PRODUTO_id = p.id JOIN item i ON p.DESCRICAO_ITEM_id = i.id JOIN fornecedor f ON p.FORNECEDOR_id = f.id JOIN unidade un ON un.id = i.unidade_id JOIN vendas vnd ON vnd.id = lv.vendas_id LEFT OUTER JOIN estoque e ON e.produto_id = lv.produto_id WHERE vnd.NEGOCIO_id = '" + DTO.Negocio_Id + "' AND lv.ATIVIDADE_id = '" + DTO.Atividade_Id + "') t CROSS JOIN(SELECT @cnt:= 0) AS dummy";
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
                    projetos.Add(new ListaProjetosDTO
                    {
                        Id = Convert.ToInt32(dr["pk"]),
                        Produto_Id = dr["id"].ToString(),
                        Codigo = Convert.ToInt32(dr["id"]).ToString("000000"),
                        Descricao = dr["descricao"].ToString(),
                        Partnumber = dr["partnumber"].ToString(),
                        Unidade = dr["un"].ToString(),
                        Fabricante = dr["rsocial"].ToString(),
                        Anotacoes = dr["anotacoes"].ToString(),
                        Fd = Convert.ToInt32(dr["fd"]),
                        Quantidade_Orcamento = Convert.ToDouble(dr["quantidade_orc"]),
                        Preco_Orcamento = Convert.ToDouble(dr["preco_orc"]),
                        Preco = Convert.ToDouble(dr["preco"]),
                        Quantidade = Convert.ToDouble(dr["quantidade"]),
                        Total = Convert.ToDouble(dr["preco_total"]),
                        Estoque = Convert.ToDouble(dr["estoque"]),
                    });
                }
            }
            return projetos;
        }
        #endregion

        #region Load Valores

        public ObservableCollection<ValoresProjetoDTO> LoadValores(InformacoesListaProjetosDTO DTO)
        {
            var valores = new ObservableCollection<ValoresProjetoDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT (SELECT COALESCE(SUM(lo.quantidade * lo.preco_orc), 0) as total from lista_orcamento lo JOIN produto p ON lo.PRODUTO_id = p.id JOIN item i ON p.DESCRICAO_ITEM_id = i.id WHERE lo.NEGOCIO_id = '" + DTO.Negocio_Id + "' AND lo.ATIVIDADES_id = '" + DTO.Atividade_Id + "' AND i.mobra = '0' AND lo.fd = '0') AS a_valor_orcamento," +
                                            "(SELECT COALESCE(SUM(lo.quantidade * lo.preco_orc), 0) as total from lista_orcamento lo JOIN produto p ON lo.PRODUTO_id = p.id JOIN item i ON p.DESCRICAO_ITEM_id = i.id WHERE lo.NEGOCIO_id = '" + DTO.Negocio_Id + "' AND lo.ATIVIDADES_id = '" + DTO.Atividade_Id + "' AND i.mobra = '0' AND lo.fd = '1') AS a_valor_fd_orcamento," +
                                            "(SELECT COALESCE(SUM(lv.quantidade * lv.preco), 0) as total from lista_vendas lv JOIN produto p ON lv.PRODUTO_id = p.id JOIN item i ON p.DESCRICAO_ITEM_id = i.id JOIN vendas vnd ON vnd.id = lv.VENDAS_id WHERE vnd.NEGOCIO_id = '" + DTO.Negocio_Id + "' AND lv.ATIVIDADE_id = '" + DTO.Atividade_Id + "' AND i.mobra = '0' AND lv.fd = '0') AS a_valor_obra," +
                                            "(SELECT COALESCE(SUM(lv.quantidade * lv.preco), 0) as total from lista_vendas lv JOIN produto p ON lv.PRODUTO_id = p.id JOIN item i ON p.DESCRICAO_ITEM_id = i.id JOIN vendas vnd ON vnd.id = lv.VENDAS_id WHERE vnd.NEGOCIO_id = '" + DTO.Negocio_Id + "' AND lv.ATIVIDADE_id = '" + DTO.Atividade_Id + "' AND i.mobra = '0' AND lv.fd = '1') AS a_valor_fd_obra," +
                                            "(SELECT COALESCE(SUM(lo.quantidade * lo.preco_orc), 0) as total from lista_orcamento lo JOIN produto p ON lo.PRODUTO_id = p.id JOIN item i ON p.DESCRICAO_ITEM_id = i.id WHERE lo.NEGOCIO_id = '" + DTO.Negocio_Id + "' AND lo.ATIVIDADES_id = '" + DTO.Atividade_Id + "' AND i.mobra = '0') AS a_valor_total_orcamento," +
                                            "(SELECT COALESCE(SUM(lv.quantidade * lv.preco), 0) as total from lista_vendas lv JOIN produto p ON lv.PRODUTO_id = p.id JOIN item i ON p.DESCRICAO_ITEM_id = i.id JOIN vendas vnd ON vnd.id = lv.VENDAS_id WHERE vnd.NEGOCIO_id = '" + DTO.Negocio_Id + "' AND lv.ATIVIDADE_id = '" + DTO.Atividade_Id + "') AS a_valor_total_obra," +
                                            "(SELECT COALESCE(SUM(lo.quantidade * lo.preco_orc), 0) as total from lista_orcamento lo JOIN produto p ON lo.PRODUTO_id = p.id JOIN item i ON p.DESCRICAO_ITEM_id = i.id JOIN atividade a ON lo.ATIVIDADES_id = a.id JOIN versao_atividade va ON a.VERSAO_ATIVIDADE_id = va.id JOIN negocio n ON lo.NEGOCIO_id = n.id WHERE lo.NEGOCIO_id = '" + DTO.Negocio_Id + "' AND n.versao_valida = va.versao_id AND i.mobra = '0' AND lo.fd = '0') AS n_valor_orcamento," +
                                            "(SELECT COALESCE(SUM(lo.quantidade * lo.preco_orc), 0) as total from lista_orcamento lo JOIN produto p ON lo.PRODUTO_id = p.id JOIN item i ON p.DESCRICAO_ITEM_id = i.id JOIN atividade a ON lo.ATIVIDADES_id = a.id JOIN versao_atividade va ON a.VERSAO_ATIVIDADE_id = va.id JOIN negocio n ON lo.NEGOCIO_id = n.id WHERE lo.NEGOCIO_id = '" + DTO.Negocio_Id + "' AND n.versao_valida = va.versao_id AND i.mobra = '0' AND lo.fd = '1') AS n_valor_fd_orcamento," +
                                            "(SELECT COALESCE(SUM(lv.quantidade * lv.preco), 0) as total from lista_vendas lv JOIN produto p ON lv.PRODUTO_id = p.id JOIN item i ON p.DESCRICAO_ITEM_id = i.id JOIN vendas vnd ON vnd.id = lv.VENDAS_id WHERE vnd.NEGOCIO_id = '" + DTO.Negocio_Id + "' AND i.mobra = '0' AND lv.fd = '0') AS n_valor_obra," +
                                            "(SELECT COALESCE(SUM(lv.quantidade * lv.preco), 0) as total from lista_vendas lv JOIN produto p ON lv.PRODUTO_id = p.id JOIN item i ON p.DESCRICAO_ITEM_id = i.id JOIN vendas vnd ON vnd.id = lv.VENDAS_id WHERE vnd.NEGOCIO_id = '" + DTO.Negocio_Id + "' AND i.mobra = '0' AND lv.fd = '1') AS n_valor_fd_obra," +
                                            "(SELECT COALESCE(SUM(lo.quantidade * lo.preco_orc), 0) as total from lista_orcamento lo JOIN produto p ON lo.PRODUTO_id = p.id JOIN item i ON p.DESCRICAO_ITEM_id = i.id JOIN atividade a ON lo.ATIVIDADES_id = a.id JOIN versao_atividade va ON a.VERSAO_ATIVIDADE_id = va.id JOIN negocio n ON lo.NEGOCIO_id = n.id WHERE lo.NEGOCIO_id = '" + DTO.Negocio_Id + "' AND n.versao_valida = va.versao_id AND i.mobra = '0') AS n_valor_total_orcamento," +
                                            "(SELECT COALESCE(SUM(lv.quantidade * lv.preco), 0) as total from lista_vendas lv JOIN produto p ON lv.PRODUTO_id = p.id JOIN item i ON p.DESCRICAO_ITEM_id = i.id JOIN vendas vnd ON vnd.id = lv.VENDAS_id WHERE vnd.NEGOCIO_id = '" + DTO.Negocio_Id + "') AS n_valor_total_obra";
                bd.Conectar();
                dt = bd.RetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                valores.Add(new ValoresProjetoDTO
                {
                    Atividade_Itens_Orcamento = Convert.ToDouble(dt.Rows[0]["a_valor_orcamento"]),
                    Atividade_Itens_Projeto = Convert.ToDouble(dt.Rows[0]["a_valor_obra"]),
                    Atividade_FD_Orcamento = Convert.ToDouble(dt.Rows[0]["a_valor_fd_orcamento"]),
                    Atividade_FD_Projeto = Convert.ToDouble(dt.Rows[0]["a_valor_fd_obra"]),
                    Atividade_Total_Orcamento = Convert.ToDouble(dt.Rows[0]["a_valor_total_orcamento"]),
                    Atividade_Total_Projeto = Convert.ToDouble(dt.Rows[0]["a_valor_total_obra"]),
                    Negocio_Itens_Orcamento = Convert.ToDouble(dt.Rows[0]["n_valor_orcamento"]),
                    Negocio_Itens_Projeto = Convert.ToDouble(dt.Rows[0]["n_valor_obra"]),
                    Negocio_FD_Orcamento = Convert.ToDouble(dt.Rows[0]["n_valor_fd_orcamento"]),
                    Negocio_FD_Projeto = Convert.ToDouble(dt.Rows[0]["n_valor_fd_obra"]),
                    Negocio_Total_Orcamento = Convert.ToDouble(dt.Rows[0]["n_valor_total_orcamento"]),
                    Negocio_Total_Projeto = Convert.ToDouble(dt.Rows[0]["n_valor_total_obra"])
                });
            }
            return valores;
        }
        

        #endregion

        #region Atualizar Quantidade

        public void AtualizarQuantidade(ListaProjetosDTO DTO)
        {
            try
            {
                var query = "UPDATE lista_vendas SET quantidade='" + DTO.Quantidade + "' WHERE id='" + DTO.Id + "'";
                bd.Conectar();
                bd.ExecutarComandoSQL(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        #endregion

        #region Atualizar FD

        public void AtualizarFD(ListaProjetosDTO DTO)
        {
            try
            {
                var query = "UPDATE lista_vendas SET fd='" + DTO.Fd + "' WHERE id='" + DTO.Id + "'";
                bd.Conectar();
                bd.ExecutarComandoSQL(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        #endregion

        #region Atualizar Valores

        public void AtualizarValores(InformacoesListaProjetosDTO DTO)
        {
            try
            {
                var query = "UPDATE lista_vendas lo JOIN produto p ON lo.PRODUTO_id = p.id JOIN atividade a ON lo.ATIVIDADE_id = a.id JOIN versao_atividade va ON a.VERSAO_ATIVIDADE_id = va.id JOIN vendas ven ON ven.id = lo.vendas_id JOIN negocio n ON ven.NEGOCIO_id = n.id SET lo.preco = CASE WHEN p.icms = '0' AND p.ipi = '0' THEN p.custounitario+(p.custounitario*(1+p.ipi)-(p.custounitario*(1+p.ipi)*p.icms))/(1)*((0))-(p.custounitario*(1+p.ipi)*p.icms)+p.custounitario*p.ipi ELSE p.custounitario+(p.custounitario*(1+p.ipi)-(p.custounitario*(1+p.ipi)*p.icms))/(1-0.18)*((0.18))-(p.custounitario*(1+p.ipi)*p.icms)+p.custounitario*p.ipi END WHERE lo.PRODUTO_id = p.id AND ven.NEGOCIO_id = '" + DTO.Negocio_Id + "' AND n.versao_valida = va.VERSAO_id";
                bd.Conectar();
                bd.ExecutarComandoSQL(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        #endregion

        #region Copiar Itens

        public void CopiarItens(ListaProjetosDTO DTO, InformacoesListaProjetosDTO informacoesDTO, AtividadesProjetoDTO atividadesDTO)
        {
            try
            {
                var query = "INSERT INTO lista_vendas (VENDAS_id, PRODUTO_id, ATIVIDADE_id, preco, anotacoes, USUARIO_id) VALUES ('" + informacoesDTO.Id + "','" + DTO.Produto_Id + "','" + atividadesDTO.Atividade_Id + "', (SELECT CASE WHEN icms = '0' AND ipi = '0' THEN custounitario+(custounitario*(1+ipi)-(custounitario*(1+ipi)*icms))/(1)*((0))-(custounitario*(1+ipi)*icms)+custounitario*ipi ELSE custounitario+(custounitario*(1+ipi)-(custounitario*(1+ipi)*icms))/(1-0.18)*((0.18))-(custounitario*(1+ipi)*icms)+custounitario*ipi END FROM produto WHERE id='" + DTO.Produto_Id + "'), (SELECT descricao from produto WHERE id='" + DTO.Produto_Id + "'), '" + loginDTO.Id + "' )";
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
        public void Excluir(ListaProjetosDTO dTO)
        {
            try
            {
                var query = "DELETE FROM lista_vendas WHERE id = '" + dTO.Id + "'";
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

    class InformacoesListaProjetosBLL : IDisposable
    {
        bool disposed = false;
        AcessoBancoDados bd = new AcessoBancoDados();

        #region Load Informações
        public ObservableCollection<InformacoesListaProjetosDTO> LoadInformacoes(InformacoesListaProjetosDTO DTO)
        {
            var informacoes = new ObservableCollection<InformacoesListaProjetosDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT DISTINCT vnd.id, n.id as negocio_id, n.descricao, c.rsocial, cid.cidade, cid.uf, n.versao_valida FROM gegetdb.negocio n JOIN estabelecimento e ON n.ESTABELECIMENTO_id = e.id JOIN cliente c ON e.CLIENTE_id = c.id JOIN cidades cid ON e.CIDADES_id = cid.id JOIN status_orcamento so ON n.STATUS_ORCAMENTO_id = so.id JOIN vendas vnd ON n.id = vnd.negocio_id JOIN atividade a ON a.NEGOCIO_id = n.id JOIN versao_atividade va ON a.VERSAO_ATIVIDADE_id = va.id WHERE n.id = '" + DTO.Id + "' AND va.VERSAO_id = n.versao_valida";
                bd.Conectar();
                dt = bd.RetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                informacoes.Add(new InformacoesListaProjetosDTO
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
                    Negocio_Id = Convert.ToInt32(dt.Rows[0]["negocio_id"])
                });
            }
            return informacoes;
        }
        #endregion

        #region Load Disciplinas
        private ObservableCollection<DisciplinasProjetoDTO> LoadDisciplinas(InformacoesListaProjetosDTO DTO)
        {
            var disciplinas = new ObservableCollection<DisciplinasProjetoDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT DISTINCT d.descricao, d.id FROM atividade a JOIN negocio n ON a.NEGOCIO_id = n.id JOIN desc_atividades da ON a.DESC_ATIVIDADES_id = da.id JOIN disciplina d ON da.DISCIPLINA_id = d.id JOIN versao_atividade va ON a.VERSAO_ATIVIDADE_id = va.id WHERE a.NEGOCIO_id = '" + DTO.Id + "' AND va.VERSAO_id = n.versao_valida ORDER BY d.descricao";
                bd.Conectar();
                dt = bd.RetDataTable(query);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.ToString());
            }
            finally
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        disciplinas.Add(new DisciplinasProjetoDTO
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
        private ObservableCollection<AtividadesProjetoDTO> LoadAtividades(InformacoesListaProjetosDTO DTO)
        {
            var atividades = new ObservableCollection<AtividadesProjetoDTO>();
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
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        atividades.Add(new AtividadesProjetoDTO
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
        private ObservableCollection<DescricaoAtividadesProjetoDTO> LoadAtividadesDescricao(InformacoesListaProjetosDTO DTO)
        {
            var atividades = new ObservableCollection<DescricaoAtividadesProjetoDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT da.id as descricao_atividade_id, a.descricao, a.id FROM atividade a JOIN negocio n ON a.NEGOCIO_id = n.id JOIN versao_atividade va ON a.VERSAO_ATIVIDADE_id = va.id JOIN desc_atividades da ON a.DESC_ATIVIDADES_id = da.id JOIN disciplina disc ON da.DISCIPLINA_id = disc.id WHERE a.NEGOCIO_id = '" + DTO.Id + "' AND a.habilitado='1' AND va.VERSAO_id = n.versao_valida";
                bd.Conectar();
                dt = bd.RetDataTable(query);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.ToString());
            }
            finally
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        atividades.Add(new DescricaoAtividadesProjetoDTO
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
