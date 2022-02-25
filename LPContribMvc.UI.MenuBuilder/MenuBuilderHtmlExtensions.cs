using System.Web.Mvc;

namespace LPContribMvc.UI.MenuBuilder
{
	public static class MenuBuilderHtmlExtensions
	{
		public static void Menu(this HtmlHelper helper, MenuItem menu)
		{
			menu.RenderHtml((ControllerContext)(object)helper.ViewContext, helper.ViewContext.Writer);
		}
	}
}
