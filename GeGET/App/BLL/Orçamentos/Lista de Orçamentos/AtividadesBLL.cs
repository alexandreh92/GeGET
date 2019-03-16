using System;
using System.Collections.Generic;
using DTO;
using DAL;
using System.Data;

namespace BLL
{
    class AtividadesBLL
    {
        #region Declarations
        AcessoBancoDados bd = new AcessoBancoDados();
        #endregion

        #region Load Cidades
        public List<AtividadeDTO> LoadAtividades(ListaOrcamentosDTO NDTO, DisciplinaDTO DTO)
        {
            var atividades = new List<AtividadeDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT da.descricao as descricao_atividade, a.descricao, a.id FROM atividade a JOIN negocio n ON a.NEGOCIO_id = n.id JOIN versao_atividade va ON a.VERSAO_ATIVIDADE_id = va.id JOIN desc_atividades da ON a.DESC_ATIVIDADES_id = da.id JOIN disciplina disc ON da.DISCIPLINA_id = disc.id WHERE a.NEGOCIO_id = '" + NDTO.Id + "' AND DISCIPLINA_id = '" + DTO.Id + "' AND va.VERSAO_id = n.versao_valida";
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
    }
}
