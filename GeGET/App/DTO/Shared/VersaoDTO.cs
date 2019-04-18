namespace DTO
{
    class VersaoDTO
    {
        private int id;
        private int num_Versao;
        private string descricao;

        public int Id { get => id; set => id = value; }
        public int Num_Versao { get => num_Versao; set => num_Versao = value; }
        public string Descricao { get => descricao; set => descricao = value; }
    }
}
