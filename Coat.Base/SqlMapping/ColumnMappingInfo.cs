using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Coat.Base.SqlMapping
{
    public class ColumnMappingInfo
    {
        private static readonly IReadOnlyDictionary<Type, DbType> _typeMap = TypeMap.Default();

        private ColumnMappingInfo()
        {
        }

        public string PropertyName { get; private set; }

        public Type PropertyType { get; private set; }

        public string ColumnName { get; private set; }

        public DbType ColumnType { get; private set; }

        public bool IsPrimaryKey { get; private set; }

        public bool IsNullable { get; private set; }

        public bool IsIdentity { get; private set; }

        public static ColumnMappingInfo From(PropertyInfo info)
        {
            var flag1 = info.HasAttribute<KeyAttribute>();
            var flag2 = info.HasAttribute<RequiredAttribute>();
            var columnMappingInfo = new ColumnMappingInfo
            {
                PropertyName = info.Name,
                PropertyType = info.PropertyType,
                ColumnName = GetColumnName(info)
            };
            var num1 = (int)_typeMap[info.PropertyType];
            columnMappingInfo.ColumnType = (DbType)num1;
            var num2 = flag1 ? 1 : 0;
            columnMappingInfo.IsPrimaryKey = num2 != 0;
            var num3 = !(flag1 | flag2) ? 1 : 0;
            columnMappingInfo.IsNullable = num3 != 0;
            var num4 = GetDatabaseGeneratedOption(info) == DatabaseGeneratedOption.Identity ? 1 : 0;
            columnMappingInfo.IsIdentity = num4 != 0;
            return columnMappingInfo;
        }

        private static string GetColumnName(PropertyInfo info)
        {
            if (info == null)
                throw new ArgumentNullException("info");
            var customAttribute = info.GetCustomAttribute<ColumnAttribute>(false);
            return (customAttribute != null ? customAttribute.Name : null) ?? info.Name;
        }

        private static DatabaseGeneratedOption GetDatabaseGeneratedOption(PropertyInfo info)
        {
            if (info == null)
                throw new ArgumentNullException("info");
            var customAttribute = info.GetCustomAttribute<DatabaseGeneratedAttribute>(false);
            if (customAttribute == null)
                return DatabaseGeneratedOption.None;
            return customAttribute.DatabaseGeneratedOption;
        }
    }
}
