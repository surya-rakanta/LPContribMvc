using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace LPContribMvc.UI.InputBuilder.ViewEngine
{
	public class InputBuilderViewEngine : WebFormViewEngine
	{
		public InputBuilderViewEngine(string[] subdirs) : base()
		{
			IEnumerable<string> source = subdirs.Concat(new string[1]
			{
				"InputBuilders"
			});
			((VirtualPathProviderViewEngine)this).PartialViewLocationFormats = source.Select((string s) => "~/Views/" + s + "/{0}.aspx").Concat(subdirs.Select((string s) => "~/Views/" + s + "/{0}.ascx")).ToArray();
			((VirtualPathProviderViewEngine)this).MasterLocationFormats = source.Select((string s) => "~/Views/" + s + "/{0}.Master").ToArray();
			((VirtualPathProviderViewEngine)this).ViewLocationFormats = source.Select((string s) => "~/Views/" + s + "/{0}.aspx").Concat(subdirs.Select((string s) => "~/Views/" + s + "/{0}.ascx")).ToArray();
		}
	}
}
