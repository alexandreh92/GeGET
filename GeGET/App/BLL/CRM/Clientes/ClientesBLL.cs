using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;
using System.Data;
using System.Windows;

namespace BLL
{
    class ClientesBLL
    {
        AcessoBancoDados bd = new AcessoBancoDados();
        ClientesDTO dto = new ClientesDTO();
        
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
            var cliente = dto.Pesquisa;
            var query = "SELECT c.id, c.status_id, c.rsocial, c.fantasia, c.data, c.CATEGORIA_CLIENTE_id, cat.categoria FROM cliente c JOIN categoria_cliente cat ON c.CATEGORIA_CLIENTE_id = cat.id WHERE "+Procurar+" ORDER BY c.fantasia";
            bd.Conectar();
            var dtt = bd.RetDataTable(query);
            foreach (DataRow dr in dtt.Rows)
            {
                clientes.Add(new ClientesDTO {Id = dr["id"].ToString(), Razao_Social = dr["rsocial"].ToString(), Nome_Fantasia = dr["fantasia"].ToString(), Categoria = dr["categoria"].ToString(), Status = Convert.ToInt32(dr["status_id"]), Categoria_Id = Convert.ToInt32(dr["categoria_cliente_id"])});
            }

            return clientes;
        }

        public void UpdateClientes(int Id, string Razao_Social, string Nome_Fantasia, int Categoria_Id, int Status_Id)
        {
            try
            {
                var query = "UPDATE cliente SET rsocial='"+ Razao_Social +"', fantasia='"+ Nome_Fantasia +"', categoria_cliente_id='"+ Categoria_Id +"', status_id='"+ Status_Id +"' WHERE id='"+ Id +"'";
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


    }
}
