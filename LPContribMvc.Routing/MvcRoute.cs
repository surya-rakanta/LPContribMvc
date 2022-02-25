using Microsoft.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace LPContribMvc.Routing
{
	[Obsolete]
	public class MvcRoute : Route
	{
		private MvcRoute(string url)
			: base(url, (IRouteHandler)new MvcRouteHandler())
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected O, but got Unknown
			base.Constraints = new RouteValueDictionary();
			base.Defaults = new RouteValueDictionary();
		}

		public static MvcRoute MapUrl(string url)
		{
			return new MvcRoute(url);
		}

		[Obsolete("Use MapUrl instead")]
		public static MvcRoute MappUrl(string url)
		{
			return new MvcRoute(url);
		}

		public MvcRoute ToDefaultAction<T>(Expression<Func<T, ActionResult>> action, object defaults) where T : IController
		{
			ToDefaultAction(action);
			foreach (KeyValuePair<string, object> item in new RouteValueDictionary(defaults))
			{
				base.Defaults.Add(item.Key, item.Value);
			}
			return this;
		}

		public MvcRoute ToDefaultAction<T>(Expression<Func<T, ActionResult>> action) where T : IController
		{
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Expected O, but got Unknown
			MethodCallExpression methodCallExpression = action.Body as MethodCallExpression;
			if (methodCallExpression == null)
			{
				throw new ArgumentException("Expression must be a method call");
			}
			if (methodCallExpression.Object != action.Parameters[0])
			{
				throw new ArgumentException("Method call must target lambda argument");
			}
			string name = methodCallExpression.Method.Name;
			object[] customAttributes = methodCallExpression.Method.GetCustomAttributes(typeof(ActionNameAttribute), inherit: false);
			if (customAttributes.Length > 0)
			{
				ActionNameAttribute val = (ActionNameAttribute)(object)(ActionNameAttribute)customAttributes[0];
				name = val.Name;
			}
			string text = typeof(T).Name;
			if (text.EndsWith("Controller", StringComparison.OrdinalIgnoreCase))
			{
				text = text.Remove(text.Length - 10, 10);
			}
			base.Defaults = (LinkBuilder.BuildParameterValuesFromExpression(methodCallExpression) ?? new RouteValueDictionary());
			foreach (KeyValuePair<string, object> item in base.Defaults.Where((KeyValuePair<string, object> x) => x.Value == null).ToList())
			{
				base.Defaults.Remove(item.Key);
			}
			base.Defaults.Add("controller", text);
			base.Defaults.Add("action", name);
			return this;
		}

		public MvcRoute WithConstraints(object constraints)
		{
			foreach (KeyValuePair<string, object> item in new RouteValueDictionary(constraints))
			{
				base.Constraints.Add(item.Key, item.Value);
			}
			return this;
		}

		public MvcRoute WithDefaults(object defaults)
		{
			foreach (KeyValuePair<string, object> item in new RouteValueDictionary(defaults))
			{
				base.Defaults.Add(item.Key, item.Value);
			}
			return this;
		}

		public MvcRoute WithNamespaces(string[] namespaces)
		{
			if (namespaces == null)
			{
				throw new ArgumentNullException("namespaces");
			}
			base.DataTokens = new RouteValueDictionary();
			base.DataTokens["Namespaces"] = namespaces;
			return this;
		}

		public MvcRoute AddWithName(string routeName, RouteCollection routes)
		{
			routes.Add(routeName, this);
			return this;
		}
	}
}
