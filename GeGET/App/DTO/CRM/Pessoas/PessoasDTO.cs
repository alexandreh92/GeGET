namespace DTO
{
    class PessoasDTO
    {
        private string id;
        private string nome;
        private string email;
        private string funcao;
        private string telefone;
        private string celular;
        private string rsocial;
        private string status_Id;

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
        public string Nome { get => nome; set => nome = value; }
        public string Email { get => email; set => email = value; }
        public string Funcao { get => funcao; set => funcao = value; }
        public string Telefone { get => telefone; set => telefone = value; }
        public string Celular { get => celular; set => celular = value; }
        public string Rsocial { get => rsocial; set => rsocial = value; }
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
        public string Status_Id { get => status_Id; set => status_Id = value; }
    }
}
