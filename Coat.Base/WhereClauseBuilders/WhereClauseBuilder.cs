using Coat.Base.SqlMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Coat.Base.WhereClauseBuilders
{
    public abstract class WhereClauseBuilder<TDataObject> : ExpressionVisitor, IWhereClauseBuilder<TDataObject>
        where TDataObject : class, new()
    {
        private readonly Dictionary<string, object> _parameterValues = new Dictionary<string, object>();
        private readonly StringBuilder _sb = new StringBuilder();
        private bool _contains;
        private bool _endsWith;
        private bool _startsWith;

        #region Protected Properties

        /// <summary>
        ///     Gets a <c>System.String</c> value which represents the AND operation in the WHERE clause.
        /// </summary>
        protected virtual string And
        {
            get { return "AND"; }
        }

        /// <summary>
        ///     Gets a <c>System.String</c> value which represents the OR operation in the WHERE clause.
        /// </summary>
        protected virtual string Or
        {
            get { return "OR"; }
        }

        /// <summary>
        ///     Gets a <c>System.String</c> value which represents the EQUAL operation in the WHERE clause.
        /// </summary>
        protected virtual string Equal
        {
            get { return "="; }
        }

        /// <summary>
        ///     Gets a <c>System.String</c> value which represents the NOT operation in the WHERE clause.
        /// </summary>
        protected virtual string Not
        {
            get { return "NOT"; }
        }

        /// <summary>
        ///     Gets a <c>System.String</c> value which represents the NOT EQUAL operation in the WHERE clause.
        /// </summary>
        protected virtual string NotEqual
        {
            get { return "<>"; }
        }

        /// <summary>
        ///     Gets a <c>System.String</c> value which represents the LIKE operation in the WHERE clause.
        /// </summary>
        protected virtual string Like
        {
            get { return "LIKE"; }
        }

        /// <summary>
        ///     Gets a <c>System.Char</c> value which represents the place-holder for the wildcard in the LIKE operation.
        /// </summary>
        protected virtual char LikeSymbol
        {
            get { return '%'; }
        }

        /// <summary>
        ///     Gets a <c>System.Char</c> value which represents the leading character to be used by the
        ///     database parameter.
        /// </summary>
        protected abstract char ParameterChar { get; }

        #endregion

        #region Private Methods

        private void Out(string s)
        {
            _sb.Append(s);
        }

        private static string GetUniqueIdentifier(int length)
        {
            int maxSize = length;
            char[] chars;
            string a;
            a = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            chars = a.ToCharArray();
            int size;
            var data = new byte[1];
            var crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            size = maxSize;
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            var result = new StringBuilder(size);
            foreach (byte b in data)
            {
                result.Append(chars[b%(chars.Length - 1)]);
            }
            // Unique identifiers cannot begin with 0-9
            if (result[0] >= '0' && result[0] <= '9')
            {
                return GetUniqueIdentifier(length);
            }
            return result.ToString();
        }

        #endregion

        #region Protected Methods

        /// <summary>
        ///     Visits the children of <see cref="System.Linq.Expressions.BinaryExpression" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        ///     The modified expression, if it or any subexpression was modified; otherwise,
        ///     returns the original expression.
        /// </returns>
        protected override Expression VisitBinary(BinaryExpression node)
        {
            string str;
            switch (node.NodeType)
            {
                case ExpressionType.Add:
                    str = "+";
                    break;
                case ExpressionType.AddChecked:
                    str = "+";
                    break;
                case ExpressionType.AndAlso:
                    str = And;
                    break;
                case ExpressionType.Divide:
                    str = "/";
                    break;
                case ExpressionType.Equal:
                    str = Equal;
                    break;
                case ExpressionType.GreaterThan:
                    str = ">";
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    str = ">=";
                    break;
                case ExpressionType.LessThan:
                    str = "<";
                    break;
                case ExpressionType.LessThanOrEqual:
                    str = "<=";
                    break;
                case ExpressionType.Modulo:
                    str = "%";
                    break;
                case ExpressionType.Multiply:
                    str = "*";
                    break;
                case ExpressionType.MultiplyChecked:
                    str = "*";
                    break;
                case ExpressionType.Not:
                    str = Not;
                    break;
                case ExpressionType.NotEqual:
                    str = NotEqual;
                    break;
                case ExpressionType.OrElse:
                    str = Or;
                    break;
                case ExpressionType.Subtract:
                    str = "-";
                    break;
                case ExpressionType.SubtractChecked:
                    str = "-";
                    break;
                default:
                    throw new NotSupportedException("can't support");
            }

            Out("(");
            Visit(node.Left);
            Out(" ");
            Out(str);
            Out(" ");
            Visit(node.Right);
            Out(")");
            return node;
        }

        /// <summary>
        ///     Visits the children of <see cref="System.Linq.Expressions.MemberExpression" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        ///     The modified expression, if it or any subexpression was modified; otherwise,
        ///     returns the original expression.
        /// </returns>
        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Member.DeclaringType == typeof(TDataObject) ||
                typeof(TDataObject).IsSubclassOf(node.Member.DeclaringType))
            {
                TableMappingInfo tableMappingInfo = TableMappingInfo.TableMapping[typeof(TDataObject)];
                ColumnMappingInfo mappedColumn =
                    tableMappingInfo.Columns.FirstOrDefault(p => p.PropertyName == node.Member.Name);
                if (mappedColumn != null)
                {
                    Out(mappedColumn.ColumnName);
                }
            }
            else
            {
                if (node.Member is FieldInfo)
                {
                    var ce = node.Expression as ConstantExpression;
                    var fi = node.Member as FieldInfo;
                    object fieldValue = fi.GetValue(ce.Value);
                    Expression constantExpr = Expression.Constant(fieldValue);
                    Visit(constantExpr);
                }
                else
                    throw new NotSupportedException("can't support");
            }
            return node;
        }

        /// <summary>
        ///     Visits the children of <see cref="System.Linq.Expressions.ConstantExpression" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        ///     The modified expression, if it or any subexpression was modified; otherwise,
        ///     returns the original expression.
        /// </returns>
        protected override Expression VisitConstant(ConstantExpression node)
        {
            string paramName = string.Format("{0}{1}", ParameterChar, GetUniqueIdentifier(5));
            Out(paramName);
            if (!_parameterValues.ContainsKey(paramName))
            {
                object v;
                if (_startsWith && node.Value is string)
                {
                    _startsWith = false;
                    v = node.Value.ToString() + LikeSymbol;
                }
                else if (_endsWith && node.Value is string)
                {
                    _endsWith = false;
                    v = LikeSymbol + node.Value.ToString();
                }
                else if (_contains && node.Value is string)
                {
                    _contains = false;
                    v = LikeSymbol + node.Value.ToString() + LikeSymbol;
                }
                else
                {
                    v = node.Value;
                }
                _parameterValues.Add(paramName, v);
            }
            return node;
        }

        /// <summary>
        ///     Visits the children of <see cref="System.Linq.Expressions.MethodCallExpression" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        ///     The modified expression, if it or any subexpression was modified; otherwise,
        ///     returns the original expression.
        /// </returns>
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            Out("(");
            Visit(node.Object);
            if (node.Arguments == null || node.Arguments.Count != 1)
                throw new NotSupportedException("can't support");
            Expression expr = node.Arguments[0];
            switch (node.Method.Name)
            {
                case "StartsWith":
                    _startsWith = true;
                    Out(" ");
                    Out(Like);
                    Out(" ");
                    break;
                case "EndsWith":
                    _endsWith = true;
                    Out(" ");
                    Out(Like);
                    Out(" ");
                    break;
                case "Equals":
                    Out(" ");
                    Out(Equal);
                    Out(" ");
                    break;
                case "Contains":
                    _contains = true;
                    Out(" ");
                    Out(Like);
                    Out(" ");
                    break;
                default:
                    throw new NotSupportedException("can't support");
            }
            if (expr is ConstantExpression || expr is MemberExpression)
                Visit(expr);
            else
                throw new NotSupportedException("can't support");
            Out(")");
            return node;
        }

        /// <summary>
        ///     Visits the children of <see cref="System.Linq.Expressions.BlockExpression" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        ///     The modified expression, if it or any subexpression was modified; otherwise,
        ///     returns the original expression.
        /// </returns>
        protected override Expression VisitBlock(BlockExpression node)
        {
            throw new NotSupportedException("can't support");
        }

        /// <summary>
        ///     Visits the children of <see cref="System.Linq.Expressions.CatchBlock" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        ///     The modified expression, if it or any subexpression was modified; otherwise,
        ///     returns the original expression.
        /// </returns>
        protected override CatchBlock VisitCatchBlock(CatchBlock node)
        {
            throw new NotSupportedException("can't support");
        }

        /// <summary>
        ///     Visits the children of <see cref="System.Linq.Expressions.ConditionalExpression" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        ///     The modified expression, if it or any subexpression was modified; otherwise,
        ///     returns the original expression.
        /// </returns>
        protected override Expression VisitConditional(ConditionalExpression node)
        {
            throw new NotSupportedException("can't support");
        }

        /// <summary>
        ///     Visits the children of <see cref="System.Linq.Expressions.DebugInfoExpression" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        ///     The modified expression, if it or any subexpression was modified; otherwise,
        ///     returns the original expression.
        /// </returns>
        protected override Expression VisitDebugInfo(DebugInfoExpression node)
        {
            throw new NotSupportedException("can't support");
        }

        /// <summary>
        ///     Visits the children of <see cref="System.Linq.Expressions.DefaultExpression" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        ///     The modified expression, if it or any subexpression was modified; otherwise,
        ///     returns the original expression.
        /// </returns>
        protected override Expression VisitDefault(DefaultExpression node)
        {
            throw new NotSupportedException("can't support");
        }

        /// <summary>
        ///     Visits the children of <see cref="System.Linq.Expressions.DynamicExpression" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        ///     The modified expression, if it or any subexpression was modified; otherwise,
        ///     returns the original expression.
        /// </returns>
        protected override Expression VisitDynamic(DynamicExpression node)
        {
            throw new NotSupportedException("can't support");
        }

        /// <summary>
        ///     Visits the children of <see cref="System.Linq.Expressions.ElementInit" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        ///     The modified expression, if it or any subexpression was modified; otherwise,
        ///     returns the original expression.
        /// </returns>
        protected override ElementInit VisitElementInit(ElementInit node)
        {
            throw new NotSupportedException("can't support");
        }

        /// <summary>
        ///     Visits the children of <see cref="System.Linq.Expressions.GotoExpression" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        ///     The modified expression, if it or any subexpression was modified; otherwise,
        ///     returns the original expression.
        /// </returns>
        protected override Expression VisitGoto(GotoExpression node)
        {
            throw new NotSupportedException("can't support");
        }

        /// <summary>
        ///     Visits the children of <see cref="System.Linq.Expressions.Expression" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        ///     The modified expression, if it or any subexpression was modified; otherwise,
        ///     returns the original expression.
        /// </returns>
        protected override Expression VisitExtension(Expression node)
        {
            throw new NotSupportedException("can't support");
        }

        /// <summary>
        ///     Visits the children of <see cref="System.Linq.Expressions.IndexExpression" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        ///     The modified expression, if it or any subexpression was modified; otherwise,
        ///     returns the original expression.
        /// </returns>
        protected override Expression VisitIndex(IndexExpression node)
        {
            throw new NotSupportedException("can't support");
        }

        /// <summary>
        ///     Visits the children of <see cref="System.Linq.Expressions.InvocationExpression" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        ///     The modified expression, if it or any subexpression was modified; otherwise,
        ///     returns the original expression.
        /// </returns>
        protected override Expression VisitInvocation(InvocationExpression node)
        {
            throw new NotSupportedException("can't support");
        }

        /// <summary>
        ///     Visits the children of <see cref="System.Linq.Expressions.LabelExpression" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        ///     The modified expression, if it or any subexpression was modified; otherwise,
        ///     returns the original expression.
        /// </returns>
        protected override Expression VisitLabel(LabelExpression node)
        {
            throw new NotSupportedException("can't support");
        }

        /// <summary>
        ///     Visits the children of <see cref="System.Linq.Expressions.LabelTarget" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        ///     The modified expression, if it or any subexpression was modified; otherwise,
        ///     returns the original expression.
        /// </returns>
        protected override LabelTarget VisitLabelTarget(LabelTarget node)
        {
            throw new NotSupportedException("can't support");
        }

        /// <summary>
        ///     Visits the children of <see cref="System.Linq.Expressions.Expression&lt;T&gt;" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        ///     The modified expression, if it or any subexpression was modified; otherwise,
        ///     returns the original expression.
        /// </returns>
        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            throw new NotSupportedException("can't support");
        }

        /// <summary>
        ///     Visits the children of <see cref="System.Linq.Expressions.ListInitExpression" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        ///     The modified expression, if it or any subexpression was modified; otherwise,
        ///     returns the original expression.
        /// </returns>
        protected override Expression VisitListInit(ListInitExpression node)
        {
            throw new NotSupportedException("can't support");
        }

        /// <summary>
        ///     Visits the children of <see cref="System.Linq.Expressions.LoopExpression" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        ///     The modified expression, if it or any subexpression was modified; otherwise,
        ///     returns the original expression.
        /// </returns>
        protected override Expression VisitLoop(LoopExpression node)
        {
            throw new NotSupportedException("can't support");
        }

        /// <summary>
        ///     Visits the children of <see cref="System.Linq.Expressions.MemberAssignment" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        ///     The modified expression, if it or any subexpression was modified; otherwise,
        ///     returns the original expression.
        /// </returns>
        protected override MemberAssignment VisitMemberAssignment(MemberAssignment node)
        {
            throw new NotSupportedException("can't support");
        }

        /// <summary>
        ///     Visits the children of <see cref="System.Linq.Expressions.MemberBinding" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        ///     The modified expression, if it or any subexpression was modified; otherwise,
        ///     returns the original expression.
        /// </returns>
        protected override MemberBinding VisitMemberBinding(MemberBinding node)
        {
            throw new NotSupportedException("can't support");
        }

        /// <summary>
        ///     Visits the children of <see cref="System.Linq.Expressions.MemberInitExpression" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        ///     The modified expression, if it or any subexpression was modified; otherwise,
        ///     returns the original expression.
        /// </returns>
        protected override Expression VisitMemberInit(MemberInitExpression node)
        {
            throw new NotSupportedException("can't support");
        }

        /// <summary>
        ///     Visits the children of <see cref="System.Linq.Expressions.MemberListBinding" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        ///     The modified expression, if it or any subexpression was modified; otherwise,
        ///     returns the original expression.
        /// </returns>
        protected override MemberListBinding VisitMemberListBinding(MemberListBinding node)
        {
            throw new NotSupportedException("can't support");
        }

        /// <summary>
        ///     Visits the children of <see cref="System.Linq.Expressions.MemberMemberBinding" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        ///     The modified expression, if it or any subexpression was modified; otherwise,
        ///     returns the original expression.
        /// </returns>
        protected override MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding node)
        {
            throw new NotSupportedException("can't support");
        }

        /// <summary>
        ///     Visits the children of <see cref="System.Linq.Expressions.NewExpression" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        ///     The modified expression, if it or any subexpression was modified; otherwise,
        ///     returns the original expression.
        /// </returns>
        protected override Expression VisitNew(NewExpression node)
        {
            throw new NotSupportedException("can't support");
        }

        /// <summary>
        ///     Visits the children of <see cref="System.Linq.Expressions.NewArrayExpression" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        ///     The modified expression, if it or any subexpression was modified; otherwise,
        ///     returns the original expression.
        /// </returns>
        protected override Expression VisitNewArray(NewArrayExpression node)
        {
            throw new NotSupportedException("can't support");
        }

        /// <summary>
        ///     Visits the children of <see cref="System.Linq.Expressions.ParameterExpression" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        ///     The modified expression, if it or any subexpression was modified; otherwise,
        ///     returns the original expression.
        /// </returns>
        protected override Expression VisitParameter(ParameterExpression node)
        {
            throw new NotSupportedException("can't support");
        }

        /// <summary>
        ///     Visits the children of <see cref="System.Linq.Expressions.RuntimeVariablesExpression" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        ///     The modified expression, if it or any subexpression was modified; otherwise,
        ///     returns the original expression.
        /// </returns>
        protected override Expression VisitRuntimeVariables(RuntimeVariablesExpression node)
        {
            throw new NotSupportedException("can't support");
        }

        /// <summary>
        ///     Visits the children of <see cref="System.Linq.Expressions.SwitchExpression" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        ///     The modified expression, if it or any subexpression was modified; otherwise,
        ///     returns the original expression.
        /// </returns>
        protected override Expression VisitSwitch(SwitchExpression node)
        {
            throw new NotSupportedException("can't support");
        }

        /// <summary>
        ///     Visits the children of <see cref="System.Linq.Expressions.SwitchCase" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        ///     The modified expression, if it or any subexpression was modified; otherwise,
        ///     returns the original expression.
        /// </returns>
        protected override SwitchCase VisitSwitchCase(SwitchCase node)
        {
            throw new NotSupportedException("can't support");
        }

        /// <summary>
        ///     Visits the children of <see cref="System.Linq.Expressions.TryExpression" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        ///     The modified expression, if it or any subexpression was modified; otherwise,
        ///     returns the original expression.
        /// </returns>
        protected override Expression VisitTry(TryExpression node)
        {
            throw new NotSupportedException("can't support");
        }

        /// <summary>
        ///     Visits the children of <see cref="System.Linq.Expressions.TypeBinaryExpression" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        ///     The modified expression, if it or any subexpression was modified; otherwise,
        ///     returns the original expression.
        /// </returns>
        protected override Expression VisitTypeBinary(TypeBinaryExpression node)
        {
            throw new NotSupportedException("can't support");
        }

        /// <summary>
        ///     Visits the children of <see cref="System.Linq.Expressions.UnaryExpression" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        ///     The modified expression, if it or any subexpression was modified; otherwise,
        ///     returns the original expression.
        /// </returns>
        protected override Expression VisitUnary(UnaryExpression node)
        {
            throw new NotSupportedException("can't support");
        }

        #endregion

        public WhereClauseBuildResult BuildWhereClause(Expression<Func<TDataObject, bool>> expression)
        {
            _sb.Clear();
            _parameterValues.Clear();
            Visit(expression.Body);
            return new WhereClauseBuildResult(_sb.ToString(), _parameterValues);
        }
    }
}