namespace DTO
{
    class NegociosDTO
    {
        private string id;
        private string numero;
        private string razao_Social;
        private string endereco;
        private string anotacoes;
        private string status_Descricao;
        private string descricao;
        private string vendedor;
        private int status_Id;
        private static string pesquisa;
        private static int status;
        private static int limite;
        private static int inicio;
        private static int final;
        private static int totalRows;
        private static int rowsLeft;
        private static bool fromParent = false;
        private static bool fromChildrenParent = false;
        private static string parentId;

        public string Id { get => id; set => id = value; }
        public string Numero { get => numero; set => numero = value; }
        public string Razao_Social { get => razao_Social; set => razao_Social = value; }
        public string Endereco { get => endereco; set => endereco = value; }
        public string Anotacoes { get => anotacoes; set => anotacoes = value; }
        public string Status_Descricao { get => status_Descricao; set => status_Descricao = value; }
        public string Descricao { get => descricao; set => descricao = value; }
        public string Vendedor { get => vendedor; set => vendedor = value; }
        public int Status_Id { get => status_Id; set => status_Id = value; }
        public string Pesquisa { get => pesquisa; set => pesquisa = value; }
        public int Status { get => status; set => status = value; }
        public int Limite { get => limite; set => limite = value; }
        public int Inicio { get => inicio; set => inicio = value; }
        public int Final { get => final; set => final = value; }
        public int TotalRows { get => totalRows; set => totalRows = value; }
        public int RowsLeft { get => rowsLeft; set => rowsLeft = value; }
        public bool FromParent { get => fromParent; set => fromParent = value; }
        public bool FromChildrenParent { get => fromChildrenParent; set => fromChildrenParent = value; }
        public string ParentId { get => parentId; set => parentId = value; }
        
    }
}
