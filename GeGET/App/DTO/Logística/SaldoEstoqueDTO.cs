using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    class SaldoEstoqueDTO
    {
        string id;
        string descricao;
        string partnumber;
        string fabricante;
        string un;
        double qtde;

        public string Id { get => id; set => id = value; }
        public string Descricao { get => descricao; set => descricao = value; }
        public string Partnumber { get => partnumber; set => partnumber = value; }
        public string Fabricante { get => fabricante; set => fabricante = value; }
        public string Un { get => un; set => un = value; }
        public double Qtde { get => qtde; set => qtde = value; }
    }
}
