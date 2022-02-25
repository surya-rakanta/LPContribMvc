using System;
using System.Web;

namespace LPContribMvc.UI.ReturnUrl
{
	public class ReturnUrlManager : IReturnUrlManager
	{
		public const string ParameterName = "ReturnUrl";

		public string GetReturnUrl()
		{
			string text = HttpContext.Current.Request.Params["ReturnUrl"];
			if (string.IsNullOrEmpty(text))
			{
				throw new ApplicationException(string.Format("The Return URL has not been set.  Check the previous page to make sure the hyperlink to this page includes {0} as a query string or form parameter.", "ReturnUrl"));
			}
			return text;
		}

		public bool HasReturnUrl()
		{
			string value = HttpContext.Current.Request.Params["ReturnUrl"];
			return !string.IsNullOrEmpty(value);
		}

		public static string GetCurrentUrl()
		{
			return HttpContext.Current.Request.RawUrl;
		}
	}
}
