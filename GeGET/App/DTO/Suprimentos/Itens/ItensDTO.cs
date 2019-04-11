namespace DTO
{
    public class ItensDTO
    {
        private int id;
        private int status_Id;
        private int grupo_Id;
        private string descricao;
        private string un;
        private int mobra;
        private string grupo_Descricao;
        private string data;
        private string usuario;
        private string status;

        public int Id { get => id; set => id = value; }
        public int Status_Id { get => status_Id; set => status_Id = value; }
        public int Grupo_Id { get => grupo_Id; set => grupo_Id = value; }
        public string Descricao { get => descricao; set => descricao = value; }
        public string Un { get => un; set => un = value; }
        public int Mobra { get => mobra; set => mobra = value; }
        public string Grupo_Descricao { get => grupo_Descricao; set => grupo_Descricao = value; }
        public string Data { get => data; set => data = value; }
        public string Status { get => status; set => status = value; }
        public string Usuario { get => usuario; set => usuario = value; }
    }
}
