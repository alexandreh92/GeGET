using System;
using System.Collections.Generic;
using DTO;
using DAL;
using System.Data;

namespace BLL
{
    class AtividadesBLL : IDisposable
    {
        #region Declarations
        bool disposed = false;
        AcessoBancoDados bd = new AcessoBancoDados();
        #endregion

        #region LoadDescricaoAtividades()
        public List<AtividadeDTO> LoadAtividades(ListaOrcamentosDTO NDTO, DisciplinaDTO DTO)
        {
            var atividades = new List<AtividadeDTO>();
            var dt = new DataTable();
            try
            {
                var query = "select distinct da.id, da.descricao from atividade a JOIN desc_atividades da ON da.id = a.desc_atividades_id join versao_atividade va ON va.id = a.versao_atividade_id join negocio n ON a.negocio_id = n.id and n.versao_valida = va.versao_id where a.negocio_id = '"+NDTO.Id+"' and a.habilitado = 1 and da.disciplina_id = '"+DTO.Id+"'";
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
                    atividades.Add(new AtividadeDTO { Id = Convert.ToInt32(item["id"]), Descricao = item["descricao"].ToString() });
                }
                bd.CloseConection();
            }
            return atividades;
        }
        #endregion

        #region Load Atividades
        public List<AtividadeDTO> LoadAtividadesDescricao(ListaOrcamentosDTO NDTO, DisciplinaDTO DTO)
        {
            var atividades = new List<AtividadeDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT da.descricao as descricao_atividade, a.descricao, a.id FROM atividade a JOIN negocio n ON a.NEGOCIO_id = n.id JOIN versao_atividade va ON a.VERSAO_ATIVIDADE_id = va.id JOIN desc_atividades da ON a.DESC_ATIVIDADES_id = da.id JOIN disciplina disc ON da.DISCIPLINA_id = disc.id WHERE a.NEGOCIO_id = '"+NDTO.Id+"' AND a.habilitado='1' AND a.desc_atividades_id = '"+DTO.Id+"' AND va.VERSAO_id = n.versao_valida";
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
                    atividades.Add(new AtividadeDTO { Id = Convert.ToInt32(item["id"]), Descricao = item["descricao"].ToString(), Descricao_Atividade = item["descricao_atividade"].ToString() });
                }
                bd.CloseConection();
            }
            return atividades;
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
