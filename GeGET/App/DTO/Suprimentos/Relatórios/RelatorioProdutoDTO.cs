using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class RelatorioProdutoDTO
    {
        private string id;
        private string item_id;
        private string ncm;
        private double custounitario;
        private double ipi;
        private string fornecedor_id;
        private string usuario_id;
        private string partnumber;
        private string data;
        private string descricao;
        private double icms;
        private string status_id;
        private string codigo_barra;
        private string item_descricao;
        private string fabricante;

        public string Id { get => id; set => id = value; }
        public string Item_id { get => item_id; set => item_id = value; }
        public string Ncm { get => ncm; set => ncm = value; }
        public double Custounitario { get => custounitario; set => custounitario = value; }
        public double Ipi { get => ipi; set => ipi = value; }
        public string Fornecedor_id { get => fornecedor_id; set => fornecedor_id = value; }
        public string Usuario_id { get => usuario_id; set => usuario_id = value; }
        public string Partnumber { get => partnumber; set => partnumber = value; }
        public string Data { get => data; set => data = value; }
        public string Descricao { get => descricao; set => descricao = value; }
        public double Icms { get => icms; set => icms = value; }
        public string Status_id { get => status_id; set => status_id = value; }
        public string Codigo_barra { get => codigo_barra; set => codigo_barra = value; }
        public string Item_descricao { get => item_descricao; set => item_descricao = value; }
        public string Fabricante { get => fabricante; set => fabricante = value; }
    }
}
