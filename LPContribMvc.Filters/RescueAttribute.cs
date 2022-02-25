using System;
using System.Linq;
using System.Web.Mvc;

namespace LPContribMvc.Filters
{
	[Serializable]
	[Obsolete("Use System.Web.Mvc.HandleErrorAttribute instead.")]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
	public class RescueAttribute : FilterAttribute, IExceptionFilter
	{
		private string _view;

		private readonly Type[] _exceptionsTypes;

		public string ViewName
		{
			get
			{
				return "Rescues/" + _view;
			}
			protected set
			{
				_view = value;
			}
		}

		public Type[] ExceptionsTypes => _exceptionsTypes;

		public bool AutoLocate
		{
			get;
			set;
		}

		public bool IgnoreAjax
		{
			get;
			set;
		}

		public Type[] IgnoreTypes
		{
			get;
			set;
		}

		public RescueAttribute(string view)
			: this(view, typeof(Exception))
		{
		}

		public RescueAttribute(string view, params Type[] exceptionTypes) : base()
		{
			if (string.IsNullOrEmpty(view))
			{
				throw new ArgumentException("view is required", "view");
			}
			_view = view;
			if (exceptionTypes != null)
			{
				_exceptionsTypes = exceptionTypes;
			}
			IgnoreTypes = new Type[0];
			AutoLocate = true;
			IgnoreAjax = true;
		}

		public virtual void OnException(ExceptionContext filterContext)
		{
			Type type = filterContext.Exception.GetBaseException().GetType();
			if ((IgnoreAjax && AjaxRequestExtensions.IsAjaxRequest(((ControllerContext)filterContext).HttpContext.Request)) || (IgnoreTypes != null && IgnoreTypes.Contains(type)))
			{
				return;
			}
			Type[] exceptionsTypes = ExceptionsTypes;
			int num = 0;
			Type type2;
			while (true)
			{
				if (num < exceptionsTypes.Length)
				{
					type2 = exceptionsTypes[num];
					if (type2.IsAssignableFrom(type))
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			if (AutoLocate)
			{
				if (ViewExists(type, (ControllerContext)(object)filterContext))
				{
					ViewName = type.Name;
					filterContext.Result = CreateActionResult(filterContext.Exception, filterContext);
					filterContext.ExceptionHandled = true;
					return;
				}
				if (ViewExists(type2, (ControllerContext)(object)filterContext))
				{
					ViewName = type2.Name;
					filterContext.Result = CreateActionResult(filterContext.Exception, filterContext);
					filterContext.ExceptionHandled = true;
					return;
				}
			}
			filterContext.Result = CreateActionResult(filterContext.Exception, filterContext);
			filterContext.ExceptionHandled = true;
		}

		protected virtual ActionResult CreateActionResult(Exception exception, ExceptionContext context)
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Expected O, but got Unknown
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Expected O, but got Unknown
			string text = (string)((ControllerContext)context).RouteData.Values["controller"];
			string text2 = (string)((ControllerContext)context).RouteData.Values["action"];
			ViewDataDictionary<HandleErrorInfo> viewData = new ViewDataDictionary<HandleErrorInfo>((HandleErrorInfo)(object)new HandleErrorInfo(exception, text, text2));
			ViewResult val = (ViewResult)(object)new ViewResult();
			((ViewResultBase)val).ViewName = ViewName;
			((ViewResultBase)val).ViewData = (ViewDataDictionary)(object)viewData;
			((ViewResultBase)val).TempData = ((ControllerContext)context).Controller.TempData;
			return (ActionResult)(object)val;
		}

		protected virtual ViewDataDictionary CreateViewData(Exception exception, ExceptionContext context)
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Expected O, but got Unknown
			string text = (string)((ControllerContext)context).RouteData.Values["controller"];
			string text2 = (string)((ControllerContext)context).RouteData.Values["action"];
			return (ViewDataDictionary)(object)new ViewDataDictionary<HandleErrorInfo>((HandleErrorInfo)(object)new HandleErrorInfo(exception, text, text2));
		}

		protected virtual bool ViewExists(Type exceptionType, ControllerContext controllerContext)
		{
			string text = "Rescues/" + exceptionType.Name;
			ViewEngineResult val = ViewEngines.Engines.FindView(controllerContext, text, (string)null);
			return val.View != null;
		}

		protected virtual ViewContext CreateViewContext(Exception exception, ControllerContext controllerContext)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected O, but got Unknown
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Expected O, but got Unknown
			return (ViewContext)(object)new ViewContext(controllerContext, (IView)null, (ViewDataDictionary)(object)new ViewDataDictionary((object)exception), controllerContext.Controller.TempData, controllerContext.HttpContext.Response.Output);
		}
	}
}
