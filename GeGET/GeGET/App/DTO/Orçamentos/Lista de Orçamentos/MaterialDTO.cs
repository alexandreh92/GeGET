namespace DTO
{
    public class MaterialDTO
    {
        private string id;
        private string anotacoes;
        private double quantidade;
        private string bdi;
        private int fd;

        public string Id { get => id; set => id = value; }
        public string Anotacoes { get => anotacoes; set => anotacoes = value; }
        public double Quantidade { get => quantidade; set => quantidade = value; }
        public string Bdi { get => bdi; set => bdi = value; }
        public int Fd { get => fd; set => fd = value; }
    }
}
