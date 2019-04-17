using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;
using System.Collections.ObjectModel;
using System.Data;

namespace BLL
{
    class UnidadeBLL
    {
        AcessoBancoDados bd = new AcessoBancoDados();

        #region Load Unidades

        public ObservableCollection<UnidadeDTO> LoadUnidades()
        {
            var unidade = new ObservableCollection<UnidadeDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT id, descricao FROM unidade";
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
                    unidade.Add(new UnidadeDTO { Id = Convert.ToInt32(dr["id"]), Descricao = dr["descricao"].ToString() });
                }
            }
            return unidade;
        }

        #endregion

    }
}
