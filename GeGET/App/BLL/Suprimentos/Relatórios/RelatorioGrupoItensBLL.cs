using System;
using System.Collections.ObjectModel;
using System.Data;
using DTO;
using DAL;

namespace BLL
{
    class RelatorioGrupoItensBLL
    {
        AcessoBancoDados bd = new AcessoBancoDados();

        public ObservableCollection<RelatorioGrupoItensDTO> ListaItens()
        {
            var itens = new ObservableCollection<RelatorioGrupoItensDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT gi.id, gi.descricao, gi.USUARIO_id, f.nome, gi.STATUS_id, st.descricao as status FROM grupo_item gi JOIN usuario u ON u.id = gi.usuario_id JOIN funcionario f ON f.id = u.funcionario_id JOIN status st ON st.id = gi.status_id";
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
                    itens.Add(new RelatorioGrupoItensDTO
                    {
                        Id = dr["id"].ToString(),
                        Descricao = dr["descricao"].ToString(),
                        Usuario_id = dr["usuario_id"].ToString(),
                        Usuario_nome = dr["nome"].ToString(),
                        Status_id = Convert.ToInt32(dr["status_id"]),
                        Status_nome = dr["status"].ToString()
                    });
                }
            }
            return itens;
        }
    }
}
