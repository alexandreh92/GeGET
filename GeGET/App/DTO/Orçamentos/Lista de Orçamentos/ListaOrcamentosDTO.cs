using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace DTO
{
    public class ListaOrcamentosDTO : INotifyPropertyChanged
    {
        private string id;
        private string produto_Id;
        private string codigo_Produto;
        private string descricao;
        private string anotacoes;
        private string partnumber;
        private string un;
        private string fabricante;
        private string disciplina;
        private string atividade;
        private string versao;
        private string descricao_Atividade;
        private string atividade_Id;
        private double quantidade;
        private double desconto;
        private double preco_Unitario;
        private double bdi;
        private double custo_Total;
        private double preco_Total;
        private int fd;

        public string Id { get => id; set => id = value; }
        public string Produto_Id { get => produto_Id; set => produto_Id = value; }
        public string Descricao { get => descricao; set => descricao = value; }
        public string Anotacoes { get => anotacoes; set => anotacoes = value; }
        public string Partnumber { get => partnumber; set => partnumber = value; }
        public string Un { get => un; set => un = value; }
        public string Fabricante { get => fabricante; set => fabricante = value; }
        public double Quantidade { get { return quantidade; }
            set
            {
                quantidade = value; 
                Custo_Total = quantidade * preco_Unitario;
                if (Fd == 1)
                {
                    Preco_Total = quantidade * preco_Unitario * 1.1;
                }
                else
                {
                    Preco_Total = quantidade * preco_Unitario * (1 + bdi / 100);
                }
            }
        }
        public double Desconto { get => desconto; set => desconto = value; }
        public double Preco_Unitario { get => preco_Unitario; set => preco_Unitario = value; }
        public double Bdi
        {
            get { return bdi; }
            set
            {
                bdi = value;
                if (Fd == 1)
                {
                    Preco_Total = quantidade * preco_Unitario * 1.1;
                }
                else
                {
                    Preco_Total = quantidade * preco_Unitario * (1 + bdi / 100);
                }
            }
        }
        public double Custo_Total
        {
            get { return custo_Total; }
            set
            {
                custo_Total = value;
                OnChanged("Custo_Total");
            }
        }
        public double Preco_Total
        {
            get { return preco_Total; }
            set
            {
                preco_Total = value;
                OnChanged("Preco_Total");
            }
        }
        public int Fd
        {
            get { return fd; }
            set
            {
                fd = value;
                if (Fd == 1)
                {
                    Preco_Total = quantidade * preco_Unitario * 1.1;
                }
                else
                {
                    Preco_Total = quantidade * preco_Unitario * (1 + bdi / 100);
                }
                OnChanged("Fd");
            }
        }

        public string Atividade_Id { get => atividade_Id; set => atividade_Id = value; }
        public string Codigo_Produto { get => codigo_Produto; set => codigo_Produto = value; }
        public string Disciplina { get => disciplina; set => disciplina = value; }
        public string Atividade { get => atividade; set => atividade = value; }
        public string Versao { get => versao; set => versao = value; }
        public string Descricao_Atividade { get => descricao_Atividade; set => descricao_Atividade = value; }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private void update()
        {
            MessageBox.Show("");
        }

        #endregion

    }

    public class InformacoesListaOrcamentosDTO
    {
        private int id;
        private int negocio_Id;
        private string descricao;
        private string razao_Social;
        private string cidade;
        private string uf;
        private string versao;
        private string atividade_Id;
        private ObservableCollection<DisciplinasOrcamentoDTO> disciplinas;
        private ObservableCollection<AtividadesOrcamentoDTO> atividades;
        private ObservableCollection<DescricaoAtividadesOrcamentoDTO> descricao_Atividades;

        public int Id { get => id; set => id = value; }
        public string Descricao { get => descricao; set => descricao = value; }
        public string Razao_Social { get => razao_Social; set => razao_Social = value; }
        public string Cidade { get => cidade; set => cidade = value; }
        public string Uf { get => uf; set => uf = value; }
        public string Versao { get => versao; set => versao = value; }
        public int Negocio_Id { get => negocio_Id; set => negocio_Id = value; }
        public string Atividade_Id { get => atividade_Id; set => atividade_Id = value; }
        internal ObservableCollection<DisciplinasOrcamentoDTO> Disciplinas { get => disciplinas; set => disciplinas = value; }
        internal ObservableCollection<AtividadesOrcamentoDTO> Atividades { get => atividades; set => atividades = value; }
        internal ObservableCollection<DescricaoAtividadesOrcamentoDTO> Descricao_Atividades { get => descricao_Atividades; set => descricao_Atividades = value; }
    }

    class DisciplinasOrcamentoDTO
    {
        private int id;
        private string descricao;

        public int Id { get => id; set => id = value; }
        public string Descricao { get => descricao; set => descricao = value; }
    }

    class AtividadesOrcamentoDTO
    {
        private int id;
        private string descricao;
        private string disciplina_Id;

        public int Id { get => id; set => id = value; }
        public string Descricao { get => descricao; set => descricao = value; }
        public string Disciplina_Id { get => disciplina_Id; set => disciplina_Id = value; }
    }

    class DescricaoAtividadesOrcamentoDTO
    {
        private int id;
        private string descricao;
        private string desc_Atividade_Id;

        public int Id { get => id; set => id = value; }
        public string Descricao { get => descricao; set => descricao = value; }
        public string Desc_Atividade_Id { get => desc_Atividade_Id; set => desc_Atividade_Id = value; }
    }

}
