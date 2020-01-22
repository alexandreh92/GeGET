using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Data;
using DTO;
using DAL;
using System.Windows;
using GeGET;

namespace BLL
{
    class GerarRequisicaoMaterialBLL : IDisposable
    {
        bool disposed = false;
        AcessoBancoDados bd = new AcessoBancoDados();
        LoginDTO loginDTO = new LoginDTO();


        #region LoadLista
        public ObservableCollection<GerarRequisicaoMaterialDTO> LoadLista(InformacoesGerarRequisicaoMaterialDTO DTO)
        {
            var itens = new ObservableCollection<GerarRequisicaoMaterialDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT t1.produto_id, t1.descricao, t1.partnumber, t1.un, t1.quantidade-coalesce(t2.quantidade,0) as saldo, t1.rsocial as fabricante FROM (SELECT lv.produto_id, i.descricao, p.partnumber, un.descricao as un, SUM(quantidade) as quantidade, f.rsocial FROM lista_vendas lv JOIN produto p ON p.id = lv.produto_id JOIN item i ON i.id = p.descricao_item_id JOIN unidade un ON un.id = i.unidade_id JOIN fornecedor f ON f.id = p.fornecedor_id WHERE lv.vendas_id = '" + DTO.Id + "' AND lv.fd = 0 AND i.mobra = 0 AND i.id != 1440 GROUP BY produto_id) as t1 LEFT OUTER JOIN (SELECT produto_id, SUM(quantidade) as quantidade from materiais_requeridos mr JOIN requisicao_material rm ON rm.id = mr.requisicao_material_id WHERE rm.vendas_id = '" + DTO.Id+"' GROUP BY PRODUTO_ID) as t2 ON t1.produto_id = t2.produto_id WHERE t1.quantidade - coalesce(t2.quantidade, 0) > 0 GROUP BY t1.produto_id";
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
                    itens.Add(new GerarRequisicaoMaterialDTO
                    {
                        Produto_Id = dr["produto_id"].ToString(),
                        Descricao = dr["descricao"].ToString(),
                        Partnumber = dr["partnumber"].ToString(),
                        Unidade = dr["un"].ToString(),
                        Saldo = Convert.ToDouble(dr["saldo"]),
                        Quantidade = Convert.ToDouble(dr["saldo"]),
                        Fabricante = dr["fabricante"].ToString(),
                    });
                }
            }
            return itens;
        }
        #endregion

        #region Gerar RM
        public string GerarRM(InformacoesGerarRequisicaoMaterialDTO DTO, ObservableCollection<GerarRequisicaoMaterialDTO> listaMateriais)
        {
            string rm;
            var dtRM = new DataTable();
            InformacoesGerarRequisicaoMaterialDTO informacoesDTO = new InformacoesGerarRequisicaoMaterialDTO();
            ObservableCollection<GerarRequisicaoMaterialDTO> listaSistema;
            informacoesDTO.Id = DTO.Id;
            bool isChanged = false;

            listaSistema = LoadLista(informacoesDTO);


            foreach (var item in listaMateriais)
            {
                foreach (var dto in listaSistema)
                {
                    if (item.Produto_Id == dto.Produto_Id)
                    {
                        if (item.Saldo != dto.Saldo)
                        {
                            isChanged = true;
                        }
                    }
                }
            }


            if (!isChanged)
            {
                try
                {
                    string substringMateriais = "";
                    foreach (var item in listaMateriais)
                    {
                        substringMateriais = substringMateriais + "((select id from requisicao_material where vendas_id = '" + DTO.Id + "' ORDER BY id desc LIMIT 1), '" + item.Produto_Id + "', '" + item.Quantidade + "'),";
                    }
                    string queryMateriais = "INSERT INTO materiais_requeridos (requisicao_material_id, produto_id, quantidade) VALUES " + substringMateriais.TrimEnd(',');

                    var queryRM = "INSERT INTO requisicao_material (vendas_id, usuario_id, description) VALUES ('" + DTO.Id + "', '" + loginDTO.Id + "', '"+ DTO.RM_Descricao +"');" + queryMateriais;
                    bd.Conectar();
                    bd.ExecutarComandoSQL(queryRM);

                    var querySelectRM = "select id from requisicao_material where vendas_id = '" + DTO.Id + "' ORDER BY id desc LIMIT 1";
                    bd.Conectar();
                    dtRM = bd.RetDataTable(querySelectRM);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
                finally
                {
                    rm = dtRM.Rows[0]["id"].ToString();
                }
            }
            else
            {
                rm = null;
                CustomOKMessageBox.Show("A requisição não foi gerada.\nAlguns itens foram alterados, a tela será recarregada.","Atenção!",Window.GetWindow(Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is Layout)));
            }

            //verifica com foreach em cada elemento do observablecollection e compara com o saldo da query
            //SELECT t1.produto_id, t1.descricao, t1.partnumber, t1.un, t1.quantidade-coalesce(t2.quantidade,0) as saldo, t1.rsocial as fabricante FROM (SELECT lv.produto_id, i.descricao, p.partnumber, un.descricao as un, SUM(quantidade) as quantidade, f.rsocial FROM lista_vendas lv JOIN produto p ON p.id = lv.produto_id JOIN item i ON i.id = p.descricao_item_id JOIN unidade un ON un.id = i.unidade_id JOIN fornecedor f ON f.id = p.fornecedor_id WHERE lv.vendas_id = '15' AND lv.fd = 0 AND i.mobra = 0 AND i.id != 1440 GROUP BY produto_id) as t1 LEFT OUTER JOIN (SELECT produto_id, SUM(quantidade) as quantidade from materiais_requeridos mr JOIN requisicao_material rm ON rm.id = mr.requisicao_material_id WHERE rm.vendas_id = '15' GROUP BY PRODUTO_ID) as t2 ON t1.produto_id = t2.produto_id WHERE t1.quantidade - coalesce(t2.quantidade, 0) > 0 AND t1.produto_id = '209' GROUP BY t1.produto_id
            //se algum variou, cai fora, se não executa o código

            
            return rm;
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

    class InformacoesGerarRequisicaoMaterialBLL : IDisposable
    {
        bool disposed = false;
        AcessoBancoDados bd = new AcessoBancoDados();
        public ObservableCollection<InformacoesGerarRequisicaoMaterialDTO> LoadInformacoes(InformacoesGerarRequisicaoMaterialDTO DTO)
        {
            var informacoes = new ObservableCollection<InformacoesGerarRequisicaoMaterialDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT DISTINCT vnd.id, n.id as negocio_id, n.descricao, c.rsocial, cid.cidade, cid.uf, n.versao_valida FROM gegetdb.negocio n JOIN estabelecimento e ON n.ESTABELECIMENTO_id = e.id JOIN cliente c ON e.CLIENTE_id = c.id JOIN cidades cid ON e.CIDADES_id = cid.id JOIN status_orcamento so ON n.STATUS_ORCAMENTO_id = so.id JOIN vendas vnd ON n.id = vnd.negocio_id JOIN atividade a ON a.NEGOCIO_id = n.id JOIN versao_atividade va ON a.VERSAO_ATIVIDADE_id = va.id WHERE n.id = '" + DTO.Negocio_Id + "' AND va.VERSAO_id = n.versao_valida";
                bd.Conectar();
                dt = bd.RetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                informacoes.Add(new InformacoesGerarRequisicaoMaterialDTO
                {
                    Id = Convert.ToInt32(dt.Rows[0]["id"]),
                    Descricao = dt.Rows[0]["descricao"].ToString(),
                    Razao_Social = dt.Rows[0]["rsocial"].ToString(),
                    Cidade = dt.Rows[0]["cidade"].ToString(),
                    Uf = dt.Rows[0]["uf"].ToString(),
                    Versao = dt.Rows[0]["versao_valida"].ToString(),
                    Negocio_Id = Convert.ToInt32(dt.Rows[0]["negocio_id"])
                });
            }
            return informacoes;
        }

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
