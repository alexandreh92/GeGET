namespace DTO
{
    public class RelatorioItensDTO
    {
        private string id;
        private string descricao;
        private string usuario_id;
        private string grupo_id;
        private string grupo_descricao;
        private int mobra;
        private string unidade_id;
        private string unidade_descricao;
        private int status_id;

        public string Id { get => id; set => id = value; }
        public string Descricao { get => descricao; set => descricao = value; }
        public string Usuario_id { get => usuario_id; set => usuario_id = value; }
        public string Grupo_id { get => grupo_id; set => grupo_id = value; }
        public string Grupo_descricao { get => grupo_descricao; set => grupo_descricao = value; }
        public int Mobra { get => mobra; set => mobra = value; }
        public string Unidade_id { get => unidade_id; set => unidade_id = value; }
        public string Unidade_descricao { get => unidade_descricao; set => unidade_descricao = value; }
        public int Status_id { get => status_id; set => status_id = value; }
    }
}
