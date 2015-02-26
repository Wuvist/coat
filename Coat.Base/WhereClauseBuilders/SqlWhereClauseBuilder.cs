namespace Coat.Base.WhereClauseBuilders
{
    public sealed class SqlWhereClauseBuilder<T> : WhereClauseBuilder<T>
        where T : class, new()
    {
        #region Protected Properties

        protected override char ParameterChar
        {
            get { return '@'; }
        }

        #endregion
    }
}