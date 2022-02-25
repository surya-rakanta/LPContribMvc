using System;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace LPContribMvc.Filters
{
	[Obsolete]
	public class RegExPreconditionFilter : PreconditionFilter
	{
		protected string _regExPattern;

		public RegExPreconditionFilter(string paramName, ParamType paramType, string regExPattern, Type exceptionToThrow)
		{
			_paramName = paramName;
			_paramType = paramType;
			_regExPattern = regExPattern;
			_exceptionToThrow = exceptionToThrow;
			_thrownExceptionMessage = Enum.GetName(typeof(ParamType), paramType) + " parameter '" + paramName + "' does not match regex " + regExPattern;
		}

		protected override bool FailedValidation(ActionExecutingContext executingContext)
		{
			switch (_paramType)
			{
			case ParamType.RouteData:
				if (((ControllerContext)executingContext).RouteData.Values.ContainsKey(_paramName) && ((ControllerContext)executingContext).RouteData.Values[_paramName] != null)
				{
					return !Regex.IsMatch(((ControllerContext)executingContext).RouteData.Values[_paramName].ToString(), _regExPattern);
				}
				return true;
			case ParamType.Request:
				if (((ControllerContext)executingContext).HttpContext.Request.Params[_paramName] != null)
				{
					return !Regex.IsMatch(((ControllerContext)executingContext).HttpContext.Request.Params[_paramName], _regExPattern);
				}
				return true;
			default:
				return false;
			}
		}
	}
}
