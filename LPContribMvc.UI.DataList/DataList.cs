using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;

namespace LPContribMvc.UI.DataList
{
	[Obsolete]
	public class DataList<T>
	{
		private const string TABLE = "table";

		private readonly IEnumerable<T> _dataSource;

		private readonly Hash _tableAttributes;

		private Action _emptyDataSourceTemplate;

		private Action _noItemTemplate;

		private Func<T, bool> _cellCondition = (T x) => true;

		private Hash _cellAttribute;

		protected TextWriter Writer
		{
			get;
			set;
		}

		public RepeatDirection RepeatDirection
		{
			get;
			set;
		}

		public int RepeatColumns
		{
			get;
			protected set;
		}

		public virtual Action<T> ItemRenderer
		{
			get;
			set;
		}

		public DataList(IEnumerable<T> dataSource, TextWriter writer)
			: this(dataSource, writer, (Hash)null)
		{
		}

		public DataList(IEnumerable<T> dataSource, TextWriter writer, Hash tableAttributes)
		{
			_dataSource = dataSource;
			RepeatColumns = 0;
			RepeatDirection = RepeatDirection.Vertical;
			_tableAttributes = tableAttributes;
			Writer = writer;
		}

		public virtual DataList<T> CellTemplate(Action<T> contentTemplate)
		{
			ItemRenderer = contentTemplate;
			return this;
		}

		protected void Write(string text)
		{
			Writer.Write(text);
		}

		public virtual DataList<T> EmptyDateSourceTemplate(Action template)
		{
			_emptyDataSourceTemplate = template;
			return this;
		}

		public virtual DataList<T> NoItemTemplate(Action template)
		{
			_noItemTemplate = template;
			return this;
		}

		public virtual DataList<T> CellCondition(Func<T, bool> func)
		{
			_cellCondition = func;
			return this;
		}

		public virtual DataList<T> CellAttributes(params Func<object, object>[] attributes)
		{
			_cellAttribute = new Hash(attributes);
			return this;
		}

		protected virtual bool ShouldRenderCell(T item)
		{
			return _cellCondition(item);
		}

		public virtual DataList<T> NumberOfColumns(int amount)
		{
			RepeatColumns = amount;
			return this;
		}

		public DataList<T> RepeatVertically()
		{
			RepeatDirection = RepeatDirection.Vertical;
			return this;
		}

		public DataList<T> RepeatHorizontally()
		{
			RepeatDirection = RepeatDirection.Horizontal;
			return this;
		}

		protected virtual void RenderCell(T item)
		{
			Writer.Write($"<td{BuildHtmlAttributes(_cellAttribute)}>");
			if (ItemRenderer != null)
			{
				ItemRenderer(item);
			}
			Writer.Write("</td>");
		}

		public virtual void Render()
		{
			Write(string.Format("<{0}{1}>", "table", BuildHtmlAttributes(_tableAttributes)));
			BuildTable();
			Write(string.Format("</{0}>", "table"));
		}

		public override string ToString()
		{
			Render();
			return null;
		}

		private void BuildTable()
		{
			IList<T> list = _dataSource.Where((T x) => _cellCondition(x)).ToList();
			if (list.Count < 1)
			{
				Write("<tr><td>");
				if (_emptyDataSourceTemplate != null)
				{
					_emptyDataSourceTemplate();
				}
				Write("</td></tr>");
			}
			else
			{
				int repeatColumns = (RepeatColumns < 1) ? 1 : RepeatColumns;
				if (RepeatDirection == RepeatDirection.Horizontal)
				{
					RenderHorizontal(repeatColumns, list);
				}
				else
				{
					RenderVertical(repeatColumns, list);
				}
			}
		}

		private void RenderHorizontal(int repeatColumns, IList<T> items)
		{
			int num = CalculateAmountOfRows(items.Count, repeatColumns);
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				Write("<tr>");
				for (int j = 0; j < repeatColumns; j++)
				{
					if (num2 + 1 <= items.Count)
					{
						RenderCell(items[num2]);
					}
					else
					{
						RenderNoItemCell();
					}
					num2++;
				}
				Write("</tr>");
			}
		}

		private void RenderVertical(int repeatColumns, IList<T> items)
		{
			int num = CalculateAmountOfRows(items.Count, repeatColumns);
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				Write("<tr>");
				for (int j = 0; j < repeatColumns; j++)
				{
					if (num2 + 1 <= items.Count)
					{
						if (j == 0)
						{
							RenderCell(items[i]);
						}
						else
						{
							RenderCell(items[j * num + i]);
						}
					}
					else
					{
						RenderNoItemCell();
					}
					num2++;
				}
				Write("</tr>");
			}
		}

		private void RenderNoItemCell()
		{
			Write($"<td{BuildHtmlAttributes(_cellAttribute)}>");
			if (_noItemTemplate != null)
			{
				_noItemTemplate();
			}
			Write("</td>");
		}

		private int CalculateAmountOfRows(int itemCount, int repeatColumns)
		{
			int num = itemCount / repeatColumns;
			if (itemCount % repeatColumns > 0)
			{
				num++;
			}
			return num;
		}

		private string BuildHtmlAttributes(IDictionary<string, object> attributes)
		{
			string text = DictionaryExtensions.ToHtmlAttributes(attributes);
			if (text.Length > 0)
			{
				text = " " + text;
			}
			return text;
		}
	}
}
