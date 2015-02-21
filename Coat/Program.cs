using System;
using Dapper;
using Dapper.Contrib.Extensions;
using Dapper.Rainbow;
using System.Collections.Generic;
using System.Transactions;
using System.Text;
using System.Data.SqlClient;

namespace Coat
{
    [Table("ELMAH_Error")]
    class AdminInfo
    {
        [Key]
        public Guid ErrorId { get; set; }
        public string Message { get; set; }

        //public string UserName { get; set; }
        //public bool IsAdmin { get; set; }

        public static SqlConnection OpenConnection()
        {
            string connStr = @"data source=.\SQLEXPRESS;Initial Catalog=d2d;user id=sa;password=tankeshi;";

            SqlConnection connection = new SqlConnection(connStr);
            connection.Open();
            return connection;
        }

        public static AdminInfo Get(string id)
        {
            using (var conn = OpenConnection())
            {
                return conn.Get<AdminInfo>(id);
            }
        }

        public bool Update()
        {
            using (var conn = OpenConnection())
            {
                return conn.Update<AdminInfo>(this);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //using (var transactionScope = new TransactionScope()) {
            //    var obj = AdminInfo.Get("14B63059-DF00-4945-AD5D-60AF0EAB6E96");
            //    var s = Snapshotter.Start<AdminInfo>(obj);

            //    //obj.Message = "bingo";
            //    Console.WriteLine(obj.Message);
            //    transactionScope.Complete();
            //}

            //var info = new DbInfo();
            //string TableName = "AdminInfo";
            //var columns = info.GetColumns(TableName);


            //var tmp = new OrmTpl(TableName, columns);
            //Console.WriteLine(tmp.TransformText());

        }
    }
}
