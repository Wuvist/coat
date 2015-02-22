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
            if (args.Length != 1) {
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
            
            foreach (var TableName in config.Tables) {
                var columns = info.GetColumns(TableName);
                var tpl = new OrmTpl(config.Namespace, TableName, columns);
                var output = System.IO.Path.Combine(config.Output, TableName +".generated.cs");
                System.IO.File.WriteAllText(output, tpl.TransformText());
            }
        }
    }
}
