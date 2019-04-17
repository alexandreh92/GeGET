using System;
using System.Collections.Generic;
using DTO;
using DAL;
using System.Data;

namespace BLL
{
    class CategoriaClienteBLL
    {
        #region Declarations
        AcessoBancoDados bd = new AcessoBancoDados();
        #endregion

        #region Methods

        #region Load Categoria Cliente
        public List<CategoriaClienteDTO> LoadCategoriaCliente()
        {
            var categorias = new List<CategoriaClienteDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT id, categoria FROM categoria_cliente ORDER BY categoria ASC";
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
                    categorias.Add(new CategoriaClienteDTO { Id = Convert.ToInt32(item["id"]), Descricao = item["categoria"].ToString() });
                }
                bd.CloseConection();
            }
            return categorias;
        }
        #endregion

        #endregion
    }
}
