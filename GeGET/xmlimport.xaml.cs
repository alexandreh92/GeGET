using Microsoft.Win32;
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
using System.Xml;

namespace GeGET
{
    /// <summary>
    /// Interaction logic for xmlimport.xaml
    /// </summary>
    public partial class xmlimport : Window
    {
        string FileName;
        XmlDocument xmlDocument;

        public xmlimport()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Selecione uma Nota Fiscal XML";
            op.Filter = "Extensible Markup Language (*.xml)|*.xml";
            if (op.ShowDialog() == true)
            {
                FileName = op.FileName;
                XMLReader();
            }
        }

        private void XMLReader()
        {
            xmlDocument = new XmlDocument();
            xmlDocument.Load(FileName);

            string codigo_acesso = xmlDocument.GetElementsByTagName("chNFe")[0].InnerText;
            string numero = xmlDocument.GetElementsByTagName("nNF")[0].InnerText;
            string fabricante = xmlDocument.GetElementsByTagName("xNome")[0].InnerText;
            XmlNodeList prod = xmlDocument.GetElementsByTagName("prod");

            foreach (XmlElement node in prod)
            {
                var codigo = node.GetElementsByTagName("cProd")[0].InnerText;
                var descricao = node.GetElementsByTagName("xProd")[0].InnerText;
                var unidade = node.GetElementsByTagName("uCom")[0].InnerText;
                var quantidade = node.GetElementsByTagName("qCom")[0].InnerText;
                var valor_unitario = node.GetElementsByTagName("vProd")[0].InnerText;
                //procurar produto por codigo de barra,
                //se retornar verdadeiro, inserir o produto,
                //caso volte vazio,
                //procurar produto por partnumber,
                //se retornar verdadeiro, adicionar produto,
                //se retornar vazio, mensagem falando que nao está cadastrado ou não conseguiu fazer o procedimento.
            }

        }

    }
}
