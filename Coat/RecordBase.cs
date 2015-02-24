using Dapper;
using Dapper.Contrib.Extensions;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Coat
{
    public abstract class RecordBase<T> where T : RecordBase<T>, new()
    {
        private static string ConnStr;
        public static void SetConn(string connStr)
        {
            ConnStr = connStr;
        }

        protected static string TableName;
        protected static string PrimaryKey;

        protected static SqlConnection OpenConnection()
        {
            if (string.IsNullOrEmpty(ConnStr))
            {
                ConnStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            }
            SqlConnection connection = new SqlConnection(ConnStr);
            connection.Open();
            return connection;
        }

        static RecordBase()
        {
            // This is the ensure sub-class static constructor will be run
            // i.e. TableName / PrimayKey etc will be set
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(T).TypeHandle);
        }

        public static List<T> GetByIDs(string[] ids)
        {
            using (var conn = OpenConnection())
            {
                var sql = "select * from " + TableName + " where " + PrimaryKey + " in [@ids]";
                var dynParms = new DynamicParameters();
                dynParms.Add("@ids", ids);

                return conn.Query<T>(sql, ids).ToList();
            }
        }

        public static List<T> Find(string where, object param = null)
        {
            using (var conn = OpenConnection())
            {
                var sql = "select * from " + TableName + " where " + where;
                return conn.Query<T>(sql, param).ToList();
            }
        }

        public static T FindOne(string where, object param = null)
        {
            using (var conn = OpenConnection())
            {
                var sql = "select top 1 * from " + TableName + " where " + where;
                return conn.Query<T>(sql, param).FirstOrDefault();
            }
        }

        public static List<T> FindAll()
        {
            using (var conn = OpenConnection())
            {
                var sql = "select * from " + TableName;
                return conn.Query<T>(sql).ToList();
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
