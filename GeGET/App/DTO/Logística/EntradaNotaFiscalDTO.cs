namespace DTO
{
    public class EntradaNotaFiscalDTO
    {
        private int id;
        private string codigo;
        private string codigo_Getac;
        private string descricao;
        private string unidade;
        private double quantidade;
        private double custo;
        private string fabricante;
        private string chave_Acesso;
        private string nota;

        public int Id { get => id; set => id = value; }
        public string Codigo { get => codigo; set => codigo = value; }
        public string Descricao { get => descricao; set => descricao = value; }
        public string Unidade { get => unidade; set => unidade = value; }
        public double Quantidade { get => quantidade; set => quantidade = value; }
        public double Custo { get => custo; set => custo = value; }
        public string Fabricante { get => fabricante; set => fabricante = value; }
        public string Chave_Acesso { get => chave_Acesso; set => chave_Acesso = value; }
        public string Nota { get => nota; set => nota = value; }
        public string Codigo_Getac { get => codigo_Getac; set => codigo_Getac = value; }
    }
}
