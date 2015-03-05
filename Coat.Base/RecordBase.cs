using Coat.Base.WhereClauseBuilders;
using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace Coat.Base
{
    public abstract class RecordBase<T, PrimaryKeyType> where T : RecordBase<T, PrimaryKeyType>, new()
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

        public static T GetByID(PrimaryKeyType id)
        {
            using (var conn = OpenConnection())
            {
                return conn.Get<T>(id);
            }
        }

        public static List<T> GetByIDs(List<PrimaryKeyType> ids)
        {
            using (var conn = OpenConnection())
            {
                var sql = "select * from " + TableName + " where " + PrimaryKey + " in @ids";

                return conn.Query<T>(sql, new { ids = ids }).ToList();
            }
        }

        public static List<T> Find(Expression<Func<T, bool>> where)
        {
            using (var conn = OpenConnection())
            {
                IWhereClauseBuilder<T> whereClause = new SqlWhereClauseBuilder<T>();
                var clause = whereClause.BuildWhereClause(where);
                var sql = "select * from " + TableName + " where " + clause.WhereClause;
                return conn.Query<T>(sql, clause.ParameterValues).ToList();
            }
        }

        public static List<T> FindWithSql(string where, object param = null)
        {
            using (var conn = OpenConnection())
            {
                var sql = "select * from " + TableName + " where " + where;
                return conn.Query<T>(sql, param).ToList();
            }
        }

        public static T FindOne(Expression<Func<T, bool>> where)
        {
            using (var conn = OpenConnection())
            {
                IWhereClauseBuilder<T> whereClause = new SqlWhereClauseBuilder<T>();
                var clause = whereClause.BuildWhereClause(where);
                var sql = "select top 1 * from " + TableName + " where " + clause.WhereClause;
                return conn.Query<T>(sql, clause.ParameterValues).FirstOrDefault();
            }
        }

        public static T FindOneWithSql(string where, object param = null)
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
