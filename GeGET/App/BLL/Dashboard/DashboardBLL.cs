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
                    //querynegocios = "SELECT n.id, n.descricao, n.anotacoes, n.prazo, n.valor_fechamento, n.data, v.nome, s.descricao as status_descricao, s.id as status_id, e.cnpj, e.endereco, cid.cidade, e.id as estabelecimento_id, c.id as cliente_id, cid.uf, c.rsocial, c.fantasia FROM negocio n JOIN vendedor v ON n.VENDEDOR_id = v.id JOIN status_orcamento s ON n.STATUS_ORCAMENTO_id = s.id JOIN orcamentista_negocio orn ON orn.negocio_id = n.id JOIN estabelecimento e ON n.ESTABELECIMENTO_id = e.id JOIN cidades cid ON e.CIDADES_id = cid.id JOIN cliente c ON e.CLIENTE_id = c.id WHERE n.status_orcamento_id != '0' AND n.status_orcamento_id != '5' AND orn.usuario_id = '"+loginDTO.Id+"' ORDER BY n.id";
                    querynegocios = "SELECT n.id, n.descricao, n.anotacoes, n.prazo, n.valor_fechamento, n.data, v.nome, s.descricao as status_descricao, s.id as status_id, e.cnpj, e.endereco, cid.cidade, e.id as estabelecimento_id, c.id as cliente_id, cid.uf, c.rsocial, c.fantasia FROM negocio n JOIN vendedor v ON n.VENDEDOR_id = v.id JOIN status_orcamento s ON n.STATUS_ORCAMENTO_id = s.id JOIN estabelecimento e ON n.ESTABELECIMENTO_id = e.id JOIN cidades cid ON e.CIDADES_id = cid.id JOIN cliente c ON e.CLIENTE_id = c.id WHERE n.status_orcamento_id != '0' AND n.status_orcamento_id != '5' ORDER BY n.id";
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
            var dtAbertos = new DataTable();
            var dtFeitos = new DataTable();
            var abertos = new ChartValues<int>();
            var feitos = new ChartValues<int>();
            var labels = new List<string>();
            double soma_feitos = 0;

            try
            {
                var query = "SELECT m as mes, COUNT(`data`) AS Total FROM(SELECT year(now()) AS y UNION ALL SELECT year(now()) - 1 AS y) `years` CROSS JOIN(SELECT  1 AS m UNION ALL SELECT  2 AS m UNION ALL SELECT  3 AS m UNION ALL SELECT  4 AS m UNION ALL SELECT  5 AS m UNION ALL SELECT  6 AS m UNION ALL SELECT  7 AS m UNION ALL SELECT  8 AS m UNION ALL SELECT  9 AS m UNION ALL SELECT 10 AS m UNION ALL SELECT 11 AS m UNION ALL SELECT 12 AS m) `months` LEFT JOIN `negocio` q ON YEAR(`data`) = y AND MONTH(`data`) = m WHERE STR_TO_DATE(CONCAT(y, '-', m, '-01'), '%Y-%m-%d') >= MAKEDATE(year(now() - interval 1 year), 1) + interval 13 month AND STR_TO_DATE(CONCAT(y, '-', m, '-01'), '%Y-%m-%d') <= now() GROUP BY y, m ORDER BY y, m";
                bd.Conectar();
                dtAbertos = bd.RetDataTable(query);
                var queryfeitos = "SELECT m as mes, COUNT(`data`) AS Total FROM(SELECT year(now()) AS y UNION ALL SELECT year(now()) - 1 AS y) `years` CROSS JOIN(SELECT  1 AS m UNION ALL SELECT  2 AS m UNION ALL SELECT  3 AS m UNION ALL SELECT  4 AS m UNION ALL SELECT  5 AS m UNION ALL SELECT  6 AS m UNION ALL SELECT  7 AS m UNION ALL SELECT  8 AS m UNION ALL SELECT  9 AS m UNION ALL SELECT 10 AS m UNION ALL SELECT 11 AS m UNION ALL SELECT 12 AS m) `months` LEFT JOIN `negocio` q ON YEAR(`data`) = y AND MONTH(`data`) = m AND (`STATUS_ORCAMENTO_id`) > 2 WHERE STR_TO_DATE(CONCAT(y, '-', m, '-01'), '%Y-%m-%d') >= MAKEDATE(year(now() - interval 1 year), 1) + interval 13 month AND STR_TO_DATE(CONCAT(y, '-', m, '-01'), '%Y-%m-%d') <= now() GROUP BY y, m ORDER BY y, m";
                bd.Conectar();
                dtFeitos = bd.RetDataTable(queryfeitos);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                foreach (DataRow dr in dtAbertos.Rows)
                {
                    abertos.Add(Convert.ToInt32(dr["total"]));
                    labels.Add(ParseMonth(dr["mes"].ToString()));
                }
                foreach (DataRow dr in dtFeitos.Rows)
                {
                    feitos.Add(Convert.ToInt32(dr["total"]));
                    soma_feitos = soma_feitos + Convert.ToDouble(dr["total"]);
                }

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
                }

            };
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
                case "1":
                    parsedmonth = "Jan";
                    break;
                case "2":
                    parsedmonth = "Fev";
                    break;
                case "3":
                    parsedmonth = "Mar";
                    break;
                case "4":
                    parsedmonth = "Abr";
                    break;
                case "5":
                    parsedmonth = "Mai";
                    break;
                case "6":
                    parsedmonth = "Jun";
                    break;
                case "7":
                    parsedmonth = "Jul";
                    break;
                case "8":
                    parsedmonth = "Ago";
                    break;
                case "9":
                    parsedmonth = "Set";
                    break;
                case "10":
                    parsedmonth = "Out";
                    break;
                case "11":
                    parsedmonth = "Nov";
                    break;
                case "12":
                    parsedmonth = "Dez";
                    break;
            }
            return parsedmonth;
        }
    }
}
