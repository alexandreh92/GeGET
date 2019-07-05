using System;
using System.Collections.Generic;
using DTO;
using DAL;
using System.Data;

namespace BLL
{
    class PrioridadeBLL : IDisposable
    {
        #region Declarations
        bool disposed = false;
        AcessoBancoDados bd = new AcessoBancoDados();
        #endregion

        #region Load Cidades
        public List<PrioridadeDTO> LoadPrioridades()
        {
            var cidades = new List<PrioridadeDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT id, descricao FROM prioridade ORDER BY descricao ASC";
                bd.Conectar();
                dt = bd.RetDataTable(query);
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                foreach (DataRow item in dt.Rows)
                {
                    cidades.Add(new PrioridadeDTO { Id = Convert.ToInt32(item["id"]), Descricao = item["descricao"].ToString() });
                }
                bd.CloseConection();
            }
            return cidades;
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
