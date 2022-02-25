using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace LPContribMvc.UI.Html
{
	[Obsolete]
	public static class ModelVisualizerExtension
	{
		public static string HtmlContainer = "\n<div class=\"ToHtmlTableContainer\" >\n<div class=\"ToHtmlTableClosedContainer\"  style=\"cursor: pointer\" >- Click to open model information\n</div>\n<div class=\"ToHtmlTableOpenedContainer\">\n<div class=\"ToHtmlTableCloseText\" style=\"cursor: pointer\" >\n+ Click to close model information\n</div>\n<table border=1  >{0}</table></div></div>\n";

		public static string OpenCloseScript = "\n<script language=\"javascript\" >\n      $(document).ready(function() {\n          setToHtmlControlHandlers();\n          defaultToHtmlUI();\n      });\n\n      function openToHtmlTable() {\n        $(this).parent().children(\".ToHtmlTableOpenedContainer\").show();\n        $(this).hide();\n      }\n\n      function closeToHtmlTable() {\n        $(this).parent().hide();\n        $(this).parent().parent().children(\".ToHtmlTableClosedContainer\").show();\n      }\n\n      //Events der Buttons registrieren\n      function setToHtmlControlHandlers() {\n          $(\".ToHtmlTableClosedContainer\").bind('click', openToHtmlTable );\n          $(\".ToHtmlTableCloseText\").bind('click', closeToHtmlTable);\n      }\n\n      function defaultToHtmlUI() {\n         $(\".ToHtmlTableOpenedContainer\").hide();\n         $(\".ToHtmlTableClosedContainer\").show();\n      }\n\n</script> ";

		public static string ModelVisualizer(this HtmlHelper htmlHelper)
		{
			if (htmlHelper.ViewContext.ViewData.Model != null)
			{
				return ModelToHtmlTable(htmlHelper.ViewContext.ViewData.Model);
			}
			return ModelToHtmlTable(htmlHelper.ViewContext.ViewData);
		}

		private static string RenderSelectListTable(IEnumerable<SelectListItem> selectListToRender)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("<tr>");
			stringBuilder.AppendFormat("<td colspan=\"3\">SelectList</td>");
			stringBuilder.AppendFormat("</tr>");
			stringBuilder.AppendFormat("<tr>");
			stringBuilder.AppendFormat("<td>Name</td>");
			stringBuilder.AppendFormat("<td>Value</td>");
			stringBuilder.AppendFormat("<td>Selected</td>");
			stringBuilder.AppendFormat("</tr>");
			foreach (SelectListItem item in selectListToRender)
			{
				stringBuilder.AppendFormat("<tr>");
				stringBuilder.AppendFormat("<td>{0}</td>", HttpUtility.HtmlEncode(item.Text));
				stringBuilder.AppendFormat("<td>{0}</td>", HttpUtility.HtmlEncode(item.Value));
				stringBuilder.AppendFormat("<td>{0}</td>", item.Selected ? "selected" : "");
				stringBuilder.AppendFormat("</tr>");
			}
			string arg = stringBuilder.ToString();
			return string.Format(HtmlContainer, arg);
		}

		private static string RenderObjectTable(object objectToRender)
		{
			string str = "<table border=\"1\" >";
			Type type = objectToRender.GetType();
			str = str + "<tr><td colspan=\"2\" >" + type.ToString() + "</td></tr>";
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			PropertyInfo[] properties = type.GetProperties();
			foreach (PropertyInfo propertyInfo in properties)
			{
				dictionary.Add(propertyInfo.Name, propertyInfo.GetValue(objectToRender, null));
			}
			string str2 = RenderDictionaryTable(dictionary);
			str += str2;
			return str + "</table>";
		}

		private static string RenderGenericList(IEnumerable listToRender)
		{
			StringBuilder stringBuilder = new StringBuilder();
			PropertyInfo[] array = new PropertyInfo[0];
			bool flag = true;
			foreach (object item in listToRender)
			{
				if (flag)
				{
					array = item.GetType().GetProperties();
					stringBuilder.Append("<tr>");
					PropertyInfo[] array2 = array;
					foreach (PropertyInfo propertyInfo in array2)
					{
						stringBuilder.AppendFormat("<td><b>{0}</b></td>", propertyInfo.Name);
					}
					stringBuilder.Append("</tr>");
					flag = false;
				}
				stringBuilder.Append("<tr>");
				PropertyInfo[] array3 = array;
				foreach (PropertyInfo propertyInfo2 in array3)
				{
					stringBuilder.AppendFormat("<td>{0}</td>", propertyInfo2.GetValue(item, null));
				}
				stringBuilder.Append("</tr>");
			}
			string arg = stringBuilder.ToString();
			return string.Format(HtmlContainer, arg);
		}

		private static bool IsMicrosoftType(Type type)
		{
			object[] customAttributes = type.Assembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), inherit: false);
			return customAttributes.OfType<AssemblyCompanyAttribute>().Any((AssemblyCompanyAttribute attr) => attr.Company == "Microsoft Corporation");
		}

		private static string RenderDictionaryTable(IDictionary<string, object> viewData)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string item in viewData.Keys.OrderBy((string val) => val).ToList())
			{
				object obj = viewData[item];
				stringBuilder.AppendFormat("<tr>");
				stringBuilder.AppendFormat("<td>{0}</td>", HttpUtility.HtmlEncode(item));
				if (obj == null)
				{
					stringBuilder.AppendFormat("<td>null</td>");
				}
				else if (obj is Enum)
				{
					stringBuilder.AppendFormat("<td>{0}</td>", obj ?? string.Empty);
				}
				else if (!IsMicrosoftType(obj.GetType()))
				{
					stringBuilder.AppendFormat("<td>{0}</td>", RenderObjectTable(obj));
				}
				else if (obj is IEnumerable<SelectListItem>)
				{
					stringBuilder.AppendFormat("<td>{0}</td>", RenderSelectListTable((IEnumerable<SelectListItem>)obj));
				}
				else if (obj.GetType().ToString().StartsWith("System.Collections.Generic.List"))
				{
					stringBuilder.AppendFormat("<td>{0}</td>", RenderGenericList((IEnumerable)obj));
				}
				else
				{
					stringBuilder.AppendFormat("<td>{0}</td>", obj ?? string.Empty);
				}
				stringBuilder.AppendFormat("</tr>");
			}
			return stringBuilder.ToString();
		}

		private static string ModelToHtmlTable(object objectToRender)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(OpenCloseScript);
			IDictionary<string, object> dictionary = null;
			if (objectToRender is ViewDataDictionary)
			{
				dictionary = (IDictionary<string, object>)objectToRender;
			}
			else
			{
				dictionary = new Dictionary<string, object>();
				PropertyInfo[] properties = objectToRender.GetType().GetProperties();
				PropertyInfo[] array = properties;
				foreach (PropertyInfo propertyInfo in array)
				{
					dictionary.Add(propertyInfo.Name, propertyInfo.GetValue(objectToRender, null));
				}
			}
			string text = "";
			string value = string.Format(arg0: (dictionary.Count <= 0) ? "<tr><td>There is no data in ViewData</td></tr>" : RenderDictionaryTable(dictionary), format: HtmlContainer);
			stringBuilder.Append(value);
			return stringBuilder.ToString();
		}
	}
}
