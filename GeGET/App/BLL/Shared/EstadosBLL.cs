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
    class EstadosBLL
    {
        AcessoBancoDados bd = new AcessoBancoDados();

        public List<EstadoDTO> LoadEstados()
        {
            var estados = new List<EstadoDTO>();
            try
            {
                var query = "SELECT id, uf FROM estados ORDER BY uf ASC";
                bd.Conectar();
                var dr = bd.RetDataTable(query);
                foreach (DataRow item in dr.Rows)
                {
                    estados.Add(new EstadoDTO { Id = Convert.ToInt32(item["id"]), Uf = item["uf"].ToString() });
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
            return estados;
        }
    }
}
