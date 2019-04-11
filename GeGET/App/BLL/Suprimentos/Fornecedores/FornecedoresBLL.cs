using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;

namespace BLL
{
    class FornecedoresBLL
    {
        AcessoBancoDados bd = new AcessoBancoDados();
        LoginDTO loginDTO = new LoginDTO();

        #region Load Fornecedores
        public ObservableCollection<FornecedoresDTO> LoadFornecedores()
        {
            var fornecedores = new ObservableCollection<FornecedoresDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT f.id, f.rsocial, f.fantasia, f.grupo_frn_id, f.endereco, cid.estado as id_estado, f.status_id, f.inscricao, f.cnpj, cid.id as id_cidade, cid.cidade, cid.uf, gf.id, gf.descricao, f.tel, f.email FROM fornecedor f JOIN grupo_frn gf ON f.GRUPO_FRN_id = gf.id JOIN cidades cid ON f.cidades_id = cid.id ORDER BY f.rsocial";
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
                    fornecedores.Add(new FornecedoresDTO { Id = Convert.ToInt32(dr["id"]), Razao_Social = dr["rsocial"].ToString(), Nome_Fantasia = dr["fantasia"].ToString(), Grupo_Forn_id = Convert.ToInt32(dr["grupo_frn_id"]), Endereco = dr["endereco"].ToString(), Cnpj = dr["cnpj"].ToString(), Ie = dr["inscricao"].ToString(), CidadeEstado = dr["cidade"].ToString() + " - " + dr["uf"].ToString(), Email = dr["email"].ToString(), Telefone = dr["tel"].ToString(), Grupo_Forn_Descricao = dr["descricao"].ToString(), Cidade_Id = Convert.ToInt32(dr["id_cidade"]), Estado_Id = Convert.ToInt32(dr["id_estado"]) });
                }
                bd.CloseConection();
            }
            return fornecedores;
        }
        #endregion

        #region Cadastrar Fornecedor

        public bool CadastrarFornecedor(FornecedoresDTO DTO)
        {
            bool sucess = false;
            try
            {
                var query = "INSERT INTO fornecedor (rsocial, cnpj, fantasia, endereco, inscricao, tel, data, email, GRUPO_FRN_id, ESTADOS_id, CIDADES_id, USUARIO_id) VALUES ('" + DTO.Razao_Social + "','" + DTO.Cnpj + "','" + DTO.Nome_Fantasia + "','" + DTO.Endereco + "','" + DTO.Ie + "','" + DTO.Telefone + "','" + DateTime.Now + "','" + DTO.Email + "','" + DTO.Grupo_Forn_id + "','" + DTO.Estado_Id + "','" + DTO.Cidade_Id + "', '" + loginDTO.Id + "')";
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

    }
}
