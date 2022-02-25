using System;
using System.Web.Mvc;

namespace LPContribMvc.UI.Html
{
	public static class HtmlHelperExtension
	{
		[Obsolete("Use Url.Content instead.")]
		public static string ResolveUrl(this HtmlHelper html, string relativeUrl)
		{
			if (relativeUrl == null)
			{
				return null;
			}
			if (!relativeUrl.StartsWith("~"))
			{
				return relativeUrl;
			}
			string applicationPath = ((ControllerContext)html.ViewContext).HttpContext.Request.ApplicationPath;
			string text = applicationPath + relativeUrl.Substring(1);
			return text.Replace("//", "/");
		}

		[Obsolete("Use Url.Content instead.")]
		public static string Stylesheet(this HtmlHelper html, string cssFile)
		{
			string relativeUrl = cssFile.Contains("~") ? cssFile : ("~/content/css/" + cssFile);
			string arg = html.ResolveUrl(relativeUrl);
			return $"<link type=\"text/css\" rel=\"stylesheet\" href=\"{arg}\" />\n";
		}

		[Obsolete("Use Url.Content instead.")]
		public static string Stylesheet(this HtmlHelper html, string cssFile, string media)
		{
			string relativeUrl = cssFile.Contains("~") ? cssFile : ("~/content/css/" + cssFile);
			string arg = html.ResolveUrl(relativeUrl);
			return $"<link type=\"text/css\" rel=\"stylesheet\" href=\"{arg}\" media=\"{media}\" />\n";
		}

		[Obsolete("Use Url.Content instead.")]
		public static string ScriptInclude(this HtmlHelper html, string jsFile)
		{
			string relativeUrl = jsFile.Contains("~") ? jsFile : ("~/Scripts/" + jsFile);
			string arg = html.ResolveUrl(relativeUrl);
			return $"<script type=\"text/javascript\" src=\"{arg}\" ></script>\n";
		}

		[Obsolete("Use Url.Content instead.")]
		public static string Favicon(this HtmlHelper html)
		{
			string arg = html.ResolveUrl("~/favicon.ico");
			return $"<link rel=\"shortcut icon\" href=\"{arg}\" />\n";
		}
	}
}
