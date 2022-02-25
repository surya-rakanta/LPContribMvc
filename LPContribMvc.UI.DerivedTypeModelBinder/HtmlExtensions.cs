using LPContribMvc.Binders;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace LPContribMvc.UI.DerivedTypeModelBinder
{
	public static class HtmlExtensions
	{
		public static MvcHtmlString TypeStamp<TModel>(this HtmlHelper<TModel> htmlHelper)
		{
			return InputExtensions.Hidden((HtmlHelper)(object)htmlHelper, DerivedTypeModelBinderCache.TypeStampFieldName, (object)DerivedTypeModelBinderCache.GetTypeName(typeof(TModel)));
		}
	}
}
