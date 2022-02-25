using System;
using System.Collections.Generic;

namespace LPContribMvc.UI.Grid
{
	public class GridModel<T> : IGridModel<T> where T : class
	{
		private readonly ColumnBuilder<T> _columnBuilder;

		private readonly GridSections<T> _sections = new GridSections<T>();

		private IGridRenderer<T> _renderer = new HtmlTableGridRenderer<T>();

		private string _emptyText;

		private IDictionary<string, object> _attributes = new Dictionary<string, object>();

		private GridSortOptions _sortOptions;

		private string _sortPrefix;

		GridSortOptions IGridModel<T>.SortOptions
		{
			get
			{
				return _sortOptions;
			}
			set
			{
				_sortOptions = value;
			}
		}

		IList<GridColumn<T>> IGridModel<T>.Columns => _columnBuilder;

		public int HeaderTreeDepth
		{
			get;
			set;
		}

		IGridRenderer<T> IGridModel<T>.Renderer
		{
			get
			{
				return _renderer;
			}
			set
			{
				_renderer = value;
			}
		}

		string IGridModel<T>.EmptyText
		{
			get
			{
				return _emptyText;
			}
			set
			{
				_emptyText = value;
			}
		}

		IDictionary<string, object> IGridModel<T>.Attributes
		{
			get
			{
				return _attributes;
			}
			set
			{
				_attributes = value;
			}
		}

		string IGridModel<T>.SortPrefix
		{
			get
			{
				return _sortPrefix;
			}
			set
			{
				_sortPrefix = value;
			}
		}

		public ColumnBuilder<T> Column => _columnBuilder;

		IGridSections<T> IGridModel<T>.Sections => _sections;

		public GridSections<T> Sections => _sections;

		public GridModel()
		{
			_emptyText = "There is no data available.";
			_columnBuilder = CreateColumnBuilder();
		}

		public void Empty(string emptyText)
		{
			_emptyText = emptyText;
		}

		public void Attributes(params Func<object, object>[] hash)
		{
			Attributes(new Hash(hash));
		}

		public void Attributes(IDictionary<string, object> attributes)
		{
			_attributes = attributes;
		}

		public void RenderUsing(IGridRenderer<T> renderer)
		{
			_renderer = renderer;
		}

		public void Sort(GridSortOptions sortOptions)
		{
			_sortOptions = sortOptions;
		}

		public void Sort(GridSortOptions sortOptions, string prefix)
		{
			_sortOptions = sortOptions;
			_sortPrefix = prefix;
		}

		protected virtual ColumnBuilder<T> CreateColumnBuilder()
		{
			return new ColumnBuilder<T>();
		}
	}
}
