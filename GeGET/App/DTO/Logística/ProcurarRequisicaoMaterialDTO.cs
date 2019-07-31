using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ProcurarRequisicaoMaterialDTO
    {
        private int id;
        private int vendas_Id;
        private int negocio_Id;
        private string razao_Social;
        private string descricao;
        private string endereco;
        private string cidadeEstado;
        private string solicitante;

        public int Id { get => id; set => id = value; }
        public int Vendas_Id { get => vendas_Id; set => vendas_Id = value; }
        public int Negocio_Id { get => negocio_Id; set => negocio_Id = value; }
        public string Razao_Social { get => razao_Social; set => razao_Social = value; }
        public string Descricao { get => descricao; set => descricao = value; }
        public string Endereco { get => endereco; set => endereco = value; }
        public string CidadeEstado { get => cidadeEstado; set => cidadeEstado = value; }
        public string Solicitante { get => solicitante; set => solicitante = value; }
    }
}
