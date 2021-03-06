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
            public bool NULLABLE { get; set; }
        }

        public class Table
        {
            public List<Column> Columns { get; set; }
            public Column PrimayColumn { get; set; }
            public string PrimaryKey { get; set; }
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

        public string GetPrimaryKey(string tableName)
        {
            using (var conn = OpenConnection())
            {
                var sql = @"SELECT Col.Column_Name from 
                                INFORMATION_SCHEMA.TABLE_CONSTRAINTS Tab, 
                                INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE Col 
                            WHERE 
                                Col.Constraint_Name = Tab.Constraint_Name
                                AND Col.Table_Name = Tab.Table_Name
                                AND Constraint_Type = 'PRIMARY KEY'
                                AND Col.Table_Name = @table_name";
                return conn.Query<string>(sql, new { table_name = tableName }).First();
            }
        }

        public Table GetTable(string tableName)
        {
            using (var conn = OpenConnection())
            {
                var result = new Table();
                result.Columns = GetColumns(tableName);
                result.PrimaryKey = GetPrimaryKey(tableName);
                foreach (var column in result.Columns)
                {
                    if (result.PrimaryKey == column.COLUMN_NAME)
                    {
                       result.PrimayColumn = column;
                       break;
                    }
                }
                if (result.PrimayColumn == null) {
                    throw new Exception(tableName + " has no primary key");
                }

                return result;
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
