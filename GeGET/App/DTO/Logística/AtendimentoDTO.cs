namespace DTO
{
    public class AtendimentoDTO
    {
        private int id;
        private string produto_Id;
        private string codigo;
        private string descricao;
        private string unidade;
        private string anotacoes;
        private string partnumber;
        private double solicitado;
        private double quantidade;
        private double estoque;
        private string fabricante;
        private bool atendido;

        public int Id { get => id; set => id = value; }
        public string Codigo { get => codigo; set => codigo = value; }
        public string Descricao { get => descricao; set => descricao = value; }
        public string Unidade { get => unidade; set => unidade = value; }
        public string Partnumber { get => partnumber; set => partnumber = value; }
        public double Solicitado { get => solicitado; set => solicitado = value; }
        public double Quantidade { get => quantidade; set => quantidade = value; }
        public double Estoque { get => estoque; set => estoque = value; }
        public bool Atendido { get => atendido; set => atendido = value; }
        public string Anotacoes { get => anotacoes; set => anotacoes = value; }
        public string Produto_Id { get => produto_Id; set => produto_Id = value; }
        public string Fabricante { get => fabricante; set => fabricante = value; }
    }


    public class InformacoesAtendimentoDTO
    {
        private int id;
        private int negocio_Id;
        private int vendas_Id;
        private string razao_Social;
        private string descricao;
        private string versao;
        private string cidade;
        private string uf;
        private string solicitante;
        private string endereco;

        public int Id { get => id; set => id = value; }
        public int Negocio_Id { get => negocio_Id; set => negocio_Id = value; }
        public string Razao_Social { get => razao_Social; set => razao_Social = value; }
        public int Vendas_Id { get => vendas_Id; set => vendas_Id = value; }
        public string Descricao { get => descricao; set => descricao = value; }
        public string Versao { get => versao; set => versao = value; }
        public string Cidade { get => cidade; set => cidade = value; }
        public string Solicitante { get => solicitante; set => solicitante = value; }
        public string Uf { get => uf; set => uf = value; }
        public string Endereco { get => endereco; set => endereco = value; }
    }
}
