﻿namespace DTO
{
    class LayoutDTO
    {
        private string id;
        private string nome;
        private string descricao;
        private string mensagem;
        private string negocio;
        private int lida;
        private string data;
        private static int quantidade;

        public string Id { get => id; set => id = value; }
        public string Nome { get => nome; set => nome = value; }
        public string Descricao { get => descricao; set => descricao = value; }
        public string Mensagem { get => mensagem; set => mensagem = value; }
        public string Negocio { get => negocio; set => negocio = value; }
        public int Lida { get => lida; set => lida = value; }
        public string Data { get => data; set => data = value; }
        public int Quantidade { get => quantidade; set => quantidade = value; }
        
    }
}
