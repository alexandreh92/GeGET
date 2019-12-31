using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    class SaldoEstoqueDTO
    {
        string produto_id;
        string item_id;
        string descricao;
        string partnumber;
        string fabricante_id;
        string fabricante;
        string un;
        double qtde;
        double estoque;

        public string Produto_Id { get => produto_id; set => produto_id = value; }
        public string Descricao { get => descricao; set => descricao = value; }
        public string Partnumber { get => partnumber; set => partnumber = value; }
        public string Fabricante { get => fabricante; set => fabricante = value; }
        public string Un { get => un; set => un = value; }
        public double Qtde { get => qtde; set => qtde = value; }
        public string Item_Id { get => item_id; set => item_id = value; }
        public double Estoque { get => estoque; set => estoque = value; }
        public string Fabricante_Id { get => fabricante_id; set => fabricante_id = value; }
    }
}
