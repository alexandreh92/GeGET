using System;
using DTO;
using DAL;
using System.Data;
using System.Collections.ObjectModel;

namespace BLL
{
    class FunildeVendasBLL : IDisposable
    {
        bool disposed = false;
        AcessoBancoDados bd = new AcessoBancoDados();
        readonly FunildeVendasDTO dto = new FunildeVendasDTO();

        public ObservableCollection<FunildeVendasDTO> LoadNegocios()
        {
            var negocios = new ObservableCollection<FunildeVendasDTO>();
            try
            {
                var query = "SELECT n.id, so.descricao as status_orcamento, n.status_orcamento_id, n.descricao, n.prazo, cid.uf, cid.cidade, c.fantasia FROM negocio n JOIN estabelecimento e ON e.id = n.estabelecimento_id JOIN cliente c ON c.id = e.cliente_id JOIN cidades cid ON cid.id = e.cidades_id JOIN status_orcamento so ON so.id = n.status_orcamento_id ORDER BY n.status_orcamento_id, n.id";
                bd.Conectar();
                var dtt = bd.RetDataTable(query);
                foreach (DataRow dr in dtt.Rows)
                {
                    negocios.Add(new FunildeVendasDTO { Id = dr["id"].ToString(), Negocio = "p" + Convert.ToInt32(dr["id"]).ToString("0000") , Descricao = dr["descricao"].ToString(), Prazo = Convert.ToDateTime(dr["prazo"]).ToString("dd/MM/yyyy"), Cidade = dr["cidade"].ToString() + " - " + dr["uf"].ToString(), Fantasia = dr["fantasia"].ToString(), Descricao_Status = dr["status_orcamento"].ToString(), Status_Orcamento = Convert.ToInt32(dr["status_orcamento_id"]) });
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                bd.CloseConection();
            }
            
            return negocios;
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
