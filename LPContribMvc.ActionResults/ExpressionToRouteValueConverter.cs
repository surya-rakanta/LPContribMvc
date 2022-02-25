using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace LPContribMvc.ActionResults
{
	public delegate RouteValueDictionary ExpressionToRouteValueConverter<TController>(Expression<Action<TController>> expression) where TController : Controller;
}
