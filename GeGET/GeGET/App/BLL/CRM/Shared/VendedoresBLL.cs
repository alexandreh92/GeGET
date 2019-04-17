using System;
using System.Collections.Generic;
using DTO;
using DAL;
using System.Data;

namespace BLL
{
    class VendedoresBLL
    {
        #region Declarations
        AcessoBancoDados bd = new AcessoBancoDados();
        #endregion

        #region Methods

        #region Load Categoria Cliente
        public List<VendedoresDTO> LoadVendedores()
        {
            var vendedor = new List<VendedoresDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT id, nome FROM gegetdb.vendedor ORDER BY nome ASC";
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
                    vendedor.Add(new VendedoresDTO { Id = Convert.ToInt32(item["id"]), Nome = item["nome"].ToString() });
                }
                bd.CloseConection();
            }
            return vendedor;
        }
        #endregion

        #endregion
    }
}
