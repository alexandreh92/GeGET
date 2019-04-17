﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using BLL;
using DTO;
using MMLib.Extensions;

namespace GeGET
{
    public partial class Fornecedores : UserControl
    {
        #region Declarations
        FornecedoresBLL bll = new FornecedoresBLL();
        FornecedoresDTO dto = new FornecedoresDTO();
        Helpers helpers = new Helpers();
        Thread t1;
        public ObservableCollection<FornecedoresDTO> listaFornecedores;

        #endregion

        #region Initialize

        public Fornecedores()
        {
            InitializeComponent();
            LoadClients();
        }

        #endregion

        #region Methods
        private void LoadClients()
        {
            Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
            {
                listaFornecedores = bll.LoadFornecedores();
                lstFornecedores.ItemsSource = listaFornecedores;
            }));
        }

        private void Commit()
        {
            Dispatcher.Invoke(DispatcherPriority.Background,
                  new Action(() =>
                  {
                      var Find = txtProcurar.Text.ToLower().RemoveDiacritics().Split(' ').ToList();
                      var filtered = listaFornecedores.Where(descricao => Find.Any(list => descricao.Razao_Social.ToLower().RemoveDiacritics().Contains(list) || descricao.Nome_Fantasia.ToLower().RemoveDiacritics().Contains(list) || descricao.Grupo_Forn_Descricao.RemoveDiacritics().ToLower().Contains(list) || descricao.Telefone.RemoveDiacritics().ToLower().Contains(list) || descricao.Email.RemoveDiacritics().ToLower().Contains(list)));
                      lstFornecedores.ItemsSource = filtered;
                  }));
        }
        #endregion

        #region Events

        #region Text Changed
        private void TxtProcurar_TextChanged(object sender, TextChangedEventArgs e)
        {
            t1 = new Thread(Commit);
            t1.Start();
        }
        #endregion

        #region Clicks

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            helpers.Close();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            helpers.OpenBack(false);
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            /*Button btn = sender as Button;
            var index = lstProdutos.Items.IndexOf(btn.DataContext);
            dto.Id = ((ProdutosDTO)lstProdutos.Items[index]).Id;
            dto.Item_Descricao = ((ProdutosDTO)lstProdutos.Items[index]).Item_Descricao;
            dto.Fornecedor = ((ProdutosDTO)lstProdutos.Items[index]).Fornecedor;
            dto.Partnumber = ((ProdutosDTO)lstProdutos.Items[index]).Partnumber;
            dto.Custo = ((ProdutosDTO)lstProdutos.Items[index]).Custo;
            dto.Ipi = ((ProdutosDTO)lstProdutos.Items[index]).Ipi;
            dto.Icms = ((ProdutosDTO)lstProdutos.Items[index]).Icms;
            dto.Ncm = ((ProdutosDTO)lstProdutos.Items[index]).Ncm;
            dto.Status_Id = ((ProdutosDTO)lstProdutos.Items[index]).Status_Id;
            dto.Anotacoes = ((ProdutosDTO)lstProdutos.Items[index]).Anotacoes;*/
            using (var form = new EditarFornecedor(dto))
            {
                form.Owner = Window.GetWindow(this);
                form.ShowDialog();
                if (form.DialogResult.Value && form.DialogResult.HasValue)
                {
                    //listaProdutos = bll.LoadProdutos();
                    //lstProdutos.ItemsSource = listaProdutos;
                }
            }
        }
        #endregion

        #endregion
    }
}