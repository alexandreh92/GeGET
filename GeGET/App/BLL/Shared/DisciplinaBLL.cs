using System;
using System.Collections.Generic;
using DTO;
using DAL;
using System.Data;

namespace BLL
{
    class DisciplinaBLL
    {
        #region Declarations
        AcessoBancoDados bd = new AcessoBancoDados();
        #endregion

        #region Load Cidades
        public List<DisciplinaDTO> LoadDisciplinas(ListaOrcamentosDTO DTO)
        {
            var disciplinas = new List<DisciplinaDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT DISTINCT d.descricao, d.id FROM atividade a JOIN negocio n ON a.NEGOCIO_id = n.id JOIN desc_atividades da ON a.DESC_ATIVIDADES_id = da.id JOIN disciplina d ON da.DISCIPLINA_id = d.id JOIN versao_atividade va ON a.VERSAO_ATIVIDADE_id = va.id WHERE a.NEGOCIO_id = '" + DTO.Id + "' AND va.VERSAO_id = n.versao_valida ORDER BY d.descricao";
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
                    disciplinas.Add(new DisciplinaDTO { Id = Convert.ToInt32(item["id"]), Descricao = item["descricao"].ToString() });
                }
                bd.CloseConection();
            }
            return disciplinas;
        }
        #endregion
    }
}
