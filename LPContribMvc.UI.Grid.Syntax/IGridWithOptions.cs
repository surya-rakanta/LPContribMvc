using System;
using LPContribMvc.Pagination;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;

namespace LPContribMvc.UI.Grid.Syntax
{
	public interface IGridWithOptions<T> : IHtmlString where T : class
	{
		[EditorBrowsable(EditorBrowsableState.Never)]
		IGridModel<T> Model
		{
			get;
		}

		IGridWithOptions<T> RenderUsing(IGridRenderer<T> renderer);

		IGridWithOptions<T> Columns(Action<ColumnBuilder<T>> columnBuilder);

		string Testing(IEnumerable<T> dataSource);

		IGridWithOptions<T> Empty(string emptyText);

		IGridWithOptions<T> Attributes(IDictionary<string, object> attributes);

		IGridWithOptions<T> RowAttributes(Func<GridRowViewData<T>, IDictionary<string, object>> attributes);

		IGridWithOptions<T> HeaderRowAttributes(IDictionary<string, object> attributes);

		IGridWithOptions<T> Sort(GridSortOptions sortOptions);

		IGridWithOptions<T> Sort(GridSortOptions sortOptions, string prefix);

		void Render();
	}
}
