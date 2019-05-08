using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using BLL;
using DTO;
using System.Linq;
using System.Windows.Controls.Primitives;

namespace GeGET
{
    public partial class CadastroAtividades : UserControl
    {

        #region Declarations
        Helpers helpers = new Helpers();
        LoginDTO login = new LoginDTO();
        CadastroAtividadesDTO dto = new CadastroAtividadesDTO();
        CadastroAtividadeBLL bll = new CadastroAtividadeBLL();
        DescricaoAtividadesBLL descricaoAtividadesBLL = new DescricaoAtividadesBLL();
        DescricaoAtividadesDTO descricaoAtividadesDTO = new DescricaoAtividadesDTO();
        AtividadeCadastradaDTO atividadeCadastradaDTO = new AtividadeCadastradaDTO();
        ManualResetEvent syncEvent = new ManualResetEvent(false);
        #endregion

        #region Initialize
        public CadastroAtividades()
        {
            InitializeComponent();
        }
        #endregion


        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            helpers.Close();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            helpers.OpenBack(false);
        }

        private void BtnPesquisa_Click(object sender, RoutedEventArgs e)
        {
            var position = Mouse.GetPosition(this);
            using (var form = new ProcurarNegocio(position))
            {
                form.Owner = Window.GetWindow(this);
                form.ShowDialog();
                if (form.DialogResult.Value && form.DialogResult.HasValue)
                {
                    dto.Id = Convert.ToInt32(form.Negocio_Id);
                    
                    dto = bll.LoadOrcamento(dto).First();

                    txtNumero.Text = dto.Numero.ToUpper();
                    txtCliente.Text = dto.Razao_Social;
                    txtNumero.Text = dto.Numero.ToUpper();
                    txtCliente.Text = dto.Razao_Social;
                    txtCNPJ.Text = dto.Cnpj;
                    txtStatus.Text = dto.Status_Descricao;
                    txtVersao.Text = Convert.ToInt32(dto.Versao_Id).ToString("00");
                    txtEndereco.Text = dto.Endereco;
                    txtCidadeEstado.Text = dto.CidadeEstado;
                    txtDescricao.Text = dto.Descricao;
                    txtVendedor.Text = dto.Vendedor;
                    txtResponsavel.Text = dto.Contato_Nome;
                    descricaoAtividadesDTO.Disciplina_Id = 100;
                    InitializeComponents();
                }
            }
        }

        private void LoadAtividadesCadastradas()
        {
            lstAtividades.ItemsSource = bll.LoadAtividadesCadastradas(dto, descricaoAtividadesDTO);
        }

        private void LoadCombobox()
        {
            cmbAtividade.ItemsSource = descricaoAtividadesBLL.LoadAtividades(descricaoAtividadesDTO);
            cmbAtividade.SelectedValuePath = "Id";
            cmbAtividade.DisplayMemberPath = "Descricao";
            cmbAtividade.SelectedIndex = 0;
        }

        private void InitializeComponents()
        {
            if (dto.Versao_Locked == 0)
            {
                LoadCombobox();
                LoadAtividadesCadastradas();
                grdAtividades.Visibility = Visibility.Visible;
            }
            else
            {
                if (grdAtividades.Visibility == Visibility.Visible)
                {
                    grdAtividades.Visibility = Visibility.Collapsed;
                }
                CustomOKMessageBox.Show("Versão do orçamento trancada. Crie uma nova versão para adicionar atividades.","Atenção!",Window.GetWindow(this));
            }
            if (btnEletrica.IsChecked == false)
            {
                btnEletrica.IsChecked = true;
            }
        }

        private void BtnEletrica_Click(object sender, RoutedEventArgs e)
        {
            descricaoAtividadesDTO.Disciplina_Id = 100;
            LoadCombobox();
            LoadAtividadesCadastradas();
        }

        private void BtnPaineis_Click(object sender, RoutedEventArgs e)
        {
            descricaoAtividadesDTO.Disciplina_Id = 200;
            LoadCombobox();
            LoadAtividadesCadastradas();
        }

        private void BtnMecanica_Click(object sender, RoutedEventArgs e)
        {
            descricaoAtividadesDTO.Disciplina_Id = 120;
            LoadCombobox();
            LoadAtividadesCadastradas();
        }

        private void BtnCadastrar_Click(object sender, RoutedEventArgs e)
        {
            if (txtDescricaoAtividade.Text.TrimStart(' ') != "")
            {
                atividadeCadastradaDTO.Atividade_id = cmbAtividade.SelectedValue.ToString();
                atividadeCadastradaDTO.Descricao = txtDescricaoAtividade.Text.Replace("'", "''").TrimStart(' ').ToUpper();
                bll.CadastrarAtividade(dto, atividadeCadastradaDTO);
                txtDescricaoAtividade.Text = "";
                LoadAtividadesCadastradas();
                CustomOKMessageBox.Show("Atividade cadastrada com sucesso.", "Sucesso!", Window.GetWindow(this));
            }
            else
            {
                CustomOKMessageBox.Show("Descrição não pode ser nula.", "Atenção!", Window.GetWindow(this));
            }
        }

        private void CbxStatus_Checked(object sender, RoutedEventArgs e)
        {
            var tgl = sender as ToggleButton;
            var index = lstAtividades.Items.IndexOf(tgl.DataContext);
            atividadeCadastradaDTO.Id = ((AtividadeCadastradaDTO)lstAtividades.Items[index]).Id;

            var obj = (ToggleButton)sender;


            if (obj.IsChecked == true)
            {
                atividadeCadastradaDTO.Habilitado = true;
                bll.Update(atividadeCadastradaDTO);
            }
            else
            {
                atividadeCadastradaDTO.Habilitado = false;
                bll.Update(atividadeCadastradaDTO);
            }
        }

        private void TxtDescricaoAtividade_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (txtDescricaoAtividade.Text.TrimStart(' ') != "")
                {
                    atividadeCadastradaDTO.Atividade_id = cmbAtividade.SelectedValue.ToString();
                    atividadeCadastradaDTO.Descricao = txtDescricaoAtividade.Text.Replace("'", "''").TrimStart(' ').ToUpper();
                    bll.CadastrarAtividade(dto, atividadeCadastradaDTO);
                    txtDescricaoAtividade.Text = "";
                    LoadAtividadesCadastradas();
                    CustomOKMessageBox.Show("Atividade cadastrada com sucesso.", "Sucesso!", Window.GetWindow(this));
                }
                else
                {
                    CustomOKMessageBox.Show("Descrição não pode ser nula.", "Atenção!", Window.GetWindow(this));
                }
            }
        }
    }
}
