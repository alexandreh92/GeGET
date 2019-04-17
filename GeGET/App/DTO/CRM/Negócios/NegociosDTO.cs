namespace DTO
{
    public class NegociosDTO
    {
        private string id;
        private string numero;
        private string razao_Social;
        private string nome_Fantasia;
        private string endereco;
        private string anotacoes;
        private string status_Descricao;
        private string descricao;
        private string vendedor;
        private string prazo;
        private string vendedor_Id;
        private string contato_Id;
        private string contato_Nome;
        private string prioridade_Id;
        private string prioridade_Descricao;
        private string cidadeEstado;
        private int status_Id;
        private string cliente_Id;
        private string estabelecimento_Id;
        private string versao_Id;
        private string cnpj;
        private static string pesquisa;
        private static int status;
        private static bool fromParent = false;
        private static bool fromChildrenParent = false;
        private static string parentId;
        private string valor_Enviado;
        private string valor_Fechamento;
        private string motivo_Cancelamento_Id;
        private string motivo_Cancelamento;
        private string valor_Perdido;
        private string versao_Descricao;
        private int progressBarValue;
        private int versao_Locked;

        public string Id { get => id; set => id = value; }
        public string Numero { get => numero; set => numero = value; }
        public string Razao_Social { get => razao_Social; set => razao_Social = value; }
        public string Endereco { get => endereco; set => endereco = value; }
        public string Anotacoes { get => anotacoes; set => anotacoes = value; }
        public string Status_Descricao { get => status_Descricao; set => status_Descricao = value; }
        public string Descricao { get => descricao; set => descricao = value; }
        public string Vendedor { get => vendedor; set => vendedor = value; }
        public int Status_Id { get => status_Id; set => status_Id = value; }
        public string Pesquisa { get => pesquisa; set => pesquisa = value; }
        public int Status { get => status; set => status = value; }
        public bool FromParent { get => fromParent; set => fromParent = value; }
        public bool FromChildrenParent { get => fromChildrenParent; set => fromChildrenParent = value; }
        public string ParentId { get => parentId; set => parentId = value; }
        public string Cliente_Id { get => cliente_Id; set => cliente_Id = value; }
        public string Estabelecimento_Id { get => estabelecimento_Id; set => estabelecimento_Id = value; }
        public string Prazo { get => prazo; set => prazo = value; }
        public string Vendedor_Id { get => vendedor_Id; set => vendedor_Id = value; }
        public string Contato_Id { get => contato_Id; set => contato_Id = value; }
        public string Prioridade_Id { get => prioridade_Id; set => prioridade_Id = value; }
        public string CidadeEstado { get => cidadeEstado; set => cidadeEstado = value; }
        public string Contato_Nome { get => contato_Nome; set => contato_Nome = value; }
        public string Prioridade_Descricao { get => prioridade_Descricao; set => prioridade_Descricao = value; }
        public string Versao_Id { get => versao_Id; set => versao_Id = value; }
        public string Cnpj { get => cnpj; set => cnpj = value; }
        public string Valor_Enviado { get => valor_Enviado; set => valor_Enviado = value; }
        public string Valor_Fechamento { get => valor_Fechamento; set => valor_Fechamento = value; }
        public string Motivo_Cancelamento_Id { get => motivo_Cancelamento_Id; set => motivo_Cancelamento_Id = value; }
        public string Valor_Perdido { get => valor_Perdido; set => valor_Perdido = value; }
        public string Motivo_Cancelamento { get => motivo_Cancelamento; set => motivo_Cancelamento = value; }
        public string Versao_Descricao { get => versao_Descricao; set => versao_Descricao = value; }
        public int ProgressBarValue { get => progressBarValue; set => progressBarValue = value; }
        public int Versao_Locked { get => versao_Locked; set => versao_Locked = value; }
        public string Nome_Fantasia { get => nome_Fantasia; set => nome_Fantasia = value; }
    }
}
