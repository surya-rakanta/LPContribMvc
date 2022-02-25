using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace LPContribMvc.UI.Grid
{
	public abstract class GridRenderer<T> : IGridRenderer<T> where T : class
	{
		private TextWriter _writer;

		private readonly ViewEngineCollection _engines;

		protected IGridModel<T> GridModel
		{
			get;
			private set;
		}

		protected IEnumerable<T> DataSource
		{
			get;
			private set;
		}

		protected ViewContext Context
		{
			get;
			private set;
		}

		protected TextWriter Writer => _writer;

		protected bool IsSortingEnabled => GridModel.SortOptions != null;

		protected GridRenderer()
			: this(ViewEngines.Engines)
		{
		}

		protected GridRenderer(ViewEngineCollection engines)
		{
			_engines = engines;
		}

		protected IDictionary<int, IList<GridColumn<T>>> _headerRows= new Dictionary<int, IList<GridColumn<T>>>();

		protected int _headerTreeDepth;

		protected IList<GridColumn<T>> _leafColumns = new List<GridColumn<T>>();

		private int CalcColSpan(GridColumn<T> column)
		{
			int nCount;
			nCount = (column.ChildColumns.Count > 0) ? 0 : 1;
			foreach (GridColumn<T> item in column.ChildColumns)
			{
				nCount += CalcColSpan(item);
			}
			column.ColSpan = nCount;
			return nCount;
		}

		private string WalkColumnTree(GridColumn<T> column, int nLevel)
		{
			string sRet = "";
			if (!_headerRows.ContainsKey(nLevel))
			{
				_headerRows.Add(nLevel, new List<GridColumn<T>>());
			}
			_headerRows[nLevel].Add(column);
			if (column.ChildColumns.Count<=0)
			{
				_leafColumns.Add(column);
			}
			foreach (GridColumn<T> item in column.ChildColumns)
			{
				sRet += WalkColumnTree(item, nLevel+1);
			}
			return sRet;
		}

		public void Render(IGridModel<T> gridModel, IEnumerable<T> dataSource, TextWriter output, ViewContext context)
		{
			_writer = output;
			GridModel = gridModel;
			DataSource = dataSource;
			Context = context;
			_headerTreeDepth = gridModel.HeaderTreeDepth;

			if (_headerTreeDepth > 0)
			{
				foreach (GridColumn<T> item in gridModel.Columns)
				{
					CalcColSpan(item);
				}
				foreach (GridColumn<T> item in gridModel.Columns)
				{
					WalkColumnTree(item, 1);
				}
			}

			RenderGridStart();

			bool flag = RenderHeader();

			if (flag)
			{
				RenderItems();
			}
			else
			{
				RenderEmpty();
			}
			RenderGridEnd(!flag);
		}

		protected void RenderText(string text)
		{
			Writer.Write(text);
		}

		protected virtual void RenderItems()
		{
			//throw new System.Exception("This is " + _writer.ToString());
			RenderBodyStart();
			bool flag = false;
			if (_headerTreeDepth <= 0)
			{
				foreach (T item in DataSource)
				{
					RenderItem(new GridRowViewData<T>(item, flag));
					flag = !flag;
				}
			}
			else
			{
				foreach (T item in DataSource)
				{
					RenderLeafItem(new GridRowViewData<T>(item, flag));
					flag = !flag;
				}
			}

			RenderBodyEnd();

		}

		protected virtual void RenderItem(GridRowViewData<T> rowData)
		{
			BaseRenderRowStart(rowData);
			foreach (GridColumn<T> item in VisibleColumns())
			{
				if (item.CustomItemRenderer != null)
				{
					item.CustomItemRenderer(new RenderingContext(Writer, Context, _engines), rowData.Item);
				}
				else
				{
					RenderStartCell(item, rowData);
					RenderCellValue(item, rowData);
					RenderEndCell();
				}
			}
			BaseRenderRowEnd(rowData);
		}
		protected virtual void RenderLeafItem(GridRowViewData<T> rowData)
		{
			BaseRenderRowStart(rowData);
			foreach (GridColumn<T> item in _leafColumns)
			{
				if (item.CustomItemRenderer != null)
				{
					item.CustomItemRenderer(new RenderingContext(Writer, Context, _engines), rowData.Item);
				}
				else
				{
					RenderStartCell(item, rowData);
					RenderCellValue(item, rowData);
					RenderEndCell();
				}
			}
			BaseRenderRowEnd(rowData);
		}

		protected virtual void RenderCellValue(GridColumn<T> column, GridRowViewData<T> rowData)
		{
			object value = column.GetValue(rowData.Item);
			if (value != null)
			{
				RenderText(value.ToString());
			}
		}

		protected virtual bool RenderMultiHeading()
		{

			if (!ShouldRenderHeader())
			{
				return false;
			}

			RenderMultiHeadStart();

			foreach (int level in _headerRows.Keys)
			{
				RenderMultiHeadRowStart();
				foreach (var item in _headerRows[level])
				{
					RenderHeaderCellStart(item, level);
					RenderHeaderText(item);
					RenderHeaderCellEnd();
				}
				RenderMultiHeadRowEnd();
			}

			RenderMultiHeadEnd();

			return true;

		}

		protected virtual bool RenderHeader()
		{
			if (!ShouldRenderHeader())
			{
				return false;
			}

			if (_headerTreeDepth>0)
			{
				RenderMultiHeading();
				return true;
			}

			RenderHeadStart();

			foreach (GridColumn<T> item in VisibleColumns())
			{
				if (item.CustomHeaderRenderer != null)
				{
					item.CustomHeaderRenderer(new RenderingContext(Writer, Context, _engines));
				}
				else
				{
					RenderHeaderCellStart(item);
					RenderHeaderText(item);
					RenderHeaderCellEnd();
				}
			}
			RenderHeadEnd();
			return true;
		}

		protected virtual void RenderHeaderText(GridColumn<T> column)
		{
			string header = column.GetHeader();
			if (header != null)
			{
				RenderText(header);
			}
			else
			{
				RenderText(column.DisplayName);
			}
		}

		protected virtual bool ShouldRenderHeader()
		{
			return !IsDataSourceEmpty();
		}

		protected bool IsDataSourceEmpty()
		{
			if (DataSource != null)
			{
				return !DataSource.Any();
			}
			return true;
		}

		protected IEnumerable<GridColumn<T>> VisibleColumns()
		{
			return GridModel.Columns.Where((GridColumn<T> x) => x.Visible);
		}

		protected void BaseRenderRowStart(GridRowViewData<T> rowData)
		{
			if (!GridModel.Sections.Row.StartSectionRenderer(rowData, new RenderingContext(Writer, Context, _engines)))
			{
				RenderRowStart(rowData);
			}
		}

		protected void BaseRenderRowEnd(GridRowViewData<T> rowData)
		{
			if (!GridModel.Sections.Row.EndSectionRenderer(rowData, new RenderingContext(Writer, Context, _engines)))
			{
				RenderRowEnd();
			}
		}

		protected abstract void RenderHeaderCellEnd();

		protected abstract void RenderHeaderCellStart(GridColumn<T> column, int nLevel=0);

		protected abstract void RenderRowStart(GridRowViewData<T> rowData);

		protected abstract void RenderRowEnd();

		protected abstract void RenderEndCell();

		protected abstract void RenderStartCell(GridColumn<T> column, GridRowViewData<T> rowViewData);

		protected abstract void RenderHeadStart();

		protected abstract void RenderMultiHeadStart();

		protected abstract void RenderMultiHeadRowStart();

		protected abstract void RenderHeadEnd();

		protected abstract void RenderMultiHeadEnd();

		protected abstract void RenderMultiHeadRowEnd();

		protected abstract void RenderGridStart();

		protected abstract void RenderGridEnd(bool isEmpty);

		protected abstract void RenderEmpty();

		protected abstract void RenderBodyStart();

		protected abstract void RenderBodyEnd();
	}
}
