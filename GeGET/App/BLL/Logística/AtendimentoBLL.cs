using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;
using System.Data;

namespace BLL
{
    class AtendimentoBLL : IDisposable
    {
        bool disposed = false;
        AcessoBancoDados bd = new AcessoBancoDados();
        LoginDTO loginDTO = new LoginDTO();
        
        public ObservableCollection<AtendimentoDTO> LoadItens(InformacoesAtendimentoDTO DTO)
        {
            var itens = new ObservableCollection<AtendimentoDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT t.* FROM (SELECT T1.id,T1.vendas_id,T1.fabricante,T1.produto_id,T1.descricao,T1.partnumber,T1.un,T1.anotacoes,SUM(t1.solicitado) - COALESCE(t3.quantidade,0) AS solicitado,COALESCE(T2.estoque,0) AS estoque FROM (SELECT mr.id,rm.id AS rm_id,rm.vendas_id,f.rsocial AS fabricante,mr.produto_id,i.descricao,p.partnumber,un.descricao AS un,p.descricao AS anotacoes,mr.quantidade AS solicitado FROM materiais_requeridos mr JOIN requisicao_material rm ON rm.id=mr.requisicao_material_id JOIN produto p ON p.id=mr.produto_id JOIN item i ON i.id=p.descricao_item_id JOIN fornecedor f ON f.id=p.fornecedor_id JOIN unidade un ON un.id=i.unidade_id) AS T1 LEFT OUTER JOIN (SELECT produto_id,SUM(quantidade) AS estoque FROM estoque WHERE quantidade>0 GROUP BY produto_id) AS T2 ON T1.produto_id=T2.produto_id LEFT OUTER JOIN (SELECT rm.id as rm_id,produto_id,SUM(quantidade) AS quantidade,rm.vendas_id FROM saida_estoque se JOIN requisicao_material rm ON se.rm_id=rm.id GROUP BY produto_id,rm_id) T3 ON T1.rm_id=T3.rm_id AND T1.produto_id=T3.produto_id WHERE T1.rm_id='"+ DTO.Id +"' GROUP BY produto_id) AS t WHERE t.solicitado>0";
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
                    itens.Add(new AtendimentoDTO
                    {
                        Id = Convert.ToInt32(dr["id"]),
                        Descricao = dr["descricao"].ToString(),
                        Produto_Id = dr["produto_id"].ToString(),
                        Fabricante = dr["fabricante"].ToString(),
                        Codigo = Convert.ToInt32(dr["produto_id"]).ToString("000000"),
                        Partnumber = dr["partnumber"].ToString(),
                        Unidade = dr["un"].ToString(), 
                        Anotacoes = dr["anotacoes"].ToString(),
                        Solicitado = Convert.ToDouble(dr["solicitado"]),
                        Estoque = Convert.ToDouble(dr["estoque"]),
                        Atendido = CheckAtendido()
                    });
                }
            }
            return itens;
        }

        public void AtenderProdutos(InformacoesAtendimentoDTO informacoesDTO, ObservableCollection<AtendimentoDTO> listaDTO)
        {
            foreach (var dto in listaDTO)
            {
                try
                {
                    var query = "call gegetdb.SP_SaidaProdutos('"+dto.Produto_Id+"', '"+dto.Quantidade+"', '"+loginDTO.Id+"', '"+informacoesDTO.Id+"');";
                    bd.Conectar();
                    bd.ExecutarComandoSQL(query);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
            }
        }

        private bool CheckAtendido()
        {
            bool isTrue = false;
            if (true)
            {

            }
            return isTrue;
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

    class InformacoesAtendimentoBLL : IDisposable
    {
        bool disposed = false;
        AcessoBancoDados bd = new AcessoBancoDados();

        public ObservableCollection<InformacoesAtendimentoDTO> LoadInformacoes(InformacoesAtendimentoDTO DTO)
        {
            var informacoes = new ObservableCollection<InformacoesAtendimentoDTO>();
            var dt = new DataTable();
            try
            {
                var query = "select rm.id, n.id as negocio_id, v.id as vendas_id, c.rsocial, n.descricao, n.versao_valida, cid.cidade, est.uf, f.nome, e.endereco from requisicao_material rm JOIN vendas v ON v.id = rm.vendas_id JOIN negocio n ON n.id = v.negocio_id JOIN usuario u ON u.id = rm.usuario_id JOIN funcionario f ON f.id = u.funcionario_id JOIN estabelecimento e ON e.id = n.estabelecimento_id JOIN cliente c ON c.id = e.cliente_id JOIN cidades cid ON cid.id = e.cidades_id JOIN estados est ON est.id = e.estados_id WHERE rm.id = '"+DTO.Id+"'";
                bd.Conectar();
                dt = bd.RetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                informacoes.Add(new InformacoesAtendimentoDTO
                {
                    Id = Convert.ToInt32(dt.Rows[0]["id"]),
                    Vendas_Id = Convert.ToInt32(dt.Rows[0]["vendas_id"]),
                    Negocio_Id = Convert.ToInt32(dt.Rows[0]["negocio_id"]),
                    Razao_Social = dt.Rows[0]["rsocial"].ToString(),
                    Cidade = dt.Rows[0]["cidade"].ToString(),
                    Uf = dt.Rows[0]["uf"].ToString(),
                    Descricao = dt.Rows[0]["descricao"].ToString(),
                    Versao = dt.Rows[0]["versao_valida"].ToString(),
                    Solicitante = dt.Rows[0]["nome"].ToString(),
                    Endereco = dt.Rows[0]["endereco"].ToString()
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
