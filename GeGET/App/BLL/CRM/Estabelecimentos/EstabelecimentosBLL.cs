using System;
using System.Collections.Generic;
using DTO;
using DAL;
using System.Data;
using System.Collections.ObjectModel;

namespace BLL
{
    class EstabelecimentosBLL
    {
        #region Declarations
        AcessoBancoDados bd = new AcessoBancoDados();
        EstabelecimentosDTO dto = new EstabelecimentosDTO();
        LoginDTO Logindto = new LoginDTO();
        #endregion

        #region Methods

        #region Load Estabelecimentos
        public ObservableCollection<EstabelecimentosDTO> LoadEstabelecimentos()
        {
            string Procurar = "";
            if (dto.FromParent)
            {
                Procurar = "WHERE c.id = '" + dto.ParentId + "'";
            }
            var estabelecimentos = new ObservableCollection<EstabelecimentosDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT e.id, e.cnpj, e.endereco, e.descricao, e.inscricao, e.status_id, cid.estado as id_estado, c.id as cliente_id, cid.id as id_cidade, e.CLIENTE_id, e.telefone, c.rsocial, c.fantasia, c.data, cid.uf, cid.cidade FROM estabelecimento e join cliente c on e.CLIENTE_id = c.id join cidades cid on e.CIDADES_id = cid.id " + Procurar + " ORDER BY c.fantasia, cid.uf, cid.cidade ASC";
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
                    estabelecimentos.Add(new EstabelecimentosDTO { Id = dr["id"].ToString(), Razao_Social = dr["rsocial"].ToString(), Nome_Fantasia = dr["fantasia"].ToString(), Cnpj = dr["cnpj"].ToString(), Status = Convert.ToInt32(dr["status_id"]), Endereco = dr["endereco"].ToString(), Cidade = dr["cidade"].ToString() + " - " + dr["uf"].ToString(), Cliente_Id = dr["cliente_id"].ToString(), Cidade_Id = dr["id_cidade"].ToString(), Ie = dr["inscricao"].ToString(), Telefone = dr["telefone"].ToString(), UF_Id = dr["id_estado"].ToString(), Descricao = dr["descricao"].ToString() });
                }
                bd.CloseConection();
            }
            return estabelecimentos;
        }
        #endregion

        #region Load Estabelecimentos
        public ObservableCollection<EstabelecimentosDTO> LoadEstabelecimentosFromClient(EstabelecimentosDTO DTO)
        {
            var estabelecimentos = new ObservableCollection<EstabelecimentosDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT e.id, e.cnpj, e.endereco, e.descricao, e.inscricao, e.status_id, cid.estado as id_estado, c.id as cliente_id, cid.id as id_cidade, e.CLIENTE_id, e.telefone, c.rsocial, c.fantasia, c.data, cid.uf, cid.cidade FROM estabelecimento e join cliente c on e.CLIENTE_id = c.id join cidades cid on e.CIDADES_id = cid.id WHERE c.id = '"+ DTO.Cliente_Id +"' ORDER BY c.fantasia, cid.uf, cid.cidade ASC";
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
                    estabelecimentos.Add(new EstabelecimentosDTO { Id = dr["id"].ToString(), Razao_Social = dr["rsocial"].ToString(), Nome_Fantasia = dr["fantasia"].ToString(), Cnpj = dr["cnpj"].ToString(), Status = Convert.ToInt32(dr["status_id"]), Endereco = dr["endereco"].ToString(), Cidade = dr["cidade"].ToString(), UF = dr["uf"].ToString(), CidadeEstado = dr["cidade"].ToString() + " - " + dr["uf"].ToString(),  Cliente_Id = dr["cliente_id"].ToString(), Cidade_Id = dr["id_cidade"].ToString(), Ie = dr["inscricao"].ToString(), Telefone = dr["telefone"].ToString(), UF_Id = dr["id_estado"].ToString(), Descricao = dr["descricao"].ToString() });
                }
                bd.CloseConection();
            }
            return estabelecimentos;
        }
        #endregion

        #region Create Estabelecimentos

        public bool CreateEstabelecimento(EstabelecimentosDTO DTO)
        {
            bool sucess = false;
            try
            {
                var data = DateTime.Now.ToString();
                var query = "INSERT INTO estabelecimento (USUARIO_id, cnpj, endereco, inscricao, telefone, data, ESTADOS_id, CIDADES_id, CLIENTE_id, descricao) VALUES('" + Logindto.Id + "','" + DTO.Cnpj + "','" + DTO.Endereco + "','" + DTO.Ie + "','" + DTO.Telefone + "','" + data + "','" + DTO.UF_Id + "','" + DTO.Cidade_Id + "', '" + DTO.Cliente_Id + "', '"+DTO.Descricao+"')";
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

        #region EditarEstabelecimento
        public void EditarEstabelecimento(EstabelecimentosDTO DTO)
        {
            try
            {
                var query = "UPDATE estabelecimento SET cnpj='" + DTO.Cnpj + "', endereco='" + DTO.Endereco + "', inscricao='" + DTO.Ie + "', cliente_id='" + DTO.Cliente_Id + "', telefone='" + DTO.Telefone + "', ESTADOS_id='" + DTO.UF_Id + "', CIDADES_id='" + DTO.Cidade_Id + "', status_id='" + DTO.Status + "', descricao='"+DTO.Descricao+"' WHERE id='" + DTO.Id + "'";
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
