using LPContribMvc.Sorting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace LPContribMvc.UI.Grid
{
	public class GridColumn<T> : IGridColumn<T> where T : class
	{
		private readonly string _name;

		private string _displayName;

		private bool _doNotSplit;

		private readonly Func<T, object> _columnValueFunc;

		private readonly Type _dataType;

		private Func<T, bool> _cellCondition = (T x) => true;

		private string _format;

		private bool _visible = true;

		private bool _htmlEncode = true;

		private readonly IDictionary<string, object> _headerAttributes = new Dictionary<string, object>();

		private List<Func<GridRowViewData<T>, IDictionary<string, object>>> _attributes = new List<Func<GridRowViewData<T>, IDictionary<string, object>>>();

		private List<GridColumn<T>> _childColumns = new List<GridColumn<T>>();

		private bool _sortable = true;

		private string _sortColumnName;

		private SortDirection? _initialDirection;

		private int? _position;

		private Func<object, object> _headerRenderer = (object x) => null;

		private int _colSpan = 0;

		public int ColSpan
		{
			get;
			set;
		}

		public List<GridColumn<T>> ChildColumns => _childColumns;

		public bool Sortable => _sortable;

		public bool Visible => _visible;

		public string SortColumnName => _sortColumnName;

		public SortDirection? InitialDirection => _initialDirection;

		public string Name => _name;

		public string DisplayName
		{
			get
			{
				if (_doNotSplit)
				{
					return _displayName;
				}
				return SplitPascalCase(_displayName);
			}
		}

		public Type ColumnType => _dataType;

		public int? Position => _position;

		[Obsolete("CustomHeaderRenderer has been deprecated. Please use Header instead.")]
		public Action<RenderingContext> CustomHeaderRenderer
		{
			get;
			set;
		}

		[Obsolete("CustomItemRenderer has been deprecated. Please use a custom column instead.")]
		public Action<RenderingContext, T> CustomItemRenderer
		{
			get;
			set;
		}

		public IDictionary<string, object> HeaderAttributes => _headerAttributes;

		public Func<GridRowViewData<T>, IDictionary<string, object>> Attributes => GetAttributesFromRow;

		public GridColumn(Func<T, object> columnValueFunc, string name, Type type)
		{
			_name = name;
			_displayName = name;
			_dataType = type;
			_columnValueFunc = columnValueFunc;
		}

		IGridColumn<T> IGridColumn<T>.Attributes(Func<GridRowViewData<T>, IDictionary<string, object>> attributes)
		{
			_attributes.Add(attributes);
			return this;
		}

		IGridColumn<T> IGridColumn<T>.Sortable(bool isColumnSortable)
		{
			_sortable = isColumnSortable;
			return this;
		}

		public IGridColumn<T> AddChild(object childColumn)
		{
			_childColumns.Add((GridColumn<T>) childColumn);
			return this;
		}

		IGridColumn<T> IGridColumn<T>.SortColumnName(string name)
		{
			_sortColumnName = name;
			return this;
		}

		IGridColumn<T> IGridColumn<T>.SortInitialDirection(SortDirection initialDirection)
		{
			_initialDirection = initialDirection;
			return this;
		}

		IGridColumn<T> IGridColumn<T>.InsertAt(int index)
		{
			_position = index;
			return this;
		}

		private IDictionary<string, object> GetAttributesFromRow(GridRowViewData<T> row)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			IEnumerable<KeyValuePair<string, object>> enumerable = _attributes.SelectMany((Func<GridRowViewData<T>, IDictionary<string, object>> attributeFunc) => attributeFunc(row));
			foreach (KeyValuePair<string, object> item in enumerable)
			{
				dictionary[item.Key] = item.Value;
			}
			return dictionary;
		}

		public IGridColumn<T> Columns(Func<ColumnBuilder<T>, object> columnBuilder)
		{

			ColumnBuilder<T> columnBuilder2 = new ColumnBuilder<T>();
			columnBuilder(columnBuilder2);
			foreach (GridColumn<T> item in columnBuilder2)
			{
				_childColumns.Add(item);
			}

			return this;

		}

		public IGridColumn<T> Named(string name)
		{
			_displayName = name;
			_doNotSplit = true;
			return this;
		}

		public IGridColumn<T> DoNotSplit()
		{
			_doNotSplit = true;
			return this;
		}

		public IGridColumn<T> Format(string format)
		{
			_format = format;
			return this;
		}

		public IGridColumn<T> CellCondition(Func<T, bool> func)
		{
			_cellCondition = func;
			return this;
		}

		IGridColumn<T> IGridColumn<T>.Visible(bool isVisible)
		{
			_visible = isVisible;
			return this;
		}

		public IGridColumn<T> Header(Func<object, object> headerRenderer)
		{
			_headerRenderer = headerRenderer;
			return this;
		}

		public IGridColumn<T> Encode(bool shouldEncode)
		{
			_htmlEncode = shouldEncode;
			return this;
		}

		[Obsolete("Use Encode(false) instead.")]
		public IGridColumn<T> DoNotEncode()
		{
			return Encode(shouldEncode: false);
		}

		IGridColumn<T> IGridColumn<T>.HeaderAttributes(IDictionary<string, object> attributes)
		{
			foreach (KeyValuePair<string, object> attribute in attributes)
			{
				_headerAttributes.Add(attribute);
			}
			return this;
		}

		private string SplitPascalCase(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return input;
			}
			return Regex.Replace(input, "([A-Z])", " $1", RegexOptions.Compiled).Trim();
		}

		public object GetValue(T instance)
		{
			if (!_cellCondition(instance))
			{
				return null;
			}
			object obj = _columnValueFunc(instance);
			if (!string.IsNullOrEmpty(_format))
			{
				obj = string.Format(_format, obj);
			}
			if (_htmlEncode && obj != null && !(obj is IHtmlString))
			{
				obj = HttpUtility.HtmlEncode(obj.ToString());
			}
			return obj;
		}

		public string GetHeader()
		{
			return _headerRenderer(null)?.ToString();
		}
	}
}
