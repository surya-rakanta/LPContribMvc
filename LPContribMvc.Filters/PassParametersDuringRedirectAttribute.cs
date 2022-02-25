using LPContribMvc.ActionResults;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;

namespace LPContribMvc.Filters
{
	public class PassParametersDuringRedirectAttribute : ActionFilterAttribute
	{
		public const string RedirectParameterPrefix = "__RedirectParameter__";

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			((ActionFilterAttribute)this).OnActionExecuting(filterContext);
			LoadParameterValuesFromTempData(filterContext);
		}

		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			IControllerExpressionContainer controllerExpressionContainer = filterContext.Result as IControllerExpressionContainer;
			RedirectToRouteResult val = filterContext.Result as RedirectToRouteResult;
			if (controllerExpressionContainer != null && controllerExpressionContainer.Expression != null && val != null)
			{
				IDictionary<string, object> dictionary = AddParameterValuesFromExpressionToTempData(((ControllerContext)filterContext).Controller.TempData, controllerExpressionContainer.Expression);
				RemoveStoredParametersFromRouteValues(val.RouteValues, dictionary.Keys);
			}
		}

		private static IDictionary<string, object> AddParameterValuesFromExpressionToTempData(TempDataDictionary tempData, MethodCallExpression call)
		{
			ParameterInfo[] parameters = call.Method.GetParameters();
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			if (parameters.Length > 0)
			{
				for (int i = 0; i < parameters.Length; i++)
				{
					Expression expression = call.Arguments[i];
					object obj = null;
					ConstantExpression constantExpression = expression as ConstantExpression;
					if (constantExpression != null)
					{
						obj = constantExpression.Value;
					}
					else
					{
						Expression<Func<object>> expression2 = Expression.Lambda<Func<object>>(Expression.Convert(expression, typeof(object)), new ParameterExpression[0]);
						obj = expression2.Compile()();
					}
					if (obj != null && !(obj is string) && (!(obj is ValueType) || !TypeDescriptor.GetConverter(obj).CanConvertFrom(typeof(string))))
					{
						tempData["__RedirectParameter__" + parameters[i].Name] = obj;
						dictionary.Add(parameters[i].Name, obj);
					}
				}
			}
			return dictionary;
		}

		private static void LoadParameterValuesFromTempData(ActionExecutingContext filterContext)
		{
			ParameterDescriptor[] parameters = filterContext.ActionDescriptor.GetParameters();
			foreach (KeyValuePair<string, object> storedParameter in GetStoredParameterValues(filterContext))
			{
				KeyValuePair<string, object> keyValuePair = storedParameter;
				if (keyValuePair.Value != null)
				{
					KeyValuePair<string, object> keyValuePair2 = storedParameter;
					string storedParameterName = GetParameterName(keyValuePair2.Key);
					if (parameters.Any(delegate(ParameterDescriptor actionParameter)
					{
						if (actionParameter.ParameterName == storedParameterName)
						{
							Type parameterType = actionParameter.ParameterType;
							KeyValuePair<string, object> keyValuePair5 = storedParameter;
							return parameterType.IsAssignableFrom(keyValuePair5.Value.GetType());
						}
						return false;
					}))
					{
						IDictionary<string, object> actionParameters = filterContext.ActionParameters;
						string key = storedParameterName;
						KeyValuePair<string, object> keyValuePair3 = storedParameter;
						actionParameters[key] = keyValuePair3.Value;
						TempDataDictionary tempData = ((ControllerContext)filterContext).Controller.TempData;
						KeyValuePair<string, object> keyValuePair4 = storedParameter;
						tempData.Keep(keyValuePair4.Key);
					}
				}
			}
		}

		private static void RemoveStoredParametersFromRouteValues(RouteValueDictionary dictionary, IEnumerable<string> keysToRemove)
		{
			foreach (string item in keysToRemove)
			{
				dictionary.Remove(item);
			}
		}

		private static string GetParameterName(string key)
		{
			if (key.StartsWith("__RedirectParameter__"))
			{
				return key.Substring("__RedirectParameter__".Length);
			}
			return key;
		}

		private static IList<KeyValuePair<string, object>> GetStoredParameterValues(ActionExecutingContext filterContext)
		{
			return ((IEnumerable<KeyValuePair<string, object>>)((ControllerContext)filterContext).Controller.TempData).Where((KeyValuePair<string, object> td) => td.Key.StartsWith("__RedirectParameter__")).ToList();
		}

		public PassParametersDuringRedirectAttribute() : base()
		{
		}
	}
}
