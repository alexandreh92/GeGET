using System;
using System.Collections.ObjectModel;
using DTO;
using DAL;
using System.Data;

namespace BLL
{
    class ProcurarRequisicaoMaterialBLL : IDisposable
    {
        bool disposed = false;
        AcessoBancoDados bd = new AcessoBancoDados();

        public ObservableCollection<ProcurarRequisicaoMaterialDTO> LoadRM()
        {
            var rm = new ObservableCollection<ProcurarRequisicaoMaterialDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT t.* FROM (SELECT T1.id,T1.vendas_id,T1.negocio_id,T1.rsocial,T1.descricao,T1.versao_valida,T1.cidade,T1.uf,T1.nome,T1.endereco,T1.produto_id,SUM(t1.solicitado) - COALESCE(t3.quantidade,0) AS solicitado,COALESCE(T2.estoque,0) AS estoque FROM (SELECT rm.id,rm.vendas_id,n.id AS negocio_id,c.rsocial,n.descricao,n.versao_valida,cid.cidade,cid.uf,f.nome,e.endereco,mr.produto_id,mr.quantidade AS solicitado FROM materiais_requeridos mr JOIN requisicao_material rm ON rm.id=mr.requisicao_material_id JOIN produto p ON p.id=mr.produto_id JOIN vendas v ON v.id=rm.vendas_id JOIN negocio n ON n.id=v.negocio_id JOIN estabelecimento e ON n.estabelecimento_id=e.id JOIN cliente c ON c.id=e.cliente_id JOIN usuario u ON u.id=rm.usuario_id JOIN funcionario f ON f.id=u.funcionario_id JOIN cidades cid ON cid.id=e.cidades_id JOIN estados est ON est.id=e.estados_id) AS T1 LEFT OUTER JOIN (SELECT produto_id,SUM(quantidade) AS estoque FROM estoque WHERE quantidade>0 GROUP BY produto_id) AS T2 ON T1.produto_id=T2.produto_id LEFT OUTER JOIN (SELECT se.rm_id,produto_id,SUM(quantidade) AS quantidade,rm.id FROM saida_estoque se JOIN requisicao_material rm ON se.rm_id=rm.id GROUP BY produto_id,rm_id) T3 ON T1.id=T3.rm_id AND T1.produto_id=T3.produto_id GROUP BY produto_id,rm_id) AS t WHERE t.solicitado>0 GROUP BY id";
                bd.Conectar();
                dt = bd.RetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                foreach (DataRow dr in dt.Rows)
                {
                    rm.Add(new ProcurarRequisicaoMaterialDTO
                    {
                        Id = Convert.ToInt32(dr["id"]),
                        Negocio_Id = Convert.ToInt32(dr["negocio_id"]),
                        Vendas_Id = Convert.ToInt32(dr["vendas_id"]),
                        Razao_Social = dr["rsocial"].ToString(),
                        Descricao = dr["descricao"].ToString(),
                        Endereco = dr["endereco"].ToString(),
                        CidadeEstado = dr["cidade"].ToString() + " - " + dr["uf"].ToString(),
                        Solicitante = dr["nome"].ToString()
                    });
                }
            }
            return rm;
        }

        public ObservableCollection<ProcurarRequisicaoMaterialDTO> LoadRMConsulta()
        {
            var rm = new ObservableCollection<ProcurarRequisicaoMaterialDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT rm.id,n.id AS negocio_id,c.rsocial,n.descricao,n.versao_valida,cid.cidade,cid.uf,f.nome,e.endereco,vendas_id FROM requisicao_material rm JOIN vendas v ON v.id=rm.vendas_id JOIN negocio n ON n.id=v.negocio_id JOIN estabelecimento e ON n.estabelecimento_id=e.id JOIN cliente c ON c.id=e.cliente_id JOIN usuario u ON u.id=rm.usuario_id JOIN funcionario f ON f.id=u.funcionario_id JOIN cidades cid ON cid.id=e.cidades_id JOIN estados est ON est.id=e.estados_id ORDER BY id ASC";
                bd.Conectar();
                dt = bd.RetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                foreach (DataRow dr in dt.Rows)
                {
                    rm.Add(new ProcurarRequisicaoMaterialDTO
                    {
                        Id = Convert.ToInt32(dr["id"]),
                        Negocio_Id = Convert.ToInt32(dr["negocio_id"]),
                        Vendas_Id = Convert.ToInt32(dr["vendas_id"]),
                        Razao_Social = dr["rsocial"].ToString(),
                        Descricao = dr["descricao"].ToString(),
                        Endereco = dr["endereco"].ToString(),
                        CidadeEstado = dr["cidade"].ToString() + " - " + dr["uf"].ToString(),
                        Solicitante = dr["nome"].ToString()
                    });
                }
            }
            return rm;
        }

        public ObservableCollection<ProcurarRequisicaoMaterialDTO> LoadRMEstorno()
        {
            var rm = new ObservableCollection<ProcurarRequisicaoMaterialDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT t.* FROM (SELECT T1.id, T1.vendas_id, T1.negocio_id, T1.rsocial, T1.descricao, T1.versao_valida, T1.cidade, T1.uf, T1.nome, T1.endereco, T1.produto_id, sum(t1.solicitado) - coalesce(t3.quantidade, 0) as solicitado, coalesce(T2.estoque, 0) as estoque FROM (SELECT rm.id, rm.vendas_id, n.id as negocio_id, c.rsocial, n.descricao, n.versao_valida, cid.cidade, cid.uf, f.nome, e.endereco, mr.produto_id, mr.quantidade as solicitado FROM materiais_requeridos mr JOIN requisicao_material rm ON rm.id = mr.requisicao_material_id JOIN produto p ON p.id = mr.produto_id JOIN vendas v ON v.id = rm.vendas_id JOIN negocio n ON n.id = v.negocio_id JOIN estabelecimento e ON n.estabelecimento_id = e.id JOIN cliente c ON c.id = e.cliente_id JOIN usuario u ON u.id = rm.usuario_id JOIN funcionario f ON f.id = u.funcionario_id JOIN cidades cid ON cid.id = e.cidades_id JOIN estados est ON est.id = e.estados_id) as T1 LEFT OUTER JOIN (select produto_id, sum(quantidade) as estoque from estoque WHERE quantidade > 0 GROUP BY produto_id) as T2 ON T1.produto_id = T2.produto_id LEFT OUTER JOIN(select produto_id, sum(quantidade) as quantidade, rm.vendas_id from saida_estoque se JOIN requisicao_material rm ON se.rm_id = rm.id GROUP BY produto_id, vendas_id) T3 ON T1.vendas_id = T3.vendas_id and T1.produto_id = T3.produto_id group by produto_id) as t group by id";
                bd.Conectar();
                dt = bd.RetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                foreach (DataRow dr in dt.Rows)
                {
                    rm.Add(new ProcurarRequisicaoMaterialDTO
                    {
                        Id = Convert.ToInt32(dr["id"]),
                        Negocio_Id = Convert.ToInt32(dr["negocio_id"]),
                        Vendas_Id = Convert.ToInt32(dr["vendas_id"]),
                        Razao_Social = dr["rsocial"].ToString(),
                        Descricao = dr["descricao"].ToString(),
                        Endereco = dr["endereco"].ToString(),
                        CidadeEstado = dr["cidade"].ToString() + " - " + dr["uf"].ToString(),
                        Solicitante = dr["nome"].ToString()
                    });
                }
            }
            return rm;
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
