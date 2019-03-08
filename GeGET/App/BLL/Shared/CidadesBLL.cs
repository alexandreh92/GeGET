using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;
using System.Data;

namespace BLL
{
    class CidadesBLL
    {
        AcessoBancoDados bd = new AcessoBancoDados();

        public List<CidadesDTO> LoadCidades(string UF_Id)
        {
            var cidades = new List<CidadesDTO>();
            try
            {
                var query = "SELECT id, cidade FROM cidades WHERE estado='"+ UF_Id +"' ORDER BY cidade ASC";
                bd.Conectar();
                var dr = bd.RetDataTable(query);
                foreach (DataRow item in dr.Rows)
                {
                    cidades.Add(new CidadesDTO { Id = Convert.ToInt32(item["id"]), Cidade = item["cidade"].ToString() });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                bd.CloseConection();
            }
            return cidades;
        }
    }
}
