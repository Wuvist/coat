using Dapper;
using Dapper.Contrib.Extensions;
using System.Data.SqlClient;
using System.Configuration;

namespace Coat
{
    public abstract class RecoadBase<T> where T : RecoadBase<T>
    {
        private static string ConnStr;
        public static void SetConn(string connStr)
        {
            ConnStr = connStr;
        }

        public static SqlConnection OpenConnection()
        {
            if (string.IsNullOrEmpty(ConnStr))
            {
                ConnStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            }
            SqlConnection connection = new SqlConnection(ConnStr);
            connection.Open();
            return connection;
        }

        public static T Get(string id)
        {
            using (var conn = OpenConnection())
            {
                return conn.Get<T>(id);
            }
        }

        public bool Update()
        {
            using (var conn = OpenConnection())
            {
                return conn.Update<T>((T)this);
            }
        }
    }
}
