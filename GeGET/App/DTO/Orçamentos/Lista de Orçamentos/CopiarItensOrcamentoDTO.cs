using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class CopiarItensOrcamentoDTO
    {
        private string id;
        private string negocio_Id;
        private string disciplina_Id;
        private string atividade_Id;
        private double quantidade;

        public string Id { get => id; set => id = value; }
        public string Negocio_Id { get => negocio_Id; set => negocio_Id = value; }
        public string Disciplina_Id { get => disciplina_Id; set => disciplina_Id = value; }
        public string Atividade_Id { get => atividade_Id; set => atividade_Id = value; }
        public double Quantidade { get => quantidade; set => quantidade = value; }
    }
}
