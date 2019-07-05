using System;
using System.Collections.ObjectModel;
using System.Data;
using DAL;
using DTO;

namespace BLL
{
    class ProcurarVendaBLL : IDisposable
    {
        bool disposed = false;
        AcessoBancoDados bd = new AcessoBancoDados();

        public ObservableCollection<ProcurarVendaDTO> LoadVendas()
        {
            var vendas = new ObservableCollection<ProcurarVendaDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT vend.id, n.id as negocio_id, n.descricao, c.rsocial, c.fantasia, e.endereco, cid.cidade, e.CLIENTE_id, cid.uf FROM negocio n JOIN estabelecimento e ON n.ESTABELECIMENTO_id = e.id JOIN cliente c ON e.CLIENTE_id = c.id JOIN vendas vend ON vend.NEGOCIO_id = n.id JOIN cidades cid ON e.CIDADES_id = cid.id";
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
                    vendas.Add(new ProcurarVendaDTO { Negocio_Id = Convert.ToInt32(dr["negocio_id"]), Razao_Social = dr["rsocial"].ToString(), Cidade_Estado = dr["cidade"].ToString() + " - " + dr["uf"].ToString(), Descricao = dr["descricao"].ToString(), Cliente_Id = Convert.ToInt32(dr["cliente_id"]), Endereco = dr["endereco"].ToString(), Id = Convert.ToInt32(dr["id"]), Nome_Fantasia = dr["fantasia"].ToString(), Negocio_Numero = "p" + Convert.ToInt32(dr["negocio_id"]).ToString("0000") });
                }
            }
            return vendas;
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
