using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class MovimentoEntradaDTO
    {
        string id;
        string produto_id;
        string descricao;
        string quantidade;
        double custo;
        DateTime data;
        string nome;
        string nota_fiscal;
        string chave_acesso;


        public string Id { get => id; set => id = value; }
        public string Produto_id { get => produto_id; set => produto_id = value; }
        public string Descricao { get => descricao; set => descricao = value; }
        public string Quantidade { get => quantidade; set => quantidade = value; }
        public DateTime Data { get => data; set => data = value; }
        public string Nome { get => nome; set => nome = value; }
        public double Custo { get => custo; set => custo = value; }
        public string Nota_fiscal { get => nota_fiscal; set => nota_fiscal = value; }
        public string Chave_acesso { get => chave_acesso; set => chave_acesso = value; }
    }
}
