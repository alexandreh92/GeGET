namespace DTO
{
    public class PessoasDTO
    {
        private string id;
        private string nome;
        private string email;
        private string funcao;
        private string telefone;
        private string celular;
        private string rsocial;
        private string status_Id;
        private string cliente_Id;
        private string funcao_Id;
        private string anotacoes;

        private static string pesquisa;
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
        public string Status_Id { get => status_Id; set => status_Id = value; }
        public string Cliente_Id { get => cliente_Id; set => cliente_Id = value; }
        public string Funcao_Id { get => funcao_Id; set => funcao_Id = value; }

        public string Pesquisa { get => pesquisa; set => pesquisa = value; }
        public bool FromParent { get => fromParent; set => fromParent = value; }
        public bool FromChildrenParent { get => fromChildrenParent; set => fromChildrenParent = value; }
        public string ParentId { get => parentId; set => parentId = value; }
        public string Anotacoes { get => anotacoes; set => anotacoes = value; }
    }
}
