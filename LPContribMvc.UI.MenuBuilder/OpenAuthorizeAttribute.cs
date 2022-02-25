using System.Web.Mvc;
using System.Web.Routing;

namespace LPContribMvc.UI.MenuBuilder
{
	public class OpenAuthorizeAttribute : AuthorizeAttribute
	{
		public OpenAuthorizeAttribute(AuthorizeAttribute attribute) : base()
		{
			Order = attribute.Order;
			Roles = attribute.Roles;
			Users = attribute.Users;
		}

		public bool Authorized(RequestContext requestContext)
		{
			return AuthorizeCore(requestContext.HttpContext);
		}
	}
}
