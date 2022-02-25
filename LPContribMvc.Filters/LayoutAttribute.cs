using System;
using System.Web.Mvc;

namespace LPContribMvc.Filters
{
	[Obsolete]
	public class LayoutAttribute : ActionFilterAttribute
	{
		public string Layout
		{
			get;
			private set;
		}

		public LayoutAttribute(string layout) : base()
		{
			Layout = layout;
		}

		public override void OnResultExecuting(ResultExecutingContext filterContext)
		{
			((ActionFilterAttribute)this).OnResultExecuting(filterContext);
			ViewResult val = filterContext.Result as ViewResult;
			if (val != null)
			{
				val.MasterName = Layout;
			}
		}
	}
}
