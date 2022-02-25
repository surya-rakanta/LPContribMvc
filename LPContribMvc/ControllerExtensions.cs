using LPContribMvc.ActionResults;
using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace LPContribMvc
{
	public static class ControllerExtensions
	{
		public static RedirectToRouteResult RedirectToAction<T>(this T controller, Expression<Action<T>> action) where T : Controller
		{
			return ((Controller)(object)controller).RedirectToAction(action);
		}

		public static RedirectToRouteResult RedirectToAction<T>(this Controller controller, Expression<Action<T>> action) where T : Controller
		{
			return (RedirectToRouteResult)(object)new RedirectToRouteResult<T>(action);
		}

		public static bool IsController(Type type)
		{
			if (type != null && type.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase) && !type.IsAbstract)
			{
				return typeof(IController).IsAssignableFrom(type);
			}
			return false;
		}
	}
}
