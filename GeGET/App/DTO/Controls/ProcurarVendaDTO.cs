namespace DTO
{
    public class ProcurarVendaDTO
    {
        private int id;
        private int negocio_Id;
        private int cliente_Id;
        private string descricao;
        private string razao_Social;
        private string nome_Fantasia;
        private string cidade_Estado;
        private string endereco;
        private string negocio_Numero;

        public int Negocio_Id { get => negocio_Id; set => negocio_Id = value; }
        public int Cliente_Id { get => cliente_Id; set => cliente_Id = value; }
        public string Descricao { get => descricao; set => descricao = value; }
        public string Razao_Social { get => razao_Social; set => razao_Social = value; }
        public string Nome_Fantasia { get => nome_Fantasia; set => nome_Fantasia = value; }
        public string Cidade_Estado { get => cidade_Estado; set => cidade_Estado = value; }
        public int Id { get => id; set => id = value; }
        public string Endereco { get => endereco; set => endereco = value; }
        public string Negocio_Numero { get => negocio_Numero; set => negocio_Numero = value; }
    }
}
