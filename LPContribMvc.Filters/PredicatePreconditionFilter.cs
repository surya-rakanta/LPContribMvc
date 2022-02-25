using System;
using System.Web.Mvc;

namespace LPContribMvc.Filters
{
	[Obsolete]
	public class PredicatePreconditionFilter : PreconditionFilter
	{
		protected string _predicateMethod;

		public PredicatePreconditionFilter(string paramName, ParamType paramType, string predicateMethod, Type exceptionToThrow)
		{
			_paramName = paramName;
			_paramType = paramType;
			_predicateMethod = predicateMethod;
			_exceptionToThrow = exceptionToThrow;
			_thrownExceptionMessage = Enum.GetName(typeof(ParamType), paramType) + " parameter '" + paramName + "' does not satisfy predicate method " + predicateMethod;
		}

		protected override bool FailedValidation(ActionExecutingContext executingContext)
		{
			Predicate<object> predicate = (Predicate<object>)Delegate.CreateDelegate(typeof(Predicate<object>), ((ControllerContext)executingContext).Controller, _predicateMethod);
			switch (_paramType)
			{
			case ParamType.RouteData:
				if (((ControllerContext)executingContext).RouteData.Values.ContainsKey(_paramName) && ((ControllerContext)executingContext).RouteData.Values[_paramName] != null)
				{
					return !predicate(((ControllerContext)executingContext).RouteData.Values[_paramName]);
				}
				return true;
			case ParamType.Request:
				if (((ControllerContext)executingContext).HttpContext.Request.Params[_paramName] != null)
				{
					return !predicate(((ControllerContext)executingContext).HttpContext.Request.Params[_paramName]);
				}
				return true;
			default:
				return false;
			}
		}
	}
}
