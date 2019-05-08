namespace DTO
{
    public class LinkarProdutoNotaFiscalDTO
    {
        private int id;
        private string descricao;
        private string anotacoes;
        private string unidade;
        private string partnumber;
        private string fabricante;
        private string codigo;
        private string codigo_Barra;

        public int Id { get => id; set => id = value; }
        public string Descricao { get => descricao; set => descricao = value; }
        public string Anotacoes { get => anotacoes; set => anotacoes = value; }
        public string Unidade { get => unidade; set => unidade = value; }
        public string Partnumber { get => partnumber; set => partnumber = value; }
        public string Fabricante { get => fabricante; set => fabricante = value; }
        public string Codigo { get => codigo; set => codigo = value; }
        public string Codigo_Barra { get => codigo_Barra; set => codigo_Barra = value; }
    }
}
