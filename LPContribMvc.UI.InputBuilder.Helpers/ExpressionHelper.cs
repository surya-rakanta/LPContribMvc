using System;
using System.Linq.Expressions;

namespace LPContribMvc.UI.InputBuilder.Helpers
{
	public static class ExpressionHelper
	{
		public static object Evaluate(LambdaExpression expression, object target)
		{
			if (target == null)
			{
				return null;
			}
			Delegate @delegate = expression.Compile();
			object result = null;
			try
			{
				result = @delegate.DynamicInvoke(target);
				return result;
			}
			catch (Exception)
			{
				return result;
			}
		}
	}
}
