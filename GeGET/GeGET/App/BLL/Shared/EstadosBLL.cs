using System;
using System.Collections.Generic;
using DTO;
using DAL;
using System.Data;

namespace BLL
{
    class EstadosBLL
    {
        #region Declarations
        AcessoBancoDados bd = new AcessoBancoDados();
        #endregion

        #region Load Estados
        public List<EstadoDTO> LoadEstados()
        {
            var estados = new List<EstadoDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT id, uf FROM estados ORDER BY uf ASC";
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
                    estados.Add(new EstadoDTO { Id = Convert.ToInt32(item["id"]), Uf = item["uf"].ToString() });
                }
                bd.CloseConection();
            }
            return estados;
        }
        #endregion
    }
}
