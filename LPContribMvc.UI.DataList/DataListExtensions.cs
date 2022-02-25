using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace LPContribMvc.UI.DataList
{
	public static class DataListExtensions
	{
		[Obsolete]
		public static DataList<T> DataList<T>(this HtmlHelper helper, IEnumerable<T> dataSource)
		{
			return new DataList<T>(dataSource, helper.ViewContext.Writer);
		}

		[Obsolete]
		public static DataList<T> DataList<T>(this HtmlHelper helper, IEnumerable<T> dataSource, params Func<object, object>[] tableAttributes)
		{
			return new DataList<T>(dataSource, helper.ViewContext.Writer, new Hash(tableAttributes));
		}
	}
}
