using System;
using System.Data;
using MySql.Data.MySqlClient;
using Devart.Data.MySql;
using System.Collections.Generic;
using Helpers;
using System.Threading.Tasks;

namespace DAL
{
    class AcessoBancoDados : IDisposable
    {
        bool disposed = false;

        public MySql.Data.MySqlClient.MySqlConnection comm;
        public Devart.Data.MySql.MySqlConnection com;
        private DataTable data;
        private DataSet ds;
        private MySql.Data.MySqlClient.MySqlDataAdapter da;
        private MySql.Data.MySqlClient.MySqlCommandBuilder cb;

        //public static string server_local = "localhost";
        //public static string server_local = "192.168.15.5";
        //public static string server_local = "getacengenharia.ddns.net";
        //public static string server_remoto = "getacengenharia.ddns.net";
        public static string server = "192.168.15.5";
        //public static string server = "getacengenharia.ddns.net";
        //public static string server = "localhost";
        public static string user = "root";
        public static string password = "TwukASH_05";
        //public static string password = "root";
        public static string database = "gegetdb";
        readonly string comStrDev = String.Format("server={0}; user id={1}; password={2}; database={3}; pooling=false;", server, user, password, database);
        readonly string commStr = String.Format("server={0}; user id={1}; password={2}; database={3}; pooling=false; Allow User Variables = true;SslMode=none", server, user, password, database);

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public void ExecutarComandoSQL(string comandoSql)
        {
            try
            {
                MySql.Data.MySqlClient.MySqlCommand comando = new MySql.Data.MySqlClient.MySqlCommand(comandoSql, comm);
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                comm.Close();
            }
            
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public DataTable RetDataTable(string sql)
        {
            data = new DataTable();
            try
            {
                da = new MySql.Data.MySqlClient.MySqlDataAdapter(sql, comm);
                cb = new MySql.Data.MySqlClient.MySqlCommandBuilder(da);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                da.Fill(data);
            }
            return data;
        }

        public async Task<DataTable> RetDataTableAsync(string sql)
        {
            var datatable = new DataTable();

            using (var con = new MySql.Data.MySqlClient.MySqlConnection(commStr))
            {
                using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, con))
                {
                    await con.OpenAsync();
                    cmd.CommandType = CommandType.Text;
                    var sda = new MySql.Data.MySqlClient.MySqlDataAdapter(cmd);
                    await sda.FillAsync(datatable);
                }
            }

            return datatable;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public MySql.Data.MySqlClient.MySqlDataReader RetDataReader(string sql)
        {
            MySql.Data.MySqlClient.MySqlCommand comando = new MySql.Data.MySqlClient.MySqlCommand(sql, comm);
            MySql.Data.MySqlClient.MySqlDataReader dr = comando.ExecuteReader();
            dr.Read();
            return dr;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public MySqlDependency Dependency(string comandoSql)
        {
            MySqlDependency dependency;
            try
            {
                Devart.Data.MySql.MySqlCommand comando = new Devart.Data.MySql.MySqlCommand(comandoSql, com);
                dependency = new MySqlDependency(comando, 1000);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                if (data!=null)
                {
                    data.Dispose();
                }
                if (ds!=null)
                {
                    ds.Dispose();
                }
                if (da!=null)
                {
                    da.Dispose();
                }
                if (cb!=null)
                {
                    cb.Dispose();
                }
                if (com!=null)
                {
                    com.Dispose();
                }
                if (comm!= null)
                {
                    comm.Dispose();
                }
            }
            disposed = true;
        }


    }
}