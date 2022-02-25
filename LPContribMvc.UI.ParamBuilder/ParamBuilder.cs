using LPContribMvc.UI.ReturnUrl;
using System.Collections.Generic;
using System.Web.Routing;

namespace LPContribMvc.UI.ParamBuilder
{
	public class ParamBuilder : ExplicitFacadeDictionary<string, object>
	{
		private readonly IReturnUrlManager _returnUrlManager;

		private readonly IDictionary<string, object> _param = new Dictionary<string, object>();

		protected override IDictionary<string, object> Wrapped => _param;

		public ParamBuilder(IReturnUrlManager returnUrlManager)
		{
			_returnUrlManager = returnUrlManager;
		}

		public RouteValueDictionary ToRoute()
		{
			return new RouteValueDictionary(this);
		}

		public static implicit operator RouteValueDictionary(ParamBuilder paramBuilder)
		{
			return new RouteValueDictionary(paramBuilder);
		}

		public ParamBuilder Add(string key, object value)
		{
			_param.Add(key, string.Concat(value));
			return this;
		}

		public ParamBuilder ReturnUrl()
		{
			return ReturnUrl(_returnUrlManager.GetReturnUrl());
		}

		public ParamBuilder ReturnUrl(string url)
		{
			_param.Add("ReturnUrl", url);
			return this;
		}
	}
}
