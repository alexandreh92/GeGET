using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;
using System.Collections.ObjectModel;
using System.Data;

namespace GeGET
{
    class DescricaoAtividadesBLL
    {
        AcessoBancoDados bd = new AcessoBancoDados();

        public ObservableCollection<DescricaoAtividadesDTO> LoadAtividades(DescricaoAtividadesDTO DTO)
        {
            var atividades = new ObservableCollection<DescricaoAtividadesDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT id, descricao, disciplina_id FROM desc_atividades WHERE disciplina_id = '"+ DTO.Disciplina_Id +"'";
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
                    atividades.Add(new DescricaoAtividadesDTO { Id = Convert.ToInt32(dr["id"]), Descricao = dr["descricao"].ToString(), Disciplina_Id = Convert.ToInt32(dr["disciplina_id"]) });
                }
            }
            return atividades;
        }
    }
}
