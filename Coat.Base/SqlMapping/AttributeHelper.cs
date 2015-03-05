using System;
using System.Linq;
using System.Reflection;

namespace Coat.Base.SqlMapping
{
    public static class AttributeHelper
    {
        public static bool HasAttribute<T>(this PropertyInfo info) where T : Attribute
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            return info.CustomAttributes.Any(p => p.AttributeType == typeof(T));
        }
    }
}
