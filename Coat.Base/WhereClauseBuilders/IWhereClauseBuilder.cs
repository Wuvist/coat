using System;
using System.Linq.Expressions;

namespace Coat.Base.WhereClauseBuilders
{
    public interface IWhereClauseBuilder<T> where T : class, new()
    {
        WhereClauseBuildResult BuildWhereClause(Expression<Func<T, bool>> expression);
    }
}