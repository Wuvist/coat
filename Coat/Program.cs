using System;
using System.Linq;
using Dapper;
using Dapper.Contrib.Extensions;
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

            var input = System.IO.File.OpenText(args[0]);
            var deserializer = new Deserializer(namingConvention: new CamelCaseNamingConvention());

            var config = deserializer.Deserialize<Config>(input);

            var info = new DbInfo(config.Conn);
            List<string> tableNames = config.Tables;
            var ignoreTables = (from table in config.Tables where table.StartsWith("-") select table.Substring(1)).ToList();
            if (config.Tables[0] == "*")
            {
                tableNames = info.GetAllTableNames();
            }

            foreach (var tableName in tableNames)
            {
                try
                {
                    if (ignoreTables.Contains(tableName))
                    {
                        continue;
                    }
                    var table = info.GetTable(tableName);
                    var tpl = new tpl.OrmTpl(config.Namespace, tableName, table);
                    var output = System.IO.Path.Combine(config.Output, tableName + ".generated.cs");
                    System.IO.File.WriteAllText(output, tpl.TransformText());
                }
                catch (Exception ex) {
                    Console.Error.WriteLine("table error: " + tableName);
                    Console.Error.WriteLine(ex.ToString());
                }
            }
        }
    }
}
