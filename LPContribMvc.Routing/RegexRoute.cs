using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Routing;

namespace LPContribMvc.Routing
{
	[Obsolete]
	public class RegexRoute : RouteBase
	{
		private readonly RouteValueDictionary _defaults;

		private readonly string[] _groupNames;

		private readonly Regex _regex;

		private readonly IRouteHandler _routeHandler;

		private readonly Func<RequestContext, RouteValueDictionary, RegexRoute, VirtualPathData> _getVirtualPath;

		private readonly string _urlGenerator;

		public RouteValueDictionary Defaults => _defaults;

		public IRouteHandler RouteHandler => _routeHandler;

		public string UrlGenerator => _urlGenerator;

		public RegexRoute(string regex, IRouteHandler routeHandler)
			: this(regex, (string)null, routeHandler)
		{
		}

		public RegexRoute(string regex, string urlGenerator, IRouteHandler routeHandler)
			: this(regex, urlGenerator, null, routeHandler)
		{
		}

		public RegexRoute(string regex, RouteValueDictionary defaults, IRouteHandler routeHandler)
			: this(regex, (string)null, defaults, routeHandler)
		{
		}

		public RegexRoute(string regex, string urlGenerator, RouteValueDictionary defaults, IRouteHandler routeHandler)
			: this(regex, urlGenerator, RealGetVirtualPath, defaults, routeHandler)
		{
		}

		public RegexRoute(string regex, Func<RequestContext, RouteValueDictionary, RegexRoute, VirtualPathData> getVirtualPath, IRouteHandler routeHandler)
			: this(regex, getVirtualPath, null, routeHandler)
		{
		}

		public RegexRoute(string regex, Func<RequestContext, RouteValueDictionary, RegexRoute, VirtualPathData> getVirtualPath, RouteValueDictionary defaults, IRouteHandler routeHandler)
			: this(regex, null, getVirtualPath, defaults, routeHandler)
		{
		}

		protected RegexRoute(string regex, string urlGenerator, Func<RequestContext, RouteValueDictionary, RegexRoute, VirtualPathData> getVirtualPath, RouteValueDictionary defaults, IRouteHandler routeHandler)
		{
			_getVirtualPath = (getVirtualPath ?? new Func<RequestContext, RouteValueDictionary, RegexRoute, VirtualPathData>(RealGetVirtualPath));
			_urlGenerator = urlGenerator;
			_defaults = defaults;
			_routeHandler = routeHandler;
			_regex = new Regex(regex, RegexOptions.IgnoreCase | RegexOptions.Compiled);
			_groupNames = _regex.GetGroupNames();
		}

		public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
		{
			return _getVirtualPath(requestContext, values, this);
		}

		private static VirtualPathData RealGetVirtualPath(RequestContext requestContext, RouteValueDictionary values, RegexRoute thisRoute)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			if (thisRoute.Defaults != null)
			{
				foreach (KeyValuePair<string, object> @default in thisRoute.Defaults)
				{
					dictionary.Add(@default.Key, @default.Value.ToString());
				}
			}
			if (values != null)
			{
				foreach (KeyValuePair<string, object> value in values)
				{
					dictionary[value.Key] = value.Value.ToString();
				}
			}
			string text = thisRoute.UrlGenerator;
			foreach (KeyValuePair<string, string> item in dictionary)
			{
				text = text.Replace("{" + item.Key + "}", item.Value);
			}
			if (Regex.IsMatch(text, "\\{\\w+\\}"))
			{
				return null;
			}
			return new VirtualPathData(thisRoute, text);
		}

		public override RouteData GetRouteData(HttpContextBase httpContext)
		{
			string request = httpContext.Request.AppRelativeCurrentExecutionFilePath.Substring(2) + httpContext.Request.PathInfo;
			return GetRouteData(request);
		}

		public virtual RouteData GetRouteData(string request)
		{
			Match match = _regex.Match(request);
			if (!match.Success)
			{
				return null;
			}
			RouteData routeData = GenerateDefaultRouteData();
			string[] groupNames = _groupNames;
			foreach (string text in groupNames)
			{
				Group group = match.Groups[text];
				if (group.Success && !string.IsNullOrEmpty(text) && !char.IsNumber(text, 0))
				{
					routeData.Values[text] = group.Value;
				}
			}
			return routeData;
		}

		private RouteData GenerateDefaultRouteData()
		{
			RouteData routeData = new RouteData(this, RouteHandler);
			if (Defaults != null)
			{
				foreach (KeyValuePair<string, object> @default in Defaults)
				{
					routeData.Values.Add(@default.Key, @default.Value);
				}
				return routeData;
			}
			return routeData;
		}
	}
}
