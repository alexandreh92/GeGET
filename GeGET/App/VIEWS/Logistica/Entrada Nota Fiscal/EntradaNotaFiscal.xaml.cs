using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BLL;
using DTO;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Threading;
using MMLib.Extensions;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.Xpf.Grid;
using Microsoft.Win32;
using System.Xml;
using System.Threading.Tasks;

namespace GeGET
{
    public partial class EntradaNotaFiscal : UserControl
    {
        #region Declarations
        Helpers helpers = new Helpers();
        XmlDocument xmlDocument;
        EntradaNotaFiscalBLL bll = new EntradaNotaFiscalBLL();
        EntradaNotaFiscalDTO dto = new EntradaNotaFiscalDTO();
        public ObservableCollection<EntradaNotaFiscalDTO> listaEntrada;
        ObservableCollection<EntradaNotaFiscalDTO> listaEntradaEstoque;
        #endregion

        #region Initialize
        public EntradaNotaFiscal()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods

        #endregion

        #region Events

        #region Clicks

        #region Botão Voltar
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            helpers.OpenBack(false);
        }
        #endregion

        #region Botão Fechar
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            helpers.Close();
        }
        #endregion

        #endregion

        #endregion

        private void ImportarXML_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Selecione uma Nota Fiscal XML";
            op.Filter = "Extensible Markup Language (*.xml)|*.xml";
            if (op.ShowDialog() == true)
            { 
                XMLReader(op.FileName);
            }
        }

        private async void XMLReader(string FileName)
        {
            WaitBox wb = new WaitBox();
            wb.Owner = Window.GetWindow(this);
            wb.Show();

            xmlDocument = new XmlDocument();
            xmlDocument.Load(FileName);
            listaEntrada = new ObservableCollection<EntradaNotaFiscalDTO>();

            bool iNotify = false;

            string codigo_acesso = xmlDocument.GetElementsByTagName("chNFe")[0].InnerText;
            string numero = xmlDocument.GetElementsByTagName("nNF")[0].InnerText;
            
            XmlNodeList prod = xmlDocument.GetElementsByTagName("prod");
            int id = 0;

            foreach (XmlElement node in prod)
            {
                string codigo_getac = "";
                var codigo = node.GetElementsByTagName("cProd")[0].InnerText;
                var descricao = node.GetElementsByTagName("xProd")[0].InnerText;
                var unidade = node.GetElementsByTagName("uCom")[0].InnerText;
                var quantidade = Convert.ToDouble(node.GetElementsByTagName("qCom")[0].InnerText.Replace(".", ","));
                var valor_unitario = Convert.ToDouble(node.GetElementsByTagName("vUnCom")[0].InnerText.Replace(".", ","));
                string fabricante = xmlDocument.GetElementsByTagName("xNome")[0].InnerText;

                #region TRATAMENTO DE UNIDADE
                if (unidade.ToLower() == "km")
                {
                    unidade = "m";
                    quantidade = quantidade * 1000;
                    valor_unitario = valor_unitario / 1000;
                }
                if (unidade.ToLower() == "ct")
                {
                    unidade = "un";
                    quantidade = quantidade * 100;
                    valor_unitario = valor_unitario / 100;
                }
                #endregion


                dto.Codigo = codigo;

                ObservableCollection<EntradaNotaFiscalDTO> findproduto = new ObservableCollection<EntradaNotaFiscalDTO>();

                await Task.Run(() => 
                {
                    findproduto = bll.FindProduto(dto);
                });

                bool ispresent = false;
                if (findproduto != null)
                {
                    descricao = findproduto.First().Descricao;
                    unidade = findproduto.First().Unidade;
                    codigo_getac = findproduto.First().Codigo_Getac;
                    fabricante = findproduto.First().Fabricante;
                    findproduto.First().Nota = numero;
                    await Task.Run(() => 
                    {
                        ispresent = bll.isPresent(findproduto.First());
                    });
                }
                else
                {
                    iNotify = true;
                    ispresent = false;
                }

                if (!ispresent)
                {
                    listaEntrada.Add(new EntradaNotaFiscalDTO
                    {
                        Id = id,
                        Codigo = codigo,
                        Descricao = descricao,
                        Unidade = unidade,
                        Quantidade = quantidade,
                        Custo = valor_unitario,
                        Nota = numero,
                        Fabricante = fabricante,
                        Chave_Acesso = codigo_acesso,
                        Codigo_Getac = codigo_getac
                    });
                    id++;
                }                
            }

            grdItens.ItemsSource = listaEntrada;
            wb.Close();
            
            if (iNotify)
            {
                CustomOKMessageBox.Show("Alguns itens não foram identificados no sistema.\nAlguns itens deverão ser linkados aos seus respectivos produtos cadastrados no sistema.","Atenção!",Window.GetWindow(this));
            }
        }

        private void BtnAtrelarProduto_Click(object sender, RoutedEventArgs e)
        {
            var handle = grdItens.GetFocusedRow();
            dto = handle as EntradaNotaFiscalDTO;
            if (dto.Codigo_Getac != "")
            {
                var result = CustomOKCancelMessageBox.Show("Este item já está linkado. Se você deseja linkar este produto novamente, o link anterior será apagado.\nDeseja continuar?", "Atenção!", Window.GetWindow(this));
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    using (var form = new LinkarProdutoNotaFiscal(dto))
                    {
                        form.Owner = Window.GetWindow(this);
                        form.ShowDialog();
                        if (form.DialogResult.HasValue && form.DialogResult.Value)
                        {
                            grdItens.SetFocusedRowCellValue("Codigo_Getac", form.Codigo_Getac);
                            grdItens.SetFocusedRowCellValue("Descricao", form.Descricao);
                            grdItens.SetFocusedRowCellValue("Fabricante", form.Fabricante);
                        }
                    }
                }
            }
            else
            {
                using (var form = new LinkarProdutoNotaFiscal(dto))
                {
                    form.Owner = Window.GetWindow(this);
                    form.ShowDialog();
                    if (form.DialogResult.HasValue && form.DialogResult.Value)
                    {
                        grdItens.SetFocusedRowCellValue("Codigo_Getac", form.Codigo_Getac);
                        grdItens.SetFocusedRowCellValue("Descricao", form.Descricao);
                        grdItens.SetFocusedRowCellValue("Fabricante", form.Fabricante);
                    }
                }
            }
        }

        private async void AdicionarEstoque_Click(object sender, RoutedEventArgs e)
        {
            int[] handles = grdItens.GetSelectedRowHandles();
            if (handles.Length > 0)
            {
                bool isLinked = true;
                foreach (var rowHandle in handles)
                {
                    var selectedItem = grdItens.GetRow(rowHandle) as EntradaNotaFiscalDTO;
                    if (selectedItem.Codigo_Getac == "")
                    {
                        isLinked = false;
                    }
                }
                if (isLinked)
                {
                    var result = CustomOKCancelMessageBox.Show("Deseja mesmo dar entrada em estoque dos produtos selecionados?", "Atenção!", Window.GetWindow(this));
                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        listaEntradaEstoque = new ObservableCollection<EntradaNotaFiscalDTO>();
                        foreach (var rowHandle in handles)
                        {
                            var selectedItem = grdItens.GetRow(rowHandle) as EntradaNotaFiscalDTO;
                            listaEntradaEstoque.Add(selectedItem);
                        }
                        await Task.Run(() =>
                        {
                            bll.InserirEstoque(listaEntradaEstoque);
                        });
                        CustomOKMessageBox.Show("Itens selecionados adicionados ao estoque com sucesso!", "Sucesso!", Window.GetWindow(this));
                        if (grdItens.SelectedItems.Count != 0)
                        {
                            List<int> selectedRowHandles = new List<int>(grdItens.GetSelectedRowHandles());
                            var descendingOrder = selectedRowHandles.OrderByDescending(i => i);
                            grdItens.BeginDataUpdate();
                            foreach (int i in descendingOrder)
                            {
                                grdView.DeleteRow(i);
                            }
                            grdItens.EndDataUpdate();
                        }
                    }
                }
                else
                {
                    CustomOKMessageBox.Show("Há itens que não possuem link com o sistema selecionados.", "Atenção!", Window.GetWindow(this));
                }
            }
        }
    }
}
