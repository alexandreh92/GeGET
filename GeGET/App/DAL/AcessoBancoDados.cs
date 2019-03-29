using System;
using System.Data;
using MySql.Data.MySqlClient;
using Devart.Data.MySql;
using System.Collections.Generic;
using Helpers;
using System.Threading.Tasks;

namespace DAL
{
    class AcessoBancoDados
    {

        public MySql.Data.MySqlClient.MySqlConnection comm;
        public Devart.Data.MySql.MySqlConnection com;
        private DataTable data;
        private DataSet ds;
        private MySql.Data.MySqlClient.MySqlDataAdapter da;
        private MySql.Data.MySqlClient.MySqlCommandBuilder cb;

        public static string server_local = "localhost";
        //public static string server_local = "192.168.15.5";
        public static string server_remoto = "getacengenharia.ddns.net";
        //public static string server = "192.168.15.5";
        public static string server = "localhost";
        public static string user = "root";
        public static string password = "root";
        public static string database = "gegetdb";
        readonly string comStrDev = String.Format("server={0}; user id={1}; password={2}; database={3}; pooling=false;", server, user, password, database);
        string commStr = String.Format("server={0}; user id={1}; password={2}; database={3}; pooling=false; Allow User Variables = true", server, user, password, database);

        public void Conectar()
        {
            if (comm != null)
                comm.Close();
            try
            {
                comm = new MySql.Data.MySqlClient.MySqlConnection(commStr);
                comm.Open();
            }

            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public void ConectarDevArt()
        {
            if (com != null)
                com.Close();

            try
            {
                com = new Devart.Data.MySql.MySqlConnection(comStrDev);
                com.Open();
            }

            catch (Devart.Data.MySql.MySqlException ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public void ExecutarComandoSQL(string comandoSql)
        {
            MySql.Data.MySqlClient.MySqlCommand comando = new MySql.Data.MySqlClient.MySqlCommand(comandoSql, comm);
            
            comando.ExecuteNonQuery();
            comm.Close();
        }

        public void ExecutarComandoSQLWithByteParameter(string comandoSql, byte[] photo)
        {
            MySql.Data.MySqlClient.MySqlCommand comando = new MySql.Data.MySqlClient.MySqlCommand(comandoSql, comm);
            comando.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@IMG", photo));
            comando.ExecuteNonQuery();
            comm.Close();
            
        }

        public async Task ExecutarComandoSQLAsync(string comandoSql)
        {
            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(commStr);
            await conn.OpenAsync();
            MySql.Data.MySqlClient.MySqlCommand comando = new MySql.Data.MySqlClient.MySqlCommand(comandoSql, conn);
            await comando.ExecuteNonQueryAsync();
            await conn.CloseAsync();
        }

        public DataTable RetDataTable(string sql)
        {
            data = new DataTable();
            da = new MySql.Data.MySqlClient.MySqlDataAdapter(sql, comm);
            cb = new MySql.Data.MySqlClient.MySqlCommandBuilder(da);
            da.Fill(data);
            return data;
        }

        public DataSet RetDataSet(List<QueryHelper> list)
        {
            ds = new DataSet();
            foreach (QueryHelper item in list)
            {
                da = new MySql.Data.MySqlClient.MySqlDataAdapter(item.Sql, comm);
                cb = new MySql.Data.MySqlClient.MySqlCommandBuilder(da);
                da.Fill(ds, item.Table);
            }
            return ds;
        }

        public MySql.Data.MySqlClient.MySqlDataReader RetDataReader(string sql)
        {
            MySql.Data.MySqlClient.MySqlCommand comando = new MySql.Data.MySqlClient.MySqlCommand(sql, comm);
            MySql.Data.MySqlClient.MySqlDataReader dr = comando.ExecuteReader();
            dr.Read();
            return dr;
        }

        public MySqlDependency Dependency(string comandoSql)
        {
            Devart.Data.MySql.MySqlCommand comando = new Devart.Data.MySql.MySqlCommand(comandoSql, com);
            MySqlDependency dependency = new MySqlDependency(comando, 1000);
            return dependency;
        }

        public void StartDependency()
        {
            MySqlDependency.Start(com);
        }

        public void StopDependency()
        {
            MySqlDependency.Stop(comStrDev);
        }

        public void CloseConection()
        {
            comm.Close();
        }

    }
}