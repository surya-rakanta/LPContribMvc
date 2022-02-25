using LPContribMvc.Sorting;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace LPContribMvc.UI.Grid
{
	public interface IGridColumn<T> 
	{
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("CustomHeaderRenderer has been deprecated. Please use Header instead.")]
		Action<RenderingContext> CustomHeaderRenderer
		{
			get;
			set;
		}

		[Obsolete("CustomItemRenderer has been deprecated. Please use column.Custom instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		Action<RenderingContext, T> CustomItemRenderer
		{
			get;
			set;
		}

		int ColSpan { get; set; }

		IGridColumn<T> Named(string name);

		IGridColumn<T> DoNotSplit();

		IGridColumn<T> Format(string format);

		IGridColumn<T> CellCondition(Func<T, bool> func);

		IGridColumn<T> Visible(bool isVisible);

		IGridColumn<T> Header(Func<object, object> customHeaderRenderer);

		IGridColumn<T> Encode(bool shouldEncode);

		[Obsolete("Use Encode(false) instead.")]
		IGridColumn<T> DoNotEncode();

		IGridColumn<T> HeaderAttributes(IDictionary<string, object> attributes);

		IGridColumn<T> Attributes(Func<GridRowViewData<T>, IDictionary<string, object>> attributes);

		IGridColumn<T> Sortable(bool isColumnSortable);

		IGridColumn<T> SortColumnName(string name);

		IGridColumn<T> AddChild(object childColumn);

		IGridColumn<T> SortInitialDirection(SortDirection initialDirection);

		IGridColumn<T> InsertAt(int index);
	}
}
