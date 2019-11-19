using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class EstornoEstoqueDTO
    {
        private string id;
        private string item_Id;
        private string un;
        private string partnumber;
        private string fornecedor;
        private string descricao;
        private double quantidade;
        private string usuario_id;
        private string data;

        public string Id { get => id; set => id = value; }
        public string Item_Id { get => item_Id; set => item_Id = value; }
        public string Un { get => un; set => un = value; }
        public string Descricao { get => descricao; set => descricao = value; }
        public double Quantidade { get => quantidade; set => quantidade = value; }
        public string Usuario_id { get => usuario_id; set => usuario_id = value; }
        public string Data { get => data; set => data = value; }
        public string Partnumber { get => partnumber; set => partnumber = value; }
        public string Fornecedor { get => fornecedor; set => fornecedor = value; }
    }
}
