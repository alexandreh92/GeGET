namespace DTO
{
    public class ClientesDTO
    {
        private string id;
        private string razao_Social;
        private string nome_Fantasia;
        private string categoria;
        private int categoria_Id;
        private static string pesquisa;
        private int status;
        private string status_descricao;
        private static bool fromParent = false;

        public string Id { get => id; set => id = value; }
        public string Razao_Social { get => razao_Social; set => razao_Social = value; }
        public string Nome_Fantasia { get => nome_Fantasia; set => nome_Fantasia = value; }
        public string Categoria { get => categoria; set => categoria = value; }
        public int Categoria_Id { get => categoria_Id; set => categoria_Id = value; }
        public string Pesquisa { get => pesquisa; set => pesquisa = value; }
        public int Status { get => status; set => status = value; }
        public bool FromParent { get => fromParent; set => fromParent = value; }
        public string Status_descricao { get => status_descricao; set => status_descricao = value; }
    }
}
