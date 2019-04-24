using System.Collections.ObjectModel;

namespace DTO
{
    public class ListaProjetosDTO
    {
        private int id;
        private string produto_Id;
        private string codigo;
        private string descricao;
        private string partnumber;
        private string unidade;
        private double quantidade_Orcamento;
        private double preco_Orcamento;
        private string anotacoes;
        private double quantidade;
        private double preco;
        private string fabricante;
        private double total;
        private int fd;

        public int Id { get => id; set => id = value; }
        public string Codigo { get => codigo; set => codigo = value; }
        public string Descricao { get => descricao; set => descricao = value; }
        public string Partnumber { get => partnumber; set => partnumber = value; }
        public string Unidade { get => unidade; set => unidade = value; }
        public double Quantidade_Orcamento { get => quantidade_Orcamento; set => quantidade_Orcamento = value; }
        public double Preco_Orcamento { get => preco_Orcamento; set => preco_Orcamento = value; }
        public string Anotacoes { get => anotacoes; set => anotacoes = value; }
        public double Quantidade
        {
            get { return quantidade; }
            set
            {
                quantidade = value;
                Total = quantidade * preco;
            }
        }
        public double Preco { get => preco; set => preco = value; }
        public string Fabricante { get => fabricante; set => fabricante = value; }
        public int Fd { get => fd; set => fd = value; }
        public double Total { get => total; set => total = value; }
        public string Produto_Id { get => produto_Id; set => produto_Id = value; }
    }

    public class InformacoesListaProjetosDTO
    {
        private int id;
        private int negocio_Id;
        private string descricao;
        private string razao_Social;
        private string cidade;
        private string uf;
        private string versao;
        private string atividade_Id;
        private ObservableCollection<DisciplinasProjetoDTO> disciplinas;
        private ObservableCollection<AtividadesProjetoDTO> atividades;
        private ObservableCollection<DescricaoAtividadesProjetoDTO> descricao_Atividades;

        public int Id { get => id; set => id = value; }
        public string Descricao { get => descricao; set => descricao = value; }
        public string Razao_Social { get => razao_Social; set => razao_Social = value; }
        public string Cidade { get => cidade; set => cidade = value; }
        public string Uf { get => uf; set => uf = value; }
        public string Versao { get => versao; set => versao = value; }
        public int Negocio_Id { get => negocio_Id; set => negocio_Id = value; }
        public string Atividade_Id { get => atividade_Id; set => atividade_Id = value; }
        internal ObservableCollection<DisciplinasProjetoDTO> Disciplinas { get => disciplinas; set => disciplinas = value; }
        internal ObservableCollection<AtividadesProjetoDTO> Atividades { get => atividades; set => atividades = value; }
        internal ObservableCollection<DescricaoAtividadesProjetoDTO> Descricao_Atividades { get => descricao_Atividades; set => descricao_Atividades = value; }
    }

    class DisciplinasProjetoDTO
    {
        private int id;
        private string descricao;

        public int Id { get => id; set => id = value; }
        public string Descricao { get => descricao; set => descricao = value; }
    }

    class AtividadesProjetoDTO
    {
        private int id;
        private string descricao;
        private string descricao_Atividade;
        private string disciplina_Id;
        private string atividade_Id;

        public int Id { get => id; set => id = value; }
        public string Descricao { get => descricao; set => descricao = value; }
        public string Descricao_Atividade { get => descricao_Atividade; set => descricao_Atividade = value; }
        public string Disciplina_Id { get => disciplina_Id; set => disciplina_Id = value; }
        public string Atividade_Id { get => atividade_Id; set => atividade_Id = value; }
    }

    class DescricaoAtividadesProjetoDTO
    {
        private int id;
        private string descricao;
        private string desc_Atividade_Id;

        public int Id { get => id; set => id = value; }
        public string Descricao { get => descricao; set => descricao = value; }
        public string Desc_Atividade_Id { get => desc_Atividade_Id; set => desc_Atividade_Id = value; }
    }


    class ValoresProjetoDTO
    {
        private double atividade_Itens_Orcamento;
        private double atividade_Itens_Projeto;
        private double atividade_FD_Orcamento;
        private double atividade_FD_Projeto;
        private double atividade_Total_Orcamento;
        private double atividade_Total_Projeto;
        private double negocio_Itens_Orcamento;
        private double negocio_Itens_Projeto;
        private double negocio_FD_Orcamento;
        private double negocio_FD_Projeto;
        private double negocio_Total_Orcamento;
        private double negocio_Total_Projeto;

        public double Atividade_Itens_Orcamento { get => atividade_Itens_Orcamento; set => atividade_Itens_Orcamento = value; }
        public double Atividade_Itens_Projeto { get => atividade_Itens_Projeto; set => atividade_Itens_Projeto = value; }
        public double Atividade_FD_Orcamento { get => atividade_FD_Orcamento; set => atividade_FD_Orcamento = value; }
        public double Atividade_FD_Projeto { get => atividade_FD_Projeto; set => atividade_FD_Projeto = value; }
        public double Atividade_Total_Orcamento { get => atividade_Total_Orcamento; set => atividade_Total_Orcamento = value; }
        public double Atividade_Total_Projeto { get => atividade_Total_Projeto; set => atividade_Total_Projeto = value; }
        public double Negocio_Itens_Orcamento { get => negocio_Itens_Orcamento; set => negocio_Itens_Orcamento = value; }
        public double Negocio_Itens_Projeto { get => negocio_Itens_Projeto; set => negocio_Itens_Projeto = value; }
        public double Negocio_FD_Orcamento { get => negocio_FD_Orcamento; set => negocio_FD_Orcamento = value; }
        public double Negocio_FD_Projeto { get => negocio_FD_Projeto; set => negocio_FD_Projeto = value; }
        public double Negocio_Total_Orcamento { get => negocio_Total_Orcamento; set => negocio_Total_Orcamento = value; }
        public double Negocio_Total_Projeto { get => negocio_Total_Projeto; set => negocio_Total_Projeto = value; }
    }

}
