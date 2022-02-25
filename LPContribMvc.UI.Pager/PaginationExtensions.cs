using LPContribMvc.Pagination;
using System;
using System.Web.Mvc;

namespace LPContribMvc.UI.Pager
{
	public static class PaginationExtensions
	{
		public static Pager Pager(this HtmlHelper helper, string viewDataKey)
		{
			IPagination pagination = helper.ViewContext.ViewData.Eval(viewDataKey) as IPagination;
			if (pagination == null)
			{
				throw new InvalidOperationException($"Item in ViewData with key '{viewDataKey}' is not an IPagination.");
			}
			return helper.Pager(pagination);
		}

		public static Pager Pager(this HtmlHelper helper, IPagination pagination)
		{
			return new Pager(pagination, helper.ViewContext);
		}
	}
}
