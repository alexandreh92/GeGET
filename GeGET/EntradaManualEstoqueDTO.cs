using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeGET
{
    public class EntradaManualEstoqueDTO
    {
        private int id;
        private string codigo;
        private string descricao;
        private string partnumber;
        private string anotacoes;
        private string fabricante;
        private string unidade;
        private double quantidade;
        private double custo;
        private string nota;

        public int Id { get => id; set => id = value; }
        public string Codigo { get => codigo; set => codigo = value; }
        public string Descricao { get => descricao; set => descricao = value; }
        public string Partnumber { get => partnumber; set => partnumber = value; }
        public string Anotacoes { get => anotacoes; set => anotacoes = value; }
        public string Fabricante { get => fabricante; set => fabricante = value; }
        public string Unidade { get => unidade; set => unidade = value; }
        public double Quantidade { get => quantidade; set => quantidade = value; }
        public double Custo { get => custo; set => custo = value; }
        public string Nota { get => nota; set => nota = value; }
    }
}
