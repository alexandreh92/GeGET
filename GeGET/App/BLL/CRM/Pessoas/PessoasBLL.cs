using System.Collections.Generic;
using DTO;
using DAL;
using System.Data;
using System;
using System.Collections.ObjectModel;

namespace BLL
{
    class PessoasBLL
    {
        #region Declarations
        AcessoBancoDados bd = new AcessoBancoDados();
        PessoasDTO dto = new PessoasDTO();
        LoginDTO Logindto = new LoginDTO();
        #endregion

        #region Methods

        #region Load Pessoas
        public ObservableCollection<PessoasDTO> LoadPessoas()
        {
            string Procurar = "";
            if (dto.FromParent)
            {
                Procurar = "WHERE c.id = '" + dto.ParentId + "'";
            }
            var pessoas = new ObservableCollection<PessoasDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT cc.id, cc.nome, cc.STATUS_id, cc.email, cc.telefone, cc.celular, cc.data, cc.USUARIO_id, fc.id as id_funcao, fc.descricao as desc_funcao, c.id as cliente_id, c.rsocial, c.fantasia FROM contato_cliente cc join cliente c on cc.CLIENTE_id = c.id join funcoes_contato fc ON cc.funcao_id = fc.id " + Procurar + " ORDER BY cc.nome";
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
                    pessoas.Add(new PessoasDTO { Id = dr["id"].ToString(), Status_Id = dr["status_id"].ToString(), Nome = dr["nome"].ToString(), Email = dr["email"].ToString(), Funcao = dr["desc_funcao"].ToString() + " NA " + dr["rsocial"].ToString(), Telefone = dr["telefone"].ToString(), Celular = dr["celular"].ToString(), Cliente_Id = dr["cliente_id"].ToString(), Funcao_Id = dr["id_funcao"].ToString(), Rsocial = dr["rsocial"].ToString() });
                }
                bd.CloseConection();
            }
            return pessoas;
        }
        #endregion

        #region Create Pessoa

        public bool CreatePessoa(PessoasDTO DTO)
        {
            bool sucess = false;
            try
            {
                var data = DateTime.Now.ToString();
                var query = "INSERT INTO contato_cliente (nome, email, funcao_id, telefone, celular, data, USUARIO_id, CLIENTE_id) VALUES ('" + DTO.Nome + "', '" + DTO.Email + "','" + DTO.Funcao_Id + "', '" + DTO.Telefone + "','" + DTO.Telefone + "','" + data + "', '" + Logindto.Id + "', '" + DTO.Cliente_Id + "')";
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

        #region Load Funcoes
        public List<FuncoesDTO> LoadFuncoes()
        {
            var funcoes = new List<FuncoesDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT id, descricao FROM funcoes_contato ORDER BY descricao ASC";
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
                    funcoes.Add(new FuncoesDTO { Id = Convert.ToInt32(dr["id"]), Descricao = dr["descricao"].ToString() });
                }
                bd.CloseConection();
            }
            return funcoes;
        }
        #endregion

        #region Update Pessoas
        public void UpdatePessoas(PessoasDTO DTO)
        {
            try
            {
                var query = "UPDATE contato_cliente SET nome='" + DTO.Nome + "' , email='" + DTO.Email + "', funcao_id='" + DTO.Funcao_Id + "', CLIENTE_id='" + DTO.Cliente_Id + "' , telefone='" + DTO.Telefone + "', celular='" + DTO.Celular + "', STATUS_id='" + DTO.Status_Id + "'  WHERE id='" + DTO.Id + "'";
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
