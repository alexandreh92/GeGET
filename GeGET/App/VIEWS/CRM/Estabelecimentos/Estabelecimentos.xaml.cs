using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BLL;
using DTO;

namespace GeGET
{
    /// <summary>
    /// Interaction logic for Teste.xaml
    /// </summary>
    public partial class Estabelecimentos : UserControl
    {
        Helpers helpers = new Helpers();
        EstabelecimentosBLL bll = new EstabelecimentosBLL();
        EstabelecimentosDTO dto = new EstabelecimentosDTO();
        NegociosDTO Negociosdto = new NegociosDTO();

        public Estabelecimentos()
        {
            InitializeComponent();
            dto.Limite = 10;
            dto.Inicio = 0;
            dto.Pesquisa = "";
            LoadClients();
        }

        private void LoadClients()
        {
            EstablishmentControl uc;
            bll.CountRows();
            var estabelecimentos = bll.LoadEstabelecimentos();

            if (estabelecimentos.Count > 0)
            {

                foreach (EstabelecimentosDTO item in estabelecimentos)
                {
                    uc = new EstablishmentControl(item.Id, item.Razao_Social, item.Nome_Fantasia,item.Cnpj, item.Endereco, item.Cidade);
                    StackPanel1.Children.Add(uc);
                    uc.btnEditar.Click += (s, e) =>
                    {
                        MessageBox.Show("");
                    };
                    uc.btnNegocios.Click += (s, e) =>
                    {
                        Negociosdto.FromChildrenParent = true;
                        Negociosdto.ParentId = item.Id;
                        helpers.Open<Negocios>(this.GetType().Name, true);
                    };
                }

                if (dto.TotalRows > dto.Inicio && dto.RowsLeft > dto.Limite)
                {
                    ButtonAdd bt = new ButtonAdd();

                    bt.btnAdd.Click += (s, e) =>
                    {
                        dto.RowsLeft = dto.RowsLeft - dto.Limite;

                        foreach (object ctrl in StackPanel1.Children)
                        {
                            if (ctrl is ButtonAdd)
                            {
                                this.Dispatcher.BeginInvoke(new Action(delegate
                                {
                                    StackPanel1.Children.Remove(((ButtonAdd)ctrl));
                                }));
                            }
                        }
                        dto.Inicio = dto.Inicio + dto.Limite;
                        if (dto.Inicio >= dto.TotalRows)
                        {
                            dto.Inicio = dto.TotalRows;
                        }

                        LoadClients();

                    };

                    StackPanel1.Children.Add(bt);

                }

            }
            else
            {
                NoResults nr = new NoResults();
                StackPanel1.Children.Add(nr);
            }

        }

        private void CommentTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Commit();
            }
        }

        private void Commit()
        {
            dto.Pesquisa = txtProcurar.Text.Replace("'", "''");
            ClearControls();
            LoadClients();
        }

        private void ClearControls()
        {
            foreach (object ctrl in StackPanel1.Children)
            {
                if (ctrl is ButtonAdd)
                {
                    this.Dispatcher.BeginInvoke(new Action(delegate
                    {
                        StackPanel1.Children.Remove(((ButtonAdd)ctrl));
                    }));
                }
                if (ctrl is EstablishmentControl)
                {
                    this.Dispatcher.BeginInvoke(new Action(delegate
                    {
                        StackPanel1.Children.Remove(((EstablishmentControl)ctrl));
                    }));
                }
                if (ctrl is NoResults)
                {
                    this.Dispatcher.BeginInvoke(new Action(delegate
                    {
                        StackPanel1.Children.Remove(((NoResults)ctrl));
                    }));
                }
            }
        }

        private void BtnPesquisa_Click(object sender, RoutedEventArgs e)
        {
            Commit();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            helpers.Close();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (dto.FromParent)
            {
                helpers.OpenBack(true);
            }
            else
            {
                helpers.OpenBack(false);
            }
        }

        void Estabelecimentos_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Closing += Estabelecimentos_Closing;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            dto.FromParent = false;
            dto.ParentId = "";
        }
    }
}
