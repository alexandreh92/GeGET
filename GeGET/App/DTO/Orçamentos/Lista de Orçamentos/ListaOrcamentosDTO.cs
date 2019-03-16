using System.ComponentModel;

namespace DTO
{
    public class ListaOrcamentosDTO : INotifyPropertyChanged
    {
        private string id;
        private string produto_Id;
        private string descricao;
        private string anotacoes;
        private string partnumber;
        private string un;
        private string fabricante;
        private string atividade_Id;
        private double quantidade;
        private double desconto;
        private double preco_Unitario;
        private double bdi;
        private double custo_Total;
        private double preco_Total;
        private int fd;
        private bool _IsSelected = false;
        public bool IsSelected { get { return _IsSelected; } set { _IsSelected = value; OnChanged("IsSelected"); } }

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
                    Preco_Total = quantidade * preco_Unitario * 1.15;
                }
                else
                {
                    Preco_Total = quantidade * preco_Unitario * (1 + bdi / 100);
                }
                OnChanged("Quantidade");
                OnChanged("Fd");
            }
        }
        public double Desconto { get => desconto; set => desconto = value; }
        public double Preco_Unitario { get => preco_Unitario; set => preco_Unitario = value; }
        public double Bdi { get => bdi; set => bdi = value; }
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
                    Preco_Total = quantidade * preco_Unitario * 1.15;
                }
                else
                {
                    Preco_Total = quantidade * preco_Unitario * (1 + bdi / 100);
                }
                OnChanged("Fd");
            }
        }

        public string Atividade_Id { get => atividade_Id; set => atividade_Id = value; }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        #endregion

    }
}
