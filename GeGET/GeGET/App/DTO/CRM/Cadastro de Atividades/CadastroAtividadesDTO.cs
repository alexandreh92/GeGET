namespace DTO
{
    public class CadastroAtividadesDTO
    {
        private int id;
        private string razao_Social;
        private string numero;
        private string cnpj;
        private string status_Descricao;
        private int versao_Id;
        private string endereco;
        private string cidadeEstado;
        private string descricao;
        private string vendedor;
        private string contato_Nome;
        private int versao_Locked;
        private string disciplina;
        private string atividade;
        private string versao_Atividade_Id;

        public int Id { get => id; set => id = value; }
        public string Razao_Social { get => razao_Social; set => razao_Social = value; }
        public string Numero { get => numero; set => numero = value; }
        public string Cnpj { get => cnpj; set => cnpj = value; }
        public string Status_Descricao { get => status_Descricao; set => status_Descricao = value; }
        public int Versao_Id { get => versao_Id; set => versao_Id = value; }
        public string Endereco { get => endereco; set => endereco = value; }
        public string CidadeEstado { get => cidadeEstado; set => cidadeEstado = value; }
        public string Descricao { get => descricao; set => descricao = value; }
        public string Vendedor { get => vendedor; set => vendedor = value; }
        public string Contato_Nome { get => contato_Nome; set => contato_Nome = value; }
        public int Versao_Locked { get => versao_Locked; set => versao_Locked = value; }
        public string Disciplina { get => disciplina; set => disciplina = value; }
        public string Atividade { get => atividade; set => atividade = value; }
        public string Versao_Atividade_Id { get => versao_Atividade_Id; set => versao_Atividade_Id = value; }
    }

    public class AtividadeCadastradaDTO
    {
        private int id;
        private string numero;
        private string descricao;
        private string atividade;
        private string atividade_id;
        private bool habilitado;

        public string Numero { get => numero; set => numero = value; }
        public string Descricao { get => descricao; set => descricao = value; }
        public string Atividade { get => atividade; set => atividade = value; }
        public bool Habilitado { get => habilitado; set => habilitado = value; }
        public int Id { get => id; set => id = value; }
        public string Atividade_id { get => atividade_id; set => atividade_id = value; }
    }

}
