using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class FunildeVendasDTO
    {
        private string id;
        private string negocio;
        private string descricao;
        private string prazo;
        private string cidade;
        private string fantasia;
        private string descricao_Status;
        private int status_Orcamento;

        public string Id { get => id; set => id = value; }
        public string Negocio { get => negocio; set => negocio = value; }
        public string Descricao { get => descricao; set => descricao = value; }
        public string Prazo { get => prazo; set => prazo = value; }
        public string Cidade { get => cidade; set => cidade = value; }
        public string Fantasia { get => fantasia; set => fantasia = value; }
        public string Descricao_Status { get => descricao_Status; set => descricao_Status = value; }
        public int Status_Orcamento { get => status_Orcamento; set => status_Orcamento = value; }
        
    }
}
