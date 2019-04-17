namespace DTO
{
    public class VersaoOrcamentoDTO
    {
        private int id;
        private string descricao;
        private int versao_Atividade_Id;
        private string atividade_Copiar;

        public int Id { get => id; set => id = value; }
        public string Descricao { get => descricao; set => descricao = value; }
        public int Versao_Atividade_Id { get => versao_Atividade_Id; set => versao_Atividade_Id = value; }
        public string Atividade_Copiar { get => atividade_Copiar; set => atividade_Copiar = value; }
    }
}
