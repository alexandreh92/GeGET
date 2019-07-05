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
    class CadastroAtividadeBLL : IDisposable
    {
        bool disposed = false;
        AcessoBancoDados bd = new AcessoBancoDados();
        LoginDTO loginDTO = new LoginDTO();

        #region LoadOrcamento
        public ObservableCollection<CadastroAtividadesDTO> LoadOrcamento(CadastroAtividadesDTO DTO)
        {
            ObservableCollection<CadastroAtividadesDTO> orcamentos = new ObservableCollection<CadastroAtividadesDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT n.id, n.descricao, n.anotacoes, cc.nome as responsavel, c.rsocial, e.cnpj, n.versao_valida, n.prazo, n.valor_fechamento, va.id as versao_atividade_id, va.locked, n.data, v.nome as vendedor, s.descricao as status_descricao, s.id as status_id, e.cnpj, e.endereco, cid.cidade, cid.uf FROM negocio n JOIN vendedor v ON n.VENDEDOR_id = v.id JOIN status_orcamento s ON n.STATUS_ORCAMENTO_id = s.id JOIN estabelecimento e ON n.ESTABELECIMENTO_id = e.id JOIN cidades cid ON e.CIDADES_id = cid.id JOIN contato_cliente cc ON n.contato_cliente_id = cc.id JOIN cliente c ON e.cliente_id = c.id join versao_atividade va ON va.negocio_id = n.id and va.versao_id = n.versao_valida WHERE n.id = '" + DTO.Id + "' ORDER BY n.id";
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
                    orcamentos.Add(new CadastroAtividadesDTO { Id = Convert.ToInt32(dr["id"]), Numero = "p" + Convert.ToInt32(dr["id"]).ToString("0000"), Razao_Social = dr["rsocial"].ToString(), Descricao = dr["descricao"].ToString(), Endereco = dr["endereco"].ToString() + " - " + dr["cidade"].ToString() + " - " + dr["uf"].ToString(), Vendedor = dr["vendedor"].ToString(), Status_Descricao = dr["status_descricao"].ToString(), CidadeEstado = dr["cidade"].ToString() + " - " + dr["uf"].ToString(), Cnpj = dr["cnpj"].ToString(), Contato_Nome = dr["responsavel"].ToString(), Versao_Id = Convert.ToInt32(dr["versao_valida"]), Versao_Locked = Convert.ToInt32(dr["locked"]), Versao_Atividade_Id = dr["versao_atividade_id"].ToString() });
                }
                bd.CloseConection();
            }
            return orcamentos;
        }
        #endregion

        #region Load Atividades Cadastradas
        public ObservableCollection<AtividadeCadastradaDTO> LoadAtividadesCadastradas(CadastroAtividadesDTO DTO, DescricaoAtividadesDTO descricaoAtividadesDTO)
        {
            ObservableCollection<AtividadeCadastradaDTO> atvidades = new ObservableCollection<AtividadeCadastradaDTO>();
            var dt = new DataTable();
            try
            {
                var query = "SELECT (@cnt := @cnt + 1) AS Num, t.* FROM (SELECT a.id, da.descricao as atividade, da.id as atividade_id, a.descricao, a.habilitado, d.descricao as descricao_disciplina FROM atividade a JOIN negocio n ON a.NEGOCIO_id = n.id JOIN desc_atividades da ON a.DESC_ATIVIDADES_id = da.id JOIN versao_atividade va ON a.VERSAO_ATIVIDADE_id = va.id JOIN disciplina d ON da.DISCIPLINA_id = d.id WHERE n.id = '" + DTO.Id + "' AND d.id = '" + descricaoAtividadesDTO.Disciplina_Id + "' AND va.VERSAO_id = n.versao_valida) t CROSS JOIN(SELECT @cnt:= 0) AS dummy";
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
                    atvidades.Add(new AtividadeCadastradaDTO { Id = Convert.ToInt32(dr["id"]), Atividade = dr["atividade"].ToString(), Descricao = dr["descricao"].ToString(), Habilitado = Convert.ToBoolean(dr["habilitado"]), Numero = Convert.ToInt32(dr["num"]).ToString("00"), Atividade_id = dr["atividade_id"].ToString() });
                }
                bd.CloseConection();
            }
            return atvidades;
        }
        #endregion

        #region Cadastrar Atividade

        public void CadastrarAtividade(CadastroAtividadesDTO DTO, AtividadeCadastradaDTO atividadeCadastradaDTO)
        {
            try
            {
                var query = "INSERT INTO atividade (descricao, NEGOCIO_id, DESC_ATIVIDADES_id, VERSAO_ATIVIDADE_id, USUARIO_id) VALUES ('" + atividadeCadastradaDTO.Descricao + "', '" + DTO.Id + "' , '" + atividadeCadastradaDTO.Atividade_id + "', '" + DTO.Versao_Atividade_Id + "', '" + loginDTO.Id + "' )";
                bd.Conectar();
                bd.ExecutarComandoSQL(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        #endregion

        #region Update Status

        public void Update(AtividadeCadastradaDTO DTO)
        {
            try
            {
                var query = "UPDATE atividade SET habilitado = '"+ Convert.ToInt32(DTO.Habilitado)+"' WHERE id = '"+DTO.Id+"'";
                bd.Conectar();
                bd.ExecutarComandoSQL(query);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.ToString());
            }
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
