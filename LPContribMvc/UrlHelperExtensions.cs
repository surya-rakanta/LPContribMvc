using Microsoft.Web.Mvc;
using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace LPContribMvc
{
	public static class UrlHelperExtensions
	{
		public static string Action<TController>(this UrlHelper urlHelper, Expression<Action<TController>> expression) where TController : Controller
		{
			return LinkBuilder.BuildUrlFromExpression<TController>(urlHelper.RequestContext, urlHelper.RouteCollection, expression);
		}

		public static string Resource(this UrlHelper urlHelper, string resourceName)
		{
			string area = (string)urlHelper.RequestContext.RouteData.DataTokens["area"];
			return urlHelper.Action("Index", "EmbeddedResource", (object)new
			{
				resourceName,
				area
			});
		}
	}
}
