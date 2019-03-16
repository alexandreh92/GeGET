namespace DTO
{
    public class OrcamentosDTO
    {
        private string id;
        private string descricao;
        private string razao_Social;
        private string cidade;
        private string uF;
        private string versao_Valida;
        private string status_Id;

        public string Id { get => id; set => id = value; }
        public string Descricao { get => descricao; set => descricao = value; }
        public string Razao_Social { get => razao_Social; set => razao_Social = value; }
        public string Cidade { get => cidade; set => cidade = value; }
        public string UF { get => uF; set => uF = value; }
        public string Versao_Valida { get => versao_Valida; set => versao_Valida = value; }
        public string Status_Id { get => status_Id; set => status_Id = value; }
    }
}
