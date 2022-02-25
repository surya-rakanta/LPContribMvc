using Microsoft.Web.Mvc.Internal;
using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace LPContribMvc.ActionResults
{
	public class RedirectToRouteResult<T> : RedirectToRouteResult, IControllerExpressionContainer where T : Controller
	{
		MethodCallExpression IControllerExpressionContainer.Expression => Expression.Body as MethodCallExpression;

		public Expression<Action<T>> Expression
		{
			get;
			private set;
		}

		public RedirectToRouteResult(Expression<Action<T>> expression)
			: this(expression, (ExpressionToRouteValueConverter<T>)((Expression<Action<T>> expr) => Microsoft.Web.Mvc.Internal.ExpressionHelper.GetRouteValuesFromExpression<T>(expr)))
		{
		}

		public RedirectToRouteResult(Expression<Action<T>> expression, ExpressionToRouteValueConverter<T> expressionParser)
			: base(expressionParser(expression))
		{
			Expression = expression;
		}
	}
}
