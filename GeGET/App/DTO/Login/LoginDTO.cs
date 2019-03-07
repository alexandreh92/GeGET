namespace DTO
{
    class LoginDTO
    {
        private static string usuario;
        private static string senha;
        private static int nivel;
        private static int id;
        private static string nome;
        private static string primeiro_Nome;
        private static string ultimo_Sobrenome;
        private static bool supressChange = false;

        public string Primeiro_Nome { get => primeiro_Nome; set => primeiro_Nome = value; }
        public string Ultimo_Sobrenome { get => ultimo_Sobrenome; set => ultimo_Sobrenome = value; }
        public string Usuario { get => usuario; set => usuario = value; }
        public string Senha { get => senha; set => senha = value; }
        public int Nivel { get => nivel; set => nivel = value; }
        public int Id { get => id; set => id = value; }
        public string Nome { get => nome; set => nome = value; }
        public bool SupressChange { get => supressChange; set => supressChange = value; }
    }
}
