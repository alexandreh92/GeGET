using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL;
using DTO;

namespace BLL
{
    public class CadastroItensLoteBLL
    {
        AcessoBancoDados bd = new AcessoBancoDados();
        LoginDTO loginDTO = new LoginDTO();
        public bool CadastroLote(DataTable dt)
        {
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var query = "INSERT INTO item (descricao, usuario_id, grupo_item_id, mobra, unidade_id) VALUES ('"+ dr["descricao"] +"', '"+ loginDTO.Id +"', '"+ dr["grupo_item"].ToString() + "', '"+ dr["mao_obra"].ToString() + "', '"+ dr["un"].ToString() + "') ";
                    bd.Conectar();
                    bd.ExecutarComandoSQL(query);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return true;
        }
    }
}
