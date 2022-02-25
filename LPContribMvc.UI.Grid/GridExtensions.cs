using LPContribMvc.UI.Grid.Syntax;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace LPContribMvc.UI.Grid
{
	public static class GridExtensions
	{
		private const string CouldNotFindView = "The view '{0}' or its master could not be found. The following locations were searched:{1}";

		public static IGrid<T> Grid<T>(this HtmlHelper helper, IEnumerable<T> dataSource) where T : class
		{
			return new Grid<T>(dataSource, helper.ViewContext);
		}

		public static IGrid<T> Grid<T>(this HtmlHelper helper, string viewDataKey) where T : class
		{
			IEnumerable<T> enumerable = helper.ViewContext.ViewData.Eval(viewDataKey) as IEnumerable<T>;
			if (enumerable == null)
			{
				throw new InvalidOperationException($"Item in ViewData with key '{viewDataKey}' is not an IEnumerable of '{typeof(T).Name}'.");
			}
			return helper.Grid(enumerable);
		}

		public static IGridColumn<T> HeaderAttributes<T>(this IGridColumn<T> column, params Func<object, object>[] hash)
		{
			return column.HeaderAttributes(new Hash(hash));
		}

		public static IGridWithOptions<T> Attributes<T>(this IGridWithOptions<T> grid, params Func<object, object>[] hash) where T : class
		{
			return grid.Attributes(new Hash(hash));
		}

		public static IGridColumn<T> Attributes<T>(this IGridColumn<T> column, params Func<object, object>[] hash)
		{
			return column.Attributes((GridRowViewData<T> x) => new Hash(hash));
		}

		public static void RowAttributes<T>(this IGridSections<T> sections, Func<GridRowViewData<T>, IDictionary<string, object>> attributes) where T : class
		{
			sections.Row.Attributes = attributes;
		}

		public static IView TryLocatePartial(this ViewEngineCollection engines, ViewContext context, string viewName)
		{
			ViewEngineResult val = engines.FindPartialView((ControllerContext)(object)context, viewName);
			if (val.View == null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (string searchedLocation in val.SearchedLocations)
				{
					stringBuilder.AppendLine();
					stringBuilder.Append(searchedLocation);
				}
				throw new InvalidOperationException($"The view '{viewName}' or its master could not be found. The following locations were searched:{stringBuilder}");
			}
			return val.View;
		}

		public static void RowStart<T>(this IGridSections<T> sections, Func<GridRowViewData<T>, string> rowStart) where T : class
		{
			sections.Row.StartSectionRenderer = delegate(GridRowViewData<T> rowData, RenderingContext context)
			{
				context.Writer.Write(rowStart(rowData));
				return true;
			};
		}

		public static void RowEnd<T>(this IGridSections<T> sections, Func<GridRowViewData<T>, string> rowEnd) where T : class
		{
			sections.Row.EndSectionRenderer = delegate(GridRowViewData<T> rowData, RenderingContext context)
			{
				context.Writer.Write(rowEnd(rowData));
				return true;
			};
		}

		[Obsolete("Please use the overload of column.Header that accepts a razor template. Eg, column.For(x => x.Foo).Header(@<div>Custom markup here</div>)")]
		public static IGridColumn<T> Header<T>(this IGridColumn<T> column, string header) where T : class
		{
			return column.Header((object x) => header);
		}

		public static void HeaderRowAttributes<T>(this IGridSections<T> sections, IDictionary<string, object> attributes) where T : class
		{
			sections.HeaderRow.Attributes = ((GridRowViewData<T> x) => attributes);
		}

		public static IGridWithOptions<T> AutoGenerateColumns<T>(this IGrid<T> grid) where T : class
		{
			AutoColumnBuilder<T> autoColumnBuilder = new AutoColumnBuilder<T>(ModelMetadataProviders.Current);
			return grid.Columns(delegate(ColumnBuilder<T> columnBuilder)
			{
				foreach (GridColumn<T> item in autoColumnBuilder)
				{
					((ICollection<GridColumn<T>>)columnBuilder).Add(item);
				}
			});
		}

		public static IGridWithOptions<T> RowStart<T>(this IGridWithOptions<T> grid, Func<T, object> template) where T : class
		{
			grid.Model.Sections.Row.StartSectionRenderer = delegate(GridRowViewData<T> rowData, RenderingContext context)
			{
				context.Writer.Write(template(rowData.Item));
				return true;
			};
			return grid;
		}

		public static IGridWithOptions<T> RowEnd<T>(this IGridWithOptions<T> grid, Func<T, object> template) where T : class
		{
			grid.Model.Sections.Row.EndSectionRenderer = delegate(GridRowViewData<T> rowData, RenderingContext context)
			{
				context.Writer.Write(template(rowData.Item));
				return true;
			};
			return grid;
		}
	}
}
