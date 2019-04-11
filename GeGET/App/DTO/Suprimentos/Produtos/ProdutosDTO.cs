namespace DTO
{
    public class ProdutosDTO
    {
        private string id;
        private string item_Descricao;
        private string item_Id;
        private string partnumber;
        private string fornecedor;
        private string fornecedor_Id;
        private string unidade;
        private string ncm;
        private double custo;
        private double ipi;
        private double icms;
        private string status_Id;
        private string anotacoes;
        private string codigo;

        public string Id { get => id; set => id = value; }
        public string Item_Descricao { get => item_Descricao; set => item_Descricao = value; }
        public string Item_Id { get => item_Id; set => item_Id = value; }
        public string Partnumber { get => partnumber; set => partnumber = value; }
        public string Fornecedor { get => fornecedor; set => fornecedor = value; }
        public string Fornecedor_Id { get => fornecedor_Id; set => fornecedor_Id = value; }
        public string Unidade { get => unidade; set => unidade = value; }
        public string Ncm { get => ncm; set => ncm = value; }
        public double Custo { get => custo; set => custo = value; }
        public double Ipi { get => ipi; set => ipi = value; }
        public double Icms { get => icms; set => icms = value; }
        public string Status_Id { get => status_Id; set => status_Id = value; }
        public string Anotacoes { get => anotacoes; set => anotacoes = value; }
        public string Codigo { get => codigo; set => codigo = value; }
    }
}
