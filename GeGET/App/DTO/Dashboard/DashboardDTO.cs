﻿using LiveCharts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DTO
{
    public class DashboardComercialDTO
    {
        private int nivel_Usuario;
        private string nome_Usuario;
        private string mensagem;
        private double orcamento_Feito;
        private double orcamento_Aberto;
        private double orcamento_Enviado;
        private double orcamento_Negociacao;
        private double orcamento_Fechado;
        private double orcamento_Cancelado;
        private double graph_1;
        private double graph_2;
        private double graph_3;
        private double graph_4;
        private string mensagem_Total_Fila;
        private string mensagem_Total_Aberto;
        private ObservableCollection<NegociosDTO> listaNegocios;
        private Func<double, string> formatter;

        public Func<double, string> Formatter { get => formatter; set => formatter = value; }
        public int Nivel_Usuario { get => nivel_Usuario; set => nivel_Usuario = value; }
        public string Nome_Usuario { get => nome_Usuario; set => nome_Usuario = value; }
        public string Mensagem { get => mensagem; set => mensagem = value; }
        public double Orcamento_Aberto { get => orcamento_Aberto; set => orcamento_Aberto = value; }
        public double Orcamento_Enviado { get => orcamento_Enviado; set => orcamento_Enviado = value; }
        public double Orcamento_Negociacao { get => orcamento_Negociacao; set => orcamento_Negociacao = value; }
        public double Orcamento_Fechado { get => orcamento_Fechado; set => orcamento_Fechado = value; }
        public double Orcamento_Cancelado { get => orcamento_Cancelado; set => orcamento_Cancelado = value; }
        public double Graph_1 { get => graph_1; set => graph_1 = value; }
        public double Graph_2 { get => graph_2; set => graph_2 = value; }
        public double Graph_3 { get => graph_3; set => graph_3 = value; }
        public double Graph_4 { get => graph_4; set => graph_4 = value; }
        public string Mensagem_Total_Fila { get => mensagem_Total_Fila; set => mensagem_Total_Fila = value; }
        public string Mensagem_Total_Aberto { get => mensagem_Total_Aberto; set => mensagem_Total_Aberto = value; }
        public ObservableCollection<NegociosDTO> ListaNegocios { get => listaNegocios; set => listaNegocios = value; }
        public double Orcamento_Feito { get => orcamento_Feito; set => orcamento_Feito = value; }
    }

    public class DashboardChartDTO
    {
                private SeriesCollection seriesViews;
        private List<string> labels;
        private double media;


        public SeriesCollection SeriesCollection { get => seriesViews; set => seriesViews = value; }
        public List<string> Labels { get => labels; set => labels = value; }
        public double Media { get => media; set => media = value; }
        
    }

}
