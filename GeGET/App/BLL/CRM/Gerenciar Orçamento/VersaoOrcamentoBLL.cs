using System;
using System.Collections.ObjectModel;
using System.Data;
using DAL;
using DTO;

namespace BLL
{
    class VersaoOrcamentoBLL : IDisposable
    {
        bool disposed = false;
        AcessoBancoDados bd = new AcessoBancoDados();

        public ObservableCollection<VersaoOrcamentoDTO> LoadVersao(NegociosDTO DTO)
        {
            ObservableCollection<VersaoOrcamentoDTO> versao = new ObservableCollection<VersaoOrcamentoDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT VERSAO_id as id, descricao FROM versao_atividade WHERE NEGOCIO_id ='" + DTO.Id + "' AND versao_id != '"+DTO.Versao_Id+"' AND locked='0'";
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
                    versao.Add(new VersaoOrcamentoDTO { Id = Convert.ToInt32(dr["id"]), Descricao = dr["descricao"].ToString() });
                }
            }
            return versao;
        }

        public ObservableCollection<VersaoOrcamentoDTO> LoadVersaoAll(NegociosDTO DTO)
        {
            ObservableCollection<VersaoOrcamentoDTO> versao = new ObservableCollection<VersaoOrcamentoDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT VERSAO_id as id, id as versao_atividade_id, descricao FROM versao_atividade WHERE NEGOCIO_id ='" + DTO.Id + "' AND versao_id != (select max(versao_id) from versao_atividade where negocio_id = '" + DTO.Id+"')";
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
                    versao.Add(new VersaoOrcamentoDTO { Id = Convert.ToInt32(dr["id"]), Descricao = dr["descricao"].ToString(), Versao_Atividade_Id = Convert.ToInt32(dr["versao_atividade_id"]) });
                }
            }
            return versao;
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
