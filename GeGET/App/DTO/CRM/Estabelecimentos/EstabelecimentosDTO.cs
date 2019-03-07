using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    class EstabelecimentosDTO
    {
        private string id;
        private string razao_Social;
        private string nome_Fantasia;
        private string cnpj;
        private string endereco;
        private string cidade;
        private static string pesquisa;
        private static int status;
        private static int limite;
        private static int inicio;
        private static int final;
        private static int totalRows;
        private static int rowsLeft;
        private static bool fromParent = false;
        private static string parentId;

        public string Id { get => id; set => id = value; }
        public string Razao_Social { get => razao_Social; set => razao_Social = value; }
        public string Nome_Fantasia { get => nome_Fantasia; set => nome_Fantasia = value; }
        public string Cnpj { get => cnpj; set => cnpj = value; }
        public string Endereco { get => endereco; set => endereco = value; }
        public string Cidade { get => cidade; set => cidade = value; }
        public string Pesquisa { get => pesquisa; set => pesquisa = value; }
        public int Status { get => status; set => status = value; }
        public int Limite { get => limite; set => limite = value; }
        public int Inicio { get => inicio; set => inicio = value; }
        public int Final { get => final; set => final = value; }
        public int TotalRows { get => totalRows; set => totalRows = value; }
        public int RowsLeft { get => rowsLeft; set => rowsLeft = value; }
        public bool FromParent { get => fromParent; set => fromParent = value; }
        public string ParentId { get => parentId; set => parentId = value; }
    }
}
