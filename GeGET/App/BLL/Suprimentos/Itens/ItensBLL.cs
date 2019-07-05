using System;
using System.Collections.ObjectModel;
using System.Data;
using DAL;
using DTO;

namespace BLL
{
    class ItensBLL : IDisposable
    {
        bool disposed = false;
        AcessoBancoDados bd = new AcessoBancoDados();
        LoginDTO loginDTO = new LoginDTO();

        #region Load Itens
        public ObservableCollection<ItensDTO> LoadItens()
        {
            var itens = new ObservableCollection<ItensDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT i.id, i.descricao, un.descricao as un, i.status_id, s.descricao as status, i.mobra, gi.id as grupo_id, gi.descricao as grupo, i.data, f.nome FROM item i JOIN grupo_item gi ON i.GRUPO_ITEM_id = gi.id JOIN usuario u ON i.USUARIO_id = u.id JOIN funcionario f ON f.id = u.funcionario_id JOIN status s ON s.id = i.status_id JOIN unidade un ON un.id = i.unidade_id";
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
                    itens.Add(new ItensDTO { Id = Convert.ToInt32(dr["id"]), Descricao = dr["descricao"].ToString(), Grupo_Descricao = dr["grupo"].ToString(), Mobra = Convert.ToInt32(dr["mobra"]), Un = dr["un"].ToString(), Status_Id = Convert.ToInt32(dr["status_id"]), Grupo_Id = Convert.ToInt32(dr["grupo_id"]), Data = Convert.ToDateTime(dr["data"]).ToString("dd/MM/yyyy"), Usuario = dr["nome"].ToString(), Status = dr["status"].ToString() });
                }
                bd.CloseConection();
            }
            return itens;
        }
        #endregion

        #region Load Procurar Itens
        public ObservableCollection<ItensDTO> LoadProcurarItens()
        {
            var itens = new ObservableCollection<ItensDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT i.id, i.descricao, un.descricao as un, i.status_id, s.descricao as status, i.mobra, gi.id as grupo_id, gi.descricao as grupo, i.data, f.nome FROM item i JOIN grupo_item gi ON i.GRUPO_ITEM_id = gi.id JOIN usuario u ON i.USUARIO_id = u.id JOIN funcionario f ON f.id = u.funcionario_id JOIN status s ON s.id = i.status_id JOIN unidade un ON un.id = i.unidade_id WHERE i.status_id = '1'";
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
                    itens.Add(new ItensDTO { Id = Convert.ToInt32(dr["id"]), Descricao = dr["descricao"].ToString(), Grupo_Descricao = dr["grupo"].ToString(), Mobra = Convert.ToInt32(dr["mobra"]), Un = dr["un"].ToString(), Status_Id = Convert.ToInt32(dr["status_id"]), Grupo_Id = Convert.ToInt32(dr["grupo_id"]), Data = Convert.ToDateTime(dr["data"]).ToString("dd/MM/yyyy"), Usuario = dr["nome"].ToString(), Status = dr["status"].ToString() });
                }
                bd.CloseConection();
            }
            return itens;
        }
        #endregion

        #region Cadastrar Item

        public bool CadastrarItem(ItensDTO DTO)
        {
            bool sucess = false;
            try
            {
                var query = "INSERT INTO item (descricao, USUARIO_id, GRUPO_ITEM_id, mobra, unidade_id, data) VALUES ('" + DTO.Descricao + "', '" + loginDTO.Id + "', '" + DTO.Grupo_Id + "', '" + DTO.Mobra + "', '" + DTO.Un + "','" + DateTime.Now + "')";
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
