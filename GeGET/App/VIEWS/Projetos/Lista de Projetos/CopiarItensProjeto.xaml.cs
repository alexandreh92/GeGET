﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DTO;
using BLL;
using System.Collections.ObjectModel;
using System.Threading;

namespace GeGET
{
    public partial class CopiarItensProjeto : Window, IDisposable
    {
        #region Declarations
        ListaProjetosDTO dto = new ListaProjetosDTO();
        ListaProjetosBLL bll = new ListaProjetosBLL();
        InformacoesListaProjetosDTO informacoesDTO = new InformacoesListaProjetosDTO();
        InformacoesListaProjetosBLL informacoesBLL = new InformacoesListaProjetosBLL();
        AtividadesProjetoDTO atividadesProjetoDTO = new AtividadesProjetoDTO();
        ObservableCollection<AtividadesProjetoDTO> listaChecked = new ObservableCollection<AtividadesProjetoDTO>();
        ObservableCollection<ListaProjetosDTO> listaCopiar = new ObservableCollection<ListaProjetosDTO>();
        WaitBox wb;
        ManualResetEvent syncEvent = new ManualResetEvent(false);
        Thread t1;
        bool disposed = false;
        #endregion

        #region Initialize
        public CopiarItensProjeto(ObservableCollection<ListaProjetosDTO> DTO, InformacoesListaProjetosDTO informacoes)
        {
            InitializeComponent();
            informacoesDTO = informacoes;
            listaCopiar = DTO;
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            LoadComboboxDisciplina();
        }
        #endregion

        #region Methods

        #region Load Combobox Disciplina
        private void LoadComboboxDisciplina()
        {
            cmbDisciplina.ItemsSource = informacoesDTO.Disciplinas;
            cmbDisciplina.SelectedValuePath = "Id";
            cmbDisciplina.DisplayMemberPath = "Descricao";
            cmbDisciplina.SelectedIndex = 0;
        }
        #endregion

        #region Copiar Valores
        private void Insert()
        {

            new Thread(() =>
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    wb = new WaitBox
                    {
                        Owner = Window.GetWindow(this)
                    };
                    wb.Show();
                }));
                syncEvent.Set();

                foreach (var atividade in listaChecked)
                {
                    atividadesProjetoDTO.Atividade_Id = atividade.Atividade_Id;
                    foreach (var item in listaCopiar)
                    {
                        dto.Produto_Id = item.Produto_Id;
                        bll.CopiarItens(dto, informacoesDTO, atividadesProjetoDTO);
                    }
                }
                t1 = new Thread(WaitBoxLoad);
                t1.Start();
            }).Start();

        }
        #endregion

        #region Wait Load

        private void WaitBoxLoad()
        {
            syncEvent.WaitOne();
            Dispatcher.Invoke(new Action(() =>
            {
                wb.Close();
                var result = CustomOKCancelMessageBox.Show("Itens Inseridos com sucesso.\nDeseja inserir os itens selecionados em outra Disciplina?", "Sucesso!", Window.GetWindow(this));
                if (result != System.Windows.Forms.DialogResult.OK)
                {
                    DialogResult = true;
                }
            }));
        }

        #endregion

        #endregion

        #region Events

        #region Clicks
        #region Botão Confirmar
        private void BtnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            int checks = 0;

            for (int i = 0; i < grdCheck.Items.Count; i++)
            {

                ContentPresenter c = (ContentPresenter)grdCheck.ItemContainerGenerator.ContainerFromItem(grdCheck.Items[i]);
                CheckBox cb = c.ContentTemplate.FindName("cbx", c) as CheckBox;
                if (cb.IsChecked.Value)
                {
                    checks++;
                    int index = grdCheck.Items.IndexOf(cb.DataContext);
                    var atividade = ((AtividadesProjetoDTO)grdCheck.Items[index]);

                    listaChecked.Add(new AtividadesProjetoDTO
                    {
                        Id = atividade.Id, Atividade_Id = atividade.Atividade_Id
                    });
                }
            }
            if (checks > 0)
            {
                Insert();
            }

        }
        #endregion

        #region Botão Cancelar
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
        #endregion
        #endregion

        #region Comboxox Selection Changed

        #region cmbDisciplina

        private void CmbDisciplina_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbDisciplina.SelectedValue != null)
            {
                var Find = cmbDisciplina.SelectedValue.ToString().ToLower();
                var filtered = informacoesDTO.Atividades.Where(descricao => descricao.Disciplina_Id == Find);
                grdCheck.ItemsSource = filtered;
            }
        }

        #endregion

        #endregion

        #endregion

        #region IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                syncEvent.Dispose();
                bll.Dispose();
                informacoesBLL.Dispose();

            }
            disposed = true;
        }
        #endregion
    }
}
