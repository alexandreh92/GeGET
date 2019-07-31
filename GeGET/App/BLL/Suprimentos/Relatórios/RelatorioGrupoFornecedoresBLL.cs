using System;
using System.Collections.ObjectModel;
using System.Data;
using DTO;
using DAL;

namespace BLL
{
    class RelatorioGrupoFornecedoresBLL
    {
        AcessoBancoDados bd = new AcessoBancoDados();

        public ObservableCollection<RelatorioGrupoFornecedoresDTO> ListaFornecedores()
        {
            var fornecedores = new ObservableCollection<RelatorioGrupoFornecedoresDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT gf.id, gf.descricao, gf.USUARIO_id, f.nome, gf.STATUS_id, st.descricao as status FROM grupo_frn gf JOIN usuario u ON u.id = gf.usuario_id JOIN funcionario f ON f.id = u.funcionario_id JOIN status st ON st.id = gf.status_id";
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
                    fornecedores.Add(new RelatorioGrupoFornecedoresDTO
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
            return fornecedores;
        }
    }
}
