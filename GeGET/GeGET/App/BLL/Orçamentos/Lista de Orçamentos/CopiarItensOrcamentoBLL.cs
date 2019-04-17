using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;
using System.Data;
using System.Windows;

namespace GeGET
{
    class CopiarItensOrcamentoBLL
    {
        AcessoBancoDados bd = new AcessoBancoDados();

        public ObservableCollection<DisciplinaDTO> LoadDisciplinas(CopiarItensOrcamentoDTO dTO)
        {
            var disciplinas = new ObservableCollection<DisciplinaDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT DISTINCT d.descricao, d.id FROM atividade a JOIN negocio n ON a.NEGOCIO_id = n.id JOIN desc_atividades da ON a.DESC_ATIVIDADES_id = da.id JOIN disciplina d ON da.DISCIPLINA_id = d.id JOIN versao_atividade va ON a.VERSAO_ATIVIDADE_id = va.id WHERE a.NEGOCIO_id = '" + dTO.Negocio_Id + "' AND va.VERSAO_id = n.versao_valida ORDER BY d.descricao";
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
                    disciplinas.Add(new DisciplinaDTO { Id = Convert.ToInt32(dr["id"]), Descricao = dr["descricao"].ToString() });
                }
                bd.CloseConection();
            }
            return disciplinas;
        }

        public ObservableCollection<AtividadeDTO> LoadAtividades(CopiarItensOrcamentoDTO dTO)
        {
            var atividades = new ObservableCollection<AtividadeDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT a.descricao, a.id, da.descricao as descricao_atividade FROM atividade a JOIN negocio n ON a.NEGOCIO_id = n.id JOIN versao_atividade va ON a.VERSAO_ATIVIDADE_id = va.id JOIN desc_atividades da ON a.DESC_ATIVIDADES_id = da.id JOIN disciplina disc ON da.DISCIPLINA_id = disc.id WHERE a.NEGOCIO_id = '" + dTO.Negocio_Id + "' AND DISCIPLINA_id = '" + dTO.Disciplina_Id + "' AND va.VERSAO_id = n.versao_valida";
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
                    atividades.Add(new AtividadeDTO { Id = Convert.ToInt32(dr["id"]), Descricao = dr["descricao"].ToString(), Descricao_Atividade = dr["descricao_atividade"].ToString() });
                }
                bd.CloseConection();
            }
            return atividades;
        }

        public void Insert(CopiarItensOrcamentoDTO dTO)
        {
            try
            {
                var query = "INSERT INTO lista_orcamento (NEGOCIO_id, PRODUTO_id, ATIVIDADES_id, preco_orc, descricao_orc) VALUES ('" + dTO.Negocio_Id + "','" + dTO.Id + "','" + dTO.Atividade_Id + "', (SELECT CASE WHEN icms = '0' AND ipi = '0' THEN custounitario+(custounitario*(1+ipi)-(custounitario*(1+ipi)*icms))/(1)*((0))-(custounitario*(1+ipi)*icms)+custounitario*ipi ELSE custounitario+(custounitario*(1+ipi)-(custounitario*(1+ipi)*icms))/(1-0.18)*((0.18))-(custounitario*(1+ipi)*icms)+custounitario*ipi END FROM produto WHERE id='" + dTO.Id + "'), (SELECT descricao from produto WHERE id='" + dTO.Id + "'))";
                bd.Conectar();
                bd.ExecutarComandoSQL(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

    }
}
