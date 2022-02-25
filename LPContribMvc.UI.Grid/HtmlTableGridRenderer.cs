using LPContribMvc.Sorting;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace LPContribMvc.UI.Grid
{
	public class HtmlTableGridRenderer<T> : GridRenderer<T> where T : class
	{
		private const string DefaultCssClass = "grid";

		public HtmlTableGridRenderer(ViewEngineCollection engines)
			: base(engines)
		{
		}

		public HtmlTableGridRenderer()
		{
		}

		protected override void RenderHeaderCellEnd()
		{
			RenderText("</th>");
		}

		protected virtual void RenderEmptyHeaderCellStart()
		{
			RenderText("<th>");
		}

		protected override void RenderHeaderCellStart(GridColumn<T> column, int nLevel = 0)
		{
			//throw new System.Exception("Table Renderer");
			Dictionary<string, object> dictionary = new Dictionary<string, object>(column.HeaderAttributes);
			if (base.IsSortingEnabled && column.Sortable && base.GridModel.SortOptions.Column == GenerateSortColumnName(column))
			{
				string text = (base.GridModel.SortOptions.Direction == SortDirection.Ascending) ? "sort_asc" : "sort_desc";
				if (dictionary.ContainsKey("class") && dictionary["class"] != null)
				{
					text = string.Join(" ", dictionary["class"].ToString(), text);
				}
				dictionary["class"] = text;
			}
			string text2 = BuildHtmlAttributes(dictionary);
			if (text2.Length > 0)
			{
				text2 = " " + text2;
			}
			int nRowSpan, nColSpan;
			string sRowSpan="", sColSpan="";
			if (nLevel>0)
			{
				nRowSpan = (column.ChildColumns.Count > 0) ? 1 : (_headerTreeDepth - nLevel + 2);
				if (nRowSpan > 1)
				{
					sRowSpan = "rowSpan=" + "\"" + nRowSpan.ToString() + "\"";
					text2 = text2 + " " + sRowSpan;
				}
				nColSpan = column.ColSpan;
				if (nColSpan>1)
				{
					sColSpan = "colSpan=" + "\"" + nColSpan.ToString() + "\"";
					text2 = text2 + " " + sColSpan;
				}
			}

			RenderText($"<th{text2}>");
		}

		protected override void RenderHeaderText(GridColumn<T> column)
		{
			if (base.IsSortingEnabled && column.Sortable)
			{
				string text = GenerateSortColumnName(column);
				bool flag = base.GridModel.SortOptions.Column == text;
				GridSortOptions gridSortOptions = new GridSortOptions();
				gridSortOptions.Column = text;
				GridSortOptions gridSortOptions2 = gridSortOptions;
				if (flag)
				{
					gridSortOptions2.Direction = ((base.GridModel.SortOptions.Direction == SortDirection.Ascending) ? SortDirection.Descending : SortDirection.Ascending);
				}
				else
				{
					gridSortOptions2.Direction = (column.InitialDirection ?? base.GridModel.SortOptions.Direction);
				}
				RouteValueDictionary routeValueDictionary = CreateRouteValuesForSortOptions(gridSortOptions2, base.GridModel.SortPrefix);
				foreach (string item in ((ControllerContext)base.Context).RequestContext.HttpContext.Request.QueryString.AllKeys.Where((string key) => key != null))
				{
					if (!routeValueDictionary.ContainsKey(item))
					{
						routeValueDictionary[item] = ((ControllerContext)base.Context).RequestContext.HttpContext.Request.QueryString[item];
					}
				}
				string text2 = HtmlHelper.GenerateLink(((ControllerContext)base.Context).RequestContext, RouteTable.Routes, column.DisplayName, (string)null, (string)null, (string)null, routeValueDictionary, (IDictionary<string, object>)null);
				RenderText(text2);
			}
			else
			{
				base.RenderHeaderText(column);
			}
		}

		private RouteValueDictionary CreateRouteValuesForSortOptions(GridSortOptions sortOptions, string prefix)
		{
			if (string.IsNullOrEmpty(prefix))
			{
				return new RouteValueDictionary(sortOptions);
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add(prefix + ".Column", sortOptions.Column);
			dictionary.Add(prefix + ".Direction", sortOptions.Direction);
			return new RouteValueDictionary(dictionary);
		}

		protected virtual string GenerateSortColumnName(GridColumn<T> column)
		{
			return column.SortColumnName ?? column.Name ?? column.DisplayName;
		}

		protected override void RenderRowStart(GridRowViewData<T> rowData)
		{
			IDictionary<string, object> dictionary = base.GridModel.Sections.Row.Attributes(rowData);
			if (!dictionary.ContainsKey("class"))
			{
				dictionary["class"] = (rowData.IsAlternate ? "gridrow_alternate" : "gridrow");
			}
			string text = BuildHtmlAttributes(dictionary);
			if (text.Length > 0)
			{
				text = " " + text;
			}
			RenderText($"<tr{text}>");
		}

		protected override void RenderRowEnd()
		{
			RenderText("</tr>");
		}

		protected override void RenderEndCell()
		{
			RenderText("</td>");
		}

		protected override void RenderStartCell(GridColumn<T> column, GridRowViewData<T> rowData)
		{
			string text = BuildHtmlAttributes(column.Attributes(rowData));
			if (text.Length > 0)
			{
				text = " " + text;
			}
			RenderText($"<td{text}>");
		}

		protected override void RenderHeadStart()
		{
			string text = BuildHtmlAttributes(base.GridModel.Sections.HeaderRow.Attributes(new GridRowViewData<T>(null, isAlternate: false)));
			if (text.Length > 0)
			{
				text = " " + text;
			}
			RenderText($"<thead><tr{text}>");
		}

		protected override void RenderMultiHeadStart()
		{
			RenderText("<thead>");
		}

		protected override void RenderMultiHeadRowStart()
		{
			string text = BuildHtmlAttributes(base.GridModel.Sections.HeaderRow.Attributes(new GridRowViewData<T>(null, isAlternate: false)));
			if (text.Length > 0)
			{
				text = " " + text;
			}
			RenderText($"<tr{text}>");
		}

		protected override void RenderHeadEnd()
		{
			RenderText("</tr></thead>");
		}

		protected override void RenderMultiHeadEnd()
		{
			RenderText("</thead>");
		}

		protected override void RenderMultiHeadRowEnd()
		{
			RenderText("</tr>");
		}

		protected override void RenderGridStart()
		{
			if (!base.GridModel.Attributes.ContainsKey("class"))
			{
				base.GridModel.Attributes["class"] = "grid";
			}
			string text = BuildHtmlAttributes(base.GridModel.Attributes);
			if (text.Length > 0)
			{
				text = " " + text;
			}
			RenderText($"<table{text}>");
		}

		protected override void RenderGridEnd(bool isEmpty)
		{
			RenderText("</table>");
		}

		protected override void RenderEmpty()
		{
			RenderHeadStart();
			RenderEmptyHeaderCellStart();
			RenderHeaderCellEnd();
			RenderHeadEnd();
			RenderBodyStart();
			RenderText("<tr><td>" + base.GridModel.EmptyText + "</td></tr>");
			RenderBodyEnd();
		}

		protected override void RenderBodyStart()
		{
			RenderText("<tbody>");
		}

		protected override void RenderBodyEnd()
		{
			RenderText("</tbody>");
		}

		protected string BuildHtmlAttributes(IDictionary<string, object> attributes)
		{
			return DictionaryExtensions.ToHtmlAttributes(attributes);
		}
	}
}
