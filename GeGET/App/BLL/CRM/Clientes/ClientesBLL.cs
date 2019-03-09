using System;
using System.Collections.Generic;
using DTO;
using DAL;
using System.Data;

namespace BLL
{
    class ClientesBLL
    {
        #region Declarations
        AcessoBancoDados bd = new AcessoBancoDados();
        LoginDTO Logindto = new LoginDTO();
        ClientesDTO dto = new ClientesDTO();
        #endregion

        #region Methods

        #region Load Clientes
        public List<ClientesDTO> LoadClientes()
        {
            int i = 1;
            string Procurar = "";
            string[] ListaClientes = dto.Pesquisa.Split(null);
            foreach (string pesq in ListaClientes)
            {
                if (i == 1)
                {
                    Procurar = Procurar + "CONCAT(rsocial, fantasia, categoria) LIKE '%" + pesq + "%'";
                }
                else
                {
                    Procurar = Procurar + " AND CONCAT(rsocial, fantasia, categoria) LIKE '%" + pesq + "%'";
                }
                i++;
            }
            var clientes = new List<ClientesDTO>();
            var dt = new DataTable();
            try
            {
                var cliente = dto.Pesquisa;
                var query = "SELECT c.id, c.status_id, c.rsocial, c.fantasia, c.data, c.CATEGORIA_CLIENTE_id, cat.categoria FROM cliente c JOIN categoria_cliente cat ON c.CATEGORIA_CLIENTE_id = cat.id WHERE " + Procurar + " ORDER BY c.fantasia";
                bd.Conectar();
                dt = bd.RetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                foreach (DataRow dr in dt.Rows)
                {
                    clientes.Add(new ClientesDTO { Id = dr["id"].ToString(), Razao_Social = dr["rsocial"].ToString(), Nome_Fantasia = dr["fantasia"].ToString(), Categoria = dr["categoria"].ToString(), Status = Convert.ToInt32(dr["status_id"]), Categoria_Id = Convert.ToInt32(dr["categoria_cliente_id"]) });
                }
                bd.CloseConection();
            }
            return clientes;
        }
        #endregion

        #region Create Clientes

        public bool CreateCliente(ClientesDTO DTO)
        {
            bool sucess = false;
            try
            {
                var data = DateTime.Now.ToString();
                var query = "INSERT INTO cliente (rsocial, fantasia, data, USUARIO_id, CATEGORIA_CLIENTE_id) VALUES('" + DTO.Razao_Social + "', '" + DTO.Nome_Fantasia + "', '" + data + "', '" + Logindto.Id + "' , '" + DTO.Categoria_Id + "')";
                bd.Conectar();
                bd.ExecutarComandoSQL(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                sucess = true;
                bd.CloseConection();
            }
            if (sucess)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Update Clientes

        public void UpdateClientes(ClientesDTO DTO)
        {
            try
            {
                var query = "UPDATE cliente SET rsocial='" + DTO.Razao_Social + "', fantasia='" + DTO.Nome_Fantasia + "', categoria_cliente_id='" + DTO.Categoria_Id + "', status_id='" + DTO.Status + "' WHERE id='" + DTO.Id + "'";
                bd.Conectar();
                bd.ExecutarComandoSQL(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                bd.CloseConection();
            }
        }

        #endregion

        #endregion
    }
}
