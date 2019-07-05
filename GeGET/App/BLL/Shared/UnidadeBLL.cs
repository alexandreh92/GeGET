using System;
using DTO;
using DAL;
using System.Collections.ObjectModel;
using System.Data;

namespace BLL
{
    class UnidadeBLL : IDisposable
    {
        bool disposed = false;
        AcessoBancoDados bd = new AcessoBancoDados();

        #region Load Unidades

        public ObservableCollection<UnidadeDTO> LoadUnidades()
        {
            var unidade = new ObservableCollection<UnidadeDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT id, descricao FROM unidade";
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
                    unidade.Add(new UnidadeDTO { Id = Convert.ToInt32(dr["id"]), Descricao = dr["descricao"].ToString() });
                }
            }
            return unidade;
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
