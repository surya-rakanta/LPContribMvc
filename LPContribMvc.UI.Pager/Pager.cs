using LPContribMvc.Pagination;
using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LPContribMvc.UI.Pager
{
	public class Pager : IHtmlString
	{
		private readonly IPagination _pagination;

		private readonly ViewContext _viewContext;

		private string _paginationFormat = "Showing {0} - {1} of {2} ";

		private string _paginationSingleFormat = "Showing {0} of {1} ";

		private string _paginationFirst = "first";

		private string _paginationPrev = "prev";

		private string _paginationNext = "next";

		private string _paginationLast = "last";

		private string _pageQueryName = "page";

		private Func<int, string> _urlBuilder;

		protected ViewContext ViewContext => _viewContext;

		public Pager(IPagination pagination, ViewContext context)
		{
			_pagination = pagination;
			_viewContext = context;
			_urlBuilder = CreateDefaultUrl;
		}

		public Pager QueryParam(string queryStringParam)
		{
			_pageQueryName = queryStringParam;
			return this;
		}

		public Pager SingleFormat(string format)
		{
			_paginationSingleFormat = format;
			return this;
		}

		public Pager Format(string format)
		{
			_paginationFormat = format;
			return this;
		}

		public Pager First(string first)
		{
			_paginationFirst = first;
			return this;
		}

		public Pager Previous(string previous)
		{
			_paginationPrev = previous;
			return this;
		}

		public Pager Next(string next)
		{
			_paginationNext = next;
			return this;
		}

		public Pager Last(string last)
		{
			_paginationLast = last;
			return this;
		}

		public Pager Link(Func<int, string> urlBuilder)
		{
			_urlBuilder = urlBuilder;
			return this;
		}

		public override string ToString()
		{
			return ToHtmlString();
		}

		public string ToHtmlString()
		{
			if (_pagination.TotalItems == 0)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<div class='pagination'>");
			RenderLeftSideOfPager(stringBuilder);
			if (_pagination.TotalPages > 1)
			{
				RenderRightSideOfPager(stringBuilder);
			}
			stringBuilder.Append("</div>");
			return stringBuilder.ToString();
		}

		protected virtual void RenderLeftSideOfPager(StringBuilder builder)
		{
			builder.Append("<span class='paginationLeft'>");
			if (_pagination.PageSize == 1)
			{
				RenderNumberOfItemsWhenThereIsOnlyOneItemPerPage(builder);
			}
			else
			{
				RenderNumberOfItemsWhenThereAreMultipleItemsPerPage(builder);
			}
			builder.Append("</span>");
		}

		protected virtual void RenderRightSideOfPager(StringBuilder builder)
		{
			builder.Append("<span class='paginationRight'>");
			if (_pagination.PageNumber == 1)
			{
				builder.Append(_paginationFirst);
			}
			else
			{
				builder.Append(CreatePageLink(1, _paginationFirst));
			}
			builder.Append(" | ");
			if (_pagination.HasPreviousPage)
			{
				builder.Append(CreatePageLink(_pagination.PageNumber - 1, _paginationPrev));
			}
			else
			{
				builder.Append(_paginationPrev);
			}
			builder.Append(" | ");
			if (_pagination.HasNextPage)
			{
				builder.Append(CreatePageLink(_pagination.PageNumber + 1, _paginationNext));
			}
			else
			{
				builder.Append(_paginationNext);
			}
			builder.Append(" | ");
			int totalPages = _pagination.TotalPages;
			if (_pagination.PageNumber < totalPages)
			{
				builder.Append(CreatePageLink(totalPages, _paginationLast));
			}
			else
			{
				builder.Append(_paginationLast);
			}
			builder.Append("</span>");
		}

		protected virtual void RenderNumberOfItemsWhenThereIsOnlyOneItemPerPage(StringBuilder builder)
		{
			builder.AppendFormat(_paginationSingleFormat, _pagination.FirstItem, _pagination.TotalItems);
		}

		protected virtual void RenderNumberOfItemsWhenThereAreMultipleItemsPerPage(StringBuilder builder)
		{
			builder.AppendFormat(_paginationFormat, _pagination.FirstItem, _pagination.LastItem, _pagination.TotalItems);
		}

		private string CreatePageLink(int pageNumber, string text)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected O, but got Unknown
			TagBuilder val = (TagBuilder)(object)new TagBuilder("a");
			val.SetInnerText(text);
			val.MergeAttribute("href", _urlBuilder(pageNumber));
			return val.ToString((TagRenderMode)0);
		}

		private string CreateDefaultUrl(int pageNumber)
		{
			RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
			foreach (string item in ((ControllerContext)_viewContext).RequestContext.HttpContext.Request.QueryString.AllKeys.Where((string key) => key != null))
			{
				routeValueDictionary[item] = ((ControllerContext)_viewContext).RequestContext.HttpContext.Request.QueryString[item];
			}
			routeValueDictionary[_pageQueryName] = pageNumber;
			return UrlHelper.GenerateUrl((string)null, (string)null, (string)null, routeValueDictionary, RouteTable.Routes, ((ControllerContext)_viewContext).RequestContext, true);
		}
	}
}
