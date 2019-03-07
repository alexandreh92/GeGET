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
    class CategoriaClienteBLL
    {
        AcessoBancoDados bd = new AcessoBancoDados();

        public List<CategoriaClienteDTO> LoadCategoriaCliente()
        {
            var categorias = new List<CategoriaClienteDTO>();
            try
            {
                var query = "SELECT id, categoria FROM categoria_cliente ORDER BY categoria ASC";
                bd.Conectar();
                var dr = bd.RetDataTable(query);
                foreach (DataRow item in dr.Rows)
                {
                    categorias.Add(new CategoriaClienteDTO { Id = Convert.ToInt32(item["id"]), Descricao = item["categoria"].ToString() });
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
            return categorias;
        }
    }
}
