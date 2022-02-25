using System.Collections.Generic;

namespace LPContribMvc.UI.Grid
{
	public interface IGridModel<T> where T : class
	{
		IGridRenderer<T> Renderer
		{
			get;
			set;
		}

		IList<GridColumn<T>> Columns
		{
			get;
		}

		IGridSections<T> Sections
		{
			get;
		}

		string EmptyText
		{
			get;
			set;
		}

		int HeaderTreeDepth
		{
			get;
			set;
		}

		IDictionary<string, object> Attributes
		{
			get;
			set;
		}

		GridSortOptions SortOptions
		{
			get;
			set;
		}

		string SortPrefix
		{
			get;
			set;
		}
	}
}
