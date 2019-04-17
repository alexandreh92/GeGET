using System.Windows.Media.Imaging;

namespace DTO
{
    public class OrcamentistasDTO
    {
        private string id;
        private string negocio_Id;
        private string nome;
        private string email;
        private string celular;
        private string nome_Simples;
        private string setor;
        private string login;
        private BitmapSource foto;

        public string Id { get => id; set => id = value; }
        public string Nome { get => nome; set => nome = value; }
        public string Email { get => email; set => email = value; }
        public string Celular { get => celular; set => celular = value; }
        public string Nome_Simples { get => nome_Simples; set => nome_Simples = value; }
        public string Setor { get => setor; set => setor = value; }
        public BitmapSource Foto { get => foto; set => foto = value; }
        public string Negocio_Id { get => negocio_Id; set => negocio_Id = value; }
        public string Login { get => login; set => login = value; }
    }
}
