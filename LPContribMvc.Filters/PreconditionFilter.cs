using System;
using System.Web.Mvc;

namespace LPContribMvc.Filters
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Interface, AllowMultiple = true)]
	[Obsolete]
	public abstract class PreconditionFilter : ActionFilterAttribute
	{
		public enum ParamType
		{
			RouteData,
			Request
		}

		protected string _paramName;

		protected ParamType _paramType;

		protected Type _exceptionToThrow;

		protected string _thrownExceptionMessage = string.Empty;

		public override void OnActionExecuting(ActionExecutingContext executingContext)
		{
			if (FailedValidation(executingContext) && typeof(Exception).IsAssignableFrom(_exceptionToThrow))
			{
				Exception ex = (Exception)_exceptionToThrow.GetConstructor(new Type[1]
				{
					typeof(string)
				}).Invoke(new object[1]
				{
					_thrownExceptionMessage
				});
				throw ex;
			}
		}

		protected abstract bool FailedValidation(ActionExecutingContext executingContext);

		protected PreconditionFilter() : base()
		{
		}
	}
}
