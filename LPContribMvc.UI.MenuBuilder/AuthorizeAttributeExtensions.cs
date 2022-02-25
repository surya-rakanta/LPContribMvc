using System.Web.Mvc;

namespace LPContribMvc.UI.MenuBuilder
{
	public static class AuthorizeAttributeExtensions
	{
		public static bool Authorized(this AuthorizeAttribute attribute, ControllerContext context)
		{
			OpenAuthorizeAttribute openAuthorizeAttribute = new OpenAuthorizeAttribute(attribute);
			return openAuthorizeAttribute.Authorized(context.RequestContext);
		}
	}
}
