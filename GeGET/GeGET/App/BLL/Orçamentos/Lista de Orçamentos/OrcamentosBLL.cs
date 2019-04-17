using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;
using System.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace BLL
{
    class OrcamentosBLL
    {
        #region Declarations
        AcessoBancoDados bd = new AcessoBancoDados();
        DataSet ds = new DataSet();
        #endregion

        #region Methods


        #region Load Clientes
        public List<OrcamentosDTO> LoadTextBoxes(ListaOrcamentosDTO DTO)
        {
            var orcamento = new List<OrcamentosDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT DISTINCT n.descricao, c.rsocial, cid.cidade, cid.uf, n.versao_valida, n.STATUS_ORCAMENTO_id FROM negocio n JOIN estabelecimento e ON n.ESTABELECIMENTO_id = e.id JOIN cliente c ON e.CLIENTE_id = c.id JOIN cidades cid ON e.CIDADES_id = cid.id JOIN status_orcamento so ON n.STATUS_ORCAMENTO_id = so.id JOIN atividade a ON a.NEGOCIO_id = n.id JOIN versao_atividade va ON a.VERSAO_ATIVIDADE_id = va.id  WHERE n.id = '"+ DTO.Id +"' AND va.VERSAO_id = n.versao_valida";
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
                    orcamento.Add(new OrcamentosDTO { Descricao = dr["descricao"].ToString(), Razao_Social = dr["rsocial"].ToString(), Cidade = dr["cidade"].ToString(), UF = dr["uf"].ToString(), Versao_Valida = dr["versao_valida"].ToString(), Status_Id = dr["STATUS_ORCAMENTO_id"].ToString() });
                }
                bd.CloseConection();
            }
            return orcamento;
        }

        #endregion
        #endregion
    }
}
