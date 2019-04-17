namespace DTO
{
    public class DescricaoAtividadesDTO
    {
        private int id;
        private string descricao;
        private int disciplina_Id;

        public int Id { get => id; set => id = value; }
        public string Descricao { get => descricao; set => descricao = value; }
        public int Disciplina_Id { get => disciplina_Id; set => disciplina_Id = value; }
    }
}
