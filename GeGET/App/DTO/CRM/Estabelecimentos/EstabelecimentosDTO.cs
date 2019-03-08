namespace DTO
{
    class EstabelecimentosDTO
    {
        private string id;
        private string cliente_Id;
        private string razao_Social;
        private string nome_Fantasia;
        private string cnpj;
        private string endereco;
        private string cidade;
        private string cidade_Id;
        private string uF_Id;
        private string ie;
        private string telefone;
        private static string pesquisa;
        private int status;
        private static bool fromParent = false;
        private static string parentId;

        public string Id { get => id; set => id = value; }
        public string Cliente_Id { get => cliente_Id; set => cliente_Id = value; }
        public string Razao_Social { get => razao_Social; set => razao_Social = value; }
        public string Nome_Fantasia { get => nome_Fantasia; set => nome_Fantasia = value; }
        public string Cnpj { get => cnpj; set => cnpj = value; }
        public string Endereco { get => endereco; set => endereco = value; }
        public string Cidade { get => cidade; set => cidade = value; }
        public string Cidade_Id { get => cidade_Id; set => cidade_Id = value; }
        public string UF_Id { get => uF_Id; set => uF_Id = value; }
        public string Ie { get => ie; set => ie = value; }
        public string Telefone { get => telefone; set => telefone = value; }
        public string Pesquisa { get => pesquisa; set => pesquisa = value; }
        public int Status { get => status; set => status = value; }
        public bool FromParent { get => fromParent; set => fromParent = value; }
        public string ParentId { get => parentId; set => parentId = value; }
        
    }
}
