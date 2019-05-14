using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Windows;
using LiveCharts;
using LiveCharts.Wpf;
using System.Windows.Media;
using System.Windows.Threading;

namespace BLL
{
    class DashboardComercialBLL
    {
        AcessoBancoDados bd = new AcessoBancoDados();
        LoginDTO loginDTO = new LoginDTO();

        public ObservableCollection<DashboardComercialDTO> Load()
        {
            var dashboard = new ObservableCollection<DashboardComercialDTO>();
            var negocios = new ObservableCollection<NegociosDTO>();

            var dt = new DataTable();
            var dt_negocios = new DataTable();
            try
            {
                var query = "SELECT (SELECT COUNT(id) FROM negocio) AS orcamentos_feitos, (SELECT COALESCE(COUNT(*),0) as quantidade FROM negocio WHERE STATUS_ORCAMENTO_id < 5 AND STATUS_ORCAMENTO_ID != 0) AS orcamentos_abertos, (SELECT COALESCE(COUNT(*),0) as quantidade FROM negocio WHERE STATUS_ORCAMENTO_id = 5) as orcamentos_fechados, (SELECT COALESCE(COUNT(*),0) as quantidade FROM negocio WHERE STATUS_ORCAMENTO_id = 0) as orcamentos_cancelados, (SELECT COALESCE(COUNT(*),0) as quantidade FROM negocio WHERE STATUS_ORCAMENTO_id = 3) as orcamentos_enviados, (SELECT COALESCE(COUNT(*),0) as quantidade FROM negocio WHERE STATUS_ORCAMENTO_id = 4) as orcamentos_negociacao, (SELECT COUNT(*) FROM orcamentista_negocio orcn JOIN negocio n ON n.id = orcn.NEGOCIO_id WHERE orcn.USUARIO_id = '1' AND n.STATUS_ORCAMENTO_id = '1') as mensagem_total_fila, (SELECT COUNT(*) FROM orcamentista_negocio orcn JOIN negocio n ON n.id = orcn.NEGOCIO_id WHERE orcn.USUARIO_id = '1' AND n.STATUS_ORCAMENTO_id = '2') as mensagem_total_aberto";
                bd.Conectar();
                dt = bd.RetDataTable(query);


                string querynegocios;
                if (loginDTO.Nivel >= 6)
                {
                    querynegocios = "SELECT n.id, n.descricao, n.anotacoes, n.prazo, n.valor_fechamento, n.data, v.nome, s.descricao as status_descricao, s.id as status_id, e.cnpj, e.endereco, cid.cidade, e.id as estabelecimento_id, c.id as cliente_id, cid.uf, c.rsocial, c.fantasia FROM negocio n JOIN vendedor v ON n.VENDEDOR_id = v.id JOIN status_orcamento s ON n.STATUS_ORCAMENTO_id = s.id JOIN estabelecimento e ON n.ESTABELECIMENTO_id = e.id JOIN cidades cid ON e.CIDADES_id = cid.id JOIN cliente c ON e.CLIENTE_id = c.id WHERE n.status_orcamento_id != '0' AND n.status_orcamento_id != '5' ORDER BY n.id";
                }
                else
                {
                    querynegocios = "SELECT n.id, n.descricao, n.anotacoes, n.prazo, n.valor_fechamento, n.data, v.nome, s.descricao as status_descricao, s.id as status_id, e.cnpj, e.endereco, cid.cidade, e.id as estabelecimento_id, c.id as cliente_id, cid.uf, c.rsocial, c.fantasia FROM negocio n JOIN vendedor v ON n.VENDEDOR_id = v.id JOIN status_orcamento s ON n.STATUS_ORCAMENTO_id = s.id JOIN orcamentista_negocio orn ON orn.negocio_id = n.id JOIN estabelecimento e ON n.ESTABELECIMENTO_id = e.id JOIN cidades cid ON e.CIDADES_id = cid.id JOIN cliente c ON e.CLIENTE_id = c.id WHERE n.status_orcamento_id != '0' AND n.status_orcamento_id != '5' AND orn.usuario_id = '"+loginDTO.Id+"' ORDER BY n.id";
                    //querynegocios = "SELECT n.id, n.descricao, n.anotacoes, n.prazo, n.valor_fechamento, n.data, v.nome, s.descricao as status_descricao, s.id as status_id, e.cnpj, e.endereco, cid.cidade, e.id as estabelecimento_id, c.id as cliente_id, cid.uf, c.rsocial, c.fantasia FROM negocio n JOIN vendedor v ON n.VENDEDOR_id = v.id JOIN status_orcamento s ON n.STATUS_ORCAMENTO_id = s.id JOIN estabelecimento e ON n.ESTABELECIMENTO_id = e.id JOIN cidades cid ON e.CIDADES_id = cid.id JOIN cliente c ON e.CLIENTE_id = c.id WHERE n.status_orcamento_id != '0' AND n.status_orcamento_id != '5' ORDER BY n.id";
                }
                bd.Conectar();
                dt_negocios = bd.RetDataTable(querynegocios);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                foreach (DataRow dr in dt_negocios.Rows)
                {
                    negocios.Add(new NegociosDTO { Id = dr["id"].ToString(), Numero = "p" + Convert.ToInt32(dr["id"]).ToString("0000"), Razao_Social = dr["rsocial"].ToString(), Descricao = dr["descricao"].ToString(), Anotacoes = dr["anotacoes"].ToString(), Status = Convert.ToInt32(dr["status_id"]), Endereco = dr["endereco"].ToString() + " - " + dr["cidade"].ToString() + " - " + dr["uf"].ToString(), Vendedor = dr["nome"].ToString(), Status_Descricao = dr["status_descricao"].ToString(), Status_Id = Convert.ToInt32(dr["status_id"]), Cliente_Id = dr["cliente_id"].ToString(), Estabelecimento_Id = dr["estabelecimento_id"].ToString(), CidadeEstado = dr["cidade"].ToString() + " - " + dr["uf"].ToString(), ProgressBarValue = Convert.ToInt32(dr["status_id"]) * 20, Nome_Fantasia = dr["fantasia"].ToString(), Prazo = Convert.ToDateTime(dr["prazo"]).ToString("dd/MM/yyyy") });
                }


                int num_mensagem = Convert.ToInt32(dt.Rows[0]["mensagem_total_aberto"]);
                int num_fila = Convert.ToInt32(dt.Rows[0]["mensagem_total_fila"]);

                #region Concordância
                string conc_abertos;
                string conc_fila;
                if (num_mensagem > 1)
                {
                    conc_abertos = "orçamentos";
                }
                else
                {
                    conc_abertos = "orçamento";
                }
                if (num_fila > 1)
                {
                    conc_fila = "orçamentos";
                }
                else
                {
                    conc_fila = "orçamento";
                }
                #endregion

                string mensagem_usuario;
                if (num_mensagem > 0 && num_fila > 0)
                {
                    mensagem_usuario = "Você possui " + num_mensagem.ToString("00") + " " + conc_abertos + " em aberto e " + num_fila.ToString("00") + " " + conc_fila + " na fila.";
                }
                else if (num_mensagem > 0 && num_fila == 0)
                {
                    mensagem_usuario = "Você possui " + num_mensagem.ToString("00") + " " + conc_abertos + " em aberto e nenhum orçamento na fila.";
                }
                else if (num_mensagem == 0 && num_fila > 0)
                {
                    mensagem_usuario = "Você possui " + num_fila.ToString("00") + " " + conc_fila + " na fila.";
                }
                else
                {
                    mensagem_usuario = "Parabéns, você não possui nenhuma pendência.";
                }
                foreach (DataRow dr in dt.Rows)
                {
                    dashboard.Add(new DashboardComercialDTO { Nome_Usuario = "Olá " + char.ToUpper(loginDTO.Primeiro_Nome[0]) + loginDTO.Primeiro_Nome.Substring(1).ToLower() + ",", Nivel_Usuario = loginDTO.Nivel, Orcamento_Aberto = dr["orcamentos_abertos"].ToString(), Orcamento_Cancelado = dr["orcamentos_cancelados"].ToString(), Orcamento_Enviado = dr["orcamentos_enviados"].ToString(), Orcamento_Fechado = dr["orcamentos_fechados"].ToString(), Orcamento_Negociacao = dr["orcamentos_negociacao"].ToString(), Orcamento_Feito = dr["orcamentos_feitos"].ToString() , Mensagem = mensagem_usuario, ListaNegocios = negocios, Graph_2 = Convert.ToDouble(dr["orcamentos_fechados"])/Convert.ToDouble(dr["orcamentos_cancelados"])*100, Graph_3 = Convert.ToDouble(dr["orcamentos_fechados"])/Convert.ToDouble(dr["orcamentos_feitos"])*100, Graph_4 = Convert.ToDouble(dr["orcamentos_cancelados"]) / Convert.ToDouble(dr["orcamentos_feitos"]) * 100 });
                }
            }
            return dashboard;
        }

