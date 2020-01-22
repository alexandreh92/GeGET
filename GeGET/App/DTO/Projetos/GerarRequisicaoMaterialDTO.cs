namespace DTO
{
    public class GerarRequisicaoMaterialDTO
    {
        private string produto_Id;
        private string descricao;
        private string partnumber;
        private string unidade;
        private double saldo;
        private double quantidade;
        private string fabricante;

        public string Produto_Id { get => produto_Id; set => produto_Id = value; }
        public string Descricao { get => descricao; set => descricao = value; }
        public string Partnumber { get => partnumber; set => partnumber = value; }
        public string Unidade { get => unidade; set => unidade = value; }
        public double Saldo { get => saldo; set => saldo = value; }
        public double Quantidade { get => quantidade; set => quantidade = value; }
        public string Fabricante { get => fabricante; set => fabricante = value; }
    }

    public class InformacoesGerarRequisicaoMaterialDTO
    {
        private int id;
        private int negocio_Id;
        private string descricao;
        private string razao_Social;
        private string cidade;
        private string uf;
        private string versao;
        private string rM_Descricao;

        public int Id { get => id; set => id = value; }
        public int Negocio_Id { get => negocio_Id; set => negocio_Id = value; }
        public string Descricao { get => descricao; set => descricao = value; }
        public string Razao_Social { get => razao_Social; set => razao_Social = value; }
        public string Cidade { get => cidade; set => cidade = value; }
        public string Uf { get => uf; set => uf = value; }
        public string Versao { get => versao; set => versao = value; }
        public string RM_Descricao { get => rM_Descricao; set => rM_Descricao = value; }
    }

}
