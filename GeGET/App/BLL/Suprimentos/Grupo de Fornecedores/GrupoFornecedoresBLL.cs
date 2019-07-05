using DAL;
using DTO;
using System.Collections.ObjectModel;
using System.Data;
using System;

namespace BLL
{
    class GrupoFornecedoresBLL : IDisposable
    {
        bool disposed = false;
        AcessoBancoDados bd = new AcessoBancoDados();
        LoginDTO loginDTO = new LoginDTO();

        #region Load Grupo de Fornecedores
        public ObservableCollection<GrupoFornecedoresDTO> LoadGrupoFornecedores()
        {
            var produtos = new ObservableCollection<GrupoFornecedoresDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT gf.id, gf.descricao, gf.data, gf.status_id, f.nome FROM grupo_frn gf JOIN usuario u ON gf.USUARIO_id = u.id JOIN funcionario f ON f.id = u.funcionario_id ORDER BY descricao ASC";
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
                    produtos.Add(new GrupoFornecedoresDTO { Id = Convert.ToInt32(dr["id"]), Descricao = dr["descricao"].ToString(), Usuario = dr["nome"].ToString(), Data = Convert.ToDateTime(dr["data"]).ToString("dd/MM/yyyy") });
                }

            }
            return produtos;
        }
        #endregion

        #region Cadastrar novo Grupo de Fornecedores

        public bool Cadastrar(GrupoFornecedoresDTO DTO)
        {
            bool sucess = false;
            try
            {
                var query = "INSERT INTO grupo_frn (descricao, usuario_id, data) VALUES ('"+DTO.Descricao+ "', '" + loginDTO.Id + "', '" + DateTime.Now + "')";
                bd.Conectar();
                bd.ExecutarComandoSQL(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                sucess = true;
            }
            return sucess;
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                bd.Dispose();
            }
            disposed = true;
        }

        #endregion
    }
}
