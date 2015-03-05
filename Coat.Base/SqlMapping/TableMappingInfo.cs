using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Coat.Base.SqlMapping
{
    public sealed class TableMappingInfo
    {
        public static readonly ConcurrentDictionary<Type, TableMappingInfo> TableMapping =
            new ConcurrentDictionary<Type, TableMappingInfo>();

        public Type Type { get; private set; }

        public string Schema { get; private set; }

        public string Name { get; private set; }

        public string FullName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Schema))
                    return Name;
                return string.Format("{0}.{1}", Schema, Name);
            }
        }

        public IReadOnlyList<ColumnMappingInfo> Columns { get; private set; }

        public static TableMappingInfo Create<T>()
        {
            return Create(typeof(T));
        }

        public static TableMappingInfo Create(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            bool lockTaken = false;
            TableMappingInfo tableMappingInfo;
            try
            {

                Monitor.Enter(TableMapping, ref lockTaken);
                if (!TableMapping.TryGetValue(type, out tableMappingInfo))
                {
                    var customAttribute = type.GetCustomAttribute<TableAttribute>(false);
                    tableMappingInfo = new TableMappingInfo
                    {
                        Schema = (customAttribute != null ? customAttribute.Schema : null),
                        Name = (customAttribute != null ? customAttribute.Name : null) ?? type.Name,
                        Type = type,
                        Columns = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                            .Where(p => p.CustomAttributes.All(c => c.AttributeType != typeof(NotMappedAttribute)))
                            .Select(ColumnMappingInfo.From).ToArray()
                    };
                    TableMapping.TryAdd(type, tableMappingInfo);
                }
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit(TableMapping);
            }
            return tableMappingInfo;
        }
    }
}
