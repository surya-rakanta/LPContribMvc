using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web.Mvc;

namespace LPContribMvc.Filters
{
	public class ModelStateToTempDataAttribute : ActionFilterAttribute
	{
		public const string TempDataKey = "__LPContribMvc_ValidationFailures__";

		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			ModelStateDictionary modelState = ((ControllerContext)filterContext).Controller.ViewData.ModelState;
			ControllerBase controller = ((ControllerContext)filterContext).Controller;
			if (filterContext.Result is ViewResultBase)
			{
				CopyTempDataToModelState(controller.ViewData.ModelState, controller.TempData);
			}
			else if ((filterContext.Result is RedirectToRouteResult || filterContext.Result is RedirectResult) && !modelState.IsValid)
			{
				CopyModelStateToTempData(controller.ViewData.ModelState, controller.TempData);
			}
		}

		private void CopyTempDataToModelState(ModelStateDictionary modelState, TempDataDictionary tempData)
		{
			if (tempData.ContainsKey("__LPContribMvc_ValidationFailures__"))
			{
				ModelStateDictionary val = tempData["__LPContribMvc_ValidationFailures__"] as ModelStateDictionary;
				if (val != null)
				{
					foreach (KeyValuePair<string, ModelState> item in val)
					{
						if (modelState.ContainsKey(item.Key))
						{
							modelState[item.Key].Value = item.Value.Value;
							foreach (ModelError item2 in (Collection<ModelError>)(object)item.Value.Errors)
							{
								((Collection<ModelError>)(object)modelState[item.Key].Errors).Add(item2);
							}
						}
						else
						{
							modelState.Add(item.Key, item.Value);
						}
					}
				}
			}
		}

		private static void CopyModelStateToTempData(ModelStateDictionary modelState, TempDataDictionary tempData)
		{
			tempData["__LPContribMvc_ValidationFailures__"] = (object)modelState;
		}

		public ModelStateToTempDataAttribute() : base()
		{
		}
	}
}
