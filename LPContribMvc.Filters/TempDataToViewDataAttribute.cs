using System.Collections.Generic;
using System.Web.Mvc;

namespace LPContribMvc.Filters
{
	public class TempDataToViewDataAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			if (filterContext.Result is ViewResult)
			{
				TempDataDictionary tempData = ((ControllerContext)filterContext).Controller.TempData;
				ViewDataDictionary viewData = ((ControllerContext)filterContext).Controller.ViewData;
				foreach (KeyValuePair<string, object> item in tempData)
				{
					if (!viewData.ContainsKey(item.Key))
					{
						viewData[item.Key] = item.Value;
					}
				}
			}
		}

		public TempDataToViewDataAttribute() : base()
		{
		}
	}
}
