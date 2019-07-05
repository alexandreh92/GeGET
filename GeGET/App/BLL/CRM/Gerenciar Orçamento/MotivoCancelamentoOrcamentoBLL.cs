using System;
using System.Collections.ObjectModel;
using System.Data;
using DAL;
using DTO;

namespace BLL
{
    class MotivoCancelamentoOrcamentoBLL : IDisposable
    {
        bool disposed = false;
        AcessoBancoDados bd = new AcessoBancoDados();

        public ObservableCollection<MotivoCancelamentoOrcamentoDTO> LoadMotivos()
        {
            ObservableCollection<MotivoCancelamentoOrcamentoDTO> motivos = new ObservableCollection<MotivoCancelamentoOrcamentoDTO>();
            DataTable dt = new DataTable();
            try
            {
                var query = "SELECT id, descricao FROM motivo_cancelamento";
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
                    motivos.Add(new MotivoCancelamentoOrcamentoDTO { Id = Convert.ToInt32(dr["id"]), Descricao = dr["descricao"].ToString() });
                }
            }
            return motivos;
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
