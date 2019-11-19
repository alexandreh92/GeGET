using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    class CadastroUsuarioDTO
    {
        private string id;
        private string name;
        private string login;
        private string password;

        public string Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Login { get => login; set => login = value; }
        public string Password { get => password; set => password = value; }
    }
}
