using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class FornecedoresDTO
    {
        private int id;
        private int grupo_Forn_id;
        private int estado_Id;
        private int cidade_Id;
        private string razao_Social;
        private string nome_Fantasia;
        private string endereco;
        private string cidadeEstado;
        private string ie;
        private string cnpj;
        private string grupo_Forn_Descricao;
        private string telefone;
        private string email;

        public int Id { get => id; set => id = value; }
        public int Grupo_Forn_id { get => grupo_Forn_id; set => grupo_Forn_id = value; }
        public int Estado_Id { get => estado_Id; set => estado_Id = value; }
        public int Cidade_Id { get => cidade_Id; set => cidade_Id = value; }
        public string Razao_Social { get => razao_Social; set => razao_Social = value; }
        public string Nome_Fantasia { get => nome_Fantasia; set => nome_Fantasia = value; }
        public string Endereco { get => endereco; set => endereco = value; }
        public string CidadeEstado { get => cidadeEstado; set => cidadeEstado = value; }
        public string Ie { get => ie; set => ie = value; }
        public string Cnpj { get => cnpj; set => cnpj = value; }
        public string Grupo_Forn_Descricao { get => grupo_Forn_Descricao; set => grupo_Forn_Descricao = value; }
        public string Telefone { get => telefone; set => telefone = value; }
        public string Email { get => email; set => email = value; }
    }
}
