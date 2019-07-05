using System;
using System.Collections.Generic;
using DTO;
using DAL;
using System.Data;

namespace BLL
{
    class CidadesBLL : IDisposable
    {
        #region Declarations
        bool disposed = false;
        AcessoBancoDados bd = new AcessoBancoDados();
        #endregion

        #region Load Cidades
        public List<CidadesDTO> LoadCidades(EstadoDTO DTO)
        {
            var cidades = new List<CidadesDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT id, cidade FROM cidades WHERE estado='" + DTO.Id + "' ORDER BY cidade ASC";
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
                    cidades.Add(new CidadesDTO { Id = Convert.ToInt32(item["id"]), Cidade = item["cidade"].ToString() });
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
