namespace DTO
{
    public class RelatorioGrupoFornecedoresDTO
    {
        private string id;
        private string descricao;
        private string usuario_id;
        private int status_id;
        private string usuario_nome;
        private string status_nome;

        public string Id { get => id; set => id = value; }
        public string Descricao { get => descricao; set => descricao = value; }
        public string Usuario_id { get => usuario_id; set => usuario_id = value; }
        public int Status_id { get => status_id; set => status_id = value; }
        public string Usuario_nome { get => usuario_nome; set => usuario_nome = value; }
        public string Status_nome { get => status_nome; set => status_nome = value; }
    }
}
