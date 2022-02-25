using LPContribMvc.UI.Grid.Syntax;
using System;

namespace LPContribMvc.UI.Grid.ActionSyntax
{
	public static class ActionSyntaxExtensions
	{
		[Obsolete("Please use the RowStart overload that takes a razor template.")]
		public static IGridWithOptions<T> RowStart<T>(this IGridWithOptions<T> grid, Action<T> block) where T : class
		{
			grid.Model.Sections.RowStart(block);
			return grid;
		}

		[Obsolete("Please use the RowStart overload that takes a razor template.")]
		public static IGridWithOptions<T> RowStart<T>(this IGridWithOptions<T> grid, Action<T, GridRowViewData<T>> block) where T : class
		{
			grid.Model.Sections.RowStart(block);
			return grid;
		}

		[Obsolete("Please use the RowEnd overload that takes a razor template.")]
		public static IGridWithOptions<T> RowEnd<T>(this IGridWithOptions<T> grid, Action<T> block) where T : class
		{
			grid.Model.Sections.RowEnd(block);
			return grid;
		}

		[Obsolete("Please use the RowStart overload that takes a razor template.")]
		public static void RowStart<T>(this IGridSections<T> sections, Action<T> block) where T : class
		{
			sections.Row.StartSectionRenderer = delegate(GridRowViewData<T> rowData, RenderingContext context)
			{
				block(rowData.Item);
				return true;
			};
		}

		[Obsolete("Please use the RowStart overload that takes a razor template.")]
		public static void RowStart<T>(this IGridSections<T> sections, Action<T, GridRowViewData<T>> block) where T : class
		{
			sections.Row.StartSectionRenderer = delegate(GridRowViewData<T> rowData, RenderingContext context)
			{
				block(rowData.Item, rowData);
				return true;
			};
		}

		[Obsolete("Please use the RowEnd overload that takes a razor template.")]
		public static void RowEnd<T>(this IGridSections<T> sections, Action<T> block) where T : class
		{
			sections.Row.EndSectionRenderer = delegate(GridRowViewData<T> rowData, RenderingContext context)
			{
				block(rowData.Item);
				return true;
			};
		}

		[Obsolete("Action Syntax extensions have been deprecated. Please use column.Header with the Razor view engine instead of using HeaderAction.")]
		public static IGridColumn<T> HeaderAction<T>(this IGridColumn<T> column, Action action)
		{
			column.CustomHeaderRenderer = delegate
			{
				action();
			};
			return column;
		}

		[Obsolete("Action Syntax extensions have been deprecated. Please use column.Custom with the Razor view engine instead of using Action.")]
		public static IGridColumn<T> Action<T>(this IGridColumn<T> column, Action<T> action)
		{
			column.CustomItemRenderer = delegate(RenderingContext context, T item)
			{
				action(item);
			};
			return column;
		}
	}
}
