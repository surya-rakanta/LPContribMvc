using Microsoft.Web.Mvc;
using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace LPContribMvc.UI.MenuBuilder
{
	public class ActionMenuItem<T> : MenuItem where T : Controller
	{
		public Expression<Action<T>> MenuAction
		{
			get;
			set;
		}

		public override void Prepare(ControllerContext controllerContext)
		{
			if (MenuAction == null)
			{
				throw new InvalidOperationException("MenuAction must be defined prior to using an ActionMenuItem");
			}
			if (string.IsNullOrEmpty(base.HelpText))
			{
				object[] customAttributes = ((MethodCallExpression)MenuAction.Body).Method.GetCustomAttributes(typeof(MenuHelpText), inherit: false);
				if (customAttributes.Length > 0)
				{
					base.HelpText = ((MenuHelpText)customAttributes[0]).HelpText;
				}
			}
			if (string.IsNullOrEmpty(base.ActionUrl))
			{
				base.ActionUrl = LinkBuilder.BuildUrlFromExpression<T>(controllerContext.RequestContext, RouteTable.Routes, MenuAction);
			}
			if (string.IsNullOrEmpty(base.Title) && string.IsNullOrEmpty(base.Icon))
			{
				object[] customAttributes2 = ((MethodCallExpression)MenuAction.Body).Method.GetCustomAttributes(typeof(MenuTitle), inherit: false);
				if (customAttributes2.Length > 0)
				{
					base.Title = ((MenuTitle)customAttributes2[0]).Title;
				}
				else
				{
					MethodCallExpression methodCallExpression = MenuAction.Body as MethodCallExpression;
					if (methodCallExpression != null)
					{
						base.Title = SplitPascalCase(methodCallExpression.Method.Name);
					}
				}
			}
			base.Prepare(controllerContext);
		}

		protected virtual string SplitPascalCase(string input)
		{
			return Regex.Replace(input, "([A-Z])", " $1", RegexOptions.Compiled).Trim();
		}

		public ActionMenuItem<T> SetMenuAction(Expression<Action<T>> menuAction)
		{
			MenuAction = menuAction;
			return this;
		}
	}
}