        public DashboardChartDTO LoadCharts() 
        {
            var chart = new DashboardChartDTO();
            var dt = new DataTable();
            var abertos = new ChartValues<int>();
            var feitos = new ChartValues<int>();
            var produtividade = new ChartValues<double>();
            var labels = new List<string>();
            double soma_feitos = 0;

            try
            {
                var query = "SELECT t1.month as month, COUNT(t2.id) as abertos, COUNT(t3.id) as feitos FROM (SELECT 1 AS mnum, 'Jan' AS month UNION ALL SELECT 2, 'Feb'  UNION ALL SELECT 3, 'Mar'  UNION ALL SELECT 4, 'Apr'  UNION ALL SELECT 5, 'May'  UNION ALL SELECT 6, 'Jun'  UNION ALL SELECT 7, 'Jul'  UNION ALL SELECT 8, 'Aug'  UNION ALL SELECT 9, 'Sep'  UNION ALL SELECT 10, 'Oct'  UNION ALL SELECT 11, 'Nov'  UNION ALL SELECT 12, 'Dec') t1 CROSS JOIN (SELECT DISTINCT YEAR(data) AS year FROM negocio) y LEFT JOIN negocio t2 ON t1.mnum = MONTH(t2.data) AND y.year = YEAR(t2.data) AND t2.data BETWEEN CURDATE() - INTERVAL 3 MONTH AND CURDATE() LEFT JOIN negocio t3 ON t2.id = t3.id AND t3.data_envio is not null WHERE STR_TO_DATE(CONCAT_WS('-', y.year, t1.month, DAY(CURDATE())), '%Y-%b-%d') BETWEEN CURDATE() -INTERVAL 3 MONTH AND CURDATE() GROUP BY y.year, t1.month, t1.mnum ORDER BY y.year, t1.mnum";
                bd.Conectar();
                dt = bd.RetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                foreach (DataRow dr in dt.Rows)
                {
                    abertos.Add(Convert.ToInt32(dr["abertos"]));
                    feitos.Add(Convert.ToInt32(dr["feitos"]));
                    if (Convert.ToInt32(dr["abertos"]) == 0)
                    {
                        produtividade.Add(0);
                    }
                    else
                    {
                        produtividade.Add(Math.Round(Convert.ToDouble(dr["feitos"]) / Convert.ToDouble(dr["abertos"]),2));
                    }
                    soma_feitos = soma_feitos + Convert.ToDouble(dr["feitos"]);
                    labels.Add(ParseMonth(dr["month"].ToString()));
                }

                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    chart.SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Executados",
                    Values = feitos,
                    Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#907A2932")),
                    Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7A2932")),
                    PointGeometry = null

                },
                new LineSeries
                {
                    Title = "Solicitados",
                    Values = abertos,
                    Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#77000000")),
                    Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000")),
                    PointGeometry = null
                },
                new LineSeries
                {
                    Title = "Produtividade",
                    Values = produtividade,
                    Fill = new SolidColorBrush(Colors.Transparent),
                    ScalesYAt = 1,
                    Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#404040")),


                }
            };
                }));

                
                chart.Labels = labels;
                chart.Media = soma_feitos / 4;
            }
            return chart;
        }


        private string ParseMonth(string month)
        {
            string parsedmonth = "";
            switch (month)
            {
                case "Jan":
                    parsedmonth = "Jan";
                    break;
                case "Feb":
                    parsedmonth = "Fev";
                    break;
                case "Mar":
                    parsedmonth = "Mar";
                    break;
                case "Apr":
                    parsedmonth = "Abr";
                    break;
                case "May":
                    parsedmonth = "Mai";
                    break;
                case "Jun":
                    parsedmonth = "Jun";
                    break;
                case "Jul":
                    parsedmonth = "Jul";
                    break;
                case "Aug":
                    parsedmonth = "Ago";
                    break;
                case "Sep":
                    parsedmonth = "Set";
                    break;
                case "Oct":
                    parsedmonth = "Out";
                    break;
                case "Nov":
                    parsedmonth = "Nov";
                    break;
                case "Dec":
                    parsedmonth = "Dez";
                    break;
            }
            return parsedmonth;
        }
    }
}
