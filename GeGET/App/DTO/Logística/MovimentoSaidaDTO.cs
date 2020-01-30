using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class MovimentoSaidaDTO
    {
        string id;
        string rm_id;
        string produto_id;
        string descricao;
        string quantidade;
        DateTime data;
        string nome;

        public string Id { get => id; set => id = value; }
        public string Rm_id { get => rm_id; set => rm_id = value; }
        public string Produto_id { get => produto_id; set => produto_id = value; }
        public string Descricao { get => descricao; set => descricao = value; }
        public string Quantidade { get => quantidade; set => quantidade = value; }
        public DateTime Data { get => data; set => data = value; }
        public string Nome { get => nome; set => nome = value; }
    }
}
