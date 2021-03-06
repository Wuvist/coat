using System;
using System.Collections.Generic;

namespace Coat.tpl
{

    partial class OrmTpl
    {
        private string TableName;
        private string ClassName;
        private string Namespace;
        private DbInfo.Table TableInfo;

        public OrmTpl(string Namespace, string tableName, DbInfo.Table table)
        {
            this.TableName = tableName;
            this.ClassName = tableName;
            foreach (var c in table.Columns) {
                // To avoid "member name cannot be same as enclosing type" restriction in C#
                if (c.COLUMN_NAME == tableName) {
                    this.ClassName = tableName + "Table";
                }
            }
            this.Namespace = Namespace;
            this.TableInfo = table;
        }

        string GetDataType(DbInfo.Column column)
        {
            switch (column.TYPE_NAME)
            {
                case "bit":
                    return "bool";
                case "date":
                case "datetime":
                    return "DateTime";
                case "smallint":
                case "tinyint":
                    return "short";
                case "int":
                case "int identity":
                    return "int";
                case "bigint":
                case "bigint identity":
                    return "long";
                case "money":
                case "decimal":
                    return "decimal";
                case "real":
                    return "float";
                case "float":
                    return "double";
                case "char":
                case "nchar":
                case "text":
                case "ntext":
                case "varchar":
                case "nvarchar":
                    return "string";
                case "uniqueidentifier":
                    return "Guid";
                default:
                    throw new Exception("Unsupported DB type: " + column.TYPE_NAME);
            }
        }

        string GetColumnType(DbInfo.Column column)
        {
            var dataType = GetDataType(column);

            // todo: must check for value type properly
            if (column.NULLABLE && dataType != "string")
            {
                return dataType + "?";
            }

            return dataType;
        }
    }
}