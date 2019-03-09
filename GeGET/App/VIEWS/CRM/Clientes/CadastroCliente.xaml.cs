﻿using System;
using System.Windows;
using System.Windows.Controls;
using BLL;
using DTO;

namespace GeGET
{
    public partial class CadastroCliente : UserControl
    {
        #region Declarations
        CategoriaClienteBLL Categoriabll = new CategoriaClienteBLL();
        ClientesBLL bll = new ClientesBLL();
        ClientesDTO dto = new ClientesDTO();
        Helpers helpers = new Helpers();
        #endregion

        #region Initialize
        public CadastroCliente()
        {
            InitializeComponent();
            cmbCategoria.ItemsSource = Categoriabll.LoadCategoriaCliente();
            cmbCategoria.DisplayMemberPath = "Descricao";
            cmbCategoria.SelectedValuePath = "Id";
        }
        #endregion

        #region Methods

        #region ClearControls
        private void ClearControls()
        {
            txtFantasia.Text = "";
            txtRazao.Text = "";
            cmbCategoria.SelectedIndex = -1;
        }
        #endregion

        #endregion

        #region Events

        #region Clicks
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            helpers.OpenBack(false);
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            helpers.Close();
        }

        private void BtnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            if (txtRazao.Text != "" && txtFantasia.Text != "" && cmbCategoria.SelectedIndex != -1)
            {
                var result = CustomOKCancelMessageBox.Show("Deseja mesmo cadastrar este cliente?", "Atenção!");
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    dto.Razao_Social = txtRazao.Text.Replace("'", "''").ToUpper();
                    dto.Nome_Fantasia = txtFantasia.Text.Replace("'", "''").ToUpper();
                    dto.Categoria_Id = Convert.ToInt32(cmbCategoria.SelectedValue);
                    if (bll.CreateCliente(dto))
                    {
                        ClearControls();
                        CustomOKCancelMessageBox.Show("Cliente cadastrado com sucesso!", "Sucesso!");
                    }
                }
            }
            else
            {
                CustomOKCancelMessageBox.Show("Campos não podem estar em branco.", "Atenção!");
            }
        }
        #endregion

        #endregion
    }
}