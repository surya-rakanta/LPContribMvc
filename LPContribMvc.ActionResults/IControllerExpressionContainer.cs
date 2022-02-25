using System.Linq.Expressions;

namespace LPContribMvc.ActionResults
{
	public interface IControllerExpressionContainer
	{
		MethodCallExpression Expression
		{
			get;
		}
	}
}
