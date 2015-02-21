using System;
using System.Collections.Generic;

namespace Coat
{

    partial class OrmTpl
    {
        private string TableName;
        private IEnumerable<DbInfo.Column> Columns;

        public OrmTpl(string tableName, IEnumerable<DbInfo.Column> columns)
        {
            this.TableName = tableName;
            this.Columns = columns;
        }

        string GetName(string name) {
            return "bingo" + name;
        }

        string GetDataType(DbInfo.Column column) {
            switch (column.TYPE_NAME) {
                case "bit":
                    return "bool";
                case "datetime":
                    return "DateTime";
                case "int":
                case "int identity":
                    return "int";
                case "ntext":
                    return "string";
                default:
                    throw new Exception("Unsupported DB type: " + column.TYPE_NAME);
            }
        }

        string GetColumnType(DbInfo.Column column)
        {
            var dataType = GetDataType(column);
            if (column.IS_NULLABLE == "YES") {
                return dataType + "?";
            }

            return dataType;
        }
    }
}