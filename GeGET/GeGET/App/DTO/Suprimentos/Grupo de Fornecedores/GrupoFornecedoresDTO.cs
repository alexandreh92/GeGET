namespace DTO
{
    public class GrupoFornecedoresDTO
    {
        private int id;
        private string descricao;
        private string usuario;
        private string data;

        public int Id { get => id; set => id = value; }
        public string Descricao { get => descricao; set => descricao = value; }
        public string Usuario { get => usuario; set => usuario = value; }
        public string Data { get => data; set => data = value; }
    }
}
