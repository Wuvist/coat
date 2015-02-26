using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coat.Base.WhereClauseBuilders
{
    public sealed class WhereClauseBuildResult
    {
        public string WhereClause { get;  private  set; }

        public Dictionary<string, object> ParameterValues { get;  private set; }

        public WhereClauseBuildResult()
        {
            
        }

        public WhereClauseBuildResult(string whereClause, Dictionary<string, object> parameterValues)
        {
            WhereClause = whereClause;
            ParameterValues = parameterValues;
        }
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(WhereClause);
            sb.Append(Environment.NewLine);
            ParameterValues.ToList().ForEach(kvp =>
            {
                sb.Append(string.Format("{0} = [{1}] (Type: {2})", kvp.Key, kvp.Value.ToString(), kvp.Value.GetType().FullName));
                sb.Append(Environment.NewLine);
            });
            return sb.ToString();
        }
    }
}