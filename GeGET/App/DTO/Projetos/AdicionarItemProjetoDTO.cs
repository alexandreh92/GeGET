namespace DTO
{
    public class AdicionarItemProjetoDTO
    {
        private int id;
        private string descricao;
        private string anotacoes;
        private string unidade;
        private string partnumber;
        private double custo;
        private string fabricante;
        private string negocio_Id;
        private string atividade_Id;
        private string codigo;

        public int Id { get => id; set => id = value; }
        public string Descricao { get => descricao; set => descricao = value; }
        public string Anotacoes { get => anotacoes; set => anotacoes = value; }
        public string Unidade { get => unidade; set => unidade = value; }
        public string Partnumber { get => partnumber; set => partnumber = value; }
        public double Custo { get => custo; set => custo = value; }
        public string Fabricante { get => fabricante; set => fabricante = value; }
        public string Negocio_Id { get => negocio_Id; set => negocio_Id = value; }
        public string Atividade_Id { get => atividade_Id; set => atividade_Id = value; }
        public string Codigo { get => codigo; set => codigo = value; }
    }
}
