using LPContribMvc.UI.InputBuilder.ViewEngine;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace LPContribMvc.PortableAreas
{
	public class EmbeddedResourceController : Controller
	{
		private static Dictionary<string, string> mimeTypes = InitializeMimeTypes();

		public ActionResult Index(string resourceName, string resourcePath)
		{
			if (!string.IsNullOrEmpty(resourcePath))
			{
				resourceName = resourcePath + "." + resourceName;
			}
			string areaName = (string)((Controller)this).RouteData.DataTokens["area"];
			AssemblyResourceStore resourceStoreForArea = AssemblyResourceManager.GetResourceStoreForArea(areaName);
			Stream resourceStream = resourceStoreForArea.GetResourceStream("~." + resourceName);
			if (resourceStream == null)
			{
				((Controller)this).Response.StatusCode = 404;
				return null;
			}
			string contentType = GetContentType(resourceName);
			return (ActionResult)(object)((EmbeddedResourceController)this).File(resourceStream, contentType);
		}

		private static string GetContentType(string resourceName)
		{
			string key = resourceName.Substring(resourceName.LastIndexOf('.')).ToLower();
			return mimeTypes[key];
		}

		private static Dictionary<string, string> InitializeMimeTypes()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add(".gif", "image/gif");
			dictionary.Add(".png", "image/png");
			dictionary.Add(".jpg", "image/jpeg");
			dictionary.Add(".js", "text/javascript");
			dictionary.Add(".css", "text/css");
			dictionary.Add(".txt", "text/plain");
			dictionary.Add(".xml", "application/xml");
			dictionary.Add(".zip", "application/zip");
			return dictionary;
		}

		public EmbeddedResourceController() : base()
		{
		}
	}
}
