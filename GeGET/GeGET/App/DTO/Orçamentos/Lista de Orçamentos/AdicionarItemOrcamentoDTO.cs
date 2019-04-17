using System.ComponentModel;

namespace DTO
{
    public class AdicionarItemOrcamentoDTO : INotifyPropertyChanged
    {
        private int id;
        private string codigo_Produto;
        private string descricao;
        private string descricao_Produto;
        private string un;
        private string partnumber;
        private double custo_Unitario;
        private string fabricante;
        private string negocio_Id;
        private string atividade_Id;
        private bool isSelected;


        public int Id { get => id; set => id = value; }
        public string Descricao { get => descricao; set => descricao = value; }
        public string Descricao_Produto { get => descricao_Produto; set => descricao_Produto = value; }
        public string Un { get => un; set => un = value; }
        public string Partnumber { get => partnumber; set => partnumber = value; }
        public double Custo_Unitario { get => custo_Unitario; set => custo_Unitario = value; }
        public string Fabricante { get => fabricante; set => fabricante = value; }
        public string Negocio_Id { get => negocio_Id; set => negocio_Id = value; }
        public string Atividade_Id { get => atividade_Id; set => atividade_Id = value; }
        public bool IsSelected {
            get { return isSelected; }
            set
            {
                isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        public string Codigo_Produto { get => codigo_Produto; set => codigo_Produto = value; }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
