using System;
using Dapper;
using Dapper.Contrib.Extensions;
using Dapper.Rainbow;
using System.Collections.Generic;
using System.Transactions;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Coat
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Use: coat.exe config.yaml");
                return;
            }

            var input = System.IO.File.OpenText("default.yaml");
            var deserializer = new Deserializer(namingConvention: new CamelCaseNamingConvention());

            var config = deserializer.Deserialize<Config>(input);

            //using (var transactionScope = new TransactionScope()) {
            //    var obj = AdminInfo.Get("14B63059-DF00-4945-AD5D-60AF0EAB6E96");
            //    var s = Snapshotter.Start<AdminInfo>(obj);

            //    //obj.Message = "bingo";
            //    Console.WriteLine(obj.Message);
            //    transactionScope.Complete();
            //}

            var info = new DbInfo(config.Conn);
            List<string> tableNames = config.Tables;
            if (config.Tables[0] == "*")
            {
                tableNames = info.GetAllTableNames();
            }

            foreach (var tableName in tableNames)
            {
                var columns = info.GetColumns(tableName);
                var tpl = new tpl.OrmTpl(config.Namespace, tableName, columns);
                var output = System.IO.Path.Combine(config.Output, tableName + ".generated.cs");
                System.IO.File.WriteAllText(output, tpl.TransformText());
            }
            System.IO.File.Copy("RecordBase.cs", System.IO.Path.Combine(config.Output, "RecordBase.generated.cs"), true);
        }
    }
}
