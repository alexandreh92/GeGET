using System;
using System.Collections.Generic;
using DTO;
using DAL;
using System.Data;

namespace BLL
{
    class VersaoBLL : IDisposable
    {
        #region Declarations
        bool disposed = false;
        AcessoBancoDados bd = new AcessoBancoDados();
        #endregion

        #region Load Versões
        public List<VersaoDTO> LoadVersao(ListaOrcamentosDTO DTO)
        {
            var versoes = new List<VersaoDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT id, versao_id, descricao FROM versao_atividade WHERE negocio_id = '"+DTO.Id+"'";
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
                    versoes.Add(new VersaoDTO { Id = Convert.ToInt32(item["id"]), Descricao = item["descricao"].ToString(), Num_Versao = Convert.ToInt32(item["versao_id"]) });
                }
                bd.CloseConection();
            }
            return versoes;
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
