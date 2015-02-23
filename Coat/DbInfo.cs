using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Coat
{
    public class DbInfo
    {
        public string Conn { get; set; }

        public DbInfo(string conn)
        {
            this.Conn = conn;
        }

        public class Column
        {
            public string COLUMN_NAME { get; set; }
            public string TYPE_NAME { get; set; }
            public string IS_NULLABLE { get; set; }
        }

        public SqlConnection OpenConnection()
        {
            SqlConnection connection = new SqlConnection(this.Conn);
            connection.Open();
            return connection;
        }

        public List<Column> GetColumns(string tableName)
        {
            using (var conn = OpenConnection())
            {
                var result = conn.Query<Column>("sp_columns", new { table_name = tableName }, commandType: CommandType.StoredProcedure);
                return result.ToList<Column>();
            }
        }

        public List<string> GetAllTableNames()
        {
            var result = new List<string>();
            using (var conn = OpenConnection())
            {
                var reader = conn.ExecuteReader("SELECT name FROM sys.Tables");
                while (reader.Read())
                {
                    result.Add(reader.GetString(0));
                }
            }
            return result;
        }
    }
}
