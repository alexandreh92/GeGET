using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DTO;
using BLL;
using System.Collections.ObjectModel;
using System.Threading;

namespace GeGET
{
    public partial class CopiarItensOrcamento : Window, IDisposable
    {
        CopiarItensOrcamentoDTO dto = new CopiarItensOrcamentoDTO();
        CopiarItensOrcamentoBLL bll = new CopiarItensOrcamentoBLL();
        ObservableCollection<CopiarItensOrcamentoDTO> listaCopiar = new ObservableCollection<CopiarItensOrcamentoDTO>();
        WaitBox wb;
        ManualResetEvent syncEvent = new ManualResetEvent(false);
        Thread t1;

        public CopiarItensOrcamento(ObservableCollection<CopiarItensOrcamentoDTO> dTOs, ListaOrcamentosDTO dTO)
        {
            InitializeComponent();
            dto.Negocio_Id = dTO.Id;
            foreach (CopiarItensOrcamentoDTO item in dTOs)
            {
                listaCopiar.Add(new CopiarItensOrcamentoDTO { Id = item.Id });
            }
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            LoadComboboxDisciplina();
        }

        private void LoadComboboxDisciplina()
        {
            var disciplinas = bll.LoadDisciplinas(dto);
            cmbDisciplina.ItemsSource = disciplinas;
            cmbDisciplina.SelectedValuePath = "Id";
            cmbDisciplina.DisplayMemberPath = "Descricao";
            cmbDisciplina.SelectedIndex = 0;
        }

        void IDisposable.Dispose()
        {
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CmbDisciplina_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dto.Disciplina_Id = cmbDisciplina.SelectedValue.ToString();
            var atividades = bll.LoadAtividades(dto);
            cmbAtividade.ItemsSource = atividades;
            cmbAtividade.SelectedValuePath = "Id";
            cmbAtividade.DisplayMemberPath = "Descricao";
            cmbAtividade.SelectedIndex = 0;
        }

        private void Insert()
        {
            dto.Atividade_Id = cmbAtividade.SelectedValue.ToString();
            new Thread(() => 
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    wb = new WaitBox();
                    wb.Show();
                }));
                syncEvent.Set();

                foreach (var item in listaCopiar)
                {
                    dto.Id = item.Id;
                    bll.Insert(dto);
                }
                t1 = new Thread(WaitBoxLoad);
                t1.Start();
            }).Start();
            
        }

        private void WaitBoxLoad()
        {
            syncEvent.WaitOne();
            Dispatcher.Invoke(new Action(() =>
            {
                wb.Close();
                var result = CustomOKCancelMessageBox.Show("Itens Inseridos com sucesso.\nDeseja inserir os itens selecionados em outra atividade?","Sucesso!", Window.GetWindow(this));
                if (result != System.Windows.Forms.DialogResult.OK)
                {
                    this.Close();
                }
            }));
        }

        private void BtnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            var result = CustomOKCancelMessageBox.Show("Você deseja mesmo copiar estes itens para a atividade '" + cmbAtividade.Text + "' ?","Atenção!", Window.GetWindow(this));
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                Insert();
            }
        }
    }
}
