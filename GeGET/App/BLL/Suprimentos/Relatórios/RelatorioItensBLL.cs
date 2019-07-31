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
    class RelatorioItensBLL
    {
        AcessoBancoDados bd = new AcessoBancoDados();

        public ObservableCollection<RelatorioItensDTO> ListaItens()
        {
            var itens = new ObservableCollection<RelatorioItensDTO>();
            var dt = new DataTable();
            try
            {
                var query = "select i.id, i.descricao, i.USUARIO_id, i.GRUPO_ITEM_id, gi.descricao as grupo, i.mobra, i.unidade_id, u.descricao as un, i.STATUS_id from item i JOIN grupo_item gi ON gi.id = i.GRUPO_ITEM_id JOIN unidade u ON u.id = i.unidade_id";
                bd.Conectar();
                dt = bd.RetDataTable(query);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                foreach (DataRow dr in dt.Rows)
                {
                    itens.Add(new RelatorioItensDTO
                    {
                        Id = dr["id"].ToString(),
                        Descricao = dr["descricao"].ToString(),
                        Usuario_id = dr["usuario_id"].ToString(),
                        Grupo_id = dr["grupo_item_id"].ToString(),
                        Grupo_descricao = dr["grupo"].ToString(),
                        Unidade_id = dr["unidade_id"].ToString(),
                        Unidade_descricao = dr["un"].ToString(),
                        Mobra = Convert.ToInt32(dr["mobra"]),
                        Status_id = Convert.ToInt32(dr["status_id"])
                    });
                }
            }
            return itens;
        }
    }
}
