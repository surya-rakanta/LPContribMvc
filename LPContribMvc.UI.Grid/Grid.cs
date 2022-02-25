using LPContribMvc.UI.Grid.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace LPContribMvc.UI.Grid
{
	public class Grid<T> : IGrid<T>, IGridWithOptions<T>, IHtmlString where T : class
	{
		private readonly ViewContext _context;

		private IGridModel<T> _gridModel = new GridModel<T>();

		public IGridModel<T> Model => _gridModel;

		public IEnumerable<T> DataSource
		{
			get;
			private set;
		}

		public Grid(IEnumerable<T> dataSource, ViewContext context)
		{
			_context = context;
			DataSource = dataSource;
		}

		public IGridWithOptions<T> RenderUsing(IGridRenderer<T> renderer)
		{
			_gridModel.Renderer = renderer;
			return this;
		}

		public string Testing(IEnumerable<T> dataSource)
		{
			return "Hello, World";
		}

		public IGridWithOptions<T> Columns(Action<ColumnBuilder<T>> columnBuilder)
		{
			ColumnBuilder<T> columnBuilder2 = new ColumnBuilder<T>();
			columnBuilder(columnBuilder2);
			foreach (GridColumn<T> item in columnBuilder2)
			{
				if (!item.Position.HasValue)
				{
					_gridModel.Columns.Add(item);
				}
				else
				{
					_gridModel.Columns.Insert(item.Position.Value, item);
				}
			}
			
			_gridModel.HeaderTreeDepth = columnBuilder2.TreeDepth;

			return this;
		}

		public IGridWithOptions<T> Empty(string emptyText)
		{
			_gridModel.EmptyText = emptyText;
			return this;
		}

		public IGridWithOptions<T> Attributes(IDictionary<string, object> attributes)
		{
			_gridModel.Attributes = attributes;
			return this;
		}

		public IGrid<T> WithModel(IGridModel<T> model)
		{
			_gridModel = model;
			return this;
		}

		public IGridWithOptions<T> Sort(GridSortOptions sortOptions)
		{
			_gridModel.SortOptions = sortOptions;
			return this;
		}

		public IGridWithOptions<T> Sort(GridSortOptions sortOptions, string prefix)
		{
			_gridModel.SortOptions = sortOptions;
			_gridModel.SortPrefix = prefix;
			return this;
		}

		public override string ToString()
		{
			return ToHtmlString();
		}

		public string ToHtmlString()
		{
			StringWriter stringWriter = new StringWriter();
			_gridModel.Renderer.Render(_gridModel, DataSource, stringWriter, _context);
		return stringWriter.ToString();
		}

		public IGridWithOptions<T> HeaderRowAttributes(IDictionary<string, object> attributes)
		{
			_gridModel.Sections.HeaderRowAttributes(attributes);
			return this;
		}

		[Obsolete("The Render method is deprecated. From within a Razor view, use @Html.Grid() without a call to Render.")]
		public void Render()
		{
			_gridModel.Renderer.Render(_gridModel, DataSource, _context.Writer, _context);
		}

		public IGridWithOptions<T> RowAttributes(Func<GridRowViewData<T>, IDictionary<string, object>> attributes)
		{
			_gridModel.Sections.RowAttributes(attributes);
			return this;
		}
	}
}
