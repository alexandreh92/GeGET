using System;
using System.Collections.Generic;
using DTO;
using DAL;
using System.Data;

namespace BLL
{
    class ContatoBLL
    {
        #region Declarations
        AcessoBancoDados bd = new AcessoBancoDados();
        #endregion

        #region Methods

        #region Load Contato Cliente
        public List<ContatoDTO> LoadContato(ContatoDTO DTO)
        {
            var contato = new List<ContatoDTO>();
            var dt = new DataTable();
            try
            {
                var query = "select id, nome from contato_cliente where cliente_id = '"+ DTO.Id +"' ORDER BY nome ASC";
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
                    contato.Add(new ContatoDTO { Id = Convert.ToInt32(item["id"]), Nome = item["nome"].ToString() });
                }
                bd.CloseConection();
            }
            return contato;
        }
        #endregion

        #endregion
    }
}
