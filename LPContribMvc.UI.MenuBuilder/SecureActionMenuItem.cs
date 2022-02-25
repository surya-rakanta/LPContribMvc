using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;

namespace LPContribMvc.UI.MenuBuilder
{
	public class SecureActionMenuItem<T> : ActionMenuItem<T> where T : Controller
	{
		public override void Prepare(ControllerContext controllerContext)
		{
			if (base.MenuAction == null)
			{
				throw new InvalidOperationException("MenuAction must be defined prior to using a secure menu item");
			}
			MethodCallExpression methodCallExpression = base.MenuAction.Body as MethodCallExpression;
			if (methodCallExpression == null)
			{
				throw new InvalidOperationException("Expression must be a method call");
			}
			AuthorizeAttribute[] authorizeAttributes = GetAuthorizeAttributes(methodCallExpression.Method);
			internalDisabled = (base.Disabled || !CanAddItem(authorizeAttributes, controllerContext));
			base.Prepare(controllerContext);
		}

		protected virtual bool CanAddItem(IEnumerable<AuthorizeAttribute> attributes, ControllerContext context)
		{
			foreach (AuthorizeAttribute attribute in attributes)
			{
				if (attribute != null && !attribute.Authorized(context))
				{
					return false;
				}
			}
			return true;
		}

		protected virtual AuthorizeAttribute[] GetAuthorizeAttributes(MethodInfo methodInfo)
		{
			if (methodInfo == null)
			{
				throw new ArgumentNullException("methodInfo");
			}
			AuthorizeAttribute[] first = (AuthorizeAttribute[])methodInfo.ReflectedType.GetCustomAttributes(typeof(AuthorizeAttribute), inherit: true);
			AuthorizeAttribute[] second = (AuthorizeAttribute[])methodInfo.GetCustomAttributes(typeof(AuthorizeAttribute), inherit: true);
			return (from attr in first.Concat(second)
				orderby ((FilterAttribute)attr).Order
				select attr).ToArray();
		}
	}
}
